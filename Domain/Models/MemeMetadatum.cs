namespace Domain.Models;

public partial class MemeMetadatum
{
    public int MemeId { get; set; }

    public int? Views { get; set; }

    public int? Shares { get; set; }

    public DateTime? LastUpdated { get; set; }

    public virtual Meme Meme { get; set; } = null!;
}
