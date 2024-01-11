using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Emarket_Website.Models;

public partial class SellerApplication
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string Address { get; set; } = null!;

    public int NumberOfShops { get; set; }

    [StringLength(50)]
    public string Description { get; set; } = null!;

    [StringLength(100)]
    public string Url { get; set; } = null!;

    public int UserId { get; set; }

    public int? ApprovedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? AcceptedDate { get; set; }

    public int ApplicationStatusId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? RequestedDate { get; set; }

    [ForeignKey("ApplicationStatusId")]
    [InverseProperty("SellerApplications")]
    public virtual ApplicationStatus ApplicationStatus { get; set; } = null!;

    [ForeignKey("ApprovedBy")]
    [InverseProperty("SellerApplicationApprovedByNavigations")]
    public virtual User? ApprovedByNavigation { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("SellerApplicationUsers")]
    public virtual User User { get; set; } = null!;
}
