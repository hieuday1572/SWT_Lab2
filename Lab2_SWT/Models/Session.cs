using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMMProject.Models;

public partial class Session
{
    [Key]
    public int SessionId { get; set; }

    public string Topic { get; set; }

    public string? LearningTeachingType { get; set; }

    public string? StudentMaterials { get; set; }

    public string? Constructivequestion { get; set; }

    [ForeignKey("Subject")]
    public string? SubjectCode { get; set; }

    public virtual Subject? Subject { get; set; }
}
