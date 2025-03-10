using Inzynierka.Application.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Inzynierka.Application.DTOs
{
    public class ContractorDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Imie jest wymagane")]
        [StringLength(50, ErrorMessage = "Imię nie może być dłuższe niż 50 znaków")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        [StringLength(50, ErrorMessage = "Nazwisko może zawierać max 50 znaków")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nazwa firmy jest wymagana")]
        [StringLength(100, ErrorMessage = "Nazwa firmymoże zawierać max 100 znaków")]
        public string CompanyName { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Adres może składać się z max 200 znaków")]
        public string Address { get; set; } = string.Empty;

        [RegularExpression(@"^\d{9,15}$", ErrorMessage = "Numer telefonu powinien zawierać minimum 9 cyfr (max 15)")]
        public string? PhoneNumber { get; set; }

        [RegularExpression(@"\d{10}", ErrorMessage = "NIP powinien zawierać 10 cyfr")]
        public string? TaxIdNumber { get; set; }

        [RegularExpression(@"^\d{9}|\d{14}$", ErrorMessage = "Regon powinien zawierać 9 lub 14 cyfr")]
        public string? NationalBusinessRegistryNumber { get; set; }

        [StringLength(100, ErrorMessage = "Email może zawierać max 100 znaków")]
        [EmailAddress(ErrorMessage = "Niepoprawny fomrat adresu")]
        public string? Email { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<ProjectDto> Projects { get; set; } = new List<ProjectDto>();
    }
}
