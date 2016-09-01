using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Courses.Models.EntityViewModels
{
    public class SpecializationViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public virtual ICollection<CourseViewModel> Courses { get; set; }
    }
}