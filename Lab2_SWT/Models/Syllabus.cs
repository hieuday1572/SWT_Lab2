using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMMProject.Models;

public partial class Syllabus
{
    [Key]
    public int SyllabusId { get; set; }

    public string? SyllabusNameVn { get; set; }

    public string? SyllabusNameEn { get; set; }
    [ForeignKey("Subject")]

    public string? SubjectCode { get; set; }

    public virtual Subject? Subject { get; set; }
    public int? NoCredit { get; set; }

    public string? DegreeLevel { get; set; }

    public string? TimeAllocation { get; set; }

    public string? PreRequisite { get; set; }

    public string? Description { get; set; }

    public string? StudentTask { get; set; }

    public string? Tool { get; set; }

    public string? ScoringScale { get; set; }
    [ForeignKey("Decision")]
    public string? DecisionNo { get; set; }

    public virtual Decision? Decision { get; set; }

    public sbyte? IsApproved { get; set; }

    public string? Note { get; set; }

    public int? MinAvgMarkToPass { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the active")]
    public sbyte? IsActive { get; set; }

    public virtual ICollection<Assessment> Assessments { get; set; }

   
}
