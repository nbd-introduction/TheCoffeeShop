using System;
using System.Collections.Generic;

namespace TheCoffeeShop.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string UserName { get; set; } = null!;

    public string PassWord { get; set; } = null!;

    public int Role { get; set; }

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Adress { get; set; }

    public string? FullName { get; set; }

    public bool? IsBan { get; set; }

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
