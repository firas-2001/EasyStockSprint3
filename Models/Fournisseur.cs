using System.ComponentModel.DataAnnotations;

namespace EasyStock.Models
{
    public class Fournisseur
    {
        public int FournisseurId { get; set; }

        [Required]
        public string Nom { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
        public string ContactEmail { get; set; } = string.Empty;

        [RegularExpression(@"^\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}$")]
        public string PhoneNumber { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; }

        public ICollection<Article> Articles { get; set; } = new List<Article>();
    }
}
