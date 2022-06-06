using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Entities;
using Entities.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WypozyczalnieFilmowMVC.Models;

namespace WypozyczalnieFilmowMVC.Controllers
{
    public class UsersController : Controller
    {
        private AppDbContext _context = AppDbContext.Create();

        // GET: Users
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                var user = User.Identity;
                ViewBag.Name = user.Name;

                ViewBag.displayMenu = "No";

                if (isAdminUser())
                {
                    ViewBag.displayMenu = "Yes";
                    return View();
                }

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Name = "Not Logged IN";
            }
            return RedirectToAction("Index", "Home");
        }

        public bool isAdminUser()
        {
            if (Request.IsAuthenticated)
            {
                var user = User.Identity;
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
                var s = userManager.GetRoles(user.GetUserId());
                return s.Count != 0 && s[0].ToString() == "Admin";
            }
            return false;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult SetRole()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_context));
            List<System.Web.Mvc.SelectListItem> roles = new List<System.Web.Mvc.SelectListItem>();
            foreach (IdentityRole role in roleManager.Roles)
            {
                roles.Add(new System.Web.Mvc.SelectListItem()
                {
                    Text = role.Name,
                    Value = role.Name
                });
            }

            var model = new AddRoleViewModel()
            {
                Roles = roles
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> SetRole(AddRoleViewModel user)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            //var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_context));
            if (ModelState.IsValid)
            {
                try
                {
                    var u = await userManager.FindByNameAsync(user.UserName);
                    u.Roles.Clear();
                    //var role = await roleManager.FindByIdAsync(user.RoleId);
                    await userManager.AddToRoleAsync(u.Id, user.RoleName);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("SetRole");
                }
                catch (Exception)
                {
                    return RedirectToAction("SetRole");
                }
            }

            return RedirectToAction("SetRole");
        }
    }
}