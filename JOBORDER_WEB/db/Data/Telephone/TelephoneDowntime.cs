using db.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.Data.Telephone
{
    public class TelephoneDowntime : ITelephoneDowntime
    {
        private JOEntities _jo;

        public TelephoneDowntime(JOEntities jo)
        {
            this._jo = jo;
        }
        public IEnumerable<sp_tel_downtime_details_Result> GetTelephoneDowntimeDetails()
        {
            return _jo.sp_tel_downtime_details().ToList();
        }
    }
}
