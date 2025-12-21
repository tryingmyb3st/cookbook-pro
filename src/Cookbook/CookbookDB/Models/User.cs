using Microsoft.AspNetCore.Identity;

namespace CookbookDB.Models;

/// <summary>
/// Пользователи
/// </summary>
public partial class User: IdentityUser<long>
{
}
