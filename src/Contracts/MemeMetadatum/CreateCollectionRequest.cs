namespace PepeProject.Contracts.MemeMetadatum
{
    public class CreateMemeMetadatumRequest
    {
        public int MemeId { get; set; }

        public long FileSize { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public string FileFormat { get; set; } = null!;

        public string MimeType { get; set; } = null!;
    }
}