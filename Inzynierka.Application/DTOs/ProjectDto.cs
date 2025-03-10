using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inzynierka.Application.DTOs
{
    public class ProjectDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Project name is required.")]
        [StringLength(150, ErrorMessage = "Nazwa projektu może maksymalnie zawierać 150 znaków")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Opis projektu może zawierać maksymalnie 500 znaków")]
        public string? Description { get; set; }

        public ICollection<MaterialDto> Materials { get; set; } = new List<MaterialDto>();

        [NotMapped]
        public decimal TotalCost => Materials?.Sum(m => m.TotalCost) ?? 0;
    }
}
