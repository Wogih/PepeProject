namespace Domain.Models;

public partial class Comment
{
    public int CommentId { get; set; }

    public int MemeId { get; set; }

    public int UserId { get; set; }

    public string CommentText { get; set; } = null!;

    public DateTime? CommentDate { get; set; }

    public virtual Meme Meme { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
