using System;
using System.Collections.Generic;

namespace AdressAccounting.Models;

public partial class Adress
{
    public int Id { get; set; }

    public int? Number { get; set; }

    public int? StreetId { get; set; }

    public int? AreaId { get; set; }

    public bool? IsActual { get; set; }

    public virtual ICollection<AdressRecord> AdressRecords { get; set; } = new List<AdressRecord>();

    public virtual AreaBuilding? Area { get; set; }

    public virtual Street? Street { get; set; }
}
