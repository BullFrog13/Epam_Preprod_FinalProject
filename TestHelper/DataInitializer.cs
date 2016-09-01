using Courses.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestHelper
{
    public class DataInitializer
    {
        public static List<Specialization> GetAllSpecializations()
        {
            var specializations = new List<Specialization>
            {
                new Specialization() {Name = "test1", Description = "test1", Courses = new List<Course>()},
                new Specialization() {Name = "test2", Description = "test2", Courses = new List<Course>()},
                new Specialization() {Name = "test3", Description = "test3", Courses = new List<Course>()},
                new Specialization() {Name = "test4", Description = "test4", Courses = new List<Course>()}
            };
            return specializations;
        }

        public static List<Course> GetAllCourses()
        {
            var courses = new List<Course>()
            {
                new Course()
                {
                    Description = "test",
                    EndDate = DateTime.Now,
                    Specialization = new Specialization(),
                    StartDate = DateTime.Now,
                    Name = "test",
                    TutorId = "fdsfsdfsdf"
                }
            };
            return courses;
        }

        public static List<Journal> GetAJournals()
        {
            var journals = new List<Journal>()
            {
                new Journal() {Mark = 0, StudentId = "dfs", CourseId = 1},
                new Journal() {Mark = 0, StudentId = "dfs", CourseId = 2}
            };
            return journals;
        }
    }
}
