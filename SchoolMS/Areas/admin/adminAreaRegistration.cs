using System.Web.Mvc;

namespace SchoolMS.Areas.admin
{
    public class adminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            #region student
            
            context.MapRoute(
                "admin_dashboard",
                "admin/dashboard",
                new { area = "admin", controller = "admin", action = "dashboard" }
            );

            context.MapRoute(
                "student_list",
                "admin/studentlist",
                new { area = "admin", controller = "students", action = "List" }
            );

            context.MapRoute(
                "student_new",
                "admin/student/new",
                new { area = "admin", controller = "students", action = "RegisterStudent" }
            );

            context.MapRoute(
                "student_edit",
                "admin/student/{id}",
                new { area = "admin", controller = "students", action = "RegisterStudent" }
            );

            context.MapRoute(
                "student_delete",
                "admin/student/delete/{id}",
                new { area = "admin", controller = "students", action = "DeleteStudent" }
            );

            #endregion student

            #region grade

            context.MapRoute(
                "grade_list",
                "admin/gradelist",
                new { area = "admin", controller = "master", action = "SchoolGrade" }
            );
            
            context.MapRoute(
                "grade_new",
                "admin/grade/new",
                new { area = "admin", controller = "master", action = "GradeEntry" }
            );

            context.MapRoute(
                "grade_edit",
                "admin/grade/{id}",
                new { area = "admin", controller = "master", action = "GradeEntry" }
            );

            context.MapRoute(
               "grade_delete",
               "admin/grade/delete/{id}",
               new { area = "admin", controller = "master", action = "DeleteGrade" }
           );

            #endregion

            #region schoolsubject

            context.MapRoute(
                "schoolsubject_list",
                "admin/subjectlist",
                new { area = "admin", controller = "master", action = "SchoolSubject" }
            );

            context.MapRoute(
               "subject_new",
               "admin/subject/new",
               new { area = "admin", controller = "master", action = "SubjectEntry" }
           );

            context.MapRoute(
                "subject_edit",
                "admin/subject/{id}",
                new { area = "admin", controller = "master", action = "SubjectEntry" }
            );

            context.MapRoute(
              "subject_delete",
              "admin/subject/delete/{id}",
              new { area = "admin", controller = "master", action = "DeleteSubject" }
          );

            #endregion schoolsubject

            #region sliderconfig

            context.MapRoute(
                "sliderconfig_list",
                "admin/sliderlist",
                new { area = "admin", controller = "master", action = "ManageSlider" }
            );

            context.MapRoute(
               "slider_new",
               "admin/slider/new",
               new { area = "admin", controller = "master", action = "Slider" }
           );

            context.MapRoute(
                "slider_edit",
                "admin/slider/{id}",
                new { area = "admin", controller = "master", action = "Slider" }
            );

            context.MapRoute(
              "slider_delete",
              "admin/slider/delete/{id}",
              new { area = "admin", controller = "master", action = "DeleteSlider" }
          );

            #endregion schoolsubject

            #region generalsettings

            context.MapRoute(
                "generalsettings_list",
                "admin/generalsettings",
                new { area = "admin", controller = "master", action = "ManageGeneralSetting" }
            );

           // context.MapRoute(
           //     "generalsettings_edit",
           //     "admin/generalsettings/{id}",
           //     new { area = "admin", controller = "master", action = "GeneralSettings" }
           // );

           // context.MapRoute(
           //    "generalsettings_new",
           //    "admin/generalsettings/new",
           //    new { area = "admin", controller = "master", action = "GeneralSettings" }
           //);

            #endregion generalsettings

            context.MapRoute(
                "admin_default",
                "admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}