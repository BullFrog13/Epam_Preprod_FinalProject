using System.ComponentModel.DataAnnotations;

namespace Courses.Models.EntityViewModels
{
    public class JournalViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string StudentId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public int Mark { get; set; }
    }
}