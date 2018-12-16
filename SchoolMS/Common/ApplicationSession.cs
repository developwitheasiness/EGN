using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolMS.Common
{
    public class ApplicationSession
    {
        private ApplicationSession()
        {
            LoginUser = "";
            IsLoggedIn = false;
            UserID = "";
            RoleIDs = new List<RoleIds>();
            UserName = "";
            SessionID = "empty";
            IsSuperAdmin = true;
            AllowedPages = new AllowedPages();
        }

        // Gets the current session.
        public static ApplicationSession Current
        {
            get
            {
                ApplicationSession session =
                (ApplicationSession)HttpContext.Current.Session["__ApplicationSession__"];
                if (session == null)
                {
                    session = new ApplicationSession();
                    HttpContext.Current.Session["__ApplicationSession__"] = session;
                }
                return session;
            }

        }

        // **** add your session properties here, e.g like this:

        public string UserID { get; set; }
        public List<RoleIds> RoleIDs { get; set; }
        public string SessionID { get; set; }
        public bool IsSuperAdmin { get; set; }
        public string UserName { get; set; }
        public AllowedPages AllowedPages { get; set; }
        public bool IsLoggedIn { get; set; }
        public string LoginUser { get; set; }
    }

    public class RoleIds
    {
        public long id { get; set; }
        public string name { get; set; }
    }

    public class AllowedPages
    {

        public AllowedPages()
        {
            PageNames = new string[] { "GetStats", "GetLeadScheduleDetails" };
        }

        public string[] PageNames { get; set; }
    }

    public enum UserRoles
    {
        Admin = 1,
        OfficeStaff = 2,
        Teacher = 3
    }
}