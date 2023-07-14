using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMMProject.Models;

public partial class Combo
{
    [Key]
    public int ComboId { get; set; }

    public string? ComboNameVn { get; set; }

    public string? ComboNameEn { get; set; }

    public string? Note { get; set; }

    public string? Tag { get; set; }

    [ForeignKey("Curriculum")]
    public int? CurriculumId { get; set; }

    public virtual Curriculum? Curriculum { get; set; }

}
