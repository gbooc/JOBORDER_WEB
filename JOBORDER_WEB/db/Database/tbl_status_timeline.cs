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
    using System.Collections.Generic;
    
    public partial class tbl_status_timeline
    {
        public int t_id { get; set; }
        public int details_id { get; set; }
        public Nullable<System.DateTime> under_assessment { get; set; }
        public Nullable<System.DateTime> pending_incharge { get; set; }
        public Nullable<System.DateTime> incharge_assigned { get; set; }
        public Nullable<System.DateTime> request_documents { get; set; }
        public Nullable<System.DateTime> review { get; set; }
        public Nullable<System.DateTime> rejected { get; set; }
        public Nullable<System.DateTime> proposal { get; set; }
        public Nullable<System.DateTime> revision { get; set; }
        public Nullable<System.DateTime> development { get; set; }
        public Nullable<System.DateTime> testing { get; set; }
    }
}
