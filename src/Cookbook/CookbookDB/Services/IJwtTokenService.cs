using CookbookDB.Models;

namespace CookbookDB.Services;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}