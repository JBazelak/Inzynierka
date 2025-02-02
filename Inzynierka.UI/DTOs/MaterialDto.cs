using System.ComponentModel.DataAnnotations;

namespace Inzynierka.UI.DTOs
{
    public class MaterialDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa materiału jest wymagana")]
        [StringLength(100, ErrorMessage = "Nazwa materiału możezawierać max 100 znaków")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ilość jest wymagana")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Ilość musi być wieksza niż 0")]
        public decimal Quantity { get; set; }

        [Required(ErrorMessage = "Jednostka jest wymagana")]
        [StringLength(50, ErrorMessage = "Jednonska może zawierać max 50 znaków")]
        public string Unit { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal PricePerUnit { get; set; }

        [Required(ErrorMessage = "Projekt jest wymagany")]
        public int ProjectId { get; set; }
        public decimal TotalCost => Quantity * PricePerUnit;
        public string? AttachmentPath { get; set; }
        public bool HasAttachment => !string.IsNullOrEmpty(AttachmentPath);
    }
}
