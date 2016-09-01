using Courses.DAL.EF;
using Courses.DAL.Entities;
using Courses.DAL.Interfaces;
using System;

namespace Courses.DAL.Repositories
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly CoursesContext _db;
        private CourseRepository _coursesRepository;
        private SpecializationRepository _specializationsRepository;
        private JournalRepository _journalsRepository;

        public EfUnitOfWork()
        {
            _db = new CoursesContext();
        }

        public IRepository<Course> Courses
        {
            get
            {
                if(_coursesRepository == null)
                {
                    _coursesRepository = new CourseRepository(_db);
                }
                return _coursesRepository;
            }
        }

        public IRepository<Specialization> Specializations
        {
            get
            {
                if(_specializationsRepository == null)
                {
                    _specializationsRepository = new SpecializationRepository(_db);
                }
                return _specializationsRepository;
            }
        }

        public IRepository<Journal> Journals
        {
            get
            {
                if (_journalsRepository == null)
                {
                    _journalsRepository = new JournalRepository(_db);
                }
                return _journalsRepository;
            }
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        private bool _disposed;

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
