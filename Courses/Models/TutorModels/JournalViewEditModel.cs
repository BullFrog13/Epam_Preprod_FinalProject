using System.ComponentModel.DataAnnotations;

namespace Courses.Models.TutorModels
{
    public class JournalViewEditModel
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int CourseId { get; set; }

        [Required]
        public int Mark { get; set; }
    }
}