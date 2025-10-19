namespace Domain.Models;

public partial class Tag
{
    public int TagId { get; set; }

    public string TagName { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<MemeTag> MemeTags { get; set; } = new List<MemeTag>();
}