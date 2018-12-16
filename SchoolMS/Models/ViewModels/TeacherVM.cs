using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolMS.Models.ViewModels
{
    public class TeacherVM
    {

        public TeacherVM()
        {
            GradeList = new List<SelectListItem>();
            TimeZoneInfos = new List<SelectListItem>();
            TeacherSubjectList = new List<TeacherSchoolSubjects>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> FromCountry { get; set; }
        public Nullable<int> FromState { get; set; }
        public Nullable<int> LivingCountry { get; set; }
        public Nullable<int> LivingState { get; set; }
        public string TimeZone { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        private Nullable<int> _gender = 0;
        public Nullable<int> Gender
        {
            get
            {
                return _gender;
            }
            set
            {
                _gender = value;
            }
        }
        public string ShortDescription { get; set; }
        public string LongIntroduction { get; set; }
        public string TeacherSince { get; set; }
        public string UniversityName { get; set; }
        public Nullable<int> YearOfPass { get; set; }
        public string CourseDegreeName { get; set; }
        public string WorkExperienceName { get; set; }
        public string WorkExperienceYear { get; set; }
        public string WorkExperienceDetails { get; set; }
        public string CertificateTitle { get; set; }
        public string CertificatePhoto { get; set; }
        public string CourceTitle { get; set; }
        public string Description { get; set; }
        public string Charges { get; set; }
        public string ViewMoreDetails { get; set; }
        public string Email { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<bool> IsBlock { get; set; }
        public int BirthDateOnly { get; set; }
        public int BirthMonthOnly { get; set; }
        public int BirthYearOnly { get; set; }
        public string ProfilePhoto { get; set; }
        public string GradeId { get; set; }
        public string[] SelectedSubejcts { get; set; }

        public List<SelectListItem> TimeZoneInfos { get; set; }
        public List<SelectListItem> GradeList { get; set; }
        public List<TeacherSchoolSubjects> TeacherSubjectList { get; set; }
    }

    public class TeacherSubjectVM
    {
        public Guid Id { get; set; }
        public string GradeId { get; set; }
        public string[] SelectedSubjects { get; set; }
    }

    public class GradeWiseSubjects
    {
        public string[] SelectedSubjects { get; set; }
        public List<SelectListItem> SubjectList { get; set; }
    }

    public class ViewTeacherVM
    {

        public ViewTeacherVM()
        {
            ExprienceList = new List<TeacherExperienceVM>();
            CertificateList = new List<TeacherCertificateVM>();
            TeacherRatings = new List<TeacherRatingVM>();
        }

        public Guid TeacherId { get; set; }
        public string TeacherName { get; set; }
        public string From { get; set; }
        public string TimeZone { get; set; }
        public string Learning { get; set; }
        public string ProfilePhoto { get; set; }
        public string LongIntroduction { get; set; }
        public string CertiPhoto { get; set; }
        public string CourceTitle { get; set; }
        public string TeacherSince { get; set; }
        public string[] Subjects { get; set; }

        public List<TeacherExperienceVM> ExprienceList { get; set; }
        public List<TeacherCertificateVM> CertificateList { get; set; }
        public List<TeacherRatingVM> TeacherRatings { get; set; }
    }

    public class TeacherSchoolSubjects
    {

        public TeacherSchoolSubjects()
        {
            SubjectList = new List<SelectListItem>();
        }

        public string GradeId { get; set; }
        public string SubjectId { get; set; }
        public List<SelectListItem> SubjectList { get; set; }
        public bool IsDelete { get; set; }
    }

    public class TeacherEducationVM
    {
        public Guid TeacherEducationId { get; set; }
        public Guid TeacherId { get; set; }
        public string UniversityName { get; set; }
        public Nullable<int> YearOfPass { get; set; }
        public string CourseDegreeName { get; set; }
    }

    public class TeacherExperienceVM
    {
        public Guid TeacherExperienceId { get; set; }
        public Guid TeacherId { get; set; }
        public string WorkExperienceName { get; set; }
        public string WorkExperienceYear { get; set; }
        public string WorkExperienceDetails { get; set; }
    }

    public class TeacherProffCoursesVM
    {
        public Guid TeacherProffId { get; set; }
        public Guid TeacherId { get; set; }
        public string CourseTitle { get; set; }
        public string Description { get; set; }
        public string Charges { get; set; }
        public string ViewMoreDetails { get; set; }
    }

    public class TeacherCertificateVM
    {
        public Guid TeacherCertificateId { get; set; }
        public Guid TeacherId { get; set; }
        public string CertificateTitle { get; set; }
        public string CertificatePhoto { get; set; }
    }

    public class TeacherRatingVM
    {
        public System.Guid RatingId { get; set; }
        public Nullable<System.Guid> TeacherId { get; set; }
        public Nullable<System.Guid> StudentId { get; set; }
        public Nullable<int> RatingValue { get; set; }
        public string Comments { get; set; }
        public string StudentName { get; set; }
    }

    public class TeacherMaterialUploadVM
    {

        public TeacherMaterialUploadVM()
        {
            GradList = new List<SelectListItem>();
            SubjectList = new List<SelectListItem>();
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid TeacherId { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public bool IsPublic { get; set; }
        public string GradeId { get; set; }
        public string SubjectId { get; set; }

        public List<SelectListItem> GradList { get; set; }
        public List<SelectListItem> SubjectList { get; set; }
    }

    public class TeacherMaterialUploadView
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid TeacherId { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public bool IsPublic { get; set; }
        public string GradeName { get; set; }
        public string SubjectName { get; set; }
    }
}