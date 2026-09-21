using System.ComponentModel.DataAnnotations;

namespace EasyStock.Models.ViewModels
{
    public class AffectationReturnViewModel
    {
        [Required(ErrorMessage = "L'état au retour est obligatoire.")]
        [Display(Name = "État au retour")]
        public string EtatRetour { get; set; } = "Fonctionnel";

        [StringLength(1000)]
        [Display(Name = "Notes")]
        public string? Notes { get; set; }
    }
}
