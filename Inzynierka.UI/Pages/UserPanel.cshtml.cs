using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Inzynierka.Application.DTOs;

public class UserPanelModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public UserPanelModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public int ContractorId { get; set; }
    public List<ProjectDto> Projects { get; set; } = new List<ProjectDto>();
    public string ErrorMessage { get; set; }

    public string SuccessMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var contractorIdString = HttpContext.Session.GetString("UserId");

        if (string.IsNullOrEmpty(contractorIdString))
        {
            return RedirectToPage("/Login");
        }

        if (!int.TryParse(contractorIdString, out var contractorId))
        {
            ErrorMessage = "Niepoprawny ID u¿ytkownika";
            return RedirectToPage("/Login");
        }

        ContractorId = contractorId;

        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri("https://material-manager.azurewebsites.net/");

            var sessionToken = HttpContext.Session.GetString("SessionToken");
            if (!string.IsNullOrEmpty(sessionToken))
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", sessionToken);
            }

            var response = await httpClient.GetAsync($"api/contractors/{ContractorId}/projects");
            if (response.IsSuccessStatusCode)
            {
                Projects = await response.Content.ReadFromJsonAsync<List<ProjectDto>>();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                ErrorMessage = "Nieautoryzowany dostêp do kontentu. Zaloguj siê ponownie.";
                return RedirectToPage("/Login");
            }
            else
            {
                ErrorMessage = "B³¹d przy ³adowaniu projektów";
            }
        }
        catch (HttpRequestException ex)
        {
            ErrorMessage = $"B³¹d: {ex.Message}";
        }

        return Page();
    }

    public async Task<IActionResult> OnPostLogoutAsync()
    {
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri("https://material-manager.azurewebsites.net/");

        try
        {
            var response = await httpClient.PostAsync("api/auth/logout", null);
            if (response.IsSuccessStatusCode)
            {
                HttpContext.Session.Clear();
                return RedirectToPage("/Login");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "B³¹d przy wylogowywaniu. Spróbuj ponownie.");
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Wyst¹pi³ b³¹d: {ex.Message}");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostDeleteProjectAsync(int contractorId, int id)
    {
        ContractorId = contractorId;

        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri("https://material-manager.azurewebsites.net/");

            var sessionToken = HttpContext.Session.GetString("SessionToken");
            if (!string.IsNullOrEmpty(sessionToken))
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", sessionToken);
            }

            var response = await httpClient.DeleteAsync($"api/contractors/{contractorId}/projects/{id}");

            if (response.IsSuccessStatusCode)
            {
                SuccessMessage = "Projekt zosta³ pomyœlnie usuniêty.";
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                ErrorMessage = $"B³¹d podczas usuwania projektu: {errorContent}";
            }
        }
        catch (HttpRequestException ex)
        {
            ErrorMessage = $"B³¹d: {ex.Message}";
        }

        return RedirectToPage();
    }


    public IActionResult OnPostAdd()
    {
        return RedirectToPage("/AddProject", new { contractorId = ContractorId });
    }
}
