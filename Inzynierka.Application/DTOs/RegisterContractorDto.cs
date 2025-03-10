using System.ComponentModel.DataAnnotations;

namespace Inzynierka.Application.DTOs
{
    public class RegisterContractorDto
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Adres może zawierać maksymalnie 200 znaków")]
        public string Address { get; set; } = string.Empty;

        [RegularExpression(@"^\d{9,15}$", ErrorMessage = "Numer telefonu powinien zawierać minimum 9 cyfr (max 15)")]
        public string? PhoneNumber { get; set; }

        [RegularExpression(@"\d{10}", ErrorMessage = "NIP powinien zawierać 10 cyfr")]
        public string? TaxIdNumber { get; set; }

        [RegularExpression(@"^\d{9}|\d{14}$", ErrorMessage = "Regon powinien zawierać 9 lub 14 cyfr")]
        public string? NationalBusinessRegistryNumber { get; set; }
    }
}
