namespace Domain.Models;

public partial class MemeMetadatum
{
    public int MetadataId { get; set; }

    public int MemeId { get; set; }

    public long FileSize { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    public string FileFormat { get; set; } = null!;

    public string MimeType { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual Meme Meme { get; set; } = null!;
}