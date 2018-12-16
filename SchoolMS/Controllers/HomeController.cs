using SchoolMS.Models.DB;
using SchoolMS.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

namespace SchoolMS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Dashboard()
        {
            return View("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult ManageProfile()
        {
            return View();
        }

        public ActionResult Teachers()
        {
            return View();
        }

        public ActionResult SignUp()
        {
            return View();
        }

        public PartialViewResult SlideImages()
        {
            List<SlideConfigVM> ListOfSlide = new List<SlideConfigVM>();
            try
            {
                using (var _db = new SchoolMSEntities())
                {
                    ListOfSlide = _db.SliderConfigs.Where(s => s.IsActive == true).ToList().Select(s => new SlideConfigVM()
                    {
                        Description = s.Description,
                        SliderImage = s.SliderImage
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                
            }
            return PartialView("_CarasousalSlides", ListOfSlide);
        }
    }
}