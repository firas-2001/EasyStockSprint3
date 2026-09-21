using System.ComponentModel.DataAnnotations;

namespace EasyStock.Models
{
    public class MouvementStock
    {
        public int MouvementStockId { get; set; }

        [Required]
        [StringLength(40)]
        [Display(Name = "Référence")]
        public string ReferenceCode { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Équipement")]
        public int ArticleId { get; set; }

        [Display(Name = "Utilisateur")]
        public int? UserId { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Type de mouvement")]
        public string TypeMouvement { get; set; } = MouvementType.Entree;

        [Range(1, int.MaxValue, ErrorMessage = "La quantité doit être supérieure à zéro.")]
        [Display(Name = "Quantité")]
        public int Quantite { get; set; }

        [Display(Name = "Stock avant")]
        public int StockAvant { get; set; }

        [Display(Name = "Stock après")]
        public int StockApres { get; set; }

        [StringLength(100)]
        [Display(Name = "Référence externe")]
        public string? ReferenceExterne { get; set; }

        [StringLength(1000)]
        [Display(Name = "Motif / notes")]
        public string? Motif { get; set; }

        [Display(Name = "Réapprovisionnement")]
        public bool EstReapprovisionnement { get; set; }

        [Display(Name = "Alerte stock faible")]
        public bool AlerteStockFaibleDeclenchee { get; set; }

        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public Article? Article { get; set; }
        public User? User { get; set; }
    }
}
