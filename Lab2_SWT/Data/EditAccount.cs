using LMMProject.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace LMMProject.Data
{
    public class EditAccount
    {
        public string UserName { get; set; } = null!;

        public IFormFile Image { get; set; }

        public long? Phone { get; set; }

        public string? Address { get; set; }

        public int? Gender { get; set; }

        public string? Gmail { get; set; }

        public string? Fullname { get; set; }

        public DateTime? Birthday { get; set; }

        public int? RoleId { get; set; }

        public sbyte? Active { get; set; }

        public virtual Role? Role { get; set; }

        public string Url { get; set; }
    }
}
