namespace Domain.Models;

public partial class MemeTag
{
    public int MemeTagId { get; set; }

    public int MemeId { get; set; }

    public int TagId { get; set; }

    public DateTime? TaggedAt { get; set; }

    public virtual Meme Meme { get; set; } = null!;

    public virtual Tag Tag { get; set; } = null!;
}