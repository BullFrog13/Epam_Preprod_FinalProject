using Courses.Models.EntityViewModels;
using System.Collections.Generic;

namespace Courses.Models.WrapModels
{
    public class CourseSpecializationWrapModel
    {
        public List<SpecializationViewModel> Specializations { get; set; }

        public List<CourseViewModel> Courses { get; set; }
    }
}