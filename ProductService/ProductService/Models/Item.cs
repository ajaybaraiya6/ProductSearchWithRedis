using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductService.Models;

public partial class Item
{
    public int ItemId { get; set; }

    public string ItemCode { get; set; } = null!;

    public int? LabId { get; set; }

    public int? ColorId { get; set; }

    public int? TypeId { get; set; }

    public int? EligibilityId { get; set; }

    public int? ClarityId { get; set; }

    public int? CutId { get; set; }

    public int? PolishId { get; set; }

    public int? SymmetryId { get; set; }

    public int? FluorescenceId { get; set; }

    public int? LocationId { get; set; }

    public double BasePrice { get; set; }

    public double? DiscountPercent { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public double? FinalPrice { get; set; }

    public virtual Clarity? Clarity { get; set; }

    public virtual Color? Color { get; set; }

    public virtual Cut? Cut { get; set; }

    public virtual Eligibility? Eligibility { get; set; }

    public virtual Fluorescence? Fluorescence { get; set; }

    public virtual Lab? Lab { get; set; }

    public virtual Location? Location { get; set; }

    public virtual Polish? Polish { get; set; }

    public virtual Symmetry? Symmetry { get; set; }

    public virtual Type? Type { get; set; }
}
