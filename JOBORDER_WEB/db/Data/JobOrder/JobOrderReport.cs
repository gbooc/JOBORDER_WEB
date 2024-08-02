using db.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.Data.JobOrder
{
    public class JobOrderReport : IJobOrderReport
    {
        private JOEntities _jo;

        public JobOrderReport(JOEntities jo)
        {
            this._jo = jo;
        }

        public IEnumerable<sp_hw_jo_monthly_report_Result> GetHWMonthlyReport(DateTime DateFilter)
        {
            return _jo.sp_hw_jo_monthly_report(DateFilter).ToList();
        }

        public IEnumerable<sp_sw_jo_monthly_report_Result> GetSWMonthlyReport(DateTime DateFilter)
        {
            return _jo.sp_sw_jo_monthly_report(DateFilter).ToList();
        }
        public IEnumerable<sp_jo_by_dept_Result> GetJOByDepartment()
        {
            return _jo.sp_jo_by_dept().ToList();
        }
        public IEnumerable<sp_apc_completed_jo_Result> GetAllCompletedJO()
        {
            return _jo.sp_apc_completed_jo().ToList();
        }
        public IEnumerable<sp_apc_delayed_jo_Result> GetAllDelayedJO()
        {
            return _jo.sp_apc_delayed_jo().ToList();
        }
        public IEnumerable<sp_apc_ontime_jo_Result> GetAllOnTimeJO()
        {
            return _jo.sp_apc_ontime_jo().ToList();
        }
        public IEnumerable<sp_graph_costcenter_Result> GetCostCenterGraph(DateTime Datefrom, DateTime DateTo)
        {
            return _jo.sp_graph_costcenter(Datefrom, DateTo).ToList();
        }
        public IEnumerable<sp_shared_jo_Result> GetSharedJO(string Assignee)
        {
            return _jo.sp_shared_jo(Assignee).ToList();
        }
    }
}
