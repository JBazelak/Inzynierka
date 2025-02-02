using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Inzynierka.UI.DTOs;

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
            httpClient.BaseAddress = new Uri("https://localhost:7255");

            var response = await httpClient.PostAsJsonAsync($"api/contractors/{userId}/projects", NewProject);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/UserPanel");
            }
            else
            {
                ErrorMessage = "Nie uda�o si� doda� projektu";
            }
        }
        catch (HttpRequestException ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
        }

        return Page();
    }
}
