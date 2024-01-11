using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Emarket_Website.Models;

public partial class CartItem
{
    [Key]
    public int Id { get; set; }

    public int ItemId { get; set; }

    public double Quantitiy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime AddedDate { get; set; }

    public int CartItemStatusId { get; set; }

    public int UserId { get; set; }

    public int ShopId { get; set; }

    [ForeignKey("CartItemStatusId")]
    [InverseProperty("CartItems")]
    public virtual CartItemStatuss CartItemStatus { get; set; } = null!;

    [ForeignKey("ItemId")]
    [InverseProperty("CartItems")]
    public virtual Item Item { get; set; } = null!;

    [ForeignKey("ShopId")]
    [InverseProperty("CartItems")]
    public virtual Shop Shop { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("CartItems")]
    public virtual User User { get; set; } = null!;
}
