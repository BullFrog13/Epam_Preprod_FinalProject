using Courses.Models.EntityViewModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace Courses.Models.AdminModels.Course
{
    public class EditCourseViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}