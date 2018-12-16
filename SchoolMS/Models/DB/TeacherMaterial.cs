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
    
    public partial class TeacherMaterial
    {
        public System.Guid Id { get; set; }
        public string Title { get; set; }
        public Nullable<System.Guid> TeacherId { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public Nullable<bool> IsPublic { get; set; }
        public string GradeId { get; set; }
        public string SubjectId { get; set; }
    
        public virtual SchoolGrade SchoolGrade { get; set; }
        public virtual SchoolSubject SchoolSubject { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}
