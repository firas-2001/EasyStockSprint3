using System.ComponentModel.DataAnnotations;

namespace EasyStock.Models
{
    public class Employe
    {
        public int EmployeId { get; set; }

        [Required(ErrorMessage = "Le nom est obligatoire.")]
        [StringLength(100)]
        public string Nom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le prénom est obligatoire.")]
        [StringLength(100)]
        public string Prenom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le courriel professionnel est obligatoire.")]
        [EmailAddress(ErrorMessage = "Le courriel professionnel n'est pas valide.")]
        [Display(Name = "Courriel professionnel")]
        public string EmailProfessionnel { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le département est obligatoire.")]
        [StringLength(100)]
        public string Departement { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le poste est obligatoire.")]
        [StringLength(100)]
        public string Poste { get; set; } = string.Empty;

        [Display(Name = "Actif")]
        public bool IsActive { get; set; }

        public string FullName => $"{Prenom} {Nom}";

        public ICollection<Affectation> Affectations { get; set; } = new List<Affectation>();
    }
}
