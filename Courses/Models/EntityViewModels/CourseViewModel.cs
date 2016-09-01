using System;
using System.ComponentModel.DataAnnotations;

namespace Courses.Models.EntityViewModels
{
    public class CourseViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [Required]
        public int? Subscribers { get; set; }

        [Required]
        public string Description { get; set; }

        public string TutorId { get; set; }

        [Required]
        public SpecializationViewModel Specialization { get; set; }

        public bool Subscribed { get; set; }

        public int Mark { get; set; }
    }
}