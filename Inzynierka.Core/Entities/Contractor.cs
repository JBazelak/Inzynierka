using System.ComponentModel.DataAnnotations;

namespace Inzynierka.Core.Entities
{
    public class Contractor
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty ;

        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Address { get; set; }

        [StringLength(15)]
        public string? PhoneNumber { get; set; }

        [StringLength(10)]
        public string? TaxIdNumber { get; set; }

        [StringLength(9)]
        public string? NationalBusinessRegistryNumber { get; set; }

        [StringLength(100)]
        public string? Email { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }

}
