using System;
using System.Collections.Generic;

namespace AdressAccounting.Models;

public partial class StreetNameRecordsStreet
{
    public int Id { get; set; }

    public int? StreetNameRecordsId { get; set; }

    public int? StreetId { get; set; }

    public virtual Street? Street { get; set; }

    public virtual StreetNameRecord? StreetNameRecords { get; set; }
}
