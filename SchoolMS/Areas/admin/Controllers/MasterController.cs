using SchoolMS.Areas.admin.ViewModel;
using SchoolMS.Common;
using SchoolMS.Models.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolMS.Areas.admin.Controllers
{
    public class MasterController : Controller
    {
        #region School Grade
        [Route("admin/gradelist")]
        public ActionResult SchoolGrade()
        {
            return View();
        }

        public JsonResult GetSchoolGradeList()
        {
            JsonResponse<List<SchoolGradeVM>> Response = new JsonResponse<List<SchoolGradeVM>>();
            try
            {
                using (var _db = new SchoolMSEntities())
                {
                    Response.Data = _db.SchoolGrades.Where(s => s.IsDelete == false).ToList()
                                                .Select(s => new SchoolGradeVM()
                                                {
                                                    Grade = s.Grade,
                                                    CreateBy = s.CreateBy,
                                                    CreateDate = s.CreateDate,
                                                    GradeId = s.Id,
                                                    IsDelete = s.IsDelete,
                                                    UpdateBy = s.UpdateBy,
                                                    UpdateDate = s.UpdateDate
                                                }).ToList();

                    if (Response.Data == null)
                    {
                        Response.Data = new List<SchoolGradeVM>();
                    }
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

        //[HttpPost]
        //[Route("admin/savegrade")]
        public JsonResult SaveGrade(SchoolGradeVM obj)
        {
            JsonResponse<bool> Response = new JsonResponse<bool>();
            try
            {
                SchoolGrade schoolGrade = new SchoolGrade();

                if (!String.IsNullOrEmpty(obj.GradeId))
                {
                    using (var _db = new SchoolMSEntities())
                    {
                        schoolGrade = _db.SchoolGrades.Find(obj.GradeId);

                        if (schoolGrade == null)
                        {
                            Response.IsError = true;
                            Response.Message = "School grade not found";
                            return Json(Response, JsonRequestBehavior.AllowGet);
                        }

                        schoolGrade.Grade = obj.Grade;
                        schoolGrade.UpdateBy = ApplicationSession.Current.UserID;
                        schoolGrade.UpdateDate = DateTime.Now;
                        _db.SaveChanges();
                    }
                }
                else
                {
                    schoolGrade.Grade = obj.Grade;
                    schoolGrade.Id = Guid.NewGuid().ToString();
                    schoolGrade.CreateBy = ApplicationSession.Current.UserID;
                    schoolGrade.CreateDate = DateTime.Now;
                    schoolGrade.IsDelete = false;

                    using (var _db = new SchoolMSEntities())
                    {
                        _db.SchoolGrades.Add(schoolGrade);
                        _db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Response.IsError = true;
                Response.Message = "Internal server error";
            }
            return Json(Response, JsonRequestBehavior.AllowGet);
        }

        //not usefull as of now because entry done directly from datatable in client side only.
        [Route("admin/grade/{id}")]
        [Route("admin/grade/new")]
        public ActionResult GradeEntry(string Id)
        {
            SchoolGradeVM obj = new SchoolGradeVM();

            if (!String.IsNullOrEmpty(Id))
            {
                var schoolGrade = new SchoolGrade();
                using (var _db = new SchoolMSEntities())
                {
                    schoolGrade = _db.SchoolGrades.Find(Id);
                }

                if (schoolGrade != null)
                {
                    obj.Grade = schoolGrade.Grade;
                }
            }
            return View(obj);
        }

        [Route("admin/grade/delete/{id}")]
        public JsonResult DeleteGrade(string id)
        {
            JsonResponse<bool> Response = new JsonResponse<bool>();

            try
            {
                SchoolGrade schoolGrade;
                using (var _db = new SchoolMSEntities())
                {
                    schoolGrade = _db.SchoolGrades.Find(id);

                    if (schoolGrade == null)
                    {
                        Response.IsError = true;
                        Response.Data = false;
                        Response.Message = "Grade not found.";
                        return Json(Response, JsonRequestBehavior.AllowGet);
                    }

                    if (schoolGrade.SchoolSubjects != null && schoolGrade.SchoolSubjects.Count() > 0)
                    {
                        _db.SchoolSubjects.RemoveRange(schoolGrade.SchoolSubjects);
                    }
                    
                    _db.SchoolGrades.Remove(schoolGrade);
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
        #endregion School Grade

        #region School Subject
        [Route("admin/subjectlist")]
        public ActionResult SchoolSubject()
        {
            return View();
        }

        public JsonResult GetSchoolSubjectList()
        {
            JsonResponse<List<SchoolSubjectVM>> Response = new JsonResponse<List<SchoolSubjectVM>>();
            try
            {
                using (var _db = new SchoolMSEntities())
                {
                    Response.Data = _db.SchoolSubjects.Where(s => s.IsDelete == false).ToList()
                                                .Select(s => new SchoolSubjectVM()
                                                {
                                                    GradeName = s.SchoolGrade != null ? s.SchoolGrade.Grade : "",
                                                    Subject = s.Subject,
                                                    GradeId = s.GradeId,
                                                    CreateBy = s.CreateBy,
                                                    CreateDate = s.CreateDate,
                                                    SchoolSubjectId = s.Id,
                                                    IsDelete = s.IsDelete,
                                                    UpdateBy = s.UpdateBy,
                                                    UpdateDate = s.UpdateDate
                                                }).ToList();

                    if (Response.Data == null)
                    {
                        Response.Data = new List<SchoolSubjectVM>();
                    }
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
        
        public JsonResult SaveSubject(SchoolSubjectVM obj)
        {
            JsonResponse<bool> Response = new JsonResponse<bool>();
            try
            {
                SchoolSubject schoolSubject = new SchoolSubject();

                if (!String.IsNullOrEmpty(obj.SchoolSubjectId))
                {
                    using (var _db = new SchoolMSEntities())
                    {
                        schoolSubject = _db.SchoolSubjects.Find(obj.SchoolSubjectId);

                        if (schoolSubject == null)
                        {
                            Response.IsError = true;
                            Response.Message = "School subject not found";
                            return Json(Response, JsonRequestBehavior.AllowGet);
                        }

                        schoolSubject.GradeId = obj.GradeId;
                        schoolSubject.Subject = obj.Subject;
                        schoolSubject.UpdateBy = ApplicationSession.Current.UserID;
                        schoolSubject.UpdateDate = DateTime.Now;
                        _db.SaveChanges();
                    }
                }
                else
                {
                    schoolSubject.GradeId = obj.GradeId;
                    schoolSubject.Subject = obj.Subject;
                    schoolSubject.Id = Guid.NewGuid().ToString();
                    schoolSubject.CreateBy = ApplicationSession.Current.UserID;
                    schoolSubject.CreateDate = DateTime.Now;
                    schoolSubject.IsDelete = false;

                    using (var _db = new SchoolMSEntities())
                    {
                        _db.SchoolSubjects.Add(schoolSubject);
                        _db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Response.IsError = true;
                Response.Message = "Internal server error";
            }
            return Json(Response, JsonRequestBehavior.AllowGet);
        }

        //not usefull as of now because entry done directly from datatable in client side only.
        [Route("admin/subject/{id}")]
        [Route("admin/subject/new")]
        public ActionResult SubjectEntry(string Id)
        {
            SchoolSubjectVM obj = new SchoolSubjectVM();

            if (!String.IsNullOrEmpty(Id))
            {
                var schoolSubject = new SchoolSubject();
                using (var _db = new SchoolMSEntities())
                {
                    schoolSubject = _db.SchoolSubjects.Find(Id);
                }

                if (schoolSubject != null)
                {
                    obj.GradeId = schoolSubject.GradeId;
                    obj.Subject = schoolSubject.Subject;
                }
            }
            return View(obj);
        }

        [Route("admin/subject/delete/{id}")]
        public JsonResult DeleteSubject(string id)
        {
            JsonResponse<bool> Response = new JsonResponse<bool>();

            try
            {
                SchoolSubject schoolSubject;
                using (var _db = new SchoolMSEntities())
                {
                    schoolSubject = _db.SchoolSubjects.Find(id);

                    if (schoolSubject == null)
                    {
                        Response.IsError = true;
                        Response.Data = false;
                        Response.Message = "Subject not found.";
                        return Json(Response, JsonRequestBehavior.AllowGet);
                    }

                    _db.SchoolSubjects.Remove(schoolSubject);
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

        public JsonResult GetGradeListForSchoolSubject()
        {
            JsonResponse<List<SelectListItem>> Response = new JsonResponse<List<SelectListItem>>();
            try
            {
                using (var _db = new SchoolMSEntities())
                {
                    Response.Data = _db.SchoolGrades.Where(s => s.IsDelete == false).ToList()
                                                .Select(s => new SelectListItem()
                                                {
                                                   Value = s.Id,
                                                   Text = s.Grade
                                                }).ToList();

                    if (Response.Data == null)
                    {
                        Response.Data = new List<SelectListItem>();
                    }
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
        #endregion School Subject

        #region SliderConfiguration
        [Route("admin/sliderlist")]
        public ActionResult ManageSlider()
        {
            return View();
        }

        public JsonResult GetSliderList()
        {
            JsonResponse<List<SliderConfigVM>> Response = new JsonResponse<List<SliderConfigVM>>();
            try
            {
                using (var _db = new SchoolMSEntities())
                {
                    Response.Data = _db.SliderConfigs.ToList()
                                                .Select(s => new SliderConfigVM()
                                                {
                                                    SliderConfigId = s.Id,
                                                    Description = s.Description,
                                                    SliderImage = s.SliderImage,
                                                    CreateBy = s.CreateBy,
                                                    CreatedOn = s.CreatedOn,
                                                    IsActive = s.IsActive,
                                                    UpdateBy = s.UpdateBy,
                                                    UpdatedOn = s.UpdatedOn
                                                }).ToList();

                    if (Response.Data == null)
                    {
                        Response.Data = new List<SliderConfigVM>();
                    }
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

        [Route("admin/slider/{id}")]
        [Route("admin/slider/new")]
        public ActionResult Slider(string Id)
        {
            SliderConfigVM obj = new SliderConfigVM();

            if (!String.IsNullOrEmpty(Id))
            {
                var sliderConfig = new SliderConfig();
                using (var _db = new SchoolMSEntities())
                {
                    sliderConfig = _db.SliderConfigs.Find(Id);
                }

                if (sliderConfig != null)
                {
                    obj.Description = sliderConfig.Description;
                    obj.SliderImage = sliderConfig.SliderImage;
                }
            }
            return View("ManageSlider");
        }

        [Route("admin/slider/delete/{id}")]
        public JsonResult DeleteSlider(string id)
        {
            JsonResponse<bool> Response = new JsonResponse<bool>();

            try
            {
                SliderConfig sliderConfig;
                using (var _db = new SchoolMSEntities())
                {
                    sliderConfig = _db.SliderConfigs.Find(id);

                    if (sliderConfig == null)
                    {
                        Response.IsError = true;
                        Response.Data = false;
                        Response.Message = "Slider not found.";
                        return Json(Response, JsonRequestBehavior.AllowGet);
                    }

                    _db.SliderConfigs.Remove(sliderConfig);
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

        public JsonResult SaveSlider(SliderConfigVM obj)
        {
            JsonResponse<bool> Response = new JsonResponse<bool>();
            try
            {
                SliderConfig sliderConfig = new SliderConfig();

                string[] FileName = new string[Request.Files.Count];
                HttpFileCollectionBase files = Request.Files;
                string fname = String.Empty;
                bool IsImageUploaded = false;
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase file = files[i];
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                        FileName[i] = file.FileName;
                    }

                    if (!Directory.Exists(Server.MapPath("~/images/")))
                    {
                        Directory.CreateDirectory("~/images/");
                    }
                    fname = Path.Combine(Server.MapPath("~/images/"), fname);
                    file.SaveAs(fname);
                    IsImageUploaded = true;
                    obj.SliderImage = "images/" + file.FileName;
                }

                if (!String.IsNullOrEmpty(obj.SliderConfigId))
                {
                    using (var _db = new SchoolMSEntities())
                    {
                        sliderConfig = _db.SliderConfigs.Find(obj.SliderConfigId);

                        if (sliderConfig == null)
                        {
                            Response.IsError = true;
                            Response.Message = "Slider not found";
                            return Json(Response, JsonRequestBehavior.AllowGet);
                        }

                        sliderConfig.Description = obj.Description;
                        if (IsImageUploaded)
                        {
                            sliderConfig.SliderImage = obj.SliderImage;
                        }
                        sliderConfig.UpdateBy = ApplicationSession.Current.UserID;
                        sliderConfig.UpdatedOn = DateTime.Now;
                        _db.SaveChanges();
                    }
                }
                else
                {
                    sliderConfig.Description = obj.Description;
                    sliderConfig.SliderImage = obj.SliderImage;
                    sliderConfig.Id = Guid.NewGuid().ToString();
                    sliderConfig.CreateBy = ApplicationSession.Current.UserID;
                    sliderConfig.CreatedOn = DateTime.Now;
                    sliderConfig.IsActive = true;

                    using (var _db = new SchoolMSEntities())
                    {
                        _db.SliderConfigs.Add(sliderConfig);
                        _db.SaveChanges();
                    }
                }

                Response.IsError = false;
            }
            catch (Exception ex)
            {
                Response.IsError = true;
                Response.Message = "Internal server error";
            }
            return Json(Response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void Upload()
        {
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];

                var fileName = Path.GetFileName(file.FileName);

                var path = Path.Combine(Server.MapPath("~/App_Data/"), fileName);
                file.SaveAs(path);
            }

        }

        #endregion SliderConfiguration

        #region GeneralSettings

        [Route("admin/generalsettings")]
        public ActionResult ManageGeneralSetting()
        {
            return View();
        }

        public JsonResult GetGSList()
        {
            JsonResponse<List<GeneralSettingsVM>> Response = new JsonResponse<List<GeneralSettingsVM>>();
            try
            {
                using (var _db = new SchoolMSEntities())
                {
                    Response.Data = _db.GeneralSettings.ToList()
                                                .Select(s => new GeneralSettingsVM()
                                                {
                                                    GeneralSettingId = s.Id,
                                                    FieldValue = s.FieldValue,
                                                    CreateBy = s.CreateBy,
                                                    CreateDate = s.CreateDate,
                                                    UpdateBy = s.UpdateBy,
                                                    UpdateDate = s.UpdateDate
                                                }).ToList();

                    if (Response.Data == null)
                    {
                        Response.Data = new List<GeneralSettingsVM>();
                    }
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

        //[Route("admin/generalsetting/{id}")]
        //public ActionResult GeneralSettings(string Id)
        //{
        //    GeneralSettingsVM obj = new GeneralSettingsVM();

        //    if (!String.IsNullOrEmpty(Id))
        //    {
        //        var generalSettings = new GeneralSetting();
        //        using (var _db = new SchoolMSEntities())
        //        {
        //            generalSettings = _db.GeneralSettings.Find(Id);
        //        }

        //        if (generalSettings != null)
        //        {
        //            obj.FieldValue = generalSettings.FieldValue;
        //        }
        //    }
        //    return View("ManageGeneralSetting");
        //}

        public JsonResult SaveGeneralSetting(GeneralSettingsVM obj)
        {
            JsonResponse<bool> Response = new JsonResponse<bool>();
            try
            {
                GeneralSetting generalSetting = new GeneralSetting();

                if (obj.GeneralSettingId != 0)
                {
                    using (var _db = new SchoolMSEntities())
                    {
                        generalSetting = _db.GeneralSettings.Find(obj.GeneralSettingId);

                        if (generalSetting == null)
                        {
                            Response.IsError = true;
                            Response.Message = "General Settings not found";
                            return Json(Response, JsonRequestBehavior.AllowGet);
                        }

                        generalSetting.FieldValue = obj.FieldValue;
                        generalSetting.UpdateBy = ApplicationSession.Current.UserID;
                        generalSetting.UpdateDate = DateTime.Now;
                        _db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Response.IsError = true;
                Response.Message = "Internal server error";
            }
            return Json(Response, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}