using Courses.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Courses.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        protected ApplicationDbContext Context { get; set; }
        protected UserManager<ApplicationUser> UserManager { get; set; }

        public RoleController()
        {
            Context = new ApplicationDbContext();
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(Context));
        }

        public ActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateRole(FormCollection collection)
        {
            try
            {
                Context.Roles.Add(new IdentityRole()
                {
                    Name = collection["RoleName"]
                });
                Context.SaveChanges();
                ViewBag.ResultMessage = "Role created successfully!";
                return RedirectToAction("RolesList", "Role");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult RolesList()
        {
            return View(Context.Roles.ToList());
        }

        public PartialViewResult DeleteRole(string roleName)
        {
            var thisRole = Context.Roles.FirstOrDefault(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase));
            Context.Roles.Remove(thisRole);
            Context.SaveChanges();
            return PartialView("_RoleTable" , Context.Roles.ToList());
        }

        [HttpGet]
        public ActionResult EditRole(string roleName)
        {
            var thisRole = Context.Roles.FirstOrDefault(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase));

            return View(thisRole);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRole(IdentityRole role)
        {
            Context.Entry(role).State = System.Data.Entity.EntityState.Modified;
            Context.SaveChanges();

            return RedirectToAction("RolesList", "Role");
        }

        public ActionResult ManageUserRoles()
        {
            var list = Context.Roles.OrderBy(r => r.Name).ToList().Select(rr =>

            new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAddToUser(string userName, string roleName)
        {
            var user = Context.Users.FirstOrDefault(u => u.UserName.Equals(userName, StringComparison.CurrentCultureIgnoreCase));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(Context));
            if (user != null) userManager.AddToRole(user.Id, roleName);

            ViewBag.ResultMessage = "Role created successfully !";

            // prepopulat roles for the view dropdown
            var list = Context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;
            Context.SaveChanges();

            return View("ManageUserRoles");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetRoles(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName)) return View("ManageUserRoles");
            var user = Context.Users.FirstOrDefault(u => u.UserName.Equals(userName, StringComparison.CurrentCultureIgnoreCase));

            if (user != null) ViewBag.RolesForThisUser = UserManager.GetRoles(user.Id);

            // prepopulat roles for the view dropdown
            var list = Context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;

            return View("ManageUserRoles");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleForUser(string userName, string roleName)
        {
            var user = Context.Users.FirstOrDefault(u => u.UserName.Equals(userName, StringComparison.CurrentCultureIgnoreCase));

            if (user != null && UserManager.IsInRole(user.Id, roleName))
            {
                UserManager.RemoveFromRole(user.Id, roleName);
                ViewBag.ResultMessage = "Role removed from this user successfully !";
            }
            else
            {
                ViewBag.ResultMessage = "This user doesn't belong to selected role.";
            }
            // prepopulat roles for the view dropdown
            var list = Context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;
            Context.SaveChanges();
            return View("ManageUserRoles");
        }

        
    }
}