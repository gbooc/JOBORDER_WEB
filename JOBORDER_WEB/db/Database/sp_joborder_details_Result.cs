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
    
    public partial class sp_joborder_details_Result
    {
        public int id { get; set; }
        public int details_id { get; set; }
        public Nullable<int> category_id { get; set; }
        public string codenumber { get; set; }
        public string jonumber { get; set; }
        public string details { get; set; }
        public string purpose { get; set; }
        public string jo_type { get; set; }
        public string jo_type_details { get; set; }
        public Nullable<int> standard_time { get; set; }
        public string jo_priority { get; set; }
        public string jo_status { get; set; }
        public string jo_diagnosis { get; set; }
        public string jo_action_taken { get; set; }
        public Nullable<int> jo_progress { get; set; }
        public string jo_remarks { get; set; }
        public string dev_app_name { get; set; }
        public string assignee { get; set; }
        public Nullable<System.DateTime> datefiled { get; set; }
        public string requestor { get; set; }
        public string req_approvedby { get; set; }
        public string req_notedby { get; set; }
        public Nullable<System.DateTime> date_started { get; set; }
        public Nullable<System.DateTime> date_needed { get; set; }
        public Nullable<int> actual_time { get; set; }
        public string web_port { get; set; }
        public string web_icon { get; set; }
        public string system_type { get; set; }
        public Nullable<System.DateTime> date_target { get; set; }
        public Nullable<System.DateTime> date_assigned { get; set; }
        public string req_contact { get; set; }
        public string req_costcenter { get; set; }
        public Nullable<System.DateTime> date_confirmed { get; set; }
        public string reason_delay { get; set; }
        public int flg_due { get; set; }
        public string path_barcode { get; set; }
    }
}