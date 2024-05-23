using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FirstPro.Models;

public partial class Orderrecipe
{
    public decimal Orderrecipe1 { get; set; }

    public decimal? Userid { get; set; }

    public decimal? Recipeid { get; set; }
    [Required]
    public DateTime? Shopdate { get; set; }

    public decimal? Totalprice { get; set; }

    public virtual Recipe? Recipe { get; set; }

    public virtual User? User { get; set; }
}
