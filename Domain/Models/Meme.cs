namespace Domain.Models;

public partial class Meme
{
    public int MemeId { get; set; }

    public int UserId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string ImageUrl { get; set; } = null!;

    public DateTime? UploadDate { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual MemeMetadatum? MemeMetadatum { get; set; }

    public virtual ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Collection> Collections { get; set; } = new List<Collection>();

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
