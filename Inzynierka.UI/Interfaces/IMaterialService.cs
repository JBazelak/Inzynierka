﻿using Inzynierka.Core.Entities;
using Inzynierka.UI.DTOs;

namespace Inzynierka.UI.Interfaces
{
    public interface IMaterialService
    {
        Task<IEnumerable<MaterialDto>> GetAllMaterialsAsync(int projectId);
        Task<Material?> GetMaterialAsync(int projectId, int materialId);
        Task<string> UploadAttachmentAsync(Material material, IFormFile file);
        Task<bool> DeleteMaterialAsync(int projectId, int materialId);
        Task<MaterialDto> AddMaterialAsync(int contractorId, int projectId, CreateMaterialDto createMaterialDto);
        Task UpdateMaterialAsync(int contractorId, int projectId, int materialId, UpdateMaterialDto updateMaterialDto);
    }
}
