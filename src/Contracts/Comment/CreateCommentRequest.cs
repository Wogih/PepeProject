namespace PepeProject.Contracts.Comment
{
    public class CreateCommentRequest
    {
        public int MemeId { get; set; }

        public int UserId { get; set; }

        public string CommentText { get; set; } = null!;

        public int? ParentCommentId { get; set; }
    }
}