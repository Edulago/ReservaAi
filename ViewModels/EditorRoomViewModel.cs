using System.ComponentModel.DataAnnotations;

namespace ReservaAi.ViewModels
{
    public class EditorRoomViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 40 caracteres")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "A capacidade é obrigatória")]
        [Range(1, 1000, ErrorMessage = "A capacidade deve estar entre 1 e 1000")]
        public int Capacity { get; set; }

        [StringLength(200, ErrorMessage = "A descrição deve ter no máximo 200 caracteres")]
        public string Description { get; set; } = string.Empty;

        [Range(0, 10, ErrorMessage = "Status inválido")]
        public int Status { get; set; }
    }
}
