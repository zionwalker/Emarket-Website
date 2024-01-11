using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Emarket_Website.Models;

public partial class VariationType
{
    [Key]
    public int Id { get; set; }

    public int ItemCatId { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [ForeignKey("ItemCatId")]
    [InverseProperty("VariationTypes")]
    public virtual ItemCategory ItemCat { get; set; } = null!;

    [InverseProperty("Var")]
    public virtual ICollection<ShopEntryVariation> ShopEntryVariations { get; set; } = new List<ShopEntryVariation>();

    [InverseProperty("Var")]
    public virtual ICollection<VariationTypePossibleValue> VariationTypePossibleValues { get; set; } = new List<VariationTypePossibleValue>();
}
