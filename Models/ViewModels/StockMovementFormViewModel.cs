using System.ComponentModel.DataAnnotations;

namespace EasyStock.Models.ViewModels
{
    public class StockMovementFormViewModel
    {
        [Required(ErrorMessage = "L'équipement est obligatoire.")]
        [Display(Name = "Équipement")]
        public int? ArticleId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La quantité doit être supérieure à zéro.")]
        [Display(Name = "Quantité")]
        public int Quantite { get; set; } = 1;

        [StringLength(100)]
        [Display(Name = "Référence")]
        public string? ReferenceExterne { get; set; }

        [StringLength(1000)]
        [Display(Name = "Motif / notes")]
        public string? Motif { get; set; }

        [Display(Name = "Réapprovisionnement")]
        public bool EstReapprovisionnement { get; set; }
    }
}
