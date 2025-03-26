using System;
using System.Collections.Generic;

namespace TheCoffeeShop.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal ProductPrice { get; set; }

    public string? Description { get; set; }

    public string? Size { get; set; }

    public string? ProductImage { get; set; }

    public int? CategoryId { get; set; }

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual Category? Category { get; set; }
}
