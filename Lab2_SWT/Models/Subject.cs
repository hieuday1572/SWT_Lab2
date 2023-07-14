using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMMProject.Models;

public partial class Subject
{
    [Key]
    public string SubjectCode { get; set; }

    public string? SubjectNameVn { get; set; }

    public string? SubjectNameEn { get; set; }

    public string? PreRequisite { get; set; }

    [ForeignKey("Status")]
    public sbyte StatusId { get; set; }

    public virtual Status Status { get; set; }

}
