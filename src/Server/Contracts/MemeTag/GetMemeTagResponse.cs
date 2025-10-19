namespace PepeProject.Contracts.MemeTag
{
    public class GetMemeTagResponse
    {
        public int MemeTagId { get; set; }

        public int MemeId { get; set; }

        public int TagId { get; set; }

        public DateTime? TaggedAt { get; set; }
    }
}