using System.ComponentModel.DataAnnotations;

namespace Inzynierka.UI.DTOs
{
    public class CreateMaterialDto
    {
        [Required(ErrorMessage = "Nazwa materiału jest wymagana")]
        [StringLength(100, ErrorMessage = "Nazwa materiału może mieć max 100 znaków")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ilość jest wymagana")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Ilość musi byc większa niż 0")]
        public decimal Quantity { get; set; }

        [Required(ErrorMessage = "Jednostka jest wymagana")]
        [StringLength(50, ErrorMessage = "Jednoska może zawierać max 50 znaków")]
        public string Unit { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "Ścieżka załącznika może składać sie z max 255 znaków")]
        public string? AttachmentPath { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal PricePerUnit { get; set; }
    }
}
