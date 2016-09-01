using Courses.Models.EntityViewModels;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Courses.Models.TutorModels
{
    public class JournalWrapModel
    {
        public int Id { get; set; }

        public CourseViewModel Course { get; set; }

        public IdentityUser User { get; set; }

        public int Mark { get; set; }
    }
}