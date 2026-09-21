using System.ComponentModel.DataAnnotations;

namespace EasyStock.Models
{
    public class Affectation
    {
        public int AffectationId { get; set; }

        [Display(Name = "Référence")]
        public string ReferenceCode { get; set; } = string.Empty;

        [Display(Name = "Équipement")]
        public int ArticleId { get; set; }

        [Display(Name = "Employé")]
        public int EmployeId { get; set; }

        [Display(Name = "État à l'affectation")]
        public string EtatSortie { get; set; } = string.Empty;

        [Display(Name = "État au retour")]
        public string? EtatRetour { get; set; }

        [Display(Name = "Date d'affectation")]
        public DateTime AssignedAt { get; set; }

        [Display(Name = "Date de retour")]
        public DateTime? ReturnedAt { get; set; }

        [Display(Name = "Affectation active")]
        public bool IsActive { get; set; }

        [StringLength(1000)]
        public string? Notes { get; set; }

        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; }

        public Article? Article { get; set; }
        public Employe? Employe { get; set; }
    }
}
