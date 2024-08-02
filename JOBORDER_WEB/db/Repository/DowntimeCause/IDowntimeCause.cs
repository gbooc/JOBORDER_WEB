using db.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.Repository.DowntimeCause
{
    public interface IDowntimeCause : IDisposable
    {
        IEnumerable<tbl_downtime_cause> GetAllDowntimeCause();
        tbl_downtime_cause GetDowntimeCauseByID(int downtime_id);
        void InserDowntimeCause(tbl_downtime_cause downtime);
        void DeleteDowntimeCause(int downtime_id);
        void UpdateDowntimeCause(tbl_downtime_cause downtime);
        void Save();
    }
}
