using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolMS.Areas.admin.Controllers
{
    public class AdminController : Controller
    {
        [Route("admin/dashboard")]
        public ActionResult Dashboard()
        {
            return View();
        }
    }
}