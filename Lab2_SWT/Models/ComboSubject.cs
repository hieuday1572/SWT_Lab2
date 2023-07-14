using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMMProject.Models;

public partial class ComboSubject
{
    [Key]
    public int id { get; set; }

    [ForeignKey("Combo")]
    public int ComboId { get; set; }

    public Combo Combo { get; set; }

    
    [ForeignKey("Subject")]
    public string SubjectCode { get; set; }

    public Subject Subject { get; set; }


}
