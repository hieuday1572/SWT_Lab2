using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace LMMProject.Models;

public partial class Account
{
    [Display(Name = "Username")]
    [Required(ErrorMessage = "User is required!")]
    [Key]
    public string UserName { get; set; } = null!;

    [Required(ErrorMessage = "Password is required!")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    public string? Image { get; set; }

    public long? Phone { get; set; }

    public string? Address { get; set; }

    public int? Gender { get; set; }

    public string? Gmail { get; set; }

    public string? Fullname { get; set; }

    public DateTime? Birthday { get; set; }

    public int? RoleId { get; set; }

    public sbyte? Active { get; set; }

    public virtual Role? Role { get; set; }
    
}
