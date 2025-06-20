using System;
using System.Collections.Generic;

namespace CoreAngularDevTest.Server;

public partial class Geometry
{
    public int Id { get; set; }

    public int BoundsId { get; set; }

    public int LocationId { get; set; }

    public string? LocationType { get; set; }

    public int ViewportId { get; set; }

    public virtual Bound Bounds { get; set; } = null!;

    public virtual Location Location { get; set; } = null!;

    public virtual ICollection<Result> Results { get; set; } = new List<Result>();

    public virtual Viewport Viewport { get; set; } = null!;
}
