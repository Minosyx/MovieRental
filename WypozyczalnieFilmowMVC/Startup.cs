using Microsoft.Owin;
using Owin;
using static WypozyczalnieFilmowMVC.DefaultUserRoles;

[assembly: OwinStartupAttribute(typeof(WypozyczalnieFilmowMVC.Startup))]
namespace WypozyczalnieFilmowMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesAndUsers().Wait();
        }
    }
}
