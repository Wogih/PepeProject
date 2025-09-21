namespace Domain.Models;

public partial class Collection
{
    public int CollectionId { get; set; }

    public int UserId { get; set; }

    public string CollectionName { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Meme> Memes { get; set; } = new List<Meme>();
}
