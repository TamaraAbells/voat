//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Voat.Data.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ViewStatistic
    {
        public int SubmissionID { get; set; }
        public string ViewerID { get; set; }
    
        public virtual Submission Submission { get; set; }
    }
}
