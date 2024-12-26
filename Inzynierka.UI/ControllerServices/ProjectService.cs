using AutoMapper;
using Inzynierka.Infrastructure.Persistance;
using Inzynierka.Core.Entities;
using Inzynierka.UI.DTOs;
using Inzynierka.UI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inzynierka.UI.Services
{
    public class ProjectService : IProjectService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProjectService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProjectDto>> GetAllAsync(int contractorId)
        {
            var projects = await _context.Projects
                .Where(p => p.ContractorId == contractorId)
                .ToListAsync();
            return _mapper.Map<IEnumerable<ProjectDto>>(projects);
        }

        public async Task<ProjectDto> GetByIdAsync(int contractorId, int id)
        {
            var project = await _context.Projects
            .Include(p => p.Materials)
            .FirstOrDefaultAsync(p => p.ContractorId == contractorId && p.Id == id);

            if (project == null)
                throw new KeyNotFoundException("Project not found.");

            return _mapper.Map<ProjectDto>(project);
        }

        public async Task<ProjectDto> CreateAsync(int contractorId, CreateProjectDto projectDto)
        {
            var contractorExists = await _context.Contractors.AnyAsync(c => c.Id == contractorId);
            if (!contractorExists)
                throw new KeyNotFoundException($"Contractor with ID {contractorId} not found.");

            var project = _mapper.Map<Project>(projectDto);
            project.ContractorId = contractorId;

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProjectDto>(project);
        }


        public async Task UpdateAsync(int contractorId, int id, CreateProjectDto projectDto)
        {
            if (id != contractorId)
                throw new ArgumentException("Project ID in the URL and body do not match.");

            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.ContractorId == contractorId && p.Id == id);

            if (project == null)
                throw new KeyNotFoundException("Project not found.");

            _mapper.Map(projectDto, project);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int contractorId, int id)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.ContractorId == contractorId && p.Id == id);

            if (project == null)
                throw new KeyNotFoundException("Project not found.");

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }

        public async Task<MaterialDto> AddMaterialAsync(int contractorId, int projectId, CreateMaterialDto createMaterialDto)
        {
            // Sprawdzenie istnienia wykonawcy
            var contractorExists = await _context.Contractors.AnyAsync(c => c.Id == contractorId);
            if (!contractorExists)
                throw new KeyNotFoundException($"Contractor with ID {contractorId} not found.");

            // Sprawdzenie istnienia projektu
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == projectId && p.ContractorId == contractorId);

            if (project == null)
                throw new KeyNotFoundException("Project not found.");

            // Dodanie materiału
            var material = _mapper.Map<Material>(createMaterialDto);
            material.ProjectId = projectId;

            _context.Materials.Add(material);
            await _context.SaveChangesAsync();

            return _mapper.Map<MaterialDto>(material);
        }



        public async Task UpdateMaterialAsync(int contractorId, int projectId, int materialId, UpdateMaterialDto updateMaterialDto)
        {
            var material = await _context.Materials
                .Include(m => m.Project)
                .FirstOrDefaultAsync(m => m.Project.ContractorId == contractorId && m.ProjectId == projectId && m.Id == materialId);

            if (material == null)
                throw new KeyNotFoundException("Material not found.");

            _mapper.Map(updateMaterialDto, material);
            material.LastUpdated = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteMaterialAsync(int contractorId, int projectId, int materialId)
        {
            var material = await _context.Materials
                .Include(m => m.Project)
                .FirstOrDefaultAsync(m => m.Project.ContractorId == contractorId && m.ProjectId == projectId && m.Id == materialId);

            if (material == null)
                throw new KeyNotFoundException("Material not found.");

            _context.Materials.Remove(material);
            await _context.SaveChangesAsync();
        }
    }
}
