using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Emarket_Website.Models;

public partial class ReviewShop
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ShopId { get; set; }

    public int RatingValue { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime RatedDate { get; set; }

    [StringLength(200)]
    public string Comment { get; set; } = null!;

    [ForeignKey("ShopId")]
    [InverseProperty("ReviewShops")]
    public virtual Shop Shop { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("ReviewShops")]
    public virtual User User { get; set; } = null!;
}
