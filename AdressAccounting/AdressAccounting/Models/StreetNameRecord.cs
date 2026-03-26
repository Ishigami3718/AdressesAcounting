using System;
using System.Collections.Generic;

namespace AdressAccounting.Models;

public partial class StreetNameRecord
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public DateOnly? DateFrom { get; set; }

    public DateOnly? DateTo { get; set; }

    public virtual ICollection<StreetNameRecordsStreet> StreetNameRecordsStreets { get; set; } = new List<StreetNameRecordsStreet>();
}
