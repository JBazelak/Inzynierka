using System.ComponentModel.DataAnnotations;

namespace Inzynierka.Application.DTOs
{
    public class CreateProjectDto
    {
        [Required(ErrorMessage = "Nazwa projektu jest wymagana")]
        [StringLength(150, ErrorMessage = "Nazwa projektu może mieć max 150 znaków")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Opis może mieć maksymalnie 500 znaków")]
        public string? Description { get; set; }

    }
}
