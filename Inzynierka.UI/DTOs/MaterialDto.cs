using System.ComponentModel.DataAnnotations;

namespace Inzynierka.UI.DTOs
{
    public class MaterialDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Material name is required.")]
        [StringLength(100, ErrorMessage = "Material name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public decimal Quantity { get; set; }

        [Required(ErrorMessage = "Unit is required.")]
        [StringLength(50, ErrorMessage = "Unit cannot exceed 50 characters.")]
        public string Unit { get; set; } = string.Empty;

        [Required(ErrorMessage = "ProjectId is required.")]
        public int ProjectId { get; set; }
    }
}
