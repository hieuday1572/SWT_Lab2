using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMMProject.Models;

public partial class Status
{
    [Key]
    public sbyte StatusId { get; set; }

    public string? StatusName { get; set; }

}
