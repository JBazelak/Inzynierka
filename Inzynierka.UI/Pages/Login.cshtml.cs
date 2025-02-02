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
                // Parsowanie odpowiedzi JSON
                var responseData = await response.Content.ReadFromJsonAsync<JsonElement>();
                var userId = responseData.GetProperty("userId").GetString();
                Console.WriteLine($"Response: {responseData}");

                if (!string.IsNullOrEmpty(userId))
                {
                    // Ustawienie sesji
                    HttpContext.Session.SetString("UserId", userId);
                    Console.WriteLine($"Sesja ustawiona: UserId = {userId}");
                }

                return RedirectToPage("/UserPanel");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                ErrorMessage = "Niepoprawny email lub has³o";
            }
            else
            {
                ErrorMessage = "Wyst¹pi³ nieoczekiwany problem. Spróbuj ponownie";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"An error occurred while connecting to the server: {ex.Message}";
        }

        return Page();
    }


}
