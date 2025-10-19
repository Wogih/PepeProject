namespace PepeProject.Contracts.Comment
{
    public class GetCommentResponse
    {
        public int CommentId { get; set; }

        public int MemeId { get; set; }

        public int UserId { get; set; }

        public string CommentText { get; set; } = null!;

        public DateTime? CommentDate { get; set; }

        public int? ParentCommentId { get; set; }

        public bool? IsEdited { get; set; }

        public DateTime? EditedAt { get; set; }
    }
}