using db.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.Data.JobOrder
{
    public class JobOrderClassification : IJobOrderClassification
    {
        private  JOEntities _jo;

        public JobOrderClassification(JOEntities jo)
        {
            this._jo = jo;
        }
        public IEnumerable<sp_joborder_classification_Result> GetJobOrderClassification()
        {
            return _jo.sp_joborder_classification().ToList();
        }

        public IEnumerable<sp_joborder_details_Result> GetListOfJobOrder()
        {
            return _jo.sp_joborder_details().ToList();
        }
        public IEnumerable<sp_jo_details_Result> SPJODetails(string JobNo)
        {
            return _jo.sp_jo_details(JobNo).ToList();
        }
        public IEnumerable<sp_jo_by_dept_details_Result> GetOnGoingJOByDept()
        {
            return _jo.sp_jo_by_dept_details().ToList();
        }
        public IEnumerable<sp_kpi_monthlyAssignedJO_Result> KPIMonthlyAssignedJO(string Year)
        {
            return _jo.sp_kpi_monthlyAssignedJO(Year).ToList();
        }
        public IEnumerable<sp_kpi_completedJO_Result> KPICompletedJO(string Year)
        {
            return _jo.sp_kpi_completedJO(Year).ToList();
        }
        public IEnumerable<sp_kpi_joPerYeardetails_Result> KPIJoPerYearDetails()
        {
            return _jo.sp_kpi_joPerYeardetails().ToList();
        }
        public IEnumerable<sp_kpi_joperyear_Result> KPIJoPerYear()
        {
            return _jo.sp_kpi_joperyear().ToList();
        }
        public IEnumerable<sp_kpi_hardware_Result> KPIHardware()
        {
            return _jo.sp_kpi_hardware().ToList();
        }
        public IEnumerable<sp_kpi_satisfactory_rate_Result> KPISatisfactoryRate()
        {
            return _jo.sp_kpi_satisfactory_rate().ToList();
        }
        public IEnumerable<sp_jodetails_by_dept_Result> MyJOAllDepartment(string Department)
        {
            return _jo.sp_jodetails_by_dept(Department).ToList();
        }
        public IEnumerable<sp_ForApprovalJO_Result> SP_ForApprovalJO(string Department, string Username)
        {
            return _jo.sp_ForApprovalJO(Department,Username).ToList();
        }
        public IEnumerable<sp_kpi_resource_utilization_Result> KPIResourceUtilization()
        {
            return _jo.sp_kpi_resource_utilization().ToList();
        }
        public IEnumerable<sp_departments_softwares_Result> DepartmentsSoftware(string Department)
        {
            return _jo.sp_departments_softwares(Department).ToList();
        }
        public IEnumerable<sp_email_RequestorAndDeptheads_Result> JODesignationEmail(string JONum)
        {
            return _jo.sp_email_RequestorAndDeptheads(JONum).ToList();
        }
        public IEnumerable<sp_GeneralSearch_Result> SPGeneralSearch(string Keyword)
        {
            return _jo.sp_GeneralSearch(Keyword).ToList();
        }

        public IEnumerable<sp_jo_pending_status_Result> SPOnGoingJO()
        {
            return _jo.sp_jo_pending_status().ToList();
        }
        public IEnumerable<sp_jo_unassigned_Result> SPJoUnassignedList()
        {
            return _jo.sp_jo_unassigned().ToList();
        }
        public IEnumerable<sp_jo_time_allocation_Result> SPTimeAllocation(int DetailsID)
        {
            return _jo.sp_jo_time_allocation(DetailsID).ToList();

        }
        public IEnumerable<sp_latest_available_apc_Result> SPLatestAvailable()
        {
            return _jo.sp_latest_available_apc().ToList();
        }
        public IEnumerable<sp_timeline_status_Result> SPTimelineStatus(int DetailsiD)
        {
            return _jo.sp_timeline_status(DetailsiD).ToList();
        }
    }
}
