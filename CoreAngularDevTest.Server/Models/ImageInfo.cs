using System;
using System.Collections.Generic;

namespace CoreAngularDevTest.Server.Models;

public partial class ImageInfo
{
    internal string? filename;

    public int? Id { get; set; }

    public decimal? PhotoId { get; set; }

    public byte[]? Imagedata { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Location { get; set; }

    public string? Region { get; set; }

    public string? Owner { get; set; }

    public string? Ownerrealname { get; set; }

    public string? Country { get; set; }

    public int? Deleted { get; set; }
}
