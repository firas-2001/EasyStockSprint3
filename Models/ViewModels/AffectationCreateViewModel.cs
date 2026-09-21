using System.ComponentModel.DataAnnotations;

namespace EasyStock.Models.ViewModels
{
    public class AffectationCreateViewModel
    {
        [Required(ErrorMessage = "L'équipement est obligatoire.")]
        [Display(Name = "Équipement")]
        public int ArticleId { get; set; }

        [Required(ErrorMessage = "L'employé est obligatoire.")]
        [Display(Name = "Employé")]
        public int EmployeId { get; set; }

        [Display(Name = "Recherche employé")]
        public string? SearchTerm { get; set; }

        [Display(Name = "Filtrer par")]
        public string SearchField { get; set; } = "Nom";

        [Required(ErrorMessage = "L'état de sortie est obligatoire.")]
        [Display(Name = "État à l'affectation")]
        public string EtatSortie { get; set; } = "Fonctionnel";

        [StringLength(1000)]
        [Display(Name = "Notes")]
        public string? Notes { get; set; }
    }
}
