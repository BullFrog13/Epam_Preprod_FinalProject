using Courses.DAL.EF;
using Courses.DAL.Entities;
using Courses.DAL.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Courses.DAL.Repositories
{
    public class SpecializationRepository : IRepository<Specialization>
    {
        private readonly CoursesContext _db;

        public SpecializationRepository(CoursesContext context)
        {
            _db = context;
        }

        public List<Specialization> GetAll()
        {
            var res = _db.Specializations.Include(x => x.Courses).ToList();
            return res;
        }

        public Specialization Get(int id)
        {
            return _db.Specializations.Include(x => x.Courses).ToList().First(x => x.Id.Equals(id));
        }

        public void Create(Specialization specialization)
        {
            _db.Specializations.Add(specialization);
            _db.SaveChanges();
        }

        public void Update(Specialization specialization)
        {
            _db.Entry(specialization).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            Specialization specialization = _db.Specializations.Find(id);
            if (specialization != null)
            {
                _db.Specializations.Remove(specialization);
                _db.SaveChanges();
            }
        }
    }
}
