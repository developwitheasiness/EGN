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
    
    public partial class TeacherRating
    {
        public System.Guid Id { get; set; }
        public Nullable<System.Guid> TeacherId { get; set; }
        public Nullable<System.Guid> StudentId { get; set; }
        public Nullable<int> RatingValue { get; set; }
        public string Comments { get; set; }
    
        public virtual Student Student { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}