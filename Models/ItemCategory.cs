using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Emarket_Website.Models;

public partial class ItemCategory
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [InverseProperty("ItemCat")]
    public virtual ICollection<Item> Items { get; set; } = new List<Item>();

    [InverseProperty("ItemCat")]
    public virtual ICollection<VariationType> VariationTypes { get; set; } = new List<VariationType>();
}
