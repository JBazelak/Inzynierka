using System.ComponentModel.DataAnnotations;

namespace Inzynierka.UI.DTOs
{
    public class ProjectDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Project name is required.")]
        [StringLength(150, ErrorMessage = "Project name cannot exceed 150 characters.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "ContractorId is required.")]
        public int ContractorId { get; set; }
    }
}
