namespace Domain.Models;

public partial class CollectionMeme
{
    public int CollectionMemeId { get; set; }

    public int CollectionId { get; set; }

    public int MemeId { get; set; }

    public DateTime? AddedAt { get; set; }

    public virtual Collection Collection { get; set; } = null!;

    public virtual Meme Meme { get; set; } = null!;
}