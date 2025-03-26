using System;
using System.Collections.Generic;

namespace TheCoffeeShop.Models;

public partial class Blog
{
    public int BlogId { get; set; }

    public string BlogName { get; set; } = null!;

    public DateTime? BlogDate { get; set; }

    public string? BlogContent { get; set; }

    public string? BlogImage { get; set; }

    public int? AccountId { get; set; }

    public int? ProductId { get; set; }

    public virtual Account? Account { get; set; }

    public virtual Product? Product { get; set; }
}
