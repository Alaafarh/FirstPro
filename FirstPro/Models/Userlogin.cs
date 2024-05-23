using System;
using System.Collections.Generic;

namespace FirstPro.Models;

public partial class Userlogin
{
    public decimal UserloginId { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public decimal? Roleid { get; set; }

    public decimal? Userid { get; set; }

    public virtual Role? Role { get; set; }

    public virtual User? User { get; set; }
}
