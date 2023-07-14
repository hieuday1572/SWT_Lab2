using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMMProject.Models;

public partial class Role
{
    [Key]
    public int RoleId { get; set; }

    public string? RoleName { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
