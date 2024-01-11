using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Emarket_Website.Models;

public partial class ShopEntryVariation
{
    [Key]
    public int Id { get; set; }

    public int ShopEntryId { get; set; }

    public int VarId { get; set; }

    [ForeignKey("ShopEntryId")]
    [InverseProperty("ShopEntryVariations")]
    public virtual ShopEntry ShopEntry { get; set; } = null!;

    [ForeignKey("VarId")]
    [InverseProperty("ShopEntryVariations")]
    public virtual VariationType Var { get; set; } = null!;
}
