using System;
using System.Collections.Generic;

namespace AdressAccounting.Models;

public partial class MergeRecord
{
    public int Id { get; set; }

    public int? StreetIdResultOfMerging { get; set; }

    public DateOnly? Date { get; set; }

    public virtual ICollection<MergedStreet> MergedStreets { get; set; } = new List<MergedStreet>();

    public virtual Street? StreetIdResultOfMergingNavigation { get; set; }
}
