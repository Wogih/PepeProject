namespace PepeProject.Contracts.Collection
{
    public class GetUploadStatResponse
    {
        public int StatId { get; set; }

        public int MemeId { get; set; }

        public int? ViewsCount { get; set; }

        public int? DownloadCount { get; set; }

        public int? ShareCount { get; set; }

        public DateTime? LastViewed { get; set; }

        public DateTime? LastUpdated { get; set; }

    }
}