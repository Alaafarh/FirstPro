using System;
using System.Collections.Generic;

namespace FirstPro.Models;

public partial class Role
{
    public decimal Roleid { get; set; }

    public string? Rolename { get; set; }

    public virtual ICollection<Userlogin> Userlogins { get; set; } = new List<Userlogin>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
