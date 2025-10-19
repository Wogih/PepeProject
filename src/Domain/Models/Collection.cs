namespace Domain.Models;

public partial class Collection
{
    public int CollectionId { get; set; }

    public int UserId { get; set; }

    public string CollectionName { get; set; } = null!;

    public string? Description { get; set; }

    public bool? IsPublic { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<CollectionMeme> CollectionMemes { get; set; } = new List<CollectionMeme>();

    public virtual User User { get; set; } = null!;
}