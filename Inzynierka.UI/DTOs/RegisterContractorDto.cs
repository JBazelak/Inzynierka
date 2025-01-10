using System.ComponentModel.DataAnnotations;

namespace Inzynierka.UI.DTOs
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

        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        public string Address { get; set; } = string.Empty;

        [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 characters.")]
        public string? PhoneNumber { get; set; }

        [StringLength(10, ErrorMessage = "Tax ID number must be 10 characters.")]
        public string? TaxIdNumber { get; set; }

        [StringLength(9, ErrorMessage = "National Business Registry Number must be 9 characters.")]
        public string? NationalBusinessRegistryNumber { get; set; }
    }
}
