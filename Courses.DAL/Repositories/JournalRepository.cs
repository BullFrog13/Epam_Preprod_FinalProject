using Courses.DAL.EF;
using Courses.DAL.Entities;
using Courses.DAL.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Courses.DAL.Repositories
{
    public class JournalRepository : IRepository<Journal>
    {
        private readonly CoursesContext _db;

        public JournalRepository(CoursesContext context)
        {
            _db = context;
        }

        public List<Journal> GetAll()
        {
            return _db.Journals.ToList();
        }

        public Journal Get(int id)
        {
            return _db.Journals.Find(id);
        }

        public void Create(Journal journal)
        {
            _db.Journals.Add(journal);
            _db.SaveChanges();
        }

        public void Update(Journal journal)
        {
            _db.Entry(journal).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            Journal journal = _db.Journals.Find(id);
            if (journal != null)
            {
                _db.Journals.Remove(journal);
                _db.SaveChanges();
            }
        }
    }
}
