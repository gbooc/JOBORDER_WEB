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
    
    public partial class sp_get_my_jo_Result
    {
        public string job_no { get; set; }
        public string app { get; set; }
        public string status { get; set; }
        public string assignee { get; set; }
        public Nullable<System.DateTime> date_filed { get; set; }
        public Nullable<System.DateTime> date_started { get; set; }
        public Nullable<System.DateTime> date_ended { get; set; }
        public Nullable<System.DateTime> date_target { get; set; }
        public Nullable<int> details_id { get; set; }
        public Nullable<System.DateTime> date_assigned { get; set; }
    }
}