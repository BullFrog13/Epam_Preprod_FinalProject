using Courses.DAL.Entities;
using System;

namespace Courses.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Course> Courses { get; }
        IRepository<Specialization> Specializations { get; }
        IRepository<Journal> Journals { get; }
        void Save();
    }
}
