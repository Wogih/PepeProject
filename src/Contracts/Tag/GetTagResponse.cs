namespace PepeProject.Contracts.Tag
{
    public class GetTagResponse
    {
        public int TagId { get; set; }

        public string TagName { get; set; } = null!;

        public DateTime? CreatedAt { get; set; }
    }
}