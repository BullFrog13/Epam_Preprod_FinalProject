using AutoMapper;
using Courses.BLL.DTO;
using Courses.Models.EntityViewModels;

namespace Courses.Automapper_samples
{
    public class CustomMapper
    {
        public static IMapper MapCourseDto(CourseDto courseDto, bool listRequired)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CourseDto, CourseViewModel>();
                cfg.CreateMap<SpecializationDto, SpecializationViewModel>();
            });
            IMapper mapper = config.CreateMapper();
            return mapper;
        }
    }
}