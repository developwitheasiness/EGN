//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SchoolMS.Models.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class TeacherCertificate
    {
        public System.Guid Id { get; set; }
        public string CertificateTitle { get; set; }
        public string CertificatePath { get; set; }
        public Nullable<System.Guid> TeacherId { get; set; }
    
        public virtual Teacher Teacher { get; set; }
    }
}
