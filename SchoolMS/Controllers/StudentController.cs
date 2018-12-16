using SchoolMS.Models.ViewModels;
using SchoolMS.Common;
using SchoolMS.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace SchoolMS.Controllers
{
    public class StudentController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult ViewProfile()
        {
            if (!string.IsNullOrEmpty(ApplicationSession.Current.UserID))
            {
                StudentInfoViewVM studViewObj = new StudentInfoViewVM();
                try
                {
                    using (var _db = new SchoolMSEntities())
                    {
                        var student = _db.Students.Find(Guid.Parse(ApplicationSession.Current.UserID));

                        if (student != null)
                        {
                            studViewObj.StudentName = student.Name;
                            TimeZoneInfo zone = TimeZoneInfo.GetSystemTimeZones().Where(t => t.Id == student.TimeZone).FirstOrDefault();
                            DateTime utcTime1 = new DateTime();
                            utcTime1 = DateTime.UtcNow;
                            //DateTimeOffset time2 = new DateTimeOffset(utcTime1,
                            //TimeZoneInfo.FindSystemTimeZoneById(zone.Id).GetUtcOffset(utcTime1));
                            //studViewObj.TimeZone = time2.Hour + ":" + time2.Minute;

                            if (zone != null)
                            {
                                TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById(zone.Id);

                                DateTime userTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime1, timeInfo);
                                studViewObj.TimeZone += userTime.Hour + ":" + userTime.Minute;
                                ;
                            }
                            
                            studViewObj.TimeZone += ", " + TimeZoneInfo.GetSystemTimeZones().Where(t => t.Id == student.TimeZone).FirstOrDefault()?.DisplayName;

                            studViewObj.From = (student.Gender != 1 ? "Male" : "Female") + ", From India";

                            if (student.StudentSubjects != null)
                            {
                                foreach (var item in student.StudentSubjects)
                                {
                                    studViewObj.Learning += item.SchoolSubject?.Subject + ", ";
                                }
                            }

                            studViewObj.ProfilePhoto = student.ProfilePhoto;
                        }
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Home", "Dashboard");
                }
                return View(studViewObj);
            }
            else
            {
                return RedirectToAction("Home", "Dashboard");
            }
        }

        public ActionResult StudentProfile()
        {
            if (!string.IsNullOrEmpty(ApplicationSession.Current.UserID))
            {
                StudentVM studObj = new StudentVM();
                try
                {
                    using (var _db = new SchoolMSEntities())
                    {
                        studObj.TimeZoneInfos = TimeZoneInfo.GetSystemTimeZones()
                                            .Select(t => new
                                            SelectListItem()
                                            {
                                                Text = t.DisplayName,
                                                Value = t.Id
                                            }).ToList();

                        studObj.GradeList = _db.SchoolGrades.ToList()
                                            .Select(g => new SelectListItem()
                                            {
                                                Text = g.Grade,
                                                Value = g.Id.ToString()
                                            }).ToList();

                        var student = _db.Students.Find(Guid.Parse(ApplicationSession.Current.UserID));

                        if (student != null)
                        {
                            studObj.BirthDate = student.BirthDate;

                            if (studObj.BirthDate != null)
                            {
                                DateTime dtBirth = Convert.ToDateTime(student.BirthDate);
                                studObj.BirthDateOnly = dtBirth.Day;
                                studObj.BirthMonthOnly = dtBirth.Month;
                                studObj.BirthYearOnly = dtBirth.Year;
                            }
                            else
                            {
                                DateTime dt = new DateTime();
                                dt = DateTime.Today;
                                studObj.BirthYearOnly = dt.Year - 60;
                                studObj.BirthMonthOnly = dt.Month;
                                studObj.BirthDateOnly = dt.Day;
                            }

                            studObj.Email = student.Email;
                            studObj.Facetime = student.Facetime;
                            studObj.FromCountry = student.FromCountry;
                            studObj.FromState = student.FromState;
                            studObj.Gender = student.Gender;
                            studObj.GoogleHangout = student.GoogleHangout;
                            studObj.Id = student.Id;
                            studObj.LivingCountry = student.LivingCountry;
                            studObj.LivingState = student.LivingState;
                            studObj.Name = student.Name;
                            studObj.NativeLanguage = student.NativeLanguage;
                            studObj.ProfilePhoto = student.ProfilePhoto;
                            studObj.QQ = student.QQ;
                            studObj.SchoolGrade = student.SchoolGrade;
                            studObj.Skype = student.Skype;
                            studObj.TimeZone = student.TimeZone;
                            studObj.Webchat = student.Webchat;
                            studObj.SchoolName = student.SchoolName;
                            studObj.ShortIntroduction = student.ShortIntroduction;
                            studObj.LongIntroduction = student.LongIntroduction;
                            studObj.LookingFor = student.LookingFor;

                            var StudentSubjects = _db.StudentSubjects.Where(s => s.StudentId == studObj.Id).ToList();

                            foreach (var item in StudentSubjects)
                            {

                                var SubjectList = _db.SchoolSubjects.Where(s => s.GradeId == item.SchoolGradeId).ToList().Select(s => new SelectListItem()
                                {
                                    Text = s.Subject,
                                    Value = s.Id
                                }).ToList();

                                studObj.StudentSubjectList.Add(new StudentSchoolSubjects()
                                {
                                    GradeId = item.SchoolGradeId,
                                    SubjectId = item.SchoolSubjectId,
                                    IsLearning = item.IsLearning,
                                    IsPrimary = item.IsPrimary,
                                    SubjectList = SubjectList
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Home", "Dashboard");
                }
                return View(studObj);
            }
            else
            {
                return RedirectToAction("Home", "Dashboard");
            }
            //return View();
        }

        public ActionResult SaveStudentPersonalInfo(StudentVM studObj, HttpPostedFileBase file)
        {
            JsonResponse<bool> Response = new JsonResponse<bool>();
            try
            {
                using (var _db = new SchoolMSEntities())
                {
                    var StudentDetail = _db.Students.Find(studObj.Id);

                    if (StudentDetail != null)
                    {
                        StudentDetail.Name = studObj.Name;
                        StudentDetail.FromCountry = studObj.FromCountry;
                        StudentDetail.FromState = studObj.FromState;
                        StudentDetail.LivingCountry = studObj.LivingCountry;
                        StudentDetail.LivingState = studObj.LivingState;
                        StudentDetail.TimeZone = studObj.TimeZone;
                        StudentDetail.BirthDate = new DateTime(studObj.BirthYearOnly, studObj.BirthMonthOnly, studObj.BirthDateOnly);
                        StudentDetail.Gender = studObj.Gender;

                        if (file != null)
                        {
                            string path = Server.MapPath("~/images/student/");
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            file.SaveAs(path + Path.GetFileName(file.FileName));
                            StudentDetail.ProfilePhoto = file.FileName;
                        }

                        _db.SaveChanges();

                        Response.IsError = false;
                    }
                    else
                    {
                        Response.IsError = true;
                        Response.Message = "No student found";
                    }
                }
            }
            catch (Exception)
            {
                Response.IsError = true;
                Response.Message = "There is some internal server error during operation. Please try again after sometime.";
            }

            return RedirectToAction("StudentProfile", "Student");
            //return Json(Response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveStudentProfileInfo(StudentVM studObj)
        {
            JsonResponse<bool> Response = new JsonResponse<bool>();
            try
            {
                using (var _db = new SchoolMSEntities())
                {
                    var StudentDetail = _db.Students.Find(studObj.Id);

                    if (StudentDetail != null)
                    {
                        StudentDetail.SchoolName = studObj.SchoolName;
                        StudentDetail.SchoolGrade = studObj.SchoolGrade;
                        StudentDetail.LookingFor = studObj.LookingFor;
                        StudentDetail.ShortIntroduction = studObj.ShortIntroduction;
                        StudentDetail.LongIntroduction = studObj.LongIntroduction;

                        _db.SaveChanges();

                        Response.IsError = false;
                    }
                    else
                    {
                        Response.IsError = true;
                        Response.Message = "No student found";
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

        public JsonResult SaveStudentCommInfo(StudentVM studObj)
        {
            JsonResponse<bool> Response = new JsonResponse<bool>();
            try
            {
                using (var _db = new SchoolMSEntities())
                {
                    var StudentDetail = _db.Students.Find(studObj.Id);

                    if (StudentDetail != null)
                    {
                        StudentDetail.Skype = studObj.Skype;
                        StudentDetail.Facetime = studObj.Facetime;
                        StudentDetail.GoogleHangout = studObj.GoogleHangout;
                        StudentDetail.QQ = studObj.QQ;
                        StudentDetail.Webchat = studObj.Webchat;

                        _db.SaveChanges();

                        Response.IsError = false;
                    }
                    else
                    {
                        Response.IsError = true;
                        Response.Message = "No student found";
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

        public JsonResult SaveStudentSubjects(StudentSubjectVM StudentSubjectDetail)
        {
            JsonResponse<bool> Response = new JsonResponse<bool>();
            try
            {
                if (StudentSubjectDetail.StudentId != Guid.Empty)
                {
                    using (var _db = new SchoolMSEntities())
                    {

                        var StudentDetail = _db.Students.Find(StudentSubjectDetail.StudentId);

                        if (StudentDetail == null)
                        {
                            Response.IsError = true;
                            Response.Message = "No student found";
                            return Json(Response, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            foreach (var item in StudentSubjectDetail.subjectList)
                            {
                                StudentSubject studSubj = _db.StudentSubjects.Where(s => s.StudentId == StudentSubjectDetail.StudentId && s.SchoolGradeId == item.GradeId && s.SchoolSubjectId == item.SubjectId).FirstOrDefault();

                                if (studSubj != null)
                                {
                                    if (item.IsDelete)
                                    {
                                        _db.StudentSubjects.Remove(studSubj);
                                    }
                                    else
                                    {
                                        studSubj.IsLearning = item.IsLearning;
                                        studSubj.IsPrimary = item.IsPrimary;
                                        studSubj.UpdateBy = ApplicationSession.Current.UserID;
                                        studSubj.UpdateDate = DateTime.Today;

                                        _db.SaveChanges();
                                    }
                                }
                                else
                                {
                                    studSubj = new StudentSubject();
                                    studSubj.Id = Guid.NewGuid().ToString();
                                    studSubj.IsLearning = item.IsLearning;
                                    studSubj.IsPrimary = item.IsPrimary;
                                    studSubj.SchoolGradeId = item.GradeId;
                                    studSubj.SchoolSubjectId = item.SubjectId;
                                    studSubj.StudentId = StudentSubjectDetail.StudentId;
                                    studSubj.CreateBy = ApplicationSession.Current.UserID;
                                    studSubj.CreateDate = DateTime.Today;

                                    _db.StudentSubjects.Add(studSubj);
                                    _db.SaveChanges();
                                }
                            }
                        }
                    }
                    Response.IsError = false;
                }
                else
                {
                    Response.IsError = true;
                    Response.Message = "Student record not found";
                }
            }
            catch (Exception)
            {
                Response.IsError = true;
                Response.Message = "There is some internal server error. Please try again after some time.";
            }
            return Json(Response, JsonRequestBehavior.AllowGet);
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

        public ActionResult Materials()
        {
            if (ApplicationSession.Current.IsLoggedIn && ApplicationSession.Current.LoginUser == "Student")
            {
                MaterialViewVM Obj = new MaterialViewVM();
                try
                {
                    using (var _db = new SchoolMSEntities())
                    {
                        Obj.GradeList = _db.SchoolGrades.Where(g => g.IsDelete == false)
                                            .Select(g => new SelectListItem()
                                            {
                                                Text = g.Grade,
                                                Value = g.Id
                                            }).ToList();
                    }
                }
                catch (Exception ex)
                {
                    return View(Obj);
                }
                return View(Obj);
            }
            else
            {
                return RedirectToAction("Dashboard");
            }
        }

        public PartialViewResult MaterialList(string GradeId, string SubjectId)
        {
            List<MaterialDownloadList> MaterialList = new List<MaterialDownloadList>();
            try
            {
                using (var _db = new SchoolMSEntities())
                {
                    MaterialList = _db.TeacherMaterials.Where(m => m.IsPublic == true && m.GradeId == GradeId && m.SubjectId == SubjectId).ToList().Select(m => new MaterialDownloadList()
                    {
                        FileName = m.FileName,
                        GradeName = m.SchoolGrade?.Grade ?? "",
                        Path = m.Path,
                        SubjectName = m.SchoolSubject?.Subject ?? "",
                        Title = m.Title
                    }).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return PartialView("_MaterialList",MaterialList);
        }
    }
}