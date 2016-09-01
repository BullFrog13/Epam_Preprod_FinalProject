using System.ComponentModel.DataAnnotations;

namespace Courses.Models.AdminModels.Specialization
{
    public class EditSpecializationViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}