using System;
using System.Collections.Generic;

namespace AdressAccounting.Models;

public partial class AreaBuilding
{
    public int Id { get; set; }

    public virtual ICollection<AdressRecord> AdressRecords { get; set; } = new List<AdressRecord>();

    public virtual ICollection<Adress> Adresses { get; set; } = new List<Adress>();
}
