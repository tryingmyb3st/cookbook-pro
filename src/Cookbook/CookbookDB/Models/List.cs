namespace CookbookDB.Models;

/// <summary>
/// списки рецептов пользователей
/// </summary>
public partial class List
{
    /// <summary>
    /// идентификатор списка
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// название списка
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// идентификатор пользователя
    /// </summary>
    public long? UserId { get; set; }

    /// <summary>
    /// описание
    /// </summary>
    public string? Description { get; set; }

    public virtual User? User { get; set; }
}
