namespace Courses.DAL.Entities
{
    public class Journal
    {
        public int Id { get; set; }

        public string StudentId { get; set; }

        public int CourseId { get; set; }

        public int Mark { get; set; }
    }
}
