using Inzynierka.Infrastructure.Persistance;
using Inzynierka.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Inzynierka.Application.Interfaces;
using Inzynierka.Application.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace Inzynierka.Application.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly AppDbContext _context;
        private readonly List<string> _allowedExtensions = new List<string> { ".pdf", ".jpg", ".png", ".jpeg" };
        private readonly IMapper _mapper;

        public MaterialService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        public async Task<IEnumerable<MaterialDto>> GetAllMaterialsAsync(int projectId)
        {
            var materials = await _context.Materials
                .Where(m => m.ProjectId == projectId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<MaterialDto>>(materials);
        }


        public async Task<Material?> GetMaterialAsync(int projectId, int materialId)
        {
            return await _context.Materials
                .Include(m => m.Project)
                .FirstOrDefaultAsync(m => m.ProjectId == projectId && m.Id == materialId);
        }

        public async Task<MaterialDto> AddMaterialAsync(int contractorId, int projectId, CreateMaterialDto createMaterialDto)
        {
            var contractorExists = await _context.Contractors.AnyAsync(c => c.Id == contractorId);
            if (!contractorExists)
                throw new KeyNotFoundException($"Wykonawca z ID {contractorId} nie został znaleziony.");

            // Sprawdzenie istnienia projektu
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == projectId && p.ContractorId == contractorId);

            if (project == null)
                throw new KeyNotFoundException("Nie znaleziono projektu");

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

            if (!string.IsNullOrEmpty(updateMaterialDto.Name))
                material.Name = updateMaterialDto.Name;

            if (updateMaterialDto.Quantity.HasValue)
                material.Quantity = updateMaterialDto.Quantity.Value;

            if (!string.IsNullOrEmpty(updateMaterialDto.Unit))
                material.Unit = updateMaterialDto.Unit;

            if (updateMaterialDto.PricePerUnit.HasValue)
                material.PricePerUnit = updateMaterialDto.PricePerUnit.Value;

            material.LastUpdated = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }



        public async Task<string> UploadAttachmentAsync(Material material, IFormFile file)
        {

            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("No file was uploaded.");
            }

            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!_allowedExtensions.Contains(fileExtension))
            {
                throw new ArgumentException($"File type '{fileExtension}' is not supported. Allowed types are: {string.Join(", ", _allowedExtensions)}.");
            }

            const long maxFileSize = 5 * 1024 * 1024; 
            if (file.Length > maxFileSize)
            {
                throw new ArgumentException("File size exceeds the maximum limit of 5 MB.");
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Attachments");
            Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, $"{Guid.NewGuid()}_{file.FileName}");

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            material.AttachmentPath = filePath;
            await _context.SaveChangesAsync();
            return filePath;
        }

        public async Task<bool> DeleteMaterialAsync(int projectId, int materialId)
        {
            var material = await _context.Materials.FirstOrDefaultAsync(m => m.Id == materialId && m.ProjectId == projectId);
            if (material == null)
                return false;

            if (!string.IsNullOrEmpty(material.AttachmentPath))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", material.AttachmentPath.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.Materials.Remove(material);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
