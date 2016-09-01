using Courses.Models.EntityViewModels;

namespace Courses.Models.StudentModels
{
    public class CourseMarkStudentViewModel
    {
        public CourseViewModel Course { get; set; }

        public int Mark { get; set; }
    }
}