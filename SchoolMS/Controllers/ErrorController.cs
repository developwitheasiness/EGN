using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolMS.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult ErrorPage(int code, string message)
        {
            ViewBag.Message = message;
            ViewBag.Code = code;
            return View("Error");
        }

        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult General(int code, string message)
        {
            ViewBag.Message = message;
            ViewBag.Code = code;
            return View("Error");
        }

        public ActionResult UnauthrizedAccess()
        {
            return View();
        }
    }
}