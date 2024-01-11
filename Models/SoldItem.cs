using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Emarket_Website.Models;

public partial class SoldItem
{
    [Key]
    public int Id { get; set; }

    public double Quantity { get; set; }

    public double Price { get; set; }

    public int ShopEntryId { get; set; }

    [ForeignKey("ShopEntryId")]
    [InverseProperty("SoldItems")]
    public virtual ShopEntry ShopEntry { get; set; } = null!;
}
