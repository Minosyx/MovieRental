using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Entities;
using Entities.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WypozyczalnieFilmowMVC
{
    public static class DefaultUserRoles
    {
        public static async Task CreateRolesAndUsers()
        {
            AppDbContext _context = AppDbContext.Create();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));

            string adminRole = "Admin";

            if (!roleManager.RoleExists(adminRole))
            {
                var role = new IdentityRole
                {
                    Name = adminRole
                };
                await roleManager.CreateAsync(role);
            }

            if (await userManager.FindByNameAsync("admin123") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "admin123",
                    Email = ConfigurationManager.AppSettings["AdminMail"],
                    EmailConfirmed = true
                };

                string pwd = ConfigurationManager.AppSettings["AdminPwd"];

                var result = await userManager.CreateAsync(user, pwd);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user.Id, adminRole);
                }
            }

            string employeeRole = "Employee";

            if (!roleManager.RoleExists(employeeRole))
            {
                var role = new IdentityRole
                {
                    Name = employeeRole
                };
                await roleManager.CreateAsync(role);
            }
        }
    }
}