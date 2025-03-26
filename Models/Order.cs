using System;
using System.Collections.Generic;

namespace TheCoffeeShop.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? CartId { get; set; }

    public DateTime? OrderDate { get; set; }

    public int? AccountId { get; set; }

    public virtual Account? Account { get; set; }

    public virtual Cart? Cart { get; set; }
}
