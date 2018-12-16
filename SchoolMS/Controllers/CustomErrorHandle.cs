using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolMS.Controllers
{
    public class CustomErrorHandleAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
        }
    }
}