using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMMProject.Models;

public partial class CurriculumSubject
{
    [Key]
    public int id { get; set; }

    [ForeignKey("Curriculum")]
    public int CurriculumId { get; set; }
    public virtual Curriculum Curriculum { get; set; }


    [ForeignKey("Subject")]
    public string SubjectCode { get; set; }
    public virtual Subject Subject { get; set; }

    public int? Semester { get; set; }

    

    
}
