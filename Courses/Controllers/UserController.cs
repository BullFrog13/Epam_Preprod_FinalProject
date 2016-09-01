using AutoMapper;
using Courses.BLL.DTO;
using Courses.BLL.Interfaces;
using Courses.Models;
using Courses.Models.EntityViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Mvc;

namespace Courses.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IJournalService _journalService;
        private readonly ICourseService _courseService;
        protected UserManager<ApplicationUser> UserManager { get; set; }
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        public UserController(IJournalService journal, ICourseService course)
        {
            _journalService = journal;
            _courseService = course;
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
        }

        private CourseViewModel Convert(CourseDto courseDto)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CourseDto, CourseViewModel>();
                cfg.CreateMap<SpecializationDto, SpecializationViewModel>();
            });
            var mapper = config.CreateMapper();
            return mapper.Map<CourseDto, CourseViewModel>(courseDto);
        }

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ViewBag.Name = user.Name;

                ViewBag.displayMenu = "No";

                if (isAdminUser())
                {
                    ViewBag.displayMenu = "Yes";
                }
                return View();
            }
            ViewBag.Name = "Not Logged in";
            return View();
        }

        public bool isAdminUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                var s = UserManager.GetRoles(user.GetUserId());
                if(s[0].ToString() == "Admin")
                {
                    return true;
                }
            }
            return false;
        }

        [HttpGet]
        public PartialViewResult SubscribeForACourse(int? courseId)
        {
            _courseService.AddSubscriber(courseId);
            var userId = User.Identity.GetUserId();
            _journalService.Create(userId, courseId);

            var courseDto = _courseService.GetCourse(courseId);
            ViewBag.Teacher = UserManager.FindById(courseDto.TutorId).UserName;
            var resultModel = Convert(courseDto);
            resultModel.Subscribed = true;
            return PartialView("_SubscribeButton", resultModel);
        }

        [HttpGet]
        public PartialViewResult UnSubscribeFromCourse(int? courseId)
        {
            _courseService.DeleteSubscriber(courseId);
            var userId = User.Identity.GetUserId();
            var journalToDelete = _journalService.GetJournalForAUser(userId, courseId.Value);

            _journalService.DeleteJournal(journalToDelete.Id);

            var courseDto = _courseService.GetCourse(courseId);

            ViewBag.Teacher = UserManager.FindById(courseDto.TutorId).UserName;
            var resultModel = Convert(courseDto);
            resultModel.Subscribed = false;
            return PartialView("_SubscribeButton", resultModel);
        }
    }
}