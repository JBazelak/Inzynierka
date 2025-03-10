using AutoMapper;
using Inzynierka.Core.Entities;
using Inzynierka.Application.DTOs;
using Inzynierka.Application.Interfaces;

namespace Inzynierka.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMaterialValidator _projectMaterialValidator;
        private readonly IMapper _mapper;

        public ProjectService(IProjectRepository projectRepository, IMaterialValidator projectMaterialValidator, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _projectMaterialValidator = projectMaterialValidator;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProjectDto>> GetAllAsync(int contractorId)
        {
            var projects = await _projectRepository.GetAllAsync(contractorId);
            return _mapper.Map<IEnumerable<ProjectDto>>(projects);
        }

        public async Task<ProjectDto> GetByIdAsync(int contractorId, int id)
        {
            var project = await _projectRepository.GetByIdAsync(contractorId, id);
            if (project == null)
                throw new KeyNotFoundException("Project not found.");

            return _mapper.Map<ProjectDto>(project);
        }

        public async Task<ProjectDto> CreateAsync(int contractorId, CreateProjectDto projectDto)
        {
            await _projectMaterialValidator.ValidateProjectExistsAsync(contractorId, 0);

            var project = _mapper.Map<Project>(projectDto);
            project.ContractorId = contractorId;

            await _projectRepository.AddAsync(project);
            return _mapper.Map<ProjectDto>(project);
        }

        public async Task UpdateAsync(int contractorId, int id, CreateProjectDto projectDto)
        {
            var project = await _projectRepository.GetByIdAsync(contractorId, id);
            if (project == null)
                throw new KeyNotFoundException("Project not found.");

            _mapper.Map(projectDto, project);
            await _projectRepository.UpdateAsync(project);
        }

        public async Task DeleteAsync(int contractorId, int id)
        {
            var project = await _projectRepository.GetByIdAsync(contractorId, id);
            if (project == null)
                throw new KeyNotFoundException("Project not found.");

            await _projectRepository.DeleteAsync(project);
        }

        public async Task<decimal> GetProjectCostAsync(int projectId)
        {
            return await _projectRepository.GetProjectCostAsync(projectId);
        }
    }
}
