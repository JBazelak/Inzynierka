using Inzynierka.UI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

public class LoginModelPage : PageModel
{
    [BindProperty]
    public LoginContractorDto Login { get; set; }

    public string ErrorMessage { get; set; }

    private readonly HttpClient _httpClient;

    public LoginModelPage(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri("https://material-manager.azurewebsites.net");
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
                // Parsowanie odpowiedzi JSON
                var responseData = await response.Content.ReadFromJsonAsync<JsonElement>();
                var userId = responseData.GetProperty("userId").GetString();

                if (!string.IsNullOrEmpty(userId))
                {
                    HttpContext.Session.SetString("UserId", userId);
                }

                return RedirectToPage("/UserPanel");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                ErrorMessage = "Niepoprawny email lub haslo";
            }
            else
            {
                ErrorMessage = "Wystapil nieoczekiwany problem. Sprobuj ponownie";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"An error occurred while connecting to the server: {ex.Message}";
        }

        return Page();
    }


}
