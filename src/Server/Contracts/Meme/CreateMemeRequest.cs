namespace PepeProject.Contracts.Meme
{
    public class CreateMemeRequest
    {
        public int UserId { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public string ImageUrl { get; set; } = null!;

        public bool? IsPublic { get; set; }
    }
}