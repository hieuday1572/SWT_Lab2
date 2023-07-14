using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMMProject.Models;

public partial class Assessment
{
    [Key]
    public int AssessmentId { get; set; }

    public string? Category { get; set; }

    public string? Type { get; set; }

    public int? Part { get; set; }

    public string? Weight { get; set; }

    public string? CompletionCriteria { get; set; }

    public string? Duration { get; set; }

    public string Clo { get; set; } = null!;

    public string? QuestionType { get; set; }

    public string? NoQuestion { get; set; }

    public string? KnowledgeSkill { get; set; }

    public string? GradingGuide { get; set; }

    public string? Note { get; set; }
    [ForeignKey("Syllabus")]
    public int? SyllabusId { get; set; }
    public virtual Syllabus? Syllabus { get; set; }
}
