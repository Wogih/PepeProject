namespace PepeProject.Contracts.UploadStat
{
    public class CreateUploadStatRequest
    {
        public int MemeId { get; set; }

        public int? ViewsCount { get; set; }

        public int? DownloadCount { get; set; }

        public int? ShareCount { get; set; }
    }
}