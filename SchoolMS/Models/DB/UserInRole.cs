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
    
    public partial class UserInRole
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public Nullable<long> RoleId { get; set; }
        public string AssignBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    
        public virtual UserRole UserRole { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
        public virtual User User2 { get; set; }
    }
}
