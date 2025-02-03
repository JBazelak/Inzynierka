using Inzynierka.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Inzynierka.Infrastructure.Services
{
    public class PdfReportService
    {
        private readonly AppDbContext _context;
        private readonly string _reportsDirectory;

        public PdfReportService(AppDbContext context)
        {
            _context = context;

            // Ustawienie katalogu raportów w katalogu aplikacji
            _reportsDirectory = Path.Combine(AppContext.BaseDirectory, "Reports");
            Directory.CreateDirectory(_reportsDirectory);

            // Załaduj pdfium.dll przed użyciem QuestPDF
            LoadPdfium();
        }

        public async Task<string> GenerateReportAsync(int projectId)
        {
            try
            {
                var project = await _context.Projects
                    .Include(p => p.Materials)
                    .Include(p => p.Contractor)
                    .FirstOrDefaultAsync(p => p.Id == projectId);

                if (project == null)
                    throw new KeyNotFoundException($"Nie znaleziono projektu o ID {projectId}");

                var fileName = $"ProjectReport_{project.Id}.pdf";
                var filePath = Path.Combine(_reportsDirectory, fileName);

                var document = DocumentService.Create(project);
                document.GeneratePdf(filePath);

                return filePath;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Błąd generowania raportu: {ex.Message}", ex);
            }
        }

        private void LoadPdfium()
        {
            try
            {
                string pdfiumPath = Path.Combine(AppContext.BaseDirectory, "pdfium.dll");

                if (!File.Exists(pdfiumPath))
                    throw new FileNotFoundException($"Plik {pdfiumPath} nie został znaleziony!");

                NativeLibrary.Load(pdfiumPath);
            }
            catch (Exception ex)
            {
                throw new DllNotFoundException("Nie udało się załadować pdfium.dll. Sprawdź, czy plik jest poprawnie wdrożony.", ex);
            }
        }
    }
}
