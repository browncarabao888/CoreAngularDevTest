using System;
using System.Collections.Generic;

namespace CoreAngularDevTest.Server.Models;

public partial class LocationGeoInfo
{
    public int Id { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Result> Results { get; set; } = new List<Result>();
}
