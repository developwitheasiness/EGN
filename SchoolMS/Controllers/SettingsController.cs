using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolMS.Controllers
{
    public class SettingsController : Controller
    {
        // GET: Settings
        public ActionResult Profile()
        {
            return View();
        }

        public ActionResult TOS()
        {
            return View();
        }
    }
}