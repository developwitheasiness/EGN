using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolMS.Common
{
    public class JsonResponse<T>
    {
        public bool IsError { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}