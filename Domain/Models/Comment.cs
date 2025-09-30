namespace Domain.Models;

public partial class Comment
{
    public int CommentId { get; set; }

    public int MemeId { get; set; }

    public int UserId { get; set; }

    public string CommentText { get; set; } = null!;

    public DateTime? CommentDate { get; set; }

    public int? ParentCommentId { get; set; }

    public bool? IsEdited { get; set; }

    public DateTime? EditedAt { get; set; }

    public virtual ICollection<Comment> InverseParentComment { get; set; } = new List<Comment>();

    public virtual Meme Meme { get; set; } = null!;

    public virtual Comment? ParentComment { get; set; }

    public virtual User User { get; set; } = null!;
}