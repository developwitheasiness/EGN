﻿using SchoolMS.Controllers;
using System.Web;
using System.Web.Mvc;

namespace SchoolMS
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomErrorHandleAttribute());
        }
    }
}
