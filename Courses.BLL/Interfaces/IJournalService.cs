using Courses.BLL.DTO;
using System.Collections.Generic;

namespace Courses.BLL.Interfaces
{
    public interface IJournalService
    {
        void Create(string studentId, int? courseId);
        bool StudentJournalExist(string studentid, int? courseId);
        JournalDto GetJournal(int? id);
        List<JournalDto> GetJournals();
        void DeleteJournal(int? id);
        void UpdateJournal(JournalDto journal);
        void SetMark(int? id, int mark);
        JournalDto GetJournalForAUser(string userId, int courseId);
        void Dispose();
    }
}
