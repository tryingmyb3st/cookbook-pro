using System.ComponentModel.DataAnnotations;

namespace CookbookWebApi.Models;

public class LoginRequest
{
    [Required(ErrorMessage = "Email обязателен")]
    [EmailAddress(ErrorMessage = "Некорректный формат email")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Пароль обязателен")]
    [MinLength(6, ErrorMessage = "Пароль должен быть минимум 6 символов")]
    public string Password { get; set; } = null!;

    public bool RememberMe { get; set; } = false;
}