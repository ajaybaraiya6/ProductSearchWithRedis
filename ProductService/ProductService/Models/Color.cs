using System;
using System.Collections.Generic;

namespace ProductService.Models;

public partial class Color
{
    public int ColorId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
