using Inzynierka.UI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class RegisterModel : PageModel
{
    [BindProperty]
    public RegisterContractorDto registerData { get; set; }

    [BindProperty]
    public string PasswordRepeat { get; set; }

    public string successMessage { get; set; } = string.Empty;
    public string errorMessage { get; set; } = string.Empty;

    private readonly IHttpClientFactory _httpClientFactory;

    public RegisterModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public void OnGet()
    {
        registerData = new RegisterContractorDto();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            if (registerData.Password != PasswordRepeat)
            {
                errorMessage = "Hasła są różne";
                return Page();
            }

            foreach (var entry in ModelState)
            {
                var key = entry.Key; // Nazwa pola
                var errors = entry.Value.Errors;

                foreach (var error in errors)
                {
                    Console.WriteLine($"Property: {key}, Error: {error.ErrorMessage}");
                }
            }

            errorMessage = "Proszę usupełnić wszystkiepola poprawnie";
            return Page();
        }

        var _httpClient = _httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7255");

        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", registerData);

            if (response.IsSuccessStatusCode)
            {
                successMessage = "Zarejestrowano pomyślnie! Możesz się teraz zalogować";
                return Page();
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                errorMessage = $"Wystąpił błąd: {errorResponse}";
                return Page();
            }
        }
        catch (HttpRequestException ex)
        {
            errorMessage = $"Wystąpił błąd: {ex.Message}";
            return Page();
        }
    }
}
