using Courses.DAL.Entities;
using System.Data.Entity;

namespace Courses.DAL.EF
{
    public class CoursesContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Journal> Journals { get; set; }

        static CoursesContext()
        {
            Database.SetInitializer(new DbInit());
        }

        public CoursesContext() : base("ConnectionDB") { }
    }
}
