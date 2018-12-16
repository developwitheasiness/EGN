using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolMS.Models.ViewModels
{
    public class StudentVM
    {

        public StudentVM()
        {
            GradeList = new List<SelectListItem>();
            StudentSubjectList = new List<StudentSchoolSubjects>();
            TimeZoneInfos = new List<SelectListItem>();
        }

        public Guid Id { get; set; }
        public List<SelectListItem> TimeZoneInfos { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string ProfilePhoto { get; set; }
        public int FromCountry { get; set; }
        public int FromState { get; set; }
        public int LivingCountry { get; set; }
        public int LivingState { get; set; }
        public string LivingAddress { get; set; }
        public string TimeZone { get; set; }
        public string SchoolName { get; set; }
        public string LookingFor { get; set; }
        public string ShortIntroduction { get; set; }
        public string LongIntroduction { get; set; }
        public string SchoolGrade { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public int BirthDateOnly { get; set; }
        public int BirthMonthOnly { get; set; }
        public int BirthYearOnly { get; set; }
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
        public string Skype { get; set; }
        public string Facetime { get; set; }

        [Display(Name = "Google Hangout")]
        public string GoogleHangout { get; set; }
        public string QQ { get; set; }
        public string Webchat { get; set; }
        public string NativeLanguage { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<bool> Isblock { get; set; }

        public List<SelectListItem> GradeList { get; set; }
        public List<StudentSchoolSubjects> StudentSubjectList { get; set; }
    }

    public class StudentInfoViewVM
    {
        public string StudentName { get; set; }
        public string From { get; set; }
        public string TimeZone { get; set; }
        public string Learning { get; set; }
        public string ProfilePhoto { get; set; }
    }

    public class StudentSchoolSubjects
    {

        public StudentSchoolSubjects()
        {
            SubjectList = new List<SelectListItem>();
        }

        public string GradeId { get; set; }
        public string SubjectId { get; set; }
        public bool IsLearning { get; set; }
        public bool IsPrimary { get; set; }
        public List<SelectListItem> SubjectList { get; set; }
        public bool IsDelete { get; set; }
    }

    public class StudentSubjectVM
    {
        public Guid StudentId { get; set; }
        public List<StudentSchoolSubjects> subjectList { get; set; }
    }

    public class SubjectSelectionVM
    {
        public SubjectSelectionVM()
        {
            SubjectList = new List<SelectListItem>();
            GradeList = new List<SelectListItem>();
        }

        public List<SelectListItem> SubjectList { get; set; }
        public List<SelectListItem> GradeList { get; set; }
        public string GradeId { get; set; }
        public string SubjectId { get; set; }
    }

    public class MaterialViewVM
    {
        public MaterialViewVM()
        {
            GradeList = new List<SelectListItem>();
        }

        public List<SelectListItem> GradeList { get; set; }
        public string GradeId { get; set; }
        public string SubjectId { get; set; }
    }

    public class MaterialDownloadList
    {
        public string GradeName { get; set; }
        public string SubjectName { get; set; }
        public string Title { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
    }
}