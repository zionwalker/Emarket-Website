using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Emarket_Website.Models;

public partial class Payment
{
    [Key]
    public int Id { get; set; }

    public int PayTypetId { get; set; }

    public int OrderId { get; set; }

    public int TransactionNumber { get; set; }

    [StringLength(100)]
    public string FileUpload { get; set; } = null!;

    [Column(TypeName = "date")]
    public DateTime Date { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("Payments")]
    public virtual Order Order { get; set; } = null!;

    [ForeignKey("PayTypetId")]
    [InverseProperty("Payments")]
    public virtual PaymentType PayTypet { get; set; } = null!;
}
