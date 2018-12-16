using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SchoolMS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "adminlogin",
                "admin",
                new { controller = "Account", action = "Login" }
            );

            routes.MapRoute(
                "stafflogin",
                "stafflogin",
                new { controller = "Account", action = "Login" }
            );

            routes.MapRoute(
                "student",
                "student",
                new { controller = "Student", action = "Login" }
            );

            routes.MapRoute(
                "materials",
                "materials",
                new { controller = "Student", action = "Materials" }
            );

            routes.MapRoute(
                "UnauthrizedAccess",
                "UnauthrizedAccess",
                new { controller = "Error", action = "UnauthrizedAccess" }
                );

            routes.MapRoute(
                "about",
                "about",
                new { controller = "Home", action = "About" }
                );

            routes.MapRoute(
                "contactus",
                "contactus",
                new { controller = "Home", action = "Contact" }
                );

            routes.MapRoute(
               "teacherprofile",
               "teacherprofile",
               new { controller = "Teacher", action = "TeacherProfile" }
               );

            routes.MapRoute(
               "profile",
               "profile",
               new { controller = "Student", action = "StudentProfile" }
               );

            routes.MapRoute(
              "viewprofile",
              "view-profile",
              new { controller = "Student", action = "ViewProfile" }
              );

            routes.MapRoute(
              "viewteacherprofile",
              "view-teacher-profile",
              new { controller = "Teacher", action = "ViewTeacherProfile" }
              );

            routes.MapRoute(
              "teacheruploadmaterial",
              "teacher-upload-material",
              new { controller = "Teacher", action = "TeacherUploadMaterial" }
              );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Dashboard", id = UrlParameter.Optional }
            );
        }
    }
}
