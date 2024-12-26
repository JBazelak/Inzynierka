using System.ComponentModel.DataAnnotations;

namespace Inzynierka.UI.DTOs
{
    public class UpdateMaterialDto
    {
        [Required(ErrorMessage = "Material Name is required.")]
        [StringLength(100, ErrorMessage = "Material Name cannot exceed 100 characters.")]
        public string? Name { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public decimal? Quantity { get; set; }

        [StringLength(50, ErrorMessage = "Unit cannot exceed 50 characters.")]
        public string? Unit { get; set; }

        [StringLength(255, ErrorMessage = "Attachment Path cannot exceed 255 characters.")]
        public string? AttachmentPath { get; set; }
    }
}
