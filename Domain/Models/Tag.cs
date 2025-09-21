using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class Tag
{
    public int TagId { get; set; }

    public string TagName { get; set; } = null!;

    public virtual ICollection<Meme> Memes { get; set; } = new List<Meme>();
}
