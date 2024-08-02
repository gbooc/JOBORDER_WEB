﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class JOEntities : DbContext
    {
        public JOEntities()
            : base("name=JOEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<tbl_apc_personnel> tbl_apc_personnel { get; set; }
        public virtual DbSet<tbl_applications> tbl_applications { get; set; }
        public virtual DbSet<tbl_category_type> tbl_category_type { get; set; }
        public virtual DbSet<tbl_downtime> tbl_downtime { get; set; }
        public virtual DbSet<tbl_downtime_cause> tbl_downtime_cause { get; set; }
        public virtual DbSet<tbl_downtime_type> tbl_downtime_type { get; set; }
        public virtual DbSet<tbl_gantt_chart> tbl_gantt_chart { get; set; }
        public virtual DbSet<tbl_joborder> tbl_joborder { get; set; }
        public virtual DbSet<tbl_joborder_category> tbl_joborder_category { get; set; }
        public virtual DbSet<tbl_joborder_details> tbl_joborder_details { get; set; }
        public virtual DbSet<tbl_network> tbl_network { get; set; }
        public virtual DbSet<tbl_priority> tbl_priority { get; set; }
        public virtual DbSet<tbl_server> tbl_server { get; set; }
        public virtual DbSet<tbl_status> tbl_status { get; set; }
        public virtual DbSet<tbl_telephone> tbl_telephone { get; set; }
        public virtual DbSet<tbl_allocated_time> tbl_allocated_time { get; set; }
        public virtual DbSet<tbl_change_employee> tbl_change_employee { get; set; }
        public virtual DbSet<tbl_telephone_Rolie> tbl_telephone_Rolie { get; set; }
        public virtual DbSet<tbl_preventive_pc> tbl_preventive_pc { get; set; }
        public virtual DbSet<tbl_preventive_diagnose> tbl_preventive_diagnose { get; set; }
        public virtual DbSet<tbl_preventive_services> tbl_preventive_services { get; set; }
        public virtual DbSet<tbl_preventive_maintenance> tbl_preventive_maintenance { get; set; }
        public virtual DbSet<vw_Employee> vw_Employee { get; set; }
        public virtual DbSet<tbl_employee_outside_rimp> tbl_employee_outside_rimp { get; set; }
        public virtual DbSet<vw_Managers> vw_Managers { get; set; }
        public virtual DbSet<tbl_cc_category> tbl_cc_category { get; set; }
        public virtual DbSet<tbl_employees> tbl_employees { get; set; }
        public virtual DbSet<tbl_sbu> tbl_sbu { get; set; }
        public virtual DbSet<tbl_sbu_costcenter> tbl_sbu_costcenter { get; set; }
        public virtual DbSet<tbl_sbu_division> tbl_sbu_division { get; set; }
        public virtual DbSet<tbl_jo_cc_details> tbl_jo_cc_details { get; set; }
        public virtual DbSet<tbl_managers_email> tbl_managers_email { get; set; }
        public virtual DbSet<tbl_costcenter_meaning> tbl_costcenter_meaning { get; set; }
        public virtual DbSet<tbl_sub_assignees> tbl_sub_assignees { get; set; }
        public virtual DbSet<tbl_daily_report> tbl_daily_report { get; set; }
        public virtual DbSet<vw_Emails> vw_Emails { get; set; }
        public virtual DbSet<vw_ForApprovalJO> vw_ForApprovalJO { get; set; }
        public virtual DbSet<tbl_onhold_message> tbl_onhold_message { get; set; }
        public virtual DbSet<tbl_status_timeline> tbl_status_timeline { get; set; }
        public virtual DbSet<tbl_jo_creation_availability> tbl_jo_creation_availability { get; set; }
        public virtual DbSet<tbl_problems> tbl_problems { get; set; }
    
        public virtual ObjectResult<sp_all_apc_jo_count_Result> sp_all_apc_jo_count()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_all_apc_jo_count_Result>("sp_all_apc_jo_count");
        }
    
        public virtual int sp_alterdiagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_alterdiagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_creatediagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_creatediagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual ObjectResult<sp_developer_jo_summary_Result> sp_developer_jo_summary(string developer)
        {
            var developerParameter = developer != null ?
                new ObjectParameter("developer", developer) :
                new ObjectParameter("developer", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_developer_jo_summary_Result>("sp_developer_jo_summary", developerParameter);
        }
    
        public virtual int sp_dropdiagram(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_dropdiagram", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_export_excel_Result> sp_export_excel(Nullable<System.DateTime> datefrom, Nullable<System.DateTime> dateended, Nullable<int> flg_report)
        {
            var datefromParameter = datefrom.HasValue ?
                new ObjectParameter("datefrom", datefrom) :
                new ObjectParameter("datefrom", typeof(System.DateTime));
    
            var dateendedParameter = dateended.HasValue ?
                new ObjectParameter("dateended", dateended) :
                new ObjectParameter("dateended", typeof(System.DateTime));
    
            var flg_reportParameter = flg_report.HasValue ?
                new ObjectParameter("flg_report", flg_report) :
                new ObjectParameter("flg_report", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_export_excel_Result>("sp_export_excel", datefromParameter, dateendedParameter, flg_reportParameter);
        }
    
        public virtual ObjectResult<sp_get_my_jo_Result> sp_get_my_jo(string user)
        {
            var userParameter = user != null ?
                new ObjectParameter("user", user) :
                new ObjectParameter("user", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_get_my_jo_Result>("sp_get_my_jo", userParameter);
        }
    
        public virtual ObjectResult<sp_hardware_counts_Result> sp_hardware_counts()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_hardware_counts_Result>("sp_hardware_counts");
        }
    
        public virtual ObjectResult<sp_helpdiagramdefinition_Result> sp_helpdiagramdefinition(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagramdefinition_Result>("sp_helpdiagramdefinition", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagrams_Result> sp_helpdiagrams(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagrams_Result>("sp_helpdiagrams", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_hw_jo_monthly_report_Result> sp_hw_jo_monthly_report(Nullable<System.DateTime> date_filter)
        {
            var date_filterParameter = date_filter.HasValue ?
                new ObjectParameter("date_filter", date_filter) :
                new ObjectParameter("date_filter", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_hw_jo_monthly_report_Result>("sp_hw_jo_monthly_report", date_filterParameter);
        }
    
        public virtual ObjectResult<sp_joborder_classification_Result> sp_joborder_classification()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_joborder_classification_Result>("sp_joborder_classification");
        }
    
        public virtual ObjectResult<sp_joborder_details_Result> sp_joborder_details()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_joborder_details_Result>("sp_joborder_details");
        }
    
        public virtual ObjectResult<sp_network_downtime_details_Result> sp_network_downtime_details()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_network_downtime_details_Result>("sp_network_downtime_details");
        }
    
        public virtual int sp_renamediagram(string diagramname, Nullable<int> owner_id, string new_diagramname)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var new_diagramnameParameter = new_diagramname != null ?
                new ObjectParameter("new_diagramname", new_diagramname) :
                new ObjectParameter("new_diagramname", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_renamediagram", diagramnameParameter, owner_idParameter, new_diagramnameParameter);
        }
    
        public virtual ObjectResult<sp_server_downtime_details_Result> sp_server_downtime_details()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_server_downtime_details_Result>("sp_server_downtime_details");
        }
    
        public virtual ObjectResult<sp_software_count_Result> sp_software_count()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_software_count_Result>("sp_software_count");
        }
    
        public virtual ObjectResult<sp_sw_jo_monthly_report_Result> sp_sw_jo_monthly_report(Nullable<System.DateTime> date_filter)
        {
            var date_filterParameter = date_filter.HasValue ?
                new ObjectParameter("date_filter", date_filter) :
                new ObjectParameter("date_filter", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_sw_jo_monthly_report_Result>("sp_sw_jo_monthly_report", date_filterParameter);
        }
    
        public virtual ObjectResult<sp_tel_downtime_details_Result> sp_tel_downtime_details()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_tel_downtime_details_Result>("sp_tel_downtime_details");
        }
    
        public virtual int sp_upgraddiagrams()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_upgraddiagrams");
        }
    
        public virtual ObjectResult<sp_get_all_actual_standard_time_Result> sp_get_all_actual_standard_time(string details_id, string type)
        {
            var details_idParameter = details_id != null ?
                new ObjectParameter("details_id", details_id) :
                new ObjectParameter("details_id", typeof(string));
    
            var typeParameter = type != null ?
                new ObjectParameter("type", type) :
                new ObjectParameter("type", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_get_all_actual_standard_time_Result>("sp_get_all_actual_standard_time", details_idParameter, typeParameter);
        }
    
        public virtual ObjectResult<sp_get_preventive_maintenance_Result> sp_get_preventive_maintenance()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_get_preventive_maintenance_Result>("sp_get_preventive_maintenance");
        }
    
        public virtual ObjectResult<sp_get_myjo_summary_sw_Result> sp_get_myjo_summary_sw(string person)
        {
            var personParameter = person != null ?
                new ObjectParameter("person", person) :
                new ObjectParameter("person", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_get_myjo_summary_sw_Result>("sp_get_myjo_summary_sw", personParameter);
        }
    
        public virtual ObjectResult<sp_get_myjo_summary_hw_Result> sp_get_myjo_summary_hw(string person)
        {
            var personParameter = person != null ?
                new ObjectParameter("person", person) :
                new ObjectParameter("person", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_get_myjo_summary_hw_Result>("sp_get_myjo_summary_hw", personParameter);
        }
    
        public virtual ObjectResult<sp_get_myjo_graph_hw_Result> sp_get_myjo_graph_hw(string person)
        {
            var personParameter = person != null ?
                new ObjectParameter("person", person) :
                new ObjectParameter("person", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_get_myjo_graph_hw_Result>("sp_get_myjo_graph_hw", personParameter);
        }
    
        public virtual ObjectResult<sp_jo_by_dept_Result> sp_jo_by_dept()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_jo_by_dept_Result>("sp_jo_by_dept");
        }
    
        public virtual ObjectResult<sp_apc_completed_jo_Result> sp_apc_completed_jo()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_apc_completed_jo_Result>("sp_apc_completed_jo");
        }
    
        public virtual ObjectResult<sp_apc_delayed_jo_Result> sp_apc_delayed_jo()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_apc_delayed_jo_Result>("sp_apc_delayed_jo");
        }
    
        public virtual ObjectResult<sp_apc_ontime_jo_Result> sp_apc_ontime_jo()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_apc_ontime_jo_Result>("sp_apc_ontime_jo");
        }
    
        public virtual ObjectResult<sp_get_sbu_description_Result> sp_get_sbu_description(Nullable<int> sBU_ID)
        {
            var sBU_IDParameter = sBU_ID.HasValue ?
                new ObjectParameter("SBU_ID", sBU_ID) :
                new ObjectParameter("SBU_ID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_get_sbu_description_Result>("sp_get_sbu_description", sBU_IDParameter);
        }
    
        public virtual ObjectResult<sp_assignee_ongoing_jo_Result> sp_assignee_ongoing_jo(string assignee)
        {
            var assigneeParameter = assignee != null ?
                new ObjectParameter("assignee", assignee) :
                new ObjectParameter("assignee", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_assignee_ongoing_jo_Result>("sp_assignee_ongoing_jo", assigneeParameter);
        }
    
        public virtual ObjectResult<sp_all_assignee_Result> sp_all_assignee(Nullable<int> details_id)
        {
            var details_idParameter = details_id.HasValue ?
                new ObjectParameter("details_id", details_id) :
                new ObjectParameter("details_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_all_assignee_Result>("sp_all_assignee", details_idParameter);
        }
    
        public virtual ObjectResult<sp_graph_costcenter_Result> sp_graph_costcenter(Nullable<System.DateTime> tmpdatefrom, Nullable<System.DateTime> tmpdateended)
        {
            var tmpdatefromParameter = tmpdatefrom.HasValue ?
                new ObjectParameter("tmpdatefrom", tmpdatefrom) :
                new ObjectParameter("tmpdatefrom", typeof(System.DateTime));
    
            var tmpdateendedParameter = tmpdateended.HasValue ?
                new ObjectParameter("tmpdateended", tmpdateended) :
                new ObjectParameter("tmpdateended", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_graph_costcenter_Result>("sp_graph_costcenter", tmpdatefromParameter, tmpdateendedParameter);
        }
    
        public virtual ObjectResult<sp_get_transaction_Result> sp_get_transaction(string empid, string datefrom, string dateto)
        {
            var empidParameter = empid != null ?
                new ObjectParameter("empid", empid) :
                new ObjectParameter("empid", typeof(string));
    
            var datefromParameter = datefrom != null ?
                new ObjectParameter("datefrom", datefrom) :
                new ObjectParameter("datefrom", typeof(string));
    
            var datetoParameter = dateto != null ?
                new ObjectParameter("dateto", dateto) :
                new ObjectParameter("dateto", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_get_transaction_Result>("sp_get_transaction", empidParameter, datefromParameter, datetoParameter);
        }
    
        public virtual ObjectResult<sp_export_daily_report_Result> sp_export_daily_report(string apcid, Nullable<System.DateTime> date)
        {
            var apcidParameter = apcid != null ?
                new ObjectParameter("apcid", apcid) :
                new ObjectParameter("apcid", typeof(string));
    
            var dateParameter = date.HasValue ?
                new ObjectParameter("date", date) :
                new ObjectParameter("date", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_export_daily_report_Result>("sp_export_daily_report", apcidParameter, dateParameter);
        }
    
        public virtual ObjectResult<sp_jo_by_dept_details_Result> sp_jo_by_dept_details()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_jo_by_dept_details_Result>("sp_jo_by_dept_details");
        }
    
        public virtual ObjectResult<sp_shared_jo_Result> sp_shared_jo(string assignee)
        {
            var assigneeParameter = assignee != null ?
                new ObjectParameter("assignee", assignee) :
                new ObjectParameter("assignee", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_shared_jo_Result>("sp_shared_jo", assigneeParameter);
        }
    
        public virtual ObjectResult<sp_kpi_completedJO_Result> sp_kpi_completedJO(string year)
        {
            var yearParameter = year != null ?
                new ObjectParameter("year", year) :
                new ObjectParameter("year", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_kpi_completedJO_Result>("sp_kpi_completedJO", yearParameter);
        }
    
        public virtual ObjectResult<sp_kpi_monthlyAssignedJO_Result> sp_kpi_monthlyAssignedJO(string year)
        {
            var yearParameter = year != null ?
                new ObjectParameter("year", year) :
                new ObjectParameter("year", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_kpi_monthlyAssignedJO_Result>("sp_kpi_monthlyAssignedJO", yearParameter);
        }
    
        public virtual ObjectResult<sp_kpi_joperyear_Result> sp_kpi_joperyear()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_kpi_joperyear_Result>("sp_kpi_joperyear");
        }
    
        public virtual ObjectResult<sp_kpi_joPerYeardetails_Result> sp_kpi_joPerYeardetails()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_kpi_joPerYeardetails_Result>("sp_kpi_joPerYeardetails");
        }
    
        public virtual ObjectResult<sp_kpi_hardware_Result> sp_kpi_hardware()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_kpi_hardware_Result>("sp_kpi_hardware");
        }
    
        public virtual ObjectResult<sp_kpi_satisfactory_rate_Result> sp_kpi_satisfactory_rate()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_kpi_satisfactory_rate_Result>("sp_kpi_satisfactory_rate");
        }
    
        public virtual ObjectResult<sp_jodetails_by_dept_Result> sp_jodetails_by_dept(string deptname)
        {
            var deptnameParameter = deptname != null ?
                new ObjectParameter("deptname", deptname) :
                new ObjectParameter("deptname", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_jodetails_by_dept_Result>("sp_jodetails_by_dept", deptnameParameter);
        }
    
        public virtual ObjectResult<sp_ForApprovalJO_Result> sp_ForApprovalJO(string department, string username)
        {
            var departmentParameter = department != null ?
                new ObjectParameter("department", department) :
                new ObjectParameter("department", typeof(string));
    
            var usernameParameter = username != null ?
                new ObjectParameter("username", username) :
                new ObjectParameter("username", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_ForApprovalJO_Result>("sp_ForApprovalJO", departmentParameter, usernameParameter);
        }
    
        public virtual ObjectResult<sp_kpi_resource_utilization_Result> sp_kpi_resource_utilization()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_kpi_resource_utilization_Result>("sp_kpi_resource_utilization");
        }
    
        public virtual ObjectResult<sp_departments_softwares_Result> sp_departments_softwares(string department)
        {
            var departmentParameter = department != null ?
                new ObjectParameter("department", department) :
                new ObjectParameter("department", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_departments_softwares_Result>("sp_departments_softwares", departmentParameter);
        }
    
        public virtual ObjectResult<sp_email_RequestorAndDeptheads_Result> sp_email_RequestorAndDeptheads(string jobno)
        {
            var jobnoParameter = jobno != null ?
                new ObjectParameter("jobno", jobno) :
                new ObjectParameter("jobno", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_email_RequestorAndDeptheads_Result>("sp_email_RequestorAndDeptheads", jobnoParameter);
        }
    
        public virtual ObjectResult<sp_GeneralSearch_Result> sp_GeneralSearch(string keyword)
        {
            var keywordParameter = keyword != null ?
                new ObjectParameter("keyword", keyword) :
                new ObjectParameter("keyword", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GeneralSearch_Result>("sp_GeneralSearch", keywordParameter);
        }
    
        public virtual ObjectResult<sp_jo_pending_status_Result> sp_jo_pending_status()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_jo_pending_status_Result>("sp_jo_pending_status");
        }
    
        public virtual ObjectResult<sp_jo_unassigned_Result> sp_jo_unassigned()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_jo_unassigned_Result>("sp_jo_unassigned");
        }
    
        public virtual ObjectResult<sp_jo_details_Result> sp_jo_details(string jobno)
        {
            var jobnoParameter = jobno != null ?
                new ObjectParameter("jobno", jobno) :
                new ObjectParameter("jobno", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_jo_details_Result>("sp_jo_details", jobnoParameter);
        }
    
        public virtual ObjectResult<sp_jo_time_allocation_Result> sp_jo_time_allocation(Nullable<int> details_id)
        {
            var details_idParameter = details_id.HasValue ?
                new ObjectParameter("details_id", details_id) :
                new ObjectParameter("details_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_jo_time_allocation_Result>("sp_jo_time_allocation", details_idParameter);
        }
    
        public virtual ObjectResult<sp_latest_available_apc_Result> sp_latest_available_apc()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_latest_available_apc_Result>("sp_latest_available_apc");
        }
    
        public virtual ObjectResult<sp_timeline_status_Result> sp_timeline_status(Nullable<int> detailsID)
        {
            var detailsIDParameter = detailsID.HasValue ?
                new ObjectParameter("DetailsID", detailsID) :
                new ObjectParameter("DetailsID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_timeline_status_Result>("sp_timeline_status", detailsIDParameter);
        }
    
        public virtual ObjectResult<sp_dashboard_percentage_of_jod_Result> sp_dashboard_percentage_of_jod()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_dashboard_percentage_of_jod_Result>("sp_dashboard_percentage_of_jod");
        }
    
        public virtual ObjectResult<sp_sw_jo_issues_Result> sp_sw_jo_issues(Nullable<int> details_ID)
        {
            var details_IDParameter = details_ID.HasValue ?
                new ObjectParameter("Details_ID", details_ID) :
                new ObjectParameter("Details_ID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_sw_jo_issues_Result>("sp_sw_jo_issues", details_IDParameter);
        }
    
        public virtual ObjectResult<sp_jo_sw_stages_and_issues_Result> sp_jo_sw_stages_and_issues()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_jo_sw_stages_and_issues_Result>("sp_jo_sw_stages_and_issues");
        }
    }
}