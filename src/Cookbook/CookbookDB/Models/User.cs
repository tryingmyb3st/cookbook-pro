namespace CookbookDB.Models;

/// <summary>
/// пользователи
/// </summary>
public partial class User
{
    /// <summary>
    /// идентификатор пользователя
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// имя пользователя
    /// </summary>
    public string Name { get; set; } = null!;

    public virtual ICollection<List> Lists { get; set; } = new List<List>();
}
