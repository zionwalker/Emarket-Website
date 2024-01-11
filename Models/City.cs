using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Emarket_Website.Models;

public partial class City
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [InverseProperty("City")]
    public virtual ICollection<Shop> Shops { get; set; } = new List<Shop>();

    [InverseProperty("City")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
