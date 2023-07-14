using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMMProject.Models;

public partial class MaterialOfTeacher
{
    [Key]
    public int Id { get; set; }

    public string? Description { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the Teacher Username")]
    [ForeignKey("Account")]
    public string TeacherUsername { get; set; }

    public virtual Account Account { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the Subject")]
    [ForeignKey("Subject")]
    public string SubjectCode { get; set; }

    public Subject Subject { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the Url")]
    public string URL { get; set; }

    public string Status { get; set; }
}
