using System;
using System.Collections.Generic;

namespace CoreAngularDevTest.Server.Models;

public partial class Account
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Emailaddress { get; set; }

    public string? Passkey { get; set; }

    public string? UserName { get; set; }

    public int? Status { get; set; }
}
