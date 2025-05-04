using System;
using System.Collections.Generic;

namespace ProductService.Models;

public partial class Clarity
{
    public int ClarityId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
