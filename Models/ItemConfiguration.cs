using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Emarket_Website.Models;

public partial class ItemConfiguration
{
    [Key]
    public int Id { get; set; }

    public int VarOptId { get; set; }

    public int ItemId { get; set; }

    [ForeignKey("ItemId")]
    [InverseProperty("ItemConfigurations")]
    public virtual Item Item { get; set; } = null!;

    [ForeignKey("VarOptId")]
    [InverseProperty("ItemConfigurations")]
    public virtual VariationTypePossibleValue VarOpt { get; set; } = null!;
}
