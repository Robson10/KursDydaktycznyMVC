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
    
    public partial class SectionContents
    {
        public int SectionContentID { get; set; }
        public Nullable<int> CourseSectionID { get; set; }
        public string SectionContentTitle { get; set; }
        public string SectionContent { get; set; }
    
        public virtual CourseSections CourseSections { get; set; }
    }
}
