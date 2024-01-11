using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Emarket_Website.Models;

public partial class Order
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime OrderDate { get; set; }

    public int OrderStatId { get; set; }

    public double TotalPrice { get; set; }

    public int ShopId { get; set; }

    [ForeignKey("OrderStatId")]
    [InverseProperty("Orders")]
    public virtual OrderStatuss OrderStat { get; set; } = null!;

    [InverseProperty("Order")]
    public virtual ICollection<Ordereditem> Ordereditems { get; set; } = new List<Ordereditem>();

    [InverseProperty("Order")]
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    [ForeignKey("ShopId")]
    [InverseProperty("Orders")]
    public virtual Shop Shop { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Orders")]
    public virtual User User { get; set; } = null!;
}
