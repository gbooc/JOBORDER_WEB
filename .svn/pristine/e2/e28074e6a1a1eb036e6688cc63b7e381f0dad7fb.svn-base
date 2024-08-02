using db.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.Repository.Downtime
{
    public interface IDowntime : IDisposable
    {
        IEnumerable<tbl_downtime> GetAllDowntime();
        tbl_downtime GetDowntimeByID(int downtime_id);
        void InserDowntime(tbl_downtime downtime);
        void DeleteDowntime(int downtime_id);
        void UpdateDowntime(tbl_downtime downtime);
        void Save();
    }
}
