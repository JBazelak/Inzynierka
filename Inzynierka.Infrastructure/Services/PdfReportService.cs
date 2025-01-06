using Inzynierka.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;

namespace Inzynierka.Infrastructure.Services
{
    public class PdfReportService
    {
        private int PAGE = 0;
        private readonly AppDbContext _context;

        public PdfReportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> GenerateReportAsync(int projectId)
        {
            var project = await _context.Projects
                .Include(p => p.Materials)
                .Include(p => p.Contractor)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
                throw new KeyNotFoundException("Project not found.");

            var fileName = $"ProjectReport_{project.Id}.pdf";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", fileName);

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            var document = DocumentService.Create(project);
            document.GeneratePdf(filePath);

            return filePath;
        }
    }
}
