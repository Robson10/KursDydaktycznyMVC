//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KursyMVC.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CourseSections
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CourseSections()
        {
            this.SectionContents = new HashSet<SectionContents>();
        }
    
        public int CourseSectionID { get; set; }
        public Nullable<int> CourseID { get; set; }
        public string SectionName { get; set; }
    
        public virtual Courses Courses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SectionContents> SectionContents { get; set; }
    }
}
