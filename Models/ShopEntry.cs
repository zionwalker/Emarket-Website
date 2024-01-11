using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Emarket_Website.Models;

public partial class ShopEntry
{
    [Key]
    public int Id { get; set; }

    public int ShopId { get; set; }

    public int ItemId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime EntryDate { get; set; }

    public double Quantity { get; set; }

    [ForeignKey("ItemId")]
    [InverseProperty("ShopEntries")]
    public virtual Item Item { get; set; } = null!;

    [ForeignKey("ShopId")]
    [InverseProperty("ShopEntries")]
    public virtual Shop Shop { get; set; } = null!;

    [InverseProperty("ShopEntry")]
    public virtual ICollection<ShopEntryVariation> ShopEntryVariations { get; set; } = new List<ShopEntryVariation>();

    [InverseProperty("ShopEntry")]
    public virtual ICollection<SoldItem> SoldItems { get; set; } = new List<SoldItem>();
}
