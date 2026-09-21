using System.ComponentModel.DataAnnotations;

namespace EasyStock.Models
{
    public class Article
    {
        public int ArticleId { get; set; }

        [Required(ErrorMessage = "Le nom de l'équipement est obligatoire.")]
        [Display(Name = "Nom")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "La description est obligatoire.")]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; }

        [Display(Name = "Catégorie")]
        public int CategoryId { get; set; }

        [Display(Name = "Fournisseur")]
        public int FournisseurId { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Stock disponible")]
        public int NombreArticleActuel { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Seuil minimal")]
        public int NombreArticleMinimum { get; set; }

        public Category? Category { get; set; }
        public Fournisseur? Fournisseur { get; set; }
        public ICollection<Affectation> Affectations { get; set; } = new List<Affectation>();
        public ICollection<MouvementStock> MouvementsStock { get; set; } = new List<MouvementStock>();
    }
}
