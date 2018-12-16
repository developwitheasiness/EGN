using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolMS.Areas.admin.ViewModel
{
    public class SchoolSubjectVM
    {
        public string SchoolSubjectId { get; set; }
        public string GradeId { get; set; }
        public string GradeName { get; set; }
        public string Subject { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }
}