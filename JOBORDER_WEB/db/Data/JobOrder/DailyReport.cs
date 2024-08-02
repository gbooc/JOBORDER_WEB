using db.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.Data.JobOrder
{
    public class DailyReport : IDailyReport
    {
        private JOEntities _jo;

        public DailyReport(JOEntities jo)
        {
            this._jo = jo;
        }

        public IEnumerable<sp_export_daily_report_Result> GetAllDailyReport(string APC, DateTime Date)
        {
            return _jo.sp_export_daily_report(APC,Date).ToList();
        }
    }
}
