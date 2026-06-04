using System;
using System.Collections.Generic;

namespace AdressAccounting.Models;

public partial class AdressRecord
{
    public int Id { get; set; }

    public int? AdressId { get; set; }

    public int? Number { get; set; }

    public DateOnly? DateFrom { get; set; }

    public DateOnly? DateTo { get; set; }

    public int? AreaId { get; set; }

    public string? StreetName { get; set; }

    public virtual Adress? Adress { get; set; }

    public virtual AreaBuilding? Area { get; set; }
}
