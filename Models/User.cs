using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyStock.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Le nom d'utilisateur est obligatoire.")]
        [StringLength(100)]
        [Display(Name = "Nom d'utilisateur")]
        public string Username { get; set; } = string.Empty;

        public string PwdHash { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'adresse courriel est obligatoire.")]
        [EmailAddress(ErrorMessage = "L'adresse courriel n'est pas valide.")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Rôle")]
        public int RoleId { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; }

        [NotMapped]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "Le mot de passe doit contenir au moins 8 caractères, une majuscule, une minuscule et un chiffre.")]
        public string? PlainPassword { get; set; }

        [NotMapped]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmation du mot de passe")]
        [Compare(nameof(PlainPassword), ErrorMessage = "Les mots de passe ne correspondent pas.")]
        public string? ConfirmPassword { get; set; }

        public Role? Role { get; set; }
        public ICollection<MouvementStock> MouvementsStock { get; set; } = new List<MouvementStock>();
    }
}
