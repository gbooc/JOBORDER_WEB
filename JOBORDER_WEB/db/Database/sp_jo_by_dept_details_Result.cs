//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace db.Database
{
    using System;
    
    public partial class sp_jo_by_dept_details_Result
    {
        public string department { get; set; }
        public string details { get; set; }
        public Nullable<int> progress_rate { get; set; }
        public string requested_by { get; set; }
        public Nullable<System.DateTime> date_target { get; set; }
        public string job_no { get; set; }
        public string assignee { get; set; }
        public Nullable<System.DateTime> date_progress_updated { get; set; }
        public string reason_delay { get; set; }
    }
}
