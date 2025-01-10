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
    public bool IsSecondStep { get; set; }

    private readonly IHttpClientFactory _httpClientFactory;

    public RegisterModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public void OnGet()
    {
        registerData = new RegisterContractorDto();
        IsSecondStep = false;
    }

    public async Task<IActionResult> OnPostAsync(string step)
    {
        ModelState.Remove("step");
        if (step == "1")
        {
            if (registerData.Password != PasswordRepeat)
            {
                errorMessage = "Please fill all fields correctly and ensure passwords match.";
                return Page();
            }

            IsSecondStep = true;
            return Page();
        }

        if (!ModelState.IsValid)
        {

            foreach (var entry in ModelState)
            {
                var key = entry.Key; // Nazwa pola
                var errors = entry.Value.Errors;

                foreach (var error in errors)
                {
                    Console.WriteLine($"Property: {key}, Error: {error.ErrorMessage}");
                }
            }

            errorMessage = "Please fill all fields correctly.";
            return Page();
        }

        var _httpClient = _httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7255");

        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", registerData);

            if (response.IsSuccessStatusCode)
            {
                successMessage = "Registration successful! You can now log in.";
                return RedirectToPage("/Login");
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                errorMessage = $"Error: {errorResponse}";
                return Page();
            }
        }
        catch (HttpRequestException ex)
        {
            errorMessage = $"An error occurred: {ex.Message}";
            return Page();
        }
    }
}
