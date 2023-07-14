using LMMProject.Models;
using Microsoft.EntityFrameworkCore;

namespace LMMProject.Data
{
    //Scaffold-DbContext "server=localhost;user=root;database=flm;password=12345;port=3306" MySql.EntityFrameworkCore -OutputDir flm -Tables thich them table nao thi tu them -f
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public virtual DbSet<Account> Account { get; set; }

        public virtual DbSet<Role> Role { get; set; }

        public virtual DbSet<Combo> Combo { get; set; }

        public virtual DbSet<Curriculum> Curriculum { get; set; }

        public virtual DbSet<CurriculumSubject> Curriculum_Subject { get; set; }

        public virtual DbSet<Decision> Decision { get; set; }

        public virtual DbSet<Status> Status { get; set; }

        public virtual DbSet<Subject> Subject { get; set; }

        public virtual DbSet<ComboSubject> Combo_Subject { get; set; }

        public virtual DbSet<MaterialOfTeacher> MaterialOfTeacher { get; set; }

        public virtual DbSet<Syllabus> Syllabus { get; set; }
        public virtual DbSet<Assessment> Assessment { get; set; }

        public virtual DbSet<Feedback> feedback { get; set; }
        public virtual DbSet<Material> Material { get; set; }

        public DbSet<LMMProject.Models.Session>? Session { get; set; }
    }
}
