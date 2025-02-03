using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Inzynierka.UI.DTOs;
using Inzynierka.Core.Entities;

public class ManageMaterialsModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ManageMaterialsModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public int ContractorId { get; set; }
    public int ProjectId { get; set; }
    public string ProjectName { get; set; }

    public List<MaterialDto> Materials { get; set; } = new List<MaterialDto>();
    public string ErrorMessage { get; set; }
    public string SuccessMessage { get; set; }

    [BindProperty]
    public MaterialDto NewMaterial { get; set; } = new MaterialDto();

    [BindProperty]
    public MaterialDto EditMaterial { get; set; } = new MaterialDto();

    [BindProperty]
    public int UploadAttachmentMaterialId { get; set; }

    [BindProperty]
    public IFormFile UploadFile { get; set; }

    public async Task<IActionResult> OnPostAddMaterialAsync(int contractorId, int projectId)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri("https://material-manager.azurewebsites.net/");

            var response = await httpClient.PostAsJsonAsync(
                $"api/contractors/{contractorId}/projects/{projectId}/materials", NewMaterial);

            var projectResponse = await httpClient.GetAsync($"api/contractors/{contractorId}/projects/{projectId}");
            if (projectResponse.IsSuccessStatusCode)
            {
                var project = await projectResponse.Content.ReadFromJsonAsync<ProjectDto>();
                ProjectName = project?.Name ?? "Unknown Project";
            }
            else
            {
                ProjectName = "Unknown Project";
                ErrorMessage = "Failed to load project details.";
            }

            if (response.IsSuccessStatusCode)
            {
                SuccessMessage = "Material added successfully.";
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                ErrorMessage = $"Failed to add material. Server response: {errorContent}";
                return Page();
            }
        }
        catch (HttpRequestException ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
            return Page();
        }

        return RedirectToPage(new { contractorId, projectId });
    }

    public async Task<IActionResult> OnGetAsync(int contractorId, int projectId)
    {
        ContractorId = contractorId;
        ProjectId = projectId;

        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri("https://material-manager.azurewebsites.net/");


            var projectResponse = await httpClient.GetAsync($"api/contractors/{contractorId}/projects/{projectId}");
            if (projectResponse.IsSuccessStatusCode)
            {
                var project = await projectResponse.Content.ReadFromJsonAsync<ProjectDto>();
                ProjectName = project?.Name ?? "Unknown Project";
            }
            else
            {
                ProjectName = "Unknown Project";
                ErrorMessage = "Failed to load project details.";
            }

            var materialsResponse = await httpClient.GetAsync($"api/contractors/{contractorId}/projects/{projectId}/materials");
            if (materialsResponse.IsSuccessStatusCode)
            {
                Materials = await materialsResponse.Content.ReadFromJsonAsync<List<MaterialDto>>();
            }
            else
            {
                ErrorMessage = "Failed to load materials.";
            }
        }
        catch (HttpRequestException ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
        }

        return Page();
    }

    public async Task<IActionResult> OnPostEditMaterialAsync(int contractorId, int projectId)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri("https://material-manager.azurewebsites.net");

            var response = await httpClient.PutAsJsonAsync(
                $"api/contractors/{contractorId}/projects/{projectId}/materials/{EditMaterial.Id}", EditMaterial);

            if (response.IsSuccessStatusCode)
            {
                SuccessMessage = "Material updated successfully.";
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                ErrorMessage = $"Failed to update material. Server response: {errorContent}";
                return Page();
            }
        }
        catch (HttpRequestException ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
            return Page();
        }

        return RedirectToPage(new { contractorId, projectId });
    }


    public async Task<IActionResult> OnGetGenerateReportAsync(int contractorId, int projectId)
    {
        ContractorId = contractorId;
        ProjectId = projectId;

        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri("https://material-manager.azurewebsites.net");

            // Pobranie pliku jako strumienia
            var response = await httpClient.GetAsync($"api/projects/{projectId}/report");

            if (response.IsSuccessStatusCode)
            {
                var fileBytes = await response.Content.ReadAsByteArrayAsync();
                var fileName = response.Content.Headers.ContentDisposition?.FileName ?? $"ProjectReport_{projectId}.pdf";

                return File(fileBytes, "application/pdf", fileName);
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                ErrorMessage = $"Nie uda³o sie wygenerowaæ raportu. OdpowiedŸ serwera: {errorContent}";
                return Page();
            }
        }
        catch (HttpRequestException ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
            return Page();
        }
    }

    public async Task<IActionResult> OnPostUploadAttachmentAsync(int contractorId, int projectId, int materialId)
    {
        ContractorId = contractorId;
        ProjectId = projectId;

        if (UploadFile == null || UploadFile.Length == 0)
        {
            ErrorMessage = "Nie wybrano pliku lub plik jest pusty";
            return RedirectToPage(new { contractorId, projectId });
        }

        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri("https://material-manager.azurewebsites.net");

            using var content = new MultipartFormDataContent();
            content.Add(new StreamContent(UploadFile.OpenReadStream()), "file", UploadFile.FileName);

            var response = await httpClient.PostAsync(
                $"api/contractors/{contractorId}/projects/{projectId}/materials/{materialId}/attachments", content);

            if (response.IsSuccessStatusCode)
            {
                SuccessMessage = "Poprawnie dodadno za³¹cznik";
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                ErrorMessage = $"Nie uda³o sie wygenerowaæ raportu. OdpowiedŸ serwera: {responseContent}";
            }
        }
        catch (HttpRequestException ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Unexpected error: {ex.Message}";
        }

        return RedirectToPage(new { contractorId, projectId });
    }
}
