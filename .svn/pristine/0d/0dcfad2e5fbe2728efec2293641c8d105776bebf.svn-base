using db.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.Data.Server
{
    public class ServerDowntime : IServerDowntime
    {
        private JOEntities _jo;

        public ServerDowntime(JOEntities jo)
        {
            this._jo = jo;
        }
        public IEnumerable<sp_server_downtime_details_Result> GetServerDowntimeDetails()
        {
            return _jo.sp_server_downtime_details().ToList();
        }
    }
}
