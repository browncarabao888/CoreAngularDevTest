using System;
using System.Collections.Generic;

namespace CoreAngularDevTest.Server.Models;

public partial class Viewport
{
    public int Id { get; set; }

    public int NortheastId { get; set; }

    public int SouthwestId { get; set; }

    public virtual ICollection<Geometry> Geometries { get; set; } = new List<Geometry>();

    public virtual Northeast Northeast { get; set; } = null!;

    public virtual Southwest Southwest { get; set; } = null!;
}
