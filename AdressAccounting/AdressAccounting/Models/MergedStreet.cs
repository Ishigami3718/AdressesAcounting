using System;
using System.Collections.Generic;

namespace AdressAccounting.Models;

public partial class MergedStreet
{
    public int Id { get; set; }

    public int? MergeRecordsId { get; set; }

    public int? StreetId { get; set; }

    public virtual MergeRecord? MergeRecords { get; set; }

    public virtual Street? Street { get; set; }
}
