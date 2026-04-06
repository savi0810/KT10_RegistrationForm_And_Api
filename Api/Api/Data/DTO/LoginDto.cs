using System.ComponentModel.DataAnnotations;

namespace Api.Data.DTO
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Некорректный формат email")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Пароль обязателен")]
        public string Password { get; set; } = null!;
    }
}
