using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Emarket_Website.Models;

[Table("ApplicationStatus")]
public partial class ApplicationStatus
{
    [Key]
    public int Id { get; set; }

    [Column("ApplicationStatus")]
    [StringLength(50)]
    public string? ApplicationStatus1 { get; set; }

    [InverseProperty("ApplicationStatus")]
    public virtual ICollection<SellerApplication> SellerApplications { get; set; } = new List<SellerApplication>();
}
