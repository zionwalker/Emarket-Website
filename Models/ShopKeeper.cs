using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Emarket_Website.Models;

public partial class ShopKeeper
{
    [Key]
    public int Id { get; set; }

    public int ShopId { get; set; }

    public int UserId { get; set; }

    [StringLength(30)]
    public string? AssignedBy { get; set; }

    [Column(TypeName = "date")]
    public DateTime AssignDate { get; set; }

    [ForeignKey("ShopId")]
    [InverseProperty("ShopKeepers")]
    public virtual Shop Shop { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("ShopKeepers")]
    public virtual User User { get; set; } = null!;
}
