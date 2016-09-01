using System.Data.Entity;

namespace Courses.DAL.EF
{
    public class DbInit : DropCreateDatabaseIfModelChanges<CoursesContext>
    {
        protected override void Seed(CoursesContext context)
        {          

        }
    }
}
