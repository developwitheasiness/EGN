using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolMS.Areas.admin.ViewModel
{
    public class SchoolGradeVM
    {
        public string GradeId { get; set; }
        public string Grade { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }
}