using System;
using System.Collections.Generic;

namespace CoreAngularDevTest.Server.Models;

public partial class Location
{
    public int Id { get; set; }

    public double Lat { get; set; }

    public double Lng { get; set; }

    public virtual ICollection<Geometry> Geometries { get; set; } = new List<Geometry>();
}
