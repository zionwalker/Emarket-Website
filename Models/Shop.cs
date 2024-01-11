using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Emarket_Website.Models;

public partial class Shop
{
    [Key]
    public int Id { get; set; }

    [StringLength(30)]
    public string Name { get; set; } = null!;

    [StringLength(50)]
    public string Address { get; set; } = null!;

    public int UserId { get; set; }

    public int ShopCatId { get; set; }

    public int CityId { get; set; }

    [StringLength(100)]
    public string? Url { get; set; }

    [InverseProperty("Shop")]
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    [ForeignKey("CityId")]
    [InverseProperty("Shops")]
    public virtual City City { get; set; } = null!;

    [InverseProperty("Shop")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [InverseProperty("Shop")]
    public virtual ICollection<ReviewShop> ReviewShops { get; set; } = new List<ReviewShop>();

    [ForeignKey("ShopCatId")]
    [InverseProperty("Shops")]
    public virtual ShopCategory ShopCat { get; set; } = null!;

    [InverseProperty("Shop")]
    public virtual ICollection<ShopEntry> ShopEntries { get; set; } = new List<ShopEntry>();

    [InverseProperty("Shop")]
    public virtual ICollection<ShopKeeper> ShopKeepers { get; set; } = new List<ShopKeeper>();

    [ForeignKey("UserId")]
    [InverseProperty("Shops")]
    public virtual User User { get; set; } = null!;
}
