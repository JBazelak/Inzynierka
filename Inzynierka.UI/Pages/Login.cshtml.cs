using Inzynierka.UI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class LoginModelPage : PageModel
{
    [BindProperty]
    public LoginContractorDto Login { get; set; }

    public string ErrorMessage { get; set; }

    private readonly HttpClient _httpClient;

    public LoginModelPage(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7255");
    }

    public void OnGet()
    {
        Login = new LoginContractorDto();
        ErrorMessage = string.Empty;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", Login);

            if (response.IsSuccessStatusCode)
            {

                return RedirectToPage("/Index");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                ErrorMessage = "Invalid email or password.";
            }
            else
            {
                ErrorMessage = "An unexpected error occurred. Please try again.";
            }
        }
        catch
        {
            ErrorMessage = "An error occurred while connecting to the server.";
        }

        return Page();
    }
}
