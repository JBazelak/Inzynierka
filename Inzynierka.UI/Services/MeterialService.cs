using Inzynierka.Infrastructure.Persistance;
using Inzynierka.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Inzynierka.UI.Interfaces;

namespace Inzynierka.UI.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly AppDbContext _context;
        private readonly List<string> _allowedExtensions = new List<string> { ".pdf", ".jpg", ".png", ".jpeg" }; 

        public MaterialService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Material?> GetMaterialAsync(int projectId, int materialId)
        {
            return await _context.Materials
                .Include(m => m.Project)
                .FirstOrDefaultAsync(m => m.ProjectId == projectId && m.Id == materialId);
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
