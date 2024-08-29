using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebApplication6
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Application["AppStartTime"] = DateTime.Now;
            InitializeIdentityForEF();
        }

        private void InitializeIdentityForEF()
        {
            using (var context = new ApplicationDbContext())
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                // Tạo Role Admin nếu nó chưa tồn tại
                if (!roleManager.RoleExists("Admin"))
                {
                    var role = new IdentityRole("Admin");
                    roleManager.Create(role);
                }

                // Tạo User Admin nếu nó chưa tồn tại
                var user = userManager.FindByName("admin@admin.com");
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = "admin@admin.com",
                        Email = "admin@admin.com"
                    };
                    userManager.Create(user, "Admin@123");
                    userManager.SetLockoutEnabled(user.Id, false);
                }

                // Gán quyền Admin cho User Admin
                var rolesForUser = userManager.GetRoles(user.Id);
                if (!rolesForUser.Contains("Admin"))
                {
                    userManager.AddToRole(user.Id, "Admin");
                }
            }
        }
        }

    }

