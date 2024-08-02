using db.Data.Dashboard;
using db.Data.JobOrder;
using db.Data.Network;
using db.Data.Server;
using db.Data.Telephone;
using db.Database;
using db.Repository;
using db.Repository.APC;
using db.Repository.Applications;
using db.Repository.Downtime;
using db.Repository.DowntimeCause;
using db.Repository.GanttChart;
using db.Repository.JobOrder;
using db.Repository.Maintenance;
using db.Repository.Network;
using db.Repository.Priority;
using db.Repository.Server;
using db.Repository.Status;
using db.Repository.Telephone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.Global
{
    public class RepositoryInitialization
    {
        //** Repository - Job Order
        public IEmployee               _employee;
        public IProgrammer             _apc;
        public IJoborder               _jo;
        public IApplications           _app;
        public IJobOrderClassification _jo_classification;
        public IPriority               _jo_priority;
        public IStatus                 _jostatus;
        public IDashboard              _dashboard;
        public IGanttChart             _gant_chart;
        //** E N D

        //** KPI **
        public IServer         _server;
        public IServerDowntime _server_downtime;
        
        public INetwork         _network;
        public INetworkDowntime _network_downtime;
       
        public IDowntime      _downtime;
        public IDowntimeCause _downtime_cause;

        public ITelephone         _telephone;
        public ITelephoneDowntime _tel_downtime;

        public IJobOrderReport _monthly_report;

        public IPreventive    _preventive;

        public IDailyReport _dreport;
        //** E N D

        // End

        // Tables
        public tbl_apc_personnel   _tbl_apc_personnel     = new tbl_apc_personnel();
        public tbl_joborder        _tbl_joborder          = new tbl_joborder();
        public tbl_joborder_details _tbl_joborder_details = new tbl_joborder_details();
        public tbl_applications    _tbl_applications      = new tbl_applications();
        public tbl_priority        _tbl_priority          = new tbl_priority();
        public tbl_status          _tbl_status            = new tbl_status();
        public tbl_category_type   _tbl_category_type     = new tbl_category_type();
        public tbl_joborder_category _tbl_category        = new tbl_joborder_category();
        public tbl_gantt_chart      _tbl_gantt_chart      = new tbl_gantt_chart();
        public tbl_downtime         _tbl_downtime         = new tbl_downtime();
        public tbl_downtime_cause   _tbl_downtime_cause   = new tbl_downtime_cause();
        public tbl_allocated_time   _tbl_allocated_time   = new tbl_allocated_time();
        public tbl_sub_assignees    _tbl_sub_assignees = new tbl_sub_assignees();

        public tbl_server           _tbl_server    = new tbl_server();
        public tbl_network          _tbl_network   = new tbl_network();
        public tbl_telephone        _tbl_telephone = new tbl_telephone();
        
        public tbl_preventive_maintenance _tbl_preventive_maintenance  = new tbl_preventive_maintenance();
        public tbl_preventive_services    _tbl_preventive_services     = new tbl_preventive_services();
        public tbl_preventive_pc          _tbl_preventive_pc           = new tbl_preventive_pc();
        public tbl_preventive_diagnose    _tbl_preventive_diagnose     = new tbl_preventive_diagnose();
       
        //Cost Center
        public tbl_sbu_costcenter _tbl_sbu_costcenter  = new tbl_sbu_costcenter();
        public tbl_sbu_division   _tbltbl_sbu_division = new tbl_sbu_division();
        public tbl_jo_cc_details _tbl_jo_cc_details    = new tbl_jo_cc_details();
        public tbl_cc_category   _tbl_cc_category      = new tbl_cc_category();
        public tbl_sbu          _tbl_sbu               = new tbl_sbu();

        //Daily report
        public tbl_daily_report _tbl_daily_report = new tbl_daily_report();

        public tbl_onhold_message _tbl_onhold_message = new tbl_onhold_message();
        public tbl_status_timeline _tbl_status_timeline = new tbl_status_timeline();
        public tbl_jo_creation_availability _tbl_jo_creation_availability = new tbl_jo_creation_availability();
        public tbl_problems _tbl_problems = new tbl_problems();

        // END  


    }
}
