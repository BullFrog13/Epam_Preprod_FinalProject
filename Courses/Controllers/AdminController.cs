using AutoMapper;
using Courses.BLL.DTO;
using Courses.BLL.Interfaces;
using Courses.Models;
using Courses.Models.AdminModels.Course;
using Courses.Models.AdminModels.Specialization;
using Courses.Models.EntityViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Courses.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();
        protected UserManager<ApplicationUser> UserManager { get; set; }

        private readonly ICourseService _courseService;
        private readonly ISpecializationService _specializationService;
        private readonly IJournalService _journalService;

        private static Logger logger = LogManager.GetCurrentClassLogger();


        public AdminController(ICourseService course, ISpecializationService specialization, IJournalService journal)
        {
            _courseService = course;
            _specializationService = specialization;
            _journalService = journal;
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
        }

        #region CoursesAction
        [HttpGet]
        public ActionResult EditCourse(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(404);
            }
            var course = _courseService.GetCourse(id);
            if(course == null)
            {
                return HttpNotFound();
            }
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CourseDto, EditCourseViewModel>();
                cfg.CreateMap<SpecializationDto, SpecializationViewModel>();
            });
            var mapper = config.CreateMapper();
            return View(mapper.Map<CourseDto, EditCourseViewModel>(course));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCourse(EditCourseViewModel courseEdit)
        {
            if (ModelState.IsValid)
            {
                var course = _courseService.GetCourse(courseEdit.Id);
                
                if(course == null)
                {
                    return HttpNotFound();
                }

                course.Name = courseEdit.Name;
                course.Description = courseEdit.Description;
                _courseService.UpdateCourse(course);

                return RedirectToAction("Courses", "Home");
            }
            ModelState.AddModelError("", "Something failed");
            return View();
        }

        [HttpGet]
        public PartialViewResult DeleteCourse(int? id)
        {
            CourseDto course = _courseService.GetCourse(id);
            IEnumerable<JournalDto> journals = _journalService.GetJournals().Where(c => c.CourseId.Equals(id));
            _courseService.DeleteCourse(course.Id);
            foreach(JournalDto journal in journals)
            {
                _journalService.DeleteJournal(journal.Id);
            }

            List<CourseDto> coursesDto = _courseService.GetCourses();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SpecializationDto, SpecializationViewModel>();
                cfg.CreateMap<CourseDto, CourseViewModel>();
            });
            IMapper mapper = config.CreateMapper();
            return PartialView("_ListTable", mapper.Map<List<CourseDto>, List<CourseViewModel>>(coursesDto));
        }

        public ActionResult CreateCourse()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var tutorUsers = roleManager.FindByName("Tutor").Users.ToList();

            var tutors = tutorUsers.Select(tutor => UserManager.FindById(tutor.UserId)).ToList();

            ViewBag.Tutors = tutors;

            var specialization = _specializationService.GetSpecializations();
            ViewBag.Specializations = specialization;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCourse(CreateCourseViewModel course, int specializationId, string tutorId)
        {
            if (ModelState.IsValid)
            {
                if (CheckDates(course.StartDate, course.EndDate))
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<CreateCourseViewModel, CourseDto>();
                        cfg.CreateMap<SpecializationViewModel, SpecializationDto>();
                    });
                    var mapper = config.CreateMapper();
                    _courseService.Create(mapper.Map<CreateCourseViewModel, CourseDto>(course), specializationId, tutorId);

                    return RedirectToAction("Courses", "Home");
                }
                ModelState.AddModelError("", "Check dates! They are wrong");
            }
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var tutorUsers = roleManager.FindByName("Tutor").Users.ToList();

            List<ApplicationUser> tutors = new List<ApplicationUser>();

            foreach (IdentityUserRole tutor in tutorUsers)
            {
                tutors.Add(UserManager.FindById(tutor.UserId));
            }
            ViewBag.Tutors = tutors;
            var specialization = _specializationService.GetSpecializations();
            ViewBag.Specializations = specialization;
            ModelState.AddModelError("", "Something failed");
            return View(course);
        }
        #endregion

        #region SpecializationsActions
        [HttpGet]
        public ActionResult EditSpecialization(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(404);
            }
            var specialization = _specializationService.GetSpecialization(id);
            if (specialization == null)
            {
                return HttpNotFound();
            }
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SpecializationDto, EditSpecializationViewModel>();
            });
            var mapper = config.CreateMapper();
            return View(mapper.Map<SpecializationDto, EditSpecializationViewModel>(specialization));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSpecialization(EditSpecializationViewModel editSpecialization)
        {
            if (ModelState.IsValid)
            {
                var specialization = _specializationService.GetSpecialization(editSpecialization.Id);

                if (specialization == null)
                {
                    return HttpNotFound();
                }

                specialization.Name = editSpecialization.Name;
                specialization.Description = editSpecialization.Description;

                _specializationService.UpdateSpecialization(specialization);

                return RedirectToAction("Specializations", "Home");
            }
            ModelState.AddModelError("", "Something failed");
            return View();
        }

        [HttpGet]
        public PartialViewResult DeleteSpecialization(int? id)
        {
            var specialization = _specializationService.GetSpecialization(id);
            _specializationService.DeleteSpecialization(specialization.Id);

            var specializationsDto = _specializationService.GetSpecializations();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SpecializationDto, SpecializationViewModel>();
                cfg.CreateMap<CourseDto, CourseViewModel>();
            });
            var mapper = config.CreateMapper();
            return PartialView("_SpecializationTablePartialDisplay", mapper.Map<List<SpecializationDto>, List<SpecializationViewModel>>(specializationsDto));
        }


        public ActionResult CreateSpecialization()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSpecialization(СreateSpecializationViewModel specialization)
        {
            if (ModelState.IsValid)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<СreateSpecializationViewModel, SpecializationDto>();
                });
                var mapper = config.CreateMapper();
                _specializationService.Create(mapper.Map<СreateSpecializationViewModel, SpecializationDto>(specialization));

                return RedirectToAction("Specializations", "Home");
            }
            ModelState.AddModelError("", "Something failed");
            return View();
        }

        #endregion

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BanUser(string userName)
        {
            if (userName != null)
            {
                var user = UserManager.FindByName(userName);
                if (user != null)
                {
                    user.IsEnabled = false;
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("Index", "Manage");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnBanUser(string userName)
        {
            if (userName != null)
            {
                var user = UserManager.FindByName(userName);
                if (user != null)
                {
                    user.IsEnabled = true;
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("Index", "Manage");
        }

        public bool CheckDates(DateTime startDate, DateTime endDate)
        {
            if (DateTime.Compare(DateTime.Now, startDate) < 0)
            {
                if(DateTime.Compare(startDate, endDate) < 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}