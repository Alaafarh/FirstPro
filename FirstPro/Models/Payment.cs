using System;
using System.Collections.Generic;

namespace FirstPro.Models;

public partial class Payment
{
    public decimal Payment1 { get; set; }

    public string? Nameoncard { get; set; }

    public decimal? Expyear { get; set; }

    public decimal? Expmonth { get; set; }

    public decimal? Amount { get; set; }

    public decimal? Cvv { get; set; }

    public string? Cardnum { get; set; }

    public decimal Userid { get; set; }

    public virtual User User { get; set; } = null!;
}
