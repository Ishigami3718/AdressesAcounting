using System;
using System.Collections.Generic;

namespace AdressAccounting.Models;

public partial class SplitResult
{
    public int Id { get; set; }

    public int? SplitRecordsId { get; set; }

    public int? StreetId { get; set; }

    public virtual SplitRecord? SplitRecords { get; set; }

    public virtual Street? Street { get; set; }
}
