using System;
using System.Collections.Generic;

namespace CoreAngularDevTest.Server;

public partial class Northeast
{
    public int Id { get; set; }

    public double Lat { get; set; }

    public double Lng { get; set; }

    public virtual ICollection<Bound> Bounds { get; set; } = new List<Bound>();

    public virtual ICollection<Viewport> Viewports { get; set; } = new List<Viewport>();
}
