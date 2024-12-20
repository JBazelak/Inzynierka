using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inzynierka.Core.Entities
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int ContractorId { get; set; }

        [ForeignKey("ContractorId")]
        public Contractor Contractor { get; set; } = new Contractor();

        public ICollection<Material> Materials { get; set; } = new List<Material>();
    }
}
