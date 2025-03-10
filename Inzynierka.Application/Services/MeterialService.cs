using AutoMapper;
using Inzynierka.Core.Entities;
using Inzynierka.Application.DTOs;
using Inzynierka.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Inzynierka.Application.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly IMaterialValidator _projectMaterialValidator;
        private readonly IMapper _mapper;

        public MaterialService(IMaterialRepository materialRepository, IMaterialValidator projectMaterialValidator, IMapper mapper)
        {
            _materialRepository = materialRepository;
            _projectMaterialValidator = projectMaterialValidator;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MaterialDto>> GetAllMaterialsAsync(int projectId)
        {
            var materials = await _materialRepository.GetAllMaterialsAsync(projectId);
            return _mapper.Map<IEnumerable<MaterialDto>>(materials);
        }

        public async Task<Material?> GetMaterialAsync(int projectId, int materialId)
        {
            return await _materialRepository.GetMaterialAsync(projectId, materialId);
        }

        public async Task<MaterialDto> AddMaterialAsync(int contractorId, int projectId, CreateMaterialDto createMaterialDto)
        {
            await _projectMaterialValidator.ValidateProjectExistsAsync(contractorId, projectId);

            var material = _mapper.Map<Material>(createMaterialDto);
            material.ProjectId = projectId;

            await _materialRepository.AddMaterialAsync(material);
            return _mapper.Map<MaterialDto>(material);
        }

        public async Task UpdateMaterialAsync(int contractorId, int projectId, int materialId, UpdateMaterialDto updateMaterialDto)
        {
            var material = await _materialRepository.GetMaterialAsync(projectId, materialId);
            if (material == null || material.Project.ContractorId != contractorId)
                throw new KeyNotFoundException("Material not found or you do not have permission.");

            _mapper.Map(updateMaterialDto, material);
            material.LastUpdated = DateTime.UtcNow;

            await _materialRepository.UpdateMaterialAsync(material);
        }

        public async Task<string> UploadAttachmentAsync(Material material, IFormFile file)
        {
            _projectMaterialValidator.ValidateFile(file);

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Attachments");
            Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, $"{Guid.NewGuid()}_{file.FileName}");

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            material.AttachmentPath = filePath;
            await _materialRepository.UpdateMaterialAsync(material);
            return filePath;
        }

        public async Task<bool> DeleteMaterialAsync(int projectId, int materialId)
        {
            var material = await _materialRepository.GetMaterialAsync(projectId, materialId);
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

            await _materialRepository.DeleteMaterialAsync(material);
            return true;
        }
    }
}
