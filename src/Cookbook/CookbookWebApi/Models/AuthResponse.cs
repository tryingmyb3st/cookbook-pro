namespace CookbookWebApi.Models;

public class AuthResponse
{
    public string Token { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public long UserId { get; set; }
    public DateTime ExpiresAt { get; set; }
}