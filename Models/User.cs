using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Emarket_Website.Models;

public partial class User
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [StringLength(30)]
    public string Email { get; set; } = null!;

    [StringLength(100)]
    public string Password { get; set; } = null!;

    [Required]
    public bool? IsActive { get; set; }

    public int? CityId { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    [ForeignKey("CityId")]
    [InverseProperty("Users")]
    public virtual City? City { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [InverseProperty("User")]
    public virtual ICollection<ReviewShop> ReviewShops { get; set; } = new List<ReviewShop>();

    [InverseProperty("User")]
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    [InverseProperty("ApprovedByNavigation")]
    public virtual ICollection<SellerApplication> SellerApplicationApprovedByNavigations { get; set; } = new List<SellerApplication>();

    [InverseProperty("User")]
    public virtual ICollection<SellerApplication> SellerApplicationUsers { get; set; } = new List<SellerApplication>();

    [InverseProperty("User")]
    public virtual ICollection<ShopKeeper> ShopKeepers { get; set; } = new List<ShopKeeper>();

    [InverseProperty("User")]
    public virtual ICollection<Shop> Shops { get; set; } = new List<Shop>();

    [InverseProperty("User")]
    public virtual ICollection<UserAddress> UserAddresses { get; set; } = new List<UserAddress>();

    [InverseProperty("User")]
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
