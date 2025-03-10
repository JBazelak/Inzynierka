using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Inzynierka.Application.DTOs;

public class AddProjectModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AddProjectModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [BindProperty]
    public ProjectDto NewProject { get; set; }

    public string ErrorMessage { get; set; }

    public void OnGet()
    {
        NewProject = new ProjectDto();
        ErrorMessage = string.Empty;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToPage("/Login");
        }

        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri("https://material-manager.azurewebsites.net");

            var response = await httpClient.PostAsJsonAsync($"api/contractors/{userId}/projects", NewProject);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/UserPanel");
            }
            else
            {
                ErrorMessage = "Nie uda³o siê dodaæ projektu";
            }
        }
        catch (HttpRequestException ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
        }

        return Page();
    }
}
