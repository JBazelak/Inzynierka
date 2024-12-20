using System.ComponentModel.DataAnnotations;

namespace Inzynierka.UI.DTOs
{
    public class UpdateContractorDto
    {

        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        public string? FirstName { get; set; } 
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        public string? LastName { get; set; } 

        [StringLength(100, ErrorMessage = "Company name cannot exceed 100 characters.")]
        public string? CompanyName { get; set; } 

        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        public string? Address { get; set; }

        [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 characters.")]
        public string? PhoneNumber { get; set; }

        [StringLength(10, ErrorMessage = "Tax ID number must be 10 characters.")]
        public string? TaxIdNumber { get; set; }

        [StringLength(9, ErrorMessage = "National Business Registry Number must be 9 characters.")]
        public string? NationalBusinessRegistryNumber { get; set; }

        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string? Email { get; set; }
    }
}
