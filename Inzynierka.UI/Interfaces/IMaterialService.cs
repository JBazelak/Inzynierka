using Inzynierka.Core.Entities;

namespace Inzynierka.UI.Interfaces
{
    public interface IMaterialService
    {
        Task<Material?> GetMaterialAsync(int projectId, int materialId);
        Task<string> UploadAttachmentAsync(Material material, IFormFile file);
        Task<bool> DeleteMaterialAsync(int projectId, int materialId);
    }
}
