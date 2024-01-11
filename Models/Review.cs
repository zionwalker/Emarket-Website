using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Emarket_Website.Models;

public partial class Review
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }

    public int RatingValue { get; set; }

    [StringLength(500)]
    public string Comment { get; set; } = null!;

    public int ItemId { get; set; }

    [ForeignKey("ItemId")]
    [InverseProperty("Reviews")]
    public virtual Item Item { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Reviews")]
    public virtual User User { get; set; } = null!;
}
