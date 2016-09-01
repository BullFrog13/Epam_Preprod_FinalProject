using AutoMapper;
using Courses.BLL.DTO;
using Courses.BLL.Interfaces;
using Courses.DAL.Entities;
using Courses.DAL.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Courses.BLL.Services
{
    public class CourseService : ICourseService
    {
        private IUnitOfWork Database { get; }

        public CourseService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public void Create(CourseDto courseDto, int specializationId, string tutorId)
        {
            if (courseDto.Name == null || tutorId == null || tutorId.Equals("") ||
                courseDto.Name.Equals("") || courseDto.StartDate == null || courseDto.EndDate == null ||
                courseDto.Description == null || courseDto.Description.Equals("") ||
                specializationId == 0)
            {
                throw  new ValidationException("Something is wrong");
            }
            Course course = new Course
            {
                Name = courseDto.Name,
                EndDate = courseDto.EndDate,
                StartDate = courseDto.StartDate,
                Subscribers = 0,
                TutorId = tutorId,
                Description = courseDto.Description,
                Specialization = Database.Specializations.Get(specializationId)
            };
            Database.Courses.Create(course);
            Database.Save();
        }

        public List<CourseDto> GetCourses()
        {
            IMapper mapper = MapCourse();
            return mapper.Map<List<Course>, List<CourseDto>>(Database.Courses.GetAll());
        }

        public CourseDto GetCourse(int? id)
        {
            var mapper = MapCourse();

            if (id == null)
            {
                throw new ValidationException("No such course id");
            }
            var course = Database.Courses.Get(id.Value);
            if(course == null)
            {
                throw new ValidationException("Course was not found");
            }
            return mapper.Map<Course, CourseDto>(course);
        }

        public void UpdateCourse(CourseDto course)
        {
            var dbCourse = Database.Courses.Get(course.Id);
            dbCourse.Name = course.Name;
            dbCourse.StartDate = course.StartDate;
            dbCourse.EndDate = course.EndDate;
            dbCourse.Description = course.Description;
           
            Database.Courses.Update(dbCourse);
        }

        public void DeleteCourse(int? id)
        {
            if(id == null)
            {
                throw new ValidationException("No such course id");
            }
            var course = Database.Courses.Get(id.Value);
            if (course == null)
            {
                throw new ValidationException("Course was not found");
            }
            Database.Courses.Delete(id.Value);
        }

        public void AddSubscriber(int? id)
        {
            if (id == null)
            {
                throw new ValidationException("No such course id");
            }
            var course = Database.Courses.Get(id.Value);
            if (course == null)
            {
                throw new ValidationException("Course was not found");
            }
            course.Subscribers++;
            Database.Courses.Update(course);
        }

        public void DeleteSubscriber(int? id)
        {
            if (id == null)
            {
                throw new ValidationException("No such course id");
            }
            var course = Database.Courses.Get(id.Value);
            if (course == null)
            {
                throw new ValidationException("Course was not found");
            }
            course.Subscribers--;
            Database.Courses.Update(course);
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        private IMapper MapCourse()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Course, CourseDto>();
                cfg.CreateMap<Specialization, SpecializationDto>();
            });
            IMapper mapper = config.CreateMapper();
            return mapper;
        }
    }
}
