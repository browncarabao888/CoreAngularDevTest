using System;
using System.Collections.Generic;

namespace CoreAngularDevTest.Server.Models;

public partial class Result
{
    public int Id { get; set; }

    public string? FormattedAddress { get; set; }

    public int GeometryId { get; set; }

    public string? PlaceId { get; set; }

    public string Types { get; set; } = null!;

    public int? LocationGeoInfoId { get; set; }

    public virtual AddressComponent? AddressComponent { get; set; }

    public virtual Geometry Geometry { get; set; } = null!;

    public virtual LocationGeoInfo? LocationGeoInfo { get; set; }
}
