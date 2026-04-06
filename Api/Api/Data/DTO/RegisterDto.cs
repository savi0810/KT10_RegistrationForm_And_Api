using System.ComponentModel.DataAnnotations;

namespace Api.Data.DTO
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Имя пользователя обязательно")]
        [MinLength(3, ErrorMessage = "Имя должно содержать не менее 3 символов")]
        [RegularExpression(@"^\p{L}+$", ErrorMessage = "Имя может содержать только буквы")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Некорректный формат email (должен содержать @ и .)")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Пароль обязателен")]
        [MinLength(8, ErrorMessage = "Пароль должен быть не менее 8 символов")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).+$",
            ErrorMessage = "Пароль должен содержать хотя бы одну букву и одну цифру")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Подтверждение пароля обязательно")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
