using db.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.Data.Dashboard
{
    public class Dashboard : IDashboard
    {
        private JOEntities _jo;

        public Dashboard(JOEntities jo)
        {
            this._jo = jo;
        }
        public IEnumerable<sp_developer_jo_summary_Result> GetDevJOSummary(string dev)
        {
            return _jo.sp_developer_jo_summary(dev).ToList();
        }
        public IEnumerable<sp_all_apc_jo_count_Result> GetAllDevJOCount()
        {
            return _jo.sp_all_apc_jo_count().ToList();
        }
        public IEnumerable<sp_dashboard_percentage_of_jod_Result> JOPercentage()
        {
            return _jo.sp_dashboard_percentage_of_jod().ToList();
        }
    }
}
