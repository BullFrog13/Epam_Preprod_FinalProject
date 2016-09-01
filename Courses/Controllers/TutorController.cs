using AutoMapper;
using Courses.BLL.DTO;
using Courses.BLL.Interfaces;
using Courses.Models;
using Courses.Models.EntityViewModels;
using Courses.Models.TutorModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Web.Mvc;

namespace Courses.Controllers
{
    [Authorize(Roles = "Tutor")]
    public class TutorController : Controller
    {
        private readonly IJournalService _journalService;
        protected ApplicationDbContext Context { get; set; }
        protected UserManager<ApplicationUser> UserManager { get; set; }

        public TutorController(IJournalService journal)
        {
            _journalService = journal;
            Context = new ApplicationDbContext();
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(Context));
        }

        [HttpGet]
        public PartialViewResult EditMark(int? journalId, int mark, int courseId)
        {
            _journalService.SetMark(journalId, mark);

            var journals = _journalService.GetJournals().Where(j => j.CourseId.Equals(courseId)).ToList();
            var resultModelList = (from journal in journals
                let config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<SpecializationDto, SpecializationViewModel>();
                    cfg.CreateMap<CourseDto, CourseViewModel>();
                })
                let mapper = config.CreateMapper()
                select new JournalWrapModel()
                {
                    User = UserManager.FindById(journal.StudentId), Mark = journal.Mark, Id = journal.Id
                }).ToList();
            ViewBag.CourseId = courseId;
            return PartialView("TutorCourseJournalPartialTable",resultModelList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMark([Bind(Include = "Id, Mark, CourseId, UserId")]JournalViewEditModel editJournal)
        {
            if (ModelState.IsValid)
            {
                var journal = _journalService.GetJournal(editJournal.Id);

                if (journal == null)
                {
                    return HttpNotFound();
                }

                journal.Mark = editJournal.Mark;
                
                _journalService.UpdateJournal(journal);

                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Something failed");
            return View();
        }

        [HttpGet]
        public ActionResult TutorCourseJournals(int id)
        {
            var journals = _journalService.GetJournals().Where(j => j.CourseId.Equals(id)).ToList();
            var resultModelList = journals.Select(journal => new JournalWrapModel()
            {
                User = UserManager.FindById(journal.StudentId), Mark = journal.Mark, Id = journal.Id
            }).ToList();
            ViewBag.CourseId = id;
            return View(resultModelList);
        }
    }
}