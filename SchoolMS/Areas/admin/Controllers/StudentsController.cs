
using SchoolMS.Areas.admin.ViewModel;
using SchoolMS.Common;
using SchoolMS.Models.DB;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace SchoolMS.Areas.admin.Controllers
{
    public class StudentsController : Controller
    {

        public StudentsController()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
        }

        [Route("admin/studentlist")]
        public ActionResult List()
        {
            return View("StudentRequestList");
        }

        public JsonResult GetStudentList()
        {
            JsonResponse<List<StudentVM>> Response = new JsonResponse<List<StudentVM>>();
            try
            {
                Response.Data = new List<StudentVM>();
                using (var _db = new SchoolMSEntities())
                {
                    Response.Data = _db.Students.ToList().Where(s => s.IsDelete == false).ToList()
                                            .Select(s => new StudentVM()
                                            {
                                                BirthDate = s.BirthDate,
                                                CreateBy = s.CreateBy,
                                                CreateDate = s.CreateDate,
                                                Email = s.Email,
                                                Facetime = s.Facetime,
                                                FromCountry = s.FromCountry,
                                                FromState = s.FromState,
                                                LivingCountry = s.LivingCountry,
                                                LivingState = s.LivingState,
                                                Gender = s.Gender,
                                                GoogleHangout = s.GoogleHangout,
                                                Id = s.Id,
                                                Isblock = s.Isblock,
                                                IsDelete = s.IsDelete,
                                                Name = s.Name,
                                                NativeLanguage = s.NativeLanguage,
                                                ProfilePhoto = s.ProfilePhoto,
                                                QQ = s.QQ,
                                                SchoolGrade = s.SchoolGrade,
                                                Skype = s.Skype,
                                                TimeZone = s.TimeZone,
                                                UpdateBy = s.UpdateBy,
                                                UpdateDate = s.UpdateDate,
                                                Webchat = s.Webchat
                                            }).ToList();
                }
                Response.IsError = false;
            }
            catch (Exception ex)
            {
                Response.IsError = true;
                Response.Message = "Internal Server Error";
            }
            return Json(Response, JsonRequestBehavior.AllowGet);
        }

        [Route("admin/student/{id}")]
        [Route("admin/student/new")]
        public ActionResult RegisterStudent(string Id)
        {
            StudentVM obj = new StudentVM();
            
            obj.TimeZoneInfos = TimeZoneInfo.GetSystemTimeZones()
                                .Select(t => new
                                SelectListItem()
                                {
                                    Text = t.DisplayName,
                                    Value = t.Id
                                }).ToList();

            if (!String.IsNullOrEmpty(Id))
            {
                Student student = new Student();

                using (var _db = new SchoolMSEntities())
                {
                    student = _db.Students.Find(Id);
                }

                if (student != null)
                {
                    obj.Email = student.Email;
                    obj.BirthDate = student.BirthDate;
                    obj.Facetime = student.Facetime;
                    obj.FromCountry = student.FromCountry;
                    obj.FromState = student.FromState;
                    obj.Gender = student.Gender != null ? Convert.ToInt32(student.Gender) : 0;
                    obj.GoogleHangout = student.GoogleHangout;
                    obj.LivingCountry = student.LivingCountry;
                    obj.LivingState = student.LivingState;
                    obj.Name = student.Name;
                    obj.NativeLanguage = student.NativeLanguage;
                    obj.QQ = student.QQ;
                    obj.SchoolGrade = student.SchoolGrade;
                    obj.Skype = student.Skype;
                    obj.Webchat = student.Webchat;
                    obj.ProfilePhoto = !String.IsNullOrEmpty(student.ProfilePhoto) ? "/Areas/admin/Images/Students/" + student.ProfilePhoto : Server.MapPath("/images/NoImageAvailable.png");
                    obj.TimeZone = student.TimeZone;
                }
                else
                {
                    obj.ProfilePhoto = Server.MapPath("/images/NoImageAvailable.png");
                }
            }
            else
            {
                obj.ProfilePhoto = Server.MapPath("/images/NoImageAvailable.png");
            }
            return View("RegisterStudent", obj);
        }

        public JsonResult SaveStudent(StudentVM obj, HttpPostedFileBase file)
        {
            JsonResponse<bool> Response = new JsonResponse<bool>();
            try
            {
                Student student = new Student();

                if (obj.Id != Guid.Empty)
                {
                    using (var _db = new SchoolMSEntities())
                    {
                        student = _db.Students.Find(obj.Id);
                    }

                    if (student == null)
                    {
                        Response.IsError = true;
                        Response.Message = "Student not found";
                        return Json(Response, JsonRequestBehavior.AllowGet);
                    }
                }

                student.BirthDate = obj.BirthDate;
                student.Email = obj.Email;
                student.Facetime = obj.Facetime;
                student.FromCountry = obj.FromCountry;
                student.FromState = obj.FromState;
                student.Gender = obj.Gender;
                student.GoogleHangout = obj.GoogleHangout;
                student.LivingCountry = obj.LivingCountry;
                student.LivingState = obj.LivingState;
                student.Name = obj.Name;
                student.NativeLanguage = obj.NativeLanguage;
                student.QQ = obj.QQ;
                student.SchoolGrade = obj.SchoolGrade;
                student.Skype = obj.Skype;
                student.TimeZone = obj.TimeZone;
                student.Webchat = obj.Webchat;

                //Need to work on it
                //student.ProfilePhoto = obj.ProfilePhoto;

                if (file != null)
                {
                    string path = Server.MapPath("~/Areas/admin/Images/Students/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    file.SaveAs(path + Path.GetFileName(file.FileName));
                    student.ProfilePhoto = file.FileName;
                }

                if (obj.Id == Guid.Empty)
                {
                    student.Id = Guid.NewGuid();
                    student.CreateBy = ApplicationSession.Current.UserID;
                    student.CreateDate = DateTime.Now;
                    student.IsDelete = false;
                    student.Isblock = false;

                    using (var _db = new SchoolMSEntities())
                    {
                        _db.Students.Add(student);
                    }
                }
                else
                {
                    student.UpdateBy = ApplicationSession.Current.UserID;
                    student.UpdateDate = DateTime.Now;
                }

                using (var _db = new SchoolMSEntities())
                {
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Response.IsError = true;
                Response.Message = "Internal server error";
            }
            return Json(Response, JsonRequestBehavior.AllowGet);
        }

        [Route("admin/student/delete/{id}")]
        public JsonResult DeleteStudent(string id)
        {
            JsonResponse<bool> Response = new JsonResponse<bool>();

            try
            {
                Student student;
                using (var _db = new SchoolMSEntities())
                {
                    student = _db.Students.Find(id);
                }

                if (student == null)
                {
                    Response.IsError = true;
                    Response.Data = false;
                    Response.Message = "Student not found.";
                    return Json(Response, JsonRequestBehavior.AllowGet);
                }

                using (var _db = new SchoolMSEntities())
                {
                    _db.Students.Remove(student);
                    //_db.SaveChanges();
                }

                using (var _db = new SchoolMSEntities())
                {
                    _db.SaveChanges();
                }

                Response.IsError = false;
                Response.Data = true;
            }
            catch (Exception ex)
            {
                Response.IsError = true;
                Response.Message = ex.Message != null ? ex.Message : ex.InnerException.Message;
            }
            return Json(Response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult BlockStudents(List<Guid> blockstudentslist)
        {
            JsonResponse<string> Response = new JsonResponse<string>();
            try
            {
                if (blockstudentslist == null)
                {
                    Response.IsError = true;
                    Response.Message = "No student selected";
                }
                else
                {
                    using (var _db = new SchoolMSEntities())
                    {
                        Response.Data = "Selected students have been blocked successfully.";
                        bool IsNotFoundOnce = false;
                        foreach (var item in blockstudentslist)
                        {
                            var studentInfo = _db.Students.Where(s => s.Id == item).FirstOrDefault();

                            if (studentInfo != null)
                            {
                                studentInfo.Isblock = true;
                            }
                            else
                            {
                                if (!IsNotFoundOnce)
                                {
                                    Response.Data += " Some students not found";
                                    IsNotFoundOnce = true;
                                }
                            }

                            _db.SaveChanges();
                        }
                    }

                    Response.IsError = false;
                }
            }
            catch (Exception ex)
            {
                Response.IsError = true;
                Response.Message = "Internal Server Error";
            }

            return Json(Response, JsonRequestBehavior.AllowGet);
        }
    }
}