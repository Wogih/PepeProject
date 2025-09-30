namespace PepeProject.Contracts.Collection
{
    public class GetCollectionResponse
    {
        public int CollectionId { get; set; }

        public int UserId { get; set; }

        public string CollectionName { get; set; } = null!;

        public string? Description { get; set; }

        public bool? IsPublic { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

    }
}