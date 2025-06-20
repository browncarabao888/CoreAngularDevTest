using System;
using System.Collections.Generic;

namespace CoreAngularDevTest.Server.Models;

public partial class Category
{
    public int? CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public int Id { get; set; }

    public int? Isactive { get; set; }
}
