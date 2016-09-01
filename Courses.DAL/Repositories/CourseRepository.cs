using Courses.DAL.EF;
using Courses.DAL.Entities;
using Courses.DAL.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Courses.DAL.Repositories
{
    public class CourseRepository : IRepository<Course>
    {
        private readonly CoursesContext _db;

        public CourseRepository(CoursesContext context)
        {
            _db = context;
        }

        public List<Course> GetAll()
        {
            return _db.Courses.Include(x => x.Specialization).ToList();
        }

        public Course Get(int id)
        {
            return _db.Courses.Include(x => x.Specialization).First(x => x.Id.Equals(id));
        }

        public void Create(Course course)
        {
            _db.Courses.Add(course);
            _db.SaveChanges();
        }

        public void Update(Course course)
        {
            _db.Entry(course).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            Course course = _db.Courses.Find(id);
            if(course != null)
            {
                _db.Courses.Remove(course);
                _db.SaveChanges();
            }
        }
    }
}
