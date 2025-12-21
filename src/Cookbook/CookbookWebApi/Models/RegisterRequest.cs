using System.ComponentModel.DataAnnotations;

namespace CookbookWebApi.Models;

public class RegisterRequest
{
    [Required(ErrorMessage = "Email обязателен")]
    [EmailAddress(ErrorMessage = "Некорректный формат email")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Пароль обязателен")]
    [MinLength(6, ErrorMessage = "Пароль должен быть минимум 6 символов")]
    public string Password { get; set; } = null!;

    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    public string ConfirmPassword { get; set; } = null!;

    [Required(ErrorMessage = "Полное имя обязательно")]
    [StringLength(100, ErrorMessage = "Имя должно быть от 2 до 100 символов", MinimumLength = 2)]
    public string FullName { get; set; } = null!;
}