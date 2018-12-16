using SchoolMS.Common;
using SchoolMS.Models.DB;
using SchoolMS.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolMS.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Teacher
        public ActionResult TeacherDetails()
        {
            return View();
        }

        public ActionResult ViewTeacherProfile()
        {
            if (!string.IsNullOrEmpty(ApplicationSession.Current.UserID))
            {
                ViewTeacherVM teachViewObj = new ViewTeacherVM();
                try
                {
                    using (var _db = new SchoolMSEntities())
                    {
                        var teach = _db.Teachers.Find(Guid.Parse(ApplicationSession.Current.UserID));

                        if (teach != null)
                        {
                            teachViewObj.TeacherName = teach.Name;
                            TimeZoneInfo zone = TimeZoneInfo.GetSystemTimeZones().Where(t => t.Id == teach.TimeZone).FirstOrDefault();
                            DateTime utcTime1 = new DateTime();
                            utcTime1 = DateTime.UtcNow;
                            //DateTimeOffset time2 = new DateTimeOffset(utcTime1,
                            //TimeZoneInfo.FindSystemTimeZoneById(zone.Id).GetUtcOffset(utcTime1));
                            //studViewObj.TimeZone = time2.Hour + ":" + time2.Minute;

                            TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById(zone.Id);

                            DateTime userTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime1, timeInfo);
                            teachViewObj.TimeZone += userTime.Hour + ":" + userTime.Minute;
                            ;
                            teachViewObj.TimeZone += ", " + TimeZoneInfo.GetSystemTimeZones().Where(t => t.Id == teach.TimeZone).FirstOrDefault()?.DisplayName;

                            teachViewObj.From = (teach.Gender != 1 ? "Male" : "Female") + ", From India";

                            //foreach (var item in teach.StudentSubjects)
                            //{
                            //    teachViewObj.Learning += item.SchoolSubject?.Subject + ", ";
                            //}

                            var ListSubjects = _db.TeacherSubjects.ToList().Where(s => s.TeacherId == Guid.Parse(ApplicationSession.Current.UserID)).ToList();

                            teachViewObj.Subjects = ListSubjects.ToList().Select(s => s.SchoolSubject?.Subject).Distinct().ToArray();

                            teachViewObj.ProfilePhoto = teach.ProfilePhoto;
                            teachViewObj.LongIntroduction = teach.LongIntroduction;
                            teachViewObj.CertiPhoto = teach.CertificatePhoto;
                            teachViewObj.CourceTitle = teach.CourceTitle;
                            teachViewObj.TeacherSince = teach.TeacherSince;

                            teachViewObj.ExprienceList = _db.TeacherExperiences.ToList().Where(e => e.TeacherId == Guid.Parse(ApplicationSession.Current.UserID)).ToList().Select(e => new TeacherExperienceVM()
                            {
                                WorkExperienceName = e.Name,
                                WorkExperienceDetails = e.Details,
                                WorkExperienceYear = e.Year
                            }).ToList();

                            teachViewObj.CertificateList = _db.TeacherCertificates.ToList().Where(e => e.TeacherId == Guid.Parse(ApplicationSession.Current.UserID)).ToList().Select(e => new TeacherCertificateVM()
                            {
                                CertificatePhoto = e.CertificatePath,
                                CertificateTitle = e.CertificateTitle
                            }).ToList();
                        }
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Home", "Dashboard");
                }
                return View(teachViewObj);
            }
            else
            {
                return RedirectToAction("Home", "Dashboard");
            }
        }

        public ActionResult ViewTeacherDetail(Guid TeacherId)
        {
            ViewTeacherVM teachViewObj = new ViewTeacherVM();
            try
            {
                using (var _db = new SchoolMSEntities())
                {
                    var teach = _db.Teachers.Find(TeacherId);

                    if (teach != null)
                    {
                        teachViewObj.TeacherId = teach.Id;
                        teachViewObj.TeacherName = teach.Name;
                        TimeZoneInfo zone = TimeZoneInfo.GetSystemTimeZones().Where(t => t.Id == teach.TimeZone).FirstOrDefault();
                        DateTime utcTime1 = new DateTime();
                        utcTime1 = DateTime.UtcNow;
                        //DateTimeOffset time2 = new DateTimeOffset(utcTime1,
                        //TimeZoneInfo.FindSystemTimeZoneById(zone.Id).GetUtcOffset(utcTime1));
                        //studViewObj.TimeZone = time2.Hour + ":" + time2.Minute;

                        TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById(zone.Id);

                        DateTime userTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime1, timeInfo);
                        teachViewObj.TimeZone += userTime.Hour + ":" + userTime.Minute;
                        ;
                        teachViewObj.TimeZone += ", " + TimeZoneInfo.GetSystemTimeZones().Where(t => t.Id == teach.TimeZone).FirstOrDefault()?.DisplayName;

                        teachViewObj.From = (teach.Gender != 1 ? "Male" : "Female") + ", From India";

                        //foreach (var item in teach.StudentSubjects)
                        //{
                        //    teachViewObj.Learning += item.SchoolSubject?.Subject + ", ";
                        //}

                        var ListSubjects = _db.TeacherSubjects.ToList().Where(s => s.TeacherId == TeacherId).ToList();

                        teachViewObj.Subjects = ListSubjects.ToList().Select(s => s.SchoolSubject?.Subject).Distinct().ToArray();

                        teachViewObj.ProfilePhoto = teach.ProfilePhoto;
                        teachViewObj.LongIntroduction = teach.LongIntroduction;
                        teachViewObj.CertiPhoto = teach.CertificatePhoto;
                        teachViewObj.CourceTitle = teach.CourceTitle;
                        teachViewObj.TeacherSince = teach.TeacherSince;

                        teachViewObj.ExprienceList = _db.TeacherExperiences.ToList().Where(e => e.TeacherId == TeacherId).ToList().Select(e => new TeacherExperienceVM()
                        {
                            WorkExperienceName = e.Name,
                            WorkExperienceDetails = e.Details,
                            WorkExperienceYear = e.Year
                        }).ToList();

                        teachViewObj.CertificateList = _db.TeacherCertificates.ToList().Where(e => e.TeacherId == TeacherId).ToList().Select(e => new TeacherCertificateVM()
                        {
                            CertificatePhoto = e.CertificatePath,
                            CertificateTitle = e.CertificateTitle
                        }).ToList();

                        if (!String.IsNullOrEmpty(ApplicationSession.Current.UserID))
                        {
                            teachViewObj.TeacherRatings = _db.TeacherRatings.ToList().Where(e => e.TeacherId == TeacherId && e.StudentId == Guid.Parse(ApplicationSession.Current.UserID)).ToList().Select(e => new TeacherRatingVM()
                            {
                                Comments = e.Comments,
                                RatingId = e.Id,
                                RatingValue = e.RatingValue,
                                StudentName = e.Student.Name
                            }).ToList();
                        }
                        else
                        {
                            teachViewObj.TeacherRatings = _db.TeacherRatings.ToList().Where(e => e.TeacherId == TeacherId).ToList().Select(e => new TeacherRatingVM()
                            {
                                Comments = e.Comments,
                                RatingId = e.Id,
                                RatingValue = e.RatingValue,
                                StudentName = e.Student.Name
                            }).ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Home", "Dashboard");
            }
            return View(teachViewObj);
        }

        public ActionResult TeacherProfile(string Message)
        {
            ViewBag.Message = Message;
            if (!string.IsNullOrEmpty(ApplicationSession.Current.UserID))
            {
                TeacherVM teachObj = new TeacherVM();
                try
                {
                    using (var _db = new SchoolMSEntities())
                    {
                        teachObj.TimeZoneInfos = TimeZoneInfo.GetSystemTimeZones()
                                            .Select(t => new
                                            SelectListItem()
                                            {
                                                Text = t.DisplayName,
                                                Value = t.Id
                                            }).ToList();

                        teachObj.GradeList = _db.SchoolGrades.ToList()
                                            .Select(g => new SelectListItem()
                                            {
                                                Text = g.Grade,
                                                Value = g.Id.ToString()
                                            }).ToList();

                        var teacher = _db.Teachers.Find(Guid.Parse(ApplicationSession.Current.UserID));

                        if (teacher != null)
                        {
                            teachObj.BirthDate = teacher.BirthDate;

                            if (teachObj.BirthDate != null)
                            {
                                DateTime dtBirth = Convert.ToDateTime(teacher.BirthDate);
                                teachObj.BirthDateOnly = dtBirth.Day;
                                teachObj.BirthMonthOnly = dtBirth.Month;
                                teachObj.BirthYearOnly = dtBirth.Year;
                            }
                            else
                            {
                                DateTime dt = new DateTime();
                                dt = DateTime.Today;
                                teachObj.BirthYearOnly = dt.Year - 60;
                                teachObj.BirthMonthOnly = dt.Month;
                                teachObj.BirthDateOnly = dt.Day;
                            }

                            teachObj.Email = teacher.Email;
                            teachObj.FromCountry = teacher.FromCountry;
                            teachObj.FromState = teacher.FromState;
                            teachObj.Gender = teacher.Gender;
                            teachObj.Id = teacher.Id;
                            teachObj.LivingCountry = teacher.LivingCountry;
                            teachObj.LivingState = teacher.LivingState;
                            teachObj.Name = teacher.Name;
                            teachObj.ProfilePhoto = teacher.ProfilePhoto;
                            teachObj.TimeZone = teacher.TimeZone;
                            teachObj.LongIntroduction = teacher.LongIntroduction;
                            teachObj.ShortDescription = teacher.ShortDescription;
                            teachObj.LongIntroduction = teacher.LongIntroduction;
                            teachObj.TeacherSince = teacher.TeacherSince;
                            teachObj.UniversityName = teacher.UniversityName;
                            teachObj.YearOfPass = teacher.YearOfPass;
                            teachObj.CourseDegreeName = teacher.CourseDegreeName;
                            //teachObj.WorkExperienceName = teacher.WorkExperienceName;
                            //teachObj.WorkExperienceYear = teacher.WorkExperienceYear;
                            //teachObj.WorkExperienceDetails = teacher.WorkExperienceDetails;
                            //teachObj.CertificateTitle = teacher.CertificateTitle;
                            //teachObj.CertificatePhoto = teacher.CertificatePhoto;
                            //teachObj.CourceTitle = teacher.CourceTitle;
                            //teachObj.Description = teacher.Description;
                            //teachObj.Charges = teacher.Charges;
                            teachObj.ViewMoreDetails = teacher.ViewMoreDetails;
                        }
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Home", "Dashboard");
                }
                return View(teachObj);
            }
            else
            {
                return RedirectToAction("Home", "Dashboard");
            }
        }

        public ActionResult SaveTeacherPersonalInfo(TeacherVM teachObj, HttpPostedFileBase file)
        {
            JsonResponse<bool> Response = new JsonResponse<bool>();
            try
            {
                using (var _db = new SchoolMSEntities())
                {
                    var TeacherDetail = _db.Teachers.Find(teachObj.Id);

                    if (TeacherDetail != null)
                    {
                        TeacherDetail.Name = teachObj.Name;
                        TeacherDetail.FromCountry = teachObj.FromCountry;
                        TeacherDetail.FromState = teachObj.FromState;
                        TeacherDetail.LivingCountry = teachObj.LivingCountry;
                        TeacherDetail.LivingState = teachObj.LivingState;
                        TeacherDetail.TimeZone = teachObj.TimeZone;
                        TeacherDetail.BirthDate = new DateTime(teachObj.BirthYearOnly, teachObj.BirthMonthOnly, teachObj.BirthDateOnly);
                        TeacherDetail.Gender = teachObj.Gender;

                        if (file != null)
                        {
                            string path = Server.MapPath("~/images/teacher/");
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            file.SaveAs(path + Path.GetFileName(file.FileName));
                            TeacherDetail.ProfilePhoto = file.FileName;
                        }

                        _db.SaveChanges();

                        Response.IsError = false;
                    }
                    else
                    {
                        Response.IsError = true;
                        Response.Message = "No teacher found";
                    }
                }
            }
            catch (Exception)
            {
                Response.IsError = true;
                Response.Message = "There is some internal server error during operation. Please try again after sometime.";
            }

            return RedirectToAction("TeacherProfile", "Teacher", new { Message = "Information update successfully" });
            //return Json(Response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveTeacherInfo(TeacherVM teachObj)
        {
            JsonResponse<bool> Response = new JsonResponse<bool>();
            try
            {
                using (var _db = new SchoolMSEntities())
                {
                    var TeacherDetail = _db.Teachers.Find(teachObj.Id);

                    if (TeacherDetail != null)
                    {
                        TeacherDetail.ShortDescription = teachObj.ShortDescription;
                        TeacherDetail.LongIntroduction = teachObj.LongIntroduction;
                        TeacherDetail.TeacherSince = teachObj.TeacherSince;

                        _db.SaveChanges();

                        Response.IsError = false;
                    }
                    else
                    {
                        Response.IsError = true;
                        Response.Message = "No teacher found";
                    }
                }
            }
            catch (Exception)
            {
                Response.IsError = true;
                Response.Message = "There is some internal server error during operation. Please try again after sometime.";
            }

            return Json(Response, JsonRequestBehavior.AllowGet);
        }

        #region Teacher Education
        public PartialViewResult TeacherEducationList(Guid TeacherId)
        {
            List<TeacherEducationVM> EducationList = new List<TeacherEducationVM>();
            try
            {
                using (var _db = new SchoolMSEntities())
                {
                    EducationList = _db.TeacherEducations.Where(s => s.TeacherId == TeacherId).ToList()
                                    .Select(s => new TeacherEducationVM()
                                    {
                                        CourseDegreeName = s.Course,
                                        TeacherEducationId = s.Id,
                                        TeacherId = (Guid)s.TeacherId,
                                        UniversityName = s.University,
                                        YearOfPass = s.Year
                                    }).ToList();
                }
            }
            catch (Exception ex)
            {

            }

            return PartialView("_TeacherEducationList", EducationList);
        }

        public JsonResult SaveTeacherEducation(TeacherEducationVM teachEduObj)
        {
            JsonResponse<bool> Response = new JsonResponse<bool>();
            try
            {
                using (var _db = new SchoolMSEntities())
                {
                    var TeacherDetail = _db.Teachers.Find(teachEduObj.TeacherId);
                    TeacherEducation TeacherEduDetail = new TeacherEducation();
                    if (TeacherDetail != null)
                    {
                        if (teachEduObj.TeacherEducationId != Guid.Empty)
                        {
                            TeacherEduDetail = _db.TeacherEducations.Find(teachEduObj.TeacherEducationId);

                            if (TeacherEduDetail == null)
                            {
                                Response.IsError = true;
                                Response.Message = "No Teacher Education detail found.";
                                return Json(Response, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            TeacherEduDetail.Id = Guid.NewGuid();
                            TeacherEduDetail.TeacherId = TeacherDetail.Id;
                        }

                        TeacherEduDetail.University = teachEduObj.UniversityName;
                        TeacherEduDetail.Year = teachEduObj.YearOfPass;
                        TeacherEduDetail.Course = teachEduObj.CourseDegreeName;

                        if (teachEduObj.TeacherEducationId == Guid.Empty)
                        {
                            _db.TeacherEducations.Add(TeacherEduDetail);
                        }

                        _db.SaveChanges();

                        Response.IsError = false;
                    }
                    else
                    {
                        Response.IsError = true;
                        Response.Message = "No teacher found";
                    }
                }
            }
            catch (Exception)
            {
                Response.IsError = true;
                Response.Message = "There is some internal server error during operation. Please try again after sometime.";
            }

            return Json(Response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteTeacherEducation(Guid TeacherEducationId)
        {
            JsonResponse<bool> Response = new JsonResponse<bool>();
            try
            {
                using (var _db = new SchoolMSEntities())
                {
                    var ExistingTeacherEducatioDetail = _db.TeacherEducations.Find(TeacherEducationId);

                    if (ExistingTeacherEducatioDetail != null)
                    {
                        _db.TeacherEducations.Remove(ExistingTeacherEducatioDetail);
                        _db.SaveChanges();
                    }
                    else
                    {
                        Response.IsError = true;
                        Response.Message = "Teacher Education details not found.";
                        return Json(Response, JsonRequestBehavior.AllowGet);
                    }
                }

                Response.IsError = false;
            }
            catch (Exception ex)
            {
                Response.IsError = true;
                Response.Message = "";
            }

            return Json(Response, JsonRequestBehavior.AllowGet);
        }
        #endregion Teacher Education

        #region Teacher Experience
        public JsonResult SaveTeacherExperience(TeacherExperienceVM teachObj)
        {
            JsonResponse<bool> Response = new JsonResponse<bool>();
            try
            {
                using (var _db = new SchoolMSEntities())
                {
                    var TeacherDetail = _db.Teachers.Find(teachObj.TeacherId);

                    if (TeacherDetail != null)
                    {
                        TeacherExperience TeacherExpDetail = new TeacherExperience();
                        if (teachObj.TeacherExperienceId != Guid.Empty)
                        {
                            TeacherExpDetail = _db.TeacherExperiences.Find(teachObj.TeacherExperienceId);

                            if (TeacherExpDetail == null)
                            {
                                Response.IsError = true;
                                Response.Message = "No Teacher Experience details found.";
                                return Json(Response, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            TeacherExpDetail.Id = Guid.NewGuid();
                            TeacherExpDetail.TeacherId = TeacherDetail.Id;
                        }

                        TeacherExpDetail.Name = teachObj.WorkExperienceName;
                        TeacherExpDetail.Year = teachObj.WorkExperienceYear;
                        TeacherExpDetail.Details = teachObj.WorkExperienceDetails;

                        if (teachObj.TeacherExperienceId == Guid.Empty)
                        {
                            _db.TeacherExperiences.Add(TeacherExpDetail);
                        }

                        _db.SaveChanges();

                        Response.IsError = false;
                    }
                    else
                    {
                        Response.IsError = true;
                        Response.Message = "No teacher found";
                    }
                }
            }
            catch (Exception)
            {
                Response.IsError = true;
                Response.Message = "There is some internal server error during operation. Please try again after sometime.";
            }

            return Json(Response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteTeacherExperience(Guid TeacherExperienceId)
        {
            JsonResponse<bool> Response = new JsonResponse<bool>();
            try
            {
                using (var _db = new SchoolMSEntities())
                {
                    var ExistingTeacherExpDetail = _db.TeacherExperiences.Find(TeacherExperienceId);

                    if (ExistingTeacherExpDetail != null)
                    {
                        _db.TeacherExperiences.Remove(ExistingTeacherExpDetail);
                        _db.SaveChanges();
                    }
                    else
                    {
                        Response.IsError = true;
                        Response.Message = "Teacher Experience details not found.";
                        return Json(Response, JsonRequestBehavior.AllowGet);
                    }
                }

                Response.IsError = false;
            }
            catch (Exception ex)
            {
                Response.IsError = true;
                Response.Message = "";
            }

            return Json(Response, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult TeacherExperienceList(Guid TeacherId)
        {
            List<TeacherExperienceVM> ExperienceList = new List<TeacherExperienceVM>();
            try
            {
                using (var _db = new SchoolMSEntities())
                {
                    ExperienceList = _db.TeacherExperiences.Where(s => s.TeacherId == TeacherId).ToList()
                                    .Select(s => new TeacherExperienceVM()
                                    {
                                        TeacherExperienceId = s.Id,
                                        WorkExperienceDetails = s.Details,
                                        TeacherId = (Guid)s.TeacherId,
                                        WorkExperienceName = s.Name,
                                        WorkExperienceYear = s.Year
                                    }).ToList();
                }
            }
            catch (Exception ex)
            {

            }

            return PartialView("_TeacherExperienceList", ExperienceList);
        }
        #endregion Teacher Experience

        #region Teacher Proffessional Course
        public JsonResult SaveTeacherProffCourses(TeacherProffCoursesVM teachObj)
        {
            JsonResponse<bool> Response = new JsonResponse<bool>();
            try
            {
                using (var _db = new SchoolMSEntities())
                {
                    var TeacherDetail = _db.Teachers.Find(teachObj.TeacherId);

                    if (TeacherDetail != null)
                    {
                        TeacherProffessionalCourse TeacherProffDetail = new TeacherProffessionalCourse();
                        if (teachObj.TeacherProffId != Guid.Empty)
                        {
                            TeacherProffDetail = _db.TeacherProffessionalCourses.Find(teachObj.TeacherProffId);

                            if (TeacherProffDetail == null)
                            {
                                Response.IsError = true;
                                Response.Message = "No Teacher Experience details found.";
                                return Json(Response, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            TeacherProffDetail.Id = Guid.NewGuid();
                            TeacherProffDetail.TeacherId = TeacherDetail.Id;
                        }

                        TeacherProffDetail.Title = teachObj.CourseTitle;
                        TeacherProffDetail.Description = teachObj.Description;
                        TeacherProffDetail.Charges = teachObj.Charges;
                        TeacherProffDetail.MoreDetails = teachObj.ViewMoreDetails;

                        if (teachObj.TeacherProffId == Guid.Empty)
                        {
                            _db.TeacherProffessionalCourses.Add(TeacherProffDetail);
                        }

                        _db.SaveChanges();

                        Response.IsError = false;
                    }
                    else
                    {
                        Response.IsError = true;
                        Response.Message = "No teacher found";
                    }
                }
            }
            catch (Exception)
            {
                Response.IsError = true;
                Response.Message = "There is some internal server error during operation. Please try again after sometime.";
            }

            return Json(Response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteTeacherProffCourse(Guid TeacherProffId)
        {
            JsonResponse<bool> Response = new JsonResponse<bool>();
            try
            {
                using (var _db = new SchoolMSEntities())
                {
                    var ExistingTeacherProffDetail = _db.TeacherProffessionalCourses.Find(TeacherProffId);

                    if (ExistingTeacherProffDetail != null)
                    {
                        _db.TeacherProffessionalCourses.Remove(ExistingTeacherProffDetail);
                        _db.SaveChanges();
                    }
                    else
                    {
                        Response.IsError = true;
                        Response.Message = "Teacher Proffessional Course details not found.";
                        return Json(Response, JsonRequestBehavior.AllowGet);
                    }
                }

                Response.IsError = false;
            }
            catch (Exception ex)
            {
                Response.IsError = true;
                Response.Message = "";
            }

            return Json(Response, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult TeacherProffCourseList(Guid TeacherId)
        {
            List<TeacherProffCoursesVM> ProffCourseList = new List<TeacherProffCoursesVM>();
            try
            {
                using (var _db = new SchoolMSEntities())
                {
                    ProffCourseList = _db.TeacherProffessionalCourses.Where(s => s.TeacherId == TeacherId).ToList()
                                    .Select(s => new TeacherProffCoursesVM()
                                    {
                                        TeacherProffId = s.Id,
                                        CourseTitle = s.Title,
                                        TeacherId = (Guid)s.TeacherId,
                                        Description = s.Description,
                                        Charges = s.Charges,
                                        ViewMoreDetails = s.MoreDetails
                                    }).ToList();
                }
            }
            catch (Exception ex)
            {

            }

            return PartialView("_TeacherProffCourseList", ProffCourseList);
        }

        public ActionResult SaveTeacherProffessional(TeacherVM teachObj)
        {
            JsonResponse<bool> Response = new JsonResponse<bool>();
            try
            {
                using (var _db = new SchoolMSEntities())
                {
                    var TeacherDetail = _db.Teachers.Find(teachObj.Id);

                    if (TeacherDetail != null)
                    {
                        TeacherDetail.CourceTitle = teachObj.CourceTitle;
                        TeacherDetail.Description = teachObj.Description;
                        TeacherDetail.ViewMoreDetails = teachObj.ViewMoreDetails;
                        TeacherDetail.Charges = teachObj.Charges;

                        _db.SaveChanges();

                        Response.IsError = false;
                    }
                    else
                    {
                        Response.IsError = true;
                        Response.Message = "No teacher found";
                    }
                }
            }
            catch (Exception)
            {
                Response.IsError = true;
                Response.Message = "There is some internal server error during operation. Please try again after sometime.";
            }

            return Json(Response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region TeacherCertificate

        public ActionResult SaveTeacherCertificate(TeacherCertificateVM teachObj, HttpPostedFileBase file)
        {
            JsonResponse<bool> Response = new JsonResponse<bool>();
            TeacherCertificate teacherCertificate = new TeacherCertificate();
            try
            {
                using (var _db = new SchoolMSEntities())
                {
                    var TeacherDetail = _db.Teachers.Find(teachObj.TeacherId);

                    if (TeacherDetail != null)
                    {

                        if (teachObj.TeacherCertificateId != Guid.Empty)
                        {
                            teacherCertificate = _db.TeacherCertificates.Find(teachObj.TeacherCertificateId);

                            if (teacherCertificate == null)
                            {
                                return RedirectToAction("TeacherProfile", "Teacher", new { Message = "Certificates not found" });
                            }
                        }

                        teacherCertificate.CertificateTitle = teachObj.CertificateTitle;

                        if (file != null)
                        {
                            string path = Server.MapPath("~/images/teacher/certificate/");
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            file.SaveAs(path + Path.GetFileName(file.FileName));
                            teacherCertificate.CertificatePath = "/images/teacher/certificate/" + file.FileName;
                        }

                        if (teachObj.TeacherCertificateId == Guid.Empty)
                        {
                            teacherCertificate.Id = Guid.NewGuid();
                            teacherCertificate.TeacherId = TeacherDetail.Id;
                            _db.TeacherCertificates.Add(teacherCertificate);
                        }

                        _db.SaveChanges();

                        Response.IsError = false;
                    }
                    else
                    {
                        Response.IsError = true;
                        Response.Message = "No teacher found";
                        return RedirectToAction("TeacherProfile", "Teacher", new { Message = "Teacher not found." });
                    }
                }
            }
            catch (Exception)
            {
                Response.IsError = true;
                Response.Message = "There is some internal server error during operation. Please try again after sometime.";
            }

            return RedirectToAction("TeacherProfile", "Teacher", new { Message = "Information update successfully" });
        }

        public PartialViewResult TeacherCertificateList(Guid TeacherId)
        {
            List<TeacherCertificateVM> CertiticateList = new List<TeacherCertificateVM>();
            try
            {
                using (var _db = new SchoolMSEntities())
                {
                    CertiticateList = _db.TeacherCertificates.Where(s => s.TeacherId == TeacherId).ToList()
                                    .Select(s => new TeacherCertificateVM()
                                    {
                                        CertificatePhoto = s.CertificatePath,
                                        CertificateTitle = s.CertificateTitle,
                                        TeacherCertificateId = s.Id,
                                        TeacherId = (Guid)s.TeacherId
                                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                return PartialView("_TeacherCertificates", CertiticateList);
            }

            return PartialView("_TeacherCertificates", CertiticateList);
        }

        public JsonResult DeleteTeacherCertificate(Guid TeacherCertificateId)
        {
            JsonResponse<bool> Response = new JsonResponse<bool>();
            try
            {
                using (var _db = new SchoolMSEntities())
                {
                    var ExistingTeacherCertificateDetail = _db.TeacherCertificates.Find(TeacherCertificateId);

                    if (ExistingTeacherCertificateDetail != null)
                    {
                        _db.TeacherCertificates.Remove(ExistingTeacherCertificateDetail);
                        _db.SaveChanges();
                    }
                    else
                    {
                        Response.IsError = true;
                        Response.Message = "Teacher Certificate details not found.";
                        return Json(Response, JsonRequestBehavior.AllowGet);
                    }
                }

                Response.IsError = false;
            }
            catch (Exception ex)
            {
                Response.IsError = true;
                Response.Message = "";
            }

            return Json(Response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        public PartialViewResult SubjectSelection()
        {
            SubjectSelectionVM obj = new SubjectSelectionVM();
            using (var _db = new SchoolMSEntities())
            {
                var GradeList = _db.SchoolGrades.ToList().Select(g => new SelectListItem()
                {
                    Text = g.Grade,
                    Value = g.Id
                }).ToList();

                string GradeId = GradeList.First().Value;

                obj.SubjectList = _db.SchoolSubjects.Where(s => s.GradeId == GradeId).Select(g => new SelectListItem()
                {
                    Text = g.Subject,
                    Value = g.Id
                }).ToList();

                obj.GradeList = GradeList;

                return PartialView("_SubjectSelection", obj);
            }
        }

        public JsonResult SaveTeacherSubject(TeacherSubjectVM Obj)
        {
            JsonResponse<bool> Response = new JsonResponse<bool>();
            using (var _db = new SchoolMSEntities())
            {
                using (DbContextTransaction dbTran = _db.Database.BeginTransaction())
                {
                    try
                    {
                        var TeacherDetail = _db.Teachers.Find(Obj.Id);

                        if (TeacherDetail == null)
                        {
                            Response.IsError = true;
                            Response.Message = "No teacher details found";
                            dbTran.Rollback();
                            return Json(Response, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            var ExistingTeacherSubjects = _db.TeacherSubjects.Where(s => s.GradeId == Obj.GradeId).ToList();

                            if (ExistingTeacherSubjects != null && ExistingTeacherSubjects.Count() > 0)
                            {
                                _db.TeacherSubjects.RemoveRange(ExistingTeacherSubjects);
                            }

                            if (Obj.SelectedSubjects.Length > 0)
                            {
                                List<TeacherSubject> AddTeacherSubject = new List<TeacherSubject>();

                                foreach (var item in Obj.SelectedSubjects)
                                {
                                    AddTeacherSubject.Add(new TeacherSubject()
                                    {
                                        GradeId = Obj.GradeId,
                                        Id = Guid.NewGuid(),
                                        SubjectId = item,
                                        TeacherId = TeacherDetail.Id
                                    });
                                }

                                _db.TeacherSubjects.AddRange(AddTeacherSubject);
                            }

                            _db.SaveChanges();
                            dbTran.Commit();
                        }

                        Response.IsError = false;
                    }
                    catch (Exception)
                    {
                        dbTran.Rollback();
                        Response.IsError = true;
                        Response.Message = "There is some error";
                    }
                }
            }
            return Json(Response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGradeWiseSubjectDetails(string GradeId, Guid TeacherId)
        {
            JsonResponse<GradeWiseSubjects> Response = new JsonResponse<GradeWiseSubjects>();

            try
            {
                using (var _db = new SchoolMSEntities())
                {
                    var SubjectList = _db.SchoolSubjects.Where(s => s.GradeId == GradeId && s.IsDelete == false).Select(s => new SelectListItem()
                    {
                        Text = s.Subject,
                        Value = s.Id
                    }).ToList();

                    var TeacherSubjectsList = _db.TeacherSubjects.Where(s => s.TeacherId == TeacherId && s.GradeId == GradeId).ToList().Select(s => s.SubjectId).ToArray();

                    foreach (var item in SubjectList)
                    {
                        if (TeacherSubjectsList.Where(s => s == item.Value).FirstOrDefault() != null)
                        {
                            item.Selected = true;
                        }
                    }

                    Response.Data = new GradeWiseSubjects();
                    Response.Data.SelectedSubjects = TeacherSubjectsList;
                    Response.Data.SubjectList = SubjectList;
                }

                Response.IsError = false;
            }
            catch (Exception ex)
            {
                Response.IsError = true;
                Response.Message = "There is some error";
            }

            return Json(Response, JsonRequestBehavior.AllowGet);
        }

        public FileResult Download(string FilePath)
        {
            byte[] fileBytes = null;
            try
            {
                fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(FilePath));
                string fileName = Path.GetFileName(FilePath);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "");
            }
        }

        #region TeacherMaterial

        public ActionResult TeacherUploadMaterial(string Message)
        {
            ViewBag.Message = Message;
            ApplicationSession.Current.UserID = "EDC23AA9-D1D1-4E09-9858-24F37A7B1F08";
            if (!string.IsNullOrEmpty(ApplicationSession.Current.UserID))
            {
                TeacherMaterialUploadVM obj = new TeacherMaterialUploadVM();
                using (var _db = new SchoolMSEntities())
                {
                    var TeacherDetail = _db.Teachers.Find(Guid.Parse(ApplicationSession.Current.UserID));

                    if (TeacherDetail == null)
                    {
                        return RedirectToAction("Home", "Dashboard");
                    }
                    else
                    {
                        obj.TeacherId = TeacherDetail.Id;
                        obj.GradList = _db.SchoolGrades.Where(g => g.IsDelete == false).ToList().Select(g => new SelectListItem()
                        {
                            Text = g.Grade,
                            Value = g.Id
                        }).ToList();
                    }
                }

                return View("UploadMaterial", obj);
            }
            else
            {
                return RedirectToAction("Home", "Dashboard");
            }
        }

        public JsonResult GetGradeWiseSubjectList(string GradeId)
        {
            JsonResponse<List<SelectListItem>> Response = new JsonResponse<List<SelectListItem>>();
            try
            {
                using (var _db = new SchoolMSEntities())
                {
                    var SubjectList = _db.SchoolSubjects.Where(s => s.GradeId == GradeId).OrderBy(s => s.Subject).ToList();

                    Response.Data = new List<SelectListItem>();

                    foreach (var item in SubjectList)
                    {
                        Response.Data.Add(new SelectListItem() { Text = item.Subject, Value = item.Id });
                    }
                }

                Response.IsError = false;
            }
            catch (Exception)
            {
                Response.IsError = true;
                Response.Message = "";
            }

            return Json(Response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveTeacherMaterial(TeacherMaterialUploadVM teachObj, HttpPostedFileBase file)
        {
            JsonResponse<bool> Response = new JsonResponse<bool>();
            TeacherMaterial teacherMaterial = new TeacherMaterial();
            try
            {
                using (var _db = new SchoolMSEntities())
                {
                    var TeacherDetail = _db.Teachers.Find(teachObj.TeacherId);

                    if (TeacherDetail != null)
                    {
                        if (teachObj.Id != Guid.Empty)
                        {
                            teacherMaterial = _db.TeacherMaterials.Find(teachObj.Id);

                            if (teacherMaterial == null)
                            {
                                return RedirectToAction("TeacherUploadMaterial", "Teacher", new { Message = "Certificates not found" });
                            }
                        }

                        teacherMaterial.Title = teachObj.Title;
                        teacherMaterial.GradeId = teachObj.GradeId;
                        teacherMaterial.SubjectId = teachObj.SubjectId;
                        teacherMaterial.IsPublic = teachObj.IsPublic;

                        if (file != null)
                        {
                            string path = Server.MapPath("~/docs/teacher/material/" + TeacherDetail.Id + "/");
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            file.SaveAs(path + Path.GetFileName(file.FileName));
                            teacherMaterial.Path = "/docs/teacher/material/" + TeacherDetail.Id + "/" + file.FileName;
                            teacherMaterial.FileName = file.FileName;
                        }

                        if (teachObj.Id == Guid.Empty)
                        {
                            teacherMaterial.Id = Guid.NewGuid();
                            teacherMaterial.TeacherId = TeacherDetail.Id;
                            _db.TeacherMaterials.Add(teacherMaterial);
                        }

                        _db.SaveChanges();

                        Response.IsError = false;
                    }
                    else
                    {
                        Response.IsError = true;
                        Response.Message = "No teacher found";
                        return RedirectToAction("TeacherUploadMaterial", "Teacher", new { Message = "Teacher not found." });
                    }
                }
            }
            catch (Exception)
            {
                Response.IsError = true;
                Response.Message = "There is some internal server error during operation. Please try again after sometime.";
            }

            return RedirectToAction("TeacherUploadMaterial", "Teacher", new { Message = "Information save successfully" });
        }

        public PartialViewResult TeacherMaterialList(Guid TeacherId)
        {
            List<TeacherMaterialUploadView> MaterialList = new List<TeacherMaterialUploadView>();
            try
            {
                using (var _db = new SchoolMSEntities())
                {
                    MaterialList = _db.TeacherMaterials.Where(s => s.TeacherId == TeacherId).ToList()
                                    .Select(s => new TeacherMaterialUploadView()
                                    {
                                        Id = s.Id,
                                        TeacherId = (Guid)s.TeacherId,
                                        FileName = s.FileName,
                                        GradeName = s.SchoolGrade.Grade,
                                        SubjectName = s.SchoolSubject?.Subject,
                                        IsPublic = Convert.ToBoolean(s.IsPublic),
                                        Path = s.Path,
                                        Title = s.Title
                                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                return PartialView("_TeacherMaterials", MaterialList);
            }

            return PartialView("_TeacherMaterials", MaterialList);
        }

        public JsonResult EditTeacherMaterial(Guid TeacherMaterialId)
        {
            JsonResponse<TeacherMaterialUploadVM> Response = new JsonResponse<TeacherMaterialUploadVM>();
            try
            {

                using (var _db = new SchoolMSEntities())
                {
                    var ExistingMaterialDetail = _db.TeacherMaterials.Find(TeacherMaterialId);

                    if (ExistingMaterialDetail != null)
                    {
                        Response.Data = new TeacherMaterialUploadVM();
                        Response.Data.GradList = _db.SchoolGrades.Where(g => g.IsDelete == false).ToList().Select(g => new SelectListItem()
                        {
                            Text = g.Grade,
                            Value = g.Id
                        }).ToList();

                        Response.Data.SubjectList = _db.SchoolSubjects.Where(g => g.IsDelete == false && g.GradeId == ExistingMaterialDetail.GradeId).ToList().Select(g => new SelectListItem()
                        {
                            Text = g.Subject,
                            Value = g.Id
                        }).ToList();

                        Response.Data.Id = ExistingMaterialDetail.Id;
                        Response.Data.Title = ExistingMaterialDetail.Title;
                        Response.Data.FileName = ExistingMaterialDetail.FileName;
                        Response.Data.GradeId = ExistingMaterialDetail.GradeId;
                        Response.Data.IsPublic = Convert.ToBoolean(ExistingMaterialDetail.IsPublic);
                        Response.Data.Path = ExistingMaterialDetail.Path;
                        Response.Data.SubjectId = ExistingMaterialDetail.SubjectId;
                        Response.Data.TeacherId = (Guid)ExistingMaterialDetail.TeacherId;

                        Response.IsError = false;
                    }
                    else
                    {
                        Response.IsError = true;
                        Response.Message = "Material details not found";
                    }
                }
            }
            catch (Exception ex)
            {
                Response.IsError = true;
                Response.Message = "There is some internal server error";
            }

            return Json(Response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteTeacherMaterial(Guid TeacherMaterialId)
        {
            JsonResponse<bool> Response = new JsonResponse<bool>();
            try
            {
                using (var _db = new SchoolMSEntities())
                {
                    var ExistingTeacheeMaterialDetail = _db.TeacherMaterials.Find(TeacherMaterialId);

                    if (ExistingTeacheeMaterialDetail != null)
                    {
                        _db.TeacherMaterials.Remove(ExistingTeacheeMaterialDetail);
                        _db.SaveChanges();
                    }
                    else
                    {
                        Response.IsError = true;
                        Response.Message = "Teacher Marerial details not found.";
                        return Json(Response, JsonRequestBehavior.AllowGet);
                    }
                }

                Response.IsError = false;
            }
            catch (Exception ex)
            {
                Response.IsError = true;
                Response.Message = "";
            }

            return Json(Response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TeacherListing()
        {
            List<ViewTeacherVM> teacherVM = new List<ViewTeacherVM>();

            using (var _db = new SchoolMSEntities())
            {
                var teacherlist = _db.Teachers.ToList();

                foreach (var item in teacherlist)
                {
                    teacherVM.Add(new ViewTeacherVM() {
                        ProfilePhoto = item.ProfilePhoto,
                        TeacherName = item.Name,
                        Subjects = item.TeacherSubjects.Where(t => t.TeacherId == item.Id).Select(t => t.SchoolSubject.Subject).ToArray(),
                        TeacherId = item.Id
                    });
                }
            }

            return View(teacherVM);
        }

        public JsonResult SaveRatings(TeacherRatingVM teacherDetail)
        {
            TeacherRating teacherRating = new TeacherRating();

            using (var _db = new SchoolMSEntities())
            {
                teacherRating = _db.TeacherRatings.ToList().Where(r => r.TeacherId == (Guid)teacherDetail.TeacherId && r.StudentId == Guid.Parse(ApplicationSession.Current.UserID)).FirstOrDefault();

                if (teacherRating != null)
                {
                    teacherRating.RatingValue = teacherDetail.RatingValue;
                    teacherRating.Comments = teacherDetail.Comments;

                    _db.SaveChanges();
                }
                else
                {
                    TeacherRating newRating = new TeacherRating();

                    newRating.RatingValue = teacherDetail.RatingValue;
                    newRating.Comments = teacherDetail.Comments;
                    newRating.Id = Guid.NewGuid();
                    newRating.StudentId = Guid.Parse(ApplicationSession.Current.UserID);
                    newRating.TeacherId = teacherDetail.TeacherId;

                    _db.TeacherRatings.Add(newRating);
                    _db.SaveChanges();
                }
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}