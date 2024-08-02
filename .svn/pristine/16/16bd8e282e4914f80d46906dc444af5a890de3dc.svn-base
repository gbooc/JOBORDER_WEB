using db.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.Data.Dashboard
{
    public interface IDashboard
    {
        IEnumerable<sp_developer_jo_summary_Result> GetDevJOSummary(string dev);
        IEnumerable<sp_all_apc_jo_count_Result> GetAllDevJOCount();
        IEnumerable<sp_dashboard_percentage_of_jod_Result> JOPercentage();
    }
}
