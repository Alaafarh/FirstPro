using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstPro.Models;

public partial class Aboutu
{
    public decimal Aboutus { get; set; }

    public string? Imageback { get; set; }

    public string? Textinsideimage { get; set; }

    [NotMapped]
    public IFormFile ImageFile { get; set; }
    public string? Imageform { get; set; }

    public string? About { get; set; }

    public string? Alltext { get; set; }
}
