using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Emarket_Website.Models;

public partial class ItemImage
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Url { get; set; } = null!;

    public int ItemId { get; set; }

    [ForeignKey("ItemId")]
    [InverseProperty("ItemImages")]
    public virtual Item Item { get; set; } = null!;
}
