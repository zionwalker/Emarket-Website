using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Emarket_Website.Models;

public partial class PaymentType
{
    [Key]
    public int Id { get; set; }

    [Column("TName")]
    [StringLength(50)]
    public string Tname { get; set; } = null!;

    [InverseProperty("PayTypet")]
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
