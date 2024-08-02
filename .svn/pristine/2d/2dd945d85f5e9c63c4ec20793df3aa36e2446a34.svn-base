using db.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.Data.JobOrder
{
    public interface IJobOrderClassification
    {
        IEnumerable<sp_joborder_classification_Result> GetJobOrderClassification();
        IEnumerable<sp_joborder_details_Result> GetListOfJobOrder();
        IEnumerable<sp_jo_by_dept_details_Result> GetOnGoingJOByDept();
        IEnumerable<sp_kpi_monthlyAssignedJO_Result> KPIMonthlyAssignedJO(string Year);
        IEnumerable<sp_kpi_completedJO_Result> KPICompletedJO(string Year);
        IEnumerable<sp_kpi_joPerYeardetails_Result> KPIJoPerYearDetails();
        IEnumerable<sp_kpi_joperyear_Result> KPIJoPerYear();
        IEnumerable<sp_kpi_hardware_Result> KPIHardware();
        IEnumerable<sp_kpi_satisfactory_rate_Result> KPISatisfactoryRate();
        IEnumerable<sp_jodetails_by_dept_Result> MyJOAllDepartment(string Dept);
        IEnumerable<sp_ForApprovalJO_Result> SP_ForApprovalJO(string Department, string Username);
        IEnumerable<sp_kpi_resource_utilization_Result> KPIResourceUtilization();
        IEnumerable<sp_departments_softwares_Result> DepartmentsSoftware(string Department);
        IEnumerable<sp_email_RequestorAndDeptheads_Result> JODesignationEmail(string JONum);
        IEnumerable<sp_GeneralSearch_Result> SPGeneralSearch(string Keyword);
        IEnumerable<sp_jo_pending_status_Result> SPOnGoingJO();
        IEnumerable<sp_jo_unassigned_Result> SPJoUnassignedList();
        IEnumerable<sp_jo_details_Result> SPJODetails(string JobNo);
        IEnumerable<sp_jo_time_allocation_Result> SPTimeAllocation(int DetailsID);
        IEnumerable<sp_latest_available_apc_Result> SPLatestAvailable();
        IEnumerable<sp_timeline_status_Result> SPTimelineStatus(int DetailsID);

    }
}
