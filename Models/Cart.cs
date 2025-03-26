using System;
using System.Collections.Generic;

namespace TheCoffeeShop.Models;

public partial class Cart
{
    public int CartId { get; set; }

    public int? AccountId { get; set; }

    public int? ProductId { get; set; }

    public decimal? TotalPrice { get; set; }

    public int? Quality { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Product? Product { get; set; }
}
