using AutoMapper;
using Courses.BLL.DTO;
using Courses.BLL.Interfaces;
using Courses.DAL.Entities;
using Courses.DAL.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Courses.BLL.Services
{
    public class JournalService : IJournalService
    {
        private IUnitOfWork Database { get; }

        public JournalService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public void Create(string studentId, int? courseId)
        {
            if(studentId == null)
            {
                throw new ValidationException("No such student id");
            }
            if (courseId == null)
            {
                throw new ValidationException("No such course id");
            }

            Journal journal = new Journal
            {
                CourseId = courseId.Value,
                StudentId = studentId,
            };
            Database.Journals.Create(journal);
            Database.Save();
        }

        public List<JournalDto> GetJournals()
        {
            IMapper mapper = MapJournal();
            var res = mapper.Map<List<Journal>, List<JournalDto>>(Database.Journals.GetAll());
            return res;
        }

        public JournalDto GetJournal(int? id)
        {
            if (id == null)
            {
                throw new ValidationException("No such post id");
            }

            var journal = Database.Journals.Get(id.Value);
            if (journal == null)
            {
                throw new ValidationException("Question was not found");
            }

            var mapper = MapJournal();

            return mapper.Map<Journal, JournalDto>(journal);
        }

        public void SetMark(int? journalId, int mark)
        {
            if (journalId != null)
            {
                var journal = Database.Journals.Get(journalId.Value);
                journal.Mark = mark;
            }
            Database.Save();
        }

        public bool StudentJournalExist(string studentId, int? courseId)
        {
            if(courseId == null)
            {
                throw new ValidationException("No such student id");
            }
            var journals = Database.Journals.GetAll();
            foreach(Journal journal in journals)
            {
                if(journal.StudentId == studentId && journal.CourseId == courseId)
                {
                    return true;
                }
            }
            return false;
        }

        public void UpdateJournal(JournalDto journal)
        {
            var dbJournal = Database.Journals.Get(journal.Id);
            dbJournal.Mark = journal.Mark;

            Database.Journals.Update(dbJournal);
        }

        public void DeleteJournal(int? id)
        {
            if (id == null)
            {
                throw new ValidationException("No such course id");
            }
            var journal = Database.Journals.Get(id.Value);
            if (journal == null)
            {
                throw new ValidationException("Course was not found");
            }
            Database.Journals.Delete(id.Value);
            Database.Save();
        }

        public JournalDto GetJournalForAUser(string userId, int courseId)
        {

            var mapper = MapJournal();
            var journal =  Database.Journals.GetAll().Where(c => c.CourseId.Equals(courseId)).SingleOrDefault(u => u.StudentId.Equals(userId));
            //Database.Journals.GetAll().Where(c => c.CourseId.Equals(courseId)).Where(u => u.StudentId.Equals(userId)).SingleOrDefault();
            return mapper.Map<Journal, JournalDto>(journal);
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        private IMapper MapJournal()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Journal, JournalDto>();
            });
            IMapper mapper = config.CreateMapper();
            return mapper;
        }
    }
}
