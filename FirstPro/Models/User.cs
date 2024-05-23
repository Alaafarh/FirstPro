using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstPro.Models;

public partial class User
{
    public decimal UserId { get; set; }
    
    public string? Fname { get; set; }

    public string? Lname { get; set; }

    public string? Imagepath { get; set; }
    [NotMapped]
    public IFormFile imagefile { get; set; }

    public string? Jop { get; set; }

    public decimal? Age { get; set; }

    public string? Adress { get; set; }

    public decimal? Roleid { get; set; }

    public decimal? Flagchif { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Checkout> Checkouts { get; set; } = new List<Checkout>();

    public virtual ICollection<Orderrecipe> Orderrecipes { get; set; } = new List<Orderrecipe>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<Testimonial> Testimonials { get; set; } = new List<Testimonial>();

    public virtual ICollection<Userlogin> Userlogins { get; set; } = new List<Userlogin>();

    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
}
