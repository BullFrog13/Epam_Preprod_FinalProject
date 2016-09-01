using AutoMapper;
using Courses.BLL.DTO;
using Courses.BLL.Interfaces;
using Courses.DAL.Entities;
using Courses.DAL.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Courses.BLL.Services
{
    public class SpecializationService : ISpecializationService
    {
        private IUnitOfWork Database { get; }

        public SpecializationService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public void Create(SpecializationDto specializationDto)
        {
            if (specializationDto.Name == null || specializationDto.Description == null)
            {
                throw new ValidationException("One of the parameters is absent");
            }
            Specialization specialization = new Specialization
            {
                Name = specializationDto.Name,
                Description = specializationDto.Description,
                Courses = new List<Course>()
            };
            Database.Specializations.Create(specialization);
            Database.Save();
        }

        public List<SpecializationDto> GetSpecializations()
        {
            var mapper = MapSpecialization();
            var res =  mapper.Map<List<Specialization>, List<SpecializationDto>>(Database.Specializations.GetAll());
            return res;
        }

        public SpecializationDto GetSpecialization(int? id)
        {
            if (id == null)
            {
                throw new ValidationException("No such post id");
            }

            Specialization specialization = Database.Specializations.Get(id.Value);

            if (specialization == null)
            {
                throw new ValidationException("Question was not found");
            }

            IMapper mapper = MapSpecialization();

            return mapper.Map<Specialization,SpecializationDto>(specialization);
        }

        public void UpdateSpecialization(SpecializationDto specialization)
        {
            var dbSpecialization = Database.Specializations.Get(specialization.Id);
            dbSpecialization.Name = specialization.Name;
            dbSpecialization.Description = specialization.Description;

            Database.Specializations.Update(dbSpecialization);
        }

        public void DeleteSpecialization(int? id)
        {
            if (id == null)
            {
                throw new ValidationException("No such course id");
            }
            var course = Database.Specializations.Get(id.Value);
            if (course == null)
            {
                throw new ValidationException("Course was not found");
            }

            var specializationCourses = Database.Courses.GetAll().Where(c => c.Specialization.Id.Equals(id)).ToList();
            var journalsDb = Database.Journals.GetAll();
            foreach(var specializationCourse in specializationCourses)
            {
               foreach(var journal in journalsDb)
                {
                    if (journal.CourseId == specializationCourse.Id)
                    {
                        Database.Journals.Delete(journal.Id);
                        Database.Save();
                    }
                }
                Database.Courses.Delete(specializationCourse.Id);
                Database.Save();
            }
            Database.Specializations.Delete(id.Value);
            Database.Save();
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        private IMapper MapSpecialization()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Specialization, SpecializationDto>()
                    .ForMember(s => s.CoursesDto, opt => opt.MapFrom(src => src.Courses)).MaxDepth(1);
                cfg.CreateMap<Course, CourseDto>();
            });
            IMapper mapper = config.CreateMapper();
            return mapper;
        }
    }
}
