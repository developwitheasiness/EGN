using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolMS.Areas.admin.ViewModel
{
    public class StudentVM
    {
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
    }
}