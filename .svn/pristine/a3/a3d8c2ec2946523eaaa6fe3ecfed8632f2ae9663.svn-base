using db.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.Data.JobOrder
{
    public interface IJobOrderReport
    {
        IEnumerable<sp_hw_jo_monthly_report_Result> GetHWMonthlyReport(DateTime DateFilter);
        IEnumerable<sp_sw_jo_monthly_report_Result> GetSWMonthlyReport(DateTime DateFilter);
        IEnumerable<sp_jo_by_dept_Result> GetJOByDepartment();
        IEnumerable<sp_apc_completed_jo_Result> GetAllCompletedJO();
        IEnumerable<sp_apc_delayed_jo_Result> GetAllDelayedJO();
        IEnumerable<sp_apc_ontime_jo_Result> GetAllOnTimeJO();
        IEnumerable<sp_graph_costcenter_Result> GetCostCenterGraph(DateTime Datefrom, DateTime DateTo);
        IEnumerable<sp_shared_jo_Result> GetSharedJO(string Assignee);
    }
}
