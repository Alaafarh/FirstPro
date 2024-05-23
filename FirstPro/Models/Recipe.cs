using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstPro.Models;

public partial class Recipe
{
    public decimal Recipeid { get; set; }

    public string? Recipename { get; set; }

    public decimal? Price { get; set; }

    public decimal? Flag { get; set; }

    public string? Imagepath { get; set; }
    [NotMapped]
    public IFormFile ImageFile { get; set; }

    public decimal? Userid { get; set; }

    public decimal? Categoryid { get; set; }

    public DateTime? Creatdate { get; set; }

    public string? Discription { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<Orderrecipe> Orderrecipes { get; set; } = new List<Orderrecipe>();

    public virtual User? User { get; set; }

    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
}
