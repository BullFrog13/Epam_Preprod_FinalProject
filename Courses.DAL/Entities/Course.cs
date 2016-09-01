using System;

namespace Courses.DAL.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public int? Subscribers { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string TutorId { get; set; }
        public string Description { get; set; }

        public virtual Specialization Specialization { get; set; }
    }
}
