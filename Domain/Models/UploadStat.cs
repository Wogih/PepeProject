using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class UploadStat
{
    public int StatId { get; set; }

    public int UserId { get; set; }

    public int? UploadCount { get; set; }

    public int? TotalViews { get; set; }

    public DateTime? LastUploadDate { get; set; }

    public virtual User User { get; set; } = null!;
}
