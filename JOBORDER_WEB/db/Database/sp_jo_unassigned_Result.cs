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
    
    public partial class sp_jo_unassigned_Result
    {
        public string job_no { get; set; }
        public string requested_by { get; set; }
        public Nullable<System.DateTime> date_filed { get; set; }
        public string status { get; set; }
        public string details { get; set; }
        public Nullable<int> type_id { get; set; }
    }
}