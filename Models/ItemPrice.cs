using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Emarket_Website.Models;

public partial class ItemPrice
{
    [Key]
    public int Id { get; set; }

    public double Price { get; set; }

    public int ItemId { get; set; }

    [ForeignKey("ItemId")]
    [InverseProperty("ItemPrices")]
    public virtual Item Item { get; set; } = null!;
}
