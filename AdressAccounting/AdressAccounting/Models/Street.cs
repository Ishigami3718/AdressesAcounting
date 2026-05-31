using System;
using System.Collections.Generic;

namespace AdressAccounting.Models;

public partial class Street
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public bool IsActual { get; set; }

    public virtual ICollection<Adress> Adresses { get; set; } = new List<Adress>();

    public virtual ICollection<MergeRecord> MergeRecords { get; set; } = new List<MergeRecord>();

    public virtual ICollection<MergedStreet> MergedStreets { get; set; } = new List<MergedStreet>();

    public virtual SplitRecord? SplitRecord { get; set; }

    public virtual ICollection<SplitResult> SplitResults { get; set; } = new List<SplitResult>();

    public virtual ICollection<StreetNameRecordsStreet> StreetNameRecordsStreets { get; set; } = new List<StreetNameRecordsStreet>();
}
