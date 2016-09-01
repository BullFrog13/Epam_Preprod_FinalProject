using System.ComponentModel.DataAnnotations;

namespace Courses.Models.AdminModels.Specialization
{
    public class СreateSpecializationViewModel
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}