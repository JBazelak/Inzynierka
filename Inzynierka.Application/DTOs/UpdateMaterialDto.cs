using System.ComponentModel.DataAnnotations;

namespace Inzynierka.Application.DTOs
{
    public class UpdateMaterialDto
    {
        [StringLength(100, ErrorMessage = "Nazwa materiału nie może przekraczać 100 znaków.")]
        public string? Name { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Ilość musi być większa niż 0.")]
        public decimal? Quantity { get; set; }

        [StringLength(50, ErrorMessage = "Jednostka nie może przekraczać 50 znaków.")]
        public string? Unit { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Cena musi być większa niż 0.")]
        public decimal? PricePerUnit { get; set; }

        [StringLength(255, ErrorMessage = "Ścieżka załącznika nie może przekraczać 255 znaków.")]
        public string? AttachmentPath { get; set; }
    }
}
