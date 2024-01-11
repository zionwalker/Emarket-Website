using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Emarket_Website.Models;

public partial class Ordereditem
{
    [Key]
    public int Id { get; set; }

    public int ItemId { get; set; }

    public int OrderId { get; set; }

    public double Quantity { get; set; }

    public double SubTotalPrice { get; set; }

    public double UnitPrice { get; set; }

    [ForeignKey("ItemId")]
    [InverseProperty("Ordereditems")]
    public virtual Item Item { get; set; } = null!;

    [ForeignKey("OrderId")]
    [InverseProperty("Ordereditems")]
    public virtual Order Order { get; set; } = null!;
}
