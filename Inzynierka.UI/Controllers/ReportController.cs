using Inzynierka.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

[Route("api/projects/{projectId}/report")]
[ApiController]
public class ReportController : ControllerBase
{
    private readonly PdfReportService _reportService;

    public ReportController(PdfReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet]
    public async Task<IActionResult> GenerateReport(int projectId)
    {
        try
        {
            var reportPath = await _reportService.GenerateReportAsync(projectId);

            // Zwraca plik do pobrania
            var fileBytes = System.IO.File.ReadAllBytes(reportPath);
            return File(fileBytes, "application/pdf", Path.GetFileName(reportPath));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
