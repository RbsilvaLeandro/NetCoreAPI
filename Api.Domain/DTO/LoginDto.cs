using System.ComponentModel.DataAnnotations;

namespace Api.Domain.DTO
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Campo obrigatório para o login")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        [StringLength(100, ErrorMessage = "E-mail deve conter no mínimo 1 caracter")]
        public string Email { get; set; }
    }
}
