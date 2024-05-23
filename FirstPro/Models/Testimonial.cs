using System;
using System.Collections.Generic;

namespace FirstPro.Models;

public partial class Testimonial
{
    public decimal Testimonialid { get; set; }

    public decimal? Flag { get; set; }

    public decimal? Userid { get; set; }

    public string? CommentUser { get; set; }

    public virtual User? User { get; set; }
}
