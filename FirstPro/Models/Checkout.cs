using System;
using System.Collections.Generic;

namespace FirstPro.Models;

public partial class Checkout
{
    public decimal Checkoutid { get; set; }

    public string? Fname { get; set; }

    public string? Lname { get; set; }

    public string? Phonenum { get; set; }

    public decimal Userid { get; set; }

    public virtual User User { get; set; } = null!;
}
