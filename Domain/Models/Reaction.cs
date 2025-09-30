namespace Domain.Models;

public partial class Reaction
{
    public int ReactionId { get; set; }

    public int MemeId { get; set; }

    public int UserId { get; set; }

    public string ReactionType { get; set; } = null!;

    public DateTime? ReactionDate { get; set; }

    public virtual Meme Meme { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}