namespace PepeProject.Contracts.Collection
{
    public class CreateCollectionRequest
    {
        public int UserId { get; set; }

        public string CollectionName { get; set; } = null!;

        public string? Description { get; set; }

        public bool? IsPublic { get; set; }
    }
}