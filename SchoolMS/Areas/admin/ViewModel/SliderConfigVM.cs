using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolMS.Areas.admin.ViewModel
{
    public class SliderConfigVM
    {
        public string SliderConfigId { get; set; }
        public string Description { get; set; }
        public string SliderImage { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
    }
}