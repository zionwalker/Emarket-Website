using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Emarket_Website.Models;

public partial class Item
{
    [Key]
    public int Id { get; set; }

    public int ItemCatId { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [StringLength(150)]
    public string Descripition { get; set; } = null!;

    public double Quantity { get; set; }

    [InverseProperty("Item")]
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    [ForeignKey("ItemCatId")]
    [InverseProperty("Items")]
    public virtual ItemCategory ItemCat { get; set; } = null!;

    [InverseProperty("Item")]
    public virtual ICollection<ItemConfiguration> ItemConfigurations { get; set; } = new List<ItemConfiguration>();

    [InverseProperty("Item")]
    public virtual ICollection<ItemImage> ItemImages { get; set; } = new List<ItemImage>();

    [InverseProperty("Item")]
    public virtual ICollection<ItemPrice> ItemPrices { get; set; } = new List<ItemPrice>();

    [InverseProperty("Item")]
    public virtual ICollection<Ordereditem> Ordereditems { get; set; } = new List<Ordereditem>();

    [InverseProperty("Item")]
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    [InverseProperty("Item")]
    public virtual ICollection<ShopEntry> ShopEntries { get; set; } = new List<ShopEntry>();
}
