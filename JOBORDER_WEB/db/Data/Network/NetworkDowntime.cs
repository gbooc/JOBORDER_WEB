using db.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.Data.Network
{
    public class NetworkDowntime : INetworkDowntime
    {
        private JOEntities _jo;

        public NetworkDowntime(JOEntities jo)
        {
            this._jo = jo;
        }

        public IEnumerable<sp_network_downtime_details_Result> GetNetworkDowntimeDetails()
        {
            return _jo.sp_network_downtime_details().ToList();
        }
    }
}
