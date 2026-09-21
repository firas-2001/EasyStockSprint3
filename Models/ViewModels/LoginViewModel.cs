using System.ComponentModel.DataAnnotations;

namespace EasyStock.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Le nom d'utilisateur est obligatoire.")]
        [Display(Name = "Nom d'utilisateur")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; } = string.Empty;

        public string? ReturnUrl { get; set; }
    }
}
