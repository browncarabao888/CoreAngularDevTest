using System;
using System.Collections.Generic;

namespace CoreAngularDevTest.Server.Models;

public partial class AddressComponent
{
    public int? Id { get; set; }

    public string? LongName { get; set; }

    public string? ShortName { get; set; }

    public string Types { get; set; } = null!;

    public int ResultId { get; set; }

    public virtual Result Result { get; set; } = null!;
}
