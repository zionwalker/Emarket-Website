using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Emarket_Website.Models;

public partial class OrderStatuss
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string Status { get; set; } = null!;

    [InverseProperty("OrderStat")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
