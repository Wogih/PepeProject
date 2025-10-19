namespace PepeProject.Contracts.CollectionMeme
{
    public class GetCollectionMemeResponse
    {
        public int CollectionMemeId { get; set; }

        public int CollectionId { get; set; }

        public int MemeId { get; set; }

        public DateTime? AddedAt { get; set; }

    }
}