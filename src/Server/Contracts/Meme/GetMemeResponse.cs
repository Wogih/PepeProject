namespace PepeProject.Contracts.Meme
{
    public class GetMemeResponse
    {
        public int MemeId { get; set; }

        public int UserId { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public string ImageUrl { get; set; } = null!;

        public DateTime? UploadDate { get; set; }

        public bool? IsPublic { get; set; }
    }
}