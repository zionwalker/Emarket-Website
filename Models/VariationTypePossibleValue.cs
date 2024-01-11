using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Emarket_Website.Models;

public partial class VariationTypePossibleValue
{
    [Key]
    public int Id { get; set; }

    public int VarId { get; set; }

    [StringLength(50)]
    public string Value { get; set; } = null!;

    [InverseProperty("VarOpt")]
    public virtual ICollection<ItemConfiguration> ItemConfigurations { get; set; } = new List<ItemConfiguration>();

    [ForeignKey("VarId")]
    [InverseProperty("VariationTypePossibleValues")]
    public virtual VariationType Var { get; set; } = null!;
}
