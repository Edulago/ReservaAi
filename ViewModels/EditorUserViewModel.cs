using System.ComponentModel.DataAnnotations;

namespace ReservaAi.ViewModels
{
    public class EditorUserViewModel
    {
        [Required(ErrorMessage = "O username é obrigatório")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "O username deve ter entre 3 e 40 caracteres")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "O email não é válido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O nome completo é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome completo deve ter entre 3 e 100 caracteres")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "A data de nascimento é obrigatória")]
        public DateOnly Birthdate { get; set; }

        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 100 caracteres")]
        public string? Password { get; set; }
    }
}
