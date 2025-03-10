using Inzynierka.Application.Interfaces;
using Inzynierka.Infrastructure.Persistance;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Inzynierka.Infrastructure.Validators
{
    public class MaterialValidator : IMaterialValidator
    {
        private readonly AppDbContext _context;
        private readonly List<string> _allowedExtensions = new List<string> { ".pdf", ".jpg", ".png", ".jpeg" };

        public MaterialValidator(AppDbContext context)
        {
            _context = context;
        }

        public async Task ValidateProjectExistsAsync(int contractorId, int projectId)
        {
            var exists = await _context.Projects.AnyAsync(p => p.Id == projectId && p.ContractorId == contractorId);
            if (!exists)
            {
                throw new KeyNotFoundException($"Project with ID {projectId} not found or does not belong to the contractor.");
            }
        }

        public async Task ValidateMaterialCreationAsync(int contractorId, int projectId)
        {
            await ValidateProjectExistsAsync(contractorId, projectId);
        }

        public void ValidateFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file was uploaded.");

            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!_allowedExtensions.Contains(fileExtension))
                throw new ArgumentException($"File type '{fileExtension}' is not supported. Allowed types: {string.Join(", ", _allowedExtensions)}.");

            const long maxFileSize = 5 * 1024 * 1024;
            if (file.Length > maxFileSize)
                throw new ArgumentException("File size exceeds the maximum limit of 5 MB.");
        }
    }
}
