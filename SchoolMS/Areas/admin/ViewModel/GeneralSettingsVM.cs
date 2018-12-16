using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolMS.Areas.admin.ViewModel
{
    public class GeneralSettingsVM
    {
        public long GeneralSettingId { get; set; }
        public string FieldValue { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    }
}