using AutoMapper;
using Courses.BLL.DTO;
using Courses.BLL.Interfaces;
using Courses.Models;
using Courses.Models.EntityViewModels;
using Courses.Models.WrapModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NLog;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Courses.Controllers
{

    [AllowAnonymous]
    public class HomeController : Controller
    {
        readonly ICourseService _courseService;
        readonly ISpecializationService _specializationService;
        readonly IJournalService _journalService;
        protected ApplicationDbContext Context { get; set; }
        protected UserManager<ApplicationUser> UserManager { get; set; }
        protected RoleManager<IdentityRole> RoleManager { get; set; }
        private static Logger logger = LogManager.GetCurrentClassLogger();


        private CourseViewModel MapCourseDto(CourseDto courseDto)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CourseDto, CourseViewModel>();
                cfg.CreateMap<SpecializationDto, SpecializationViewModel>();
            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<CourseDto, CourseViewModel>(courseDto);
        }

        public HomeController(ICourseService course, ISpecializationService specialization, IJournalService journal)
        {
            _courseService = course;
            _specializationService = specialization;
            _journalService = journal;
            Context = new ApplicationDbContext();
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(Context));
            RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            logger.Trace("Home controller init");
        }

        public ActionResult Index()
        {
            CourseSpecializationWrapModel viewModel = new CourseSpecializationWrapModel();
            var courses = _courseService.GetCourses().OrderByDescending(o => o.Subscribers).Take(8);
            var spec = _specializationService.GetSpecializations();
            List<CourseViewModel> resListCourses = new List<CourseViewModel>();
            List<SpecializationViewModel> resListSpec = new List<SpecializationViewModel>();
            foreach (CourseDto courseDto in courses)
            {
                CourseViewModel course = new CourseViewModel
                {
                    Id = courseDto.Id,
                    StartDate = courseDto.StartDate,
                    EndDate = courseDto.EndDate,
                    Name = courseDto.Name,
                    Subscribers = courseDto.Subscribers,
                };
                resListCourses.Add(course);
            }
            foreach (SpecializationDto specDto in spec)
            {
                SpecializationViewModel specialization = new SpecializationViewModel
                {
                    Id = specDto.Id,
                    Name = specDto.Name,
                    Description = specDto.Description
                };
                resListSpec.Add(specialization);
            }
            viewModel.Courses = resListCourses;
            viewModel.Specializations = resListSpec;
            logger.Trace("/Home/Index/ request");
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult ReadCourse(int? id)
        {
            if (id == null)
            {
                logger.Error("ReadCourse request with null id");
                return new HttpStatusCodeResult(404);
            }
            var courseDto = _courseService.GetCourse(id);
            if (courseDto == null)
            {
                logger.Error("ReadCourse request, course not found in db, id : {0}", id);
                return HttpNotFound();
            }
            var user = User.Identity.GetUserId();

            var studentSubscribed = _journalService.StudentJournalExist(user, courseDto.Id);

            ViewBag.Teacher = UserManager.FindById(courseDto.TutorId).UserName;
            CourseViewModel resultModel = MapCourseDto(courseDto);
            resultModel.Subscribed = studentSubscribed;

            logger.Debug("Get course with id{0}", id);
            return View(resultModel);
        }

        [HttpGet]
        public ActionResult ReadSpec(int? id)
        {
            if (id == null)
            {
                logger.Error("ReadSpec request with null id");
                return new HttpStatusCodeResult(404);
            }
            var specDto = _specializationService.GetSpecialization(id);
            if (specDto == null)
            {
                logger.Error("ReadSpec request, specialization not found in db, id : {0}", id);
                return HttpNotFound();
            }
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SpecializationDto, SpecializationViewModel>();
                cfg.CreateMap<CourseDto, CourseViewModel>();
            });
            IMapper mapper = config.CreateMapper();
            logger.Debug("Get specialization with id {0}", id);
            return View(mapper.Map<SpecializationDto, SpecializationViewModel>(specDto));
        }

        [HttpGet]
        public ActionResult SpecializationCourse(int? id)
        {
            if (id == null)
            {
                logger.Error("SpecializationCourse request with null id");
                return new HttpStatusCodeResult(404);
            }
            var specDto = _specializationService.GetSpecialization(id);
            if (specDto == null)
            {
                logger.Error("SpecializationCourse request, specialization not found in db, id : {0}", id);
                return HttpNotFound();
            }
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SpecializationDto, SpecializationViewModel>()
                    .ForMember(s => s.Courses, opt => opt.MapFrom(src => src.CoursesDto)).MaxDepth(1);

                cfg.CreateMap<CourseDto, CourseViewModel>();
            });
            IMapper mapper = config.CreateMapper();
            return View(mapper.Map<SpecializationDto, SpecializationViewModel>(specDto));
        }

        public ActionResult Specializations()
        {
            var specializationsDto = _specializationService.GetSpecializations();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SpecializationDto, SpecializationViewModel>();
                cfg.CreateMap<CourseDto, CourseViewModel>();
            });
            IMapper mapper = config.CreateMapper();
            return View(mapper.Map<List<SpecializationDto>, List<SpecializationViewModel>>(specializationsDto));
        }

        public ActionResult Courses()
        {
            var coursesDto = _courseService.GetCourses();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SpecializationDto, SpecializationViewModel>();
                cfg.CreateMap<CourseDto, CourseViewModel>();
            });
            IMapper mapper = config.CreateMapper();
            return View(mapper.Map<List<CourseDto>, List<CourseViewModel>>(coursesDto));
        }

        public ActionResult Teachers()
        {
            List<IdentityUserRole> teacherUsers = RoleManager.FindByName("Tutor").Users.ToList();
            List<ApplicationUser> teachers = new List<ApplicationUser>();

            foreach (IdentityUserRole role in teacherUsers)
            {
                teachers.Add(UserManager.FindById(role.UserId));
            }
            return View(teachers);
        }

        [HttpGet]
        public ActionResult TeacherCourses(string id)
        {
            if (id == "" || id == null) {
                logger.Error("TeacherCourses request with empty id");
                return HttpNotFound();
            }
            var coursesDto = _courseService.GetCourses();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SpecializationDto, SpecializationViewModel>();
                cfg.CreateMap<CourseDto, CourseViewModel>();
            });
            IMapper mapper = config.CreateMapper();
            return View(mapper.Map<List<CourseDto>, List<CourseViewModel>>(coursesDto.Where(s =>
                                    s.TutorId.Equals(id)).ToList()));
        }

        protected override void Dispose(bool disposing)
        {
            _courseService.Dispose();
            base.Dispose(disposing);
        }

    }
}