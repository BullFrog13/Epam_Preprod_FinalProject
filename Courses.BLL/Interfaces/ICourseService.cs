using Courses.BLL.DTO;
using System.Collections.Generic;

namespace Courses.BLL.Interfaces
{
    public interface ICourseService
    {
        void Create(CourseDto item, int specializationId, string tutorId);
        CourseDto GetCourse(int? id);
        List<CourseDto> GetCourses();
        void UpdateCourse(CourseDto course);
        void DeleteCourse(int? id);
        void AddSubscriber(int? id);
        void DeleteSubscriber(int? id);
        void Dispose();
    }
}
