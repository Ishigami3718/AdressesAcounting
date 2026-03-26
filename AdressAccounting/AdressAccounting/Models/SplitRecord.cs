using System;
using System.Collections.Generic;

namespace AdressAccounting.Models;

public partial class SplitRecord
{
    public int Id { get; set; }

    public int? StreetIdSplittedStreet { get; set; }

    public DateOnly? Date { get; set; }

    public virtual Street IdNavigation { get; set; } = null!;

    public virtual ICollection<SplitResult> SplitResults { get; set; } = new List<SplitResult>();
}
