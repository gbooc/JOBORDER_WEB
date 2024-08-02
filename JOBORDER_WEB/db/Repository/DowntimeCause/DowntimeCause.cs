using db.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.Repository.DowntimeCause
{
    public class DowntimeCause : IDowntimeCause, IDisposable
    {
        private JOEntities _jo;

        public DowntimeCause(JOEntities jo)
        {
            this._jo = jo;
        }
        public IEnumerable<tbl_downtime_cause> GetAllDowntimeCause()
        {
            return _jo.tbl_downtime_cause.ToList();
        }

        public tbl_downtime_cause GetDowntimeCauseByID(int downtime_id)
        {
            return _jo.tbl_downtime_cause.Find(downtime_id);
        }

        public void InserDowntimeCause(tbl_downtime_cause downtime)
        {
            _jo.tbl_downtime_cause.Add(downtime);
        }

        public void UpdateDowntimeCause(tbl_downtime_cause downtime)
        {
            _jo.Entry(downtime).State = EntityState.Modified;
        }

        public void DeleteDowntimeCause(int downtime_id)
        {
            tbl_downtime_cause downtime = _jo.tbl_downtime_cause.Find(downtime_id);
            _jo.tbl_downtime_cause.Remove(downtime);
        }

        public void Save()
        {
            _jo.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _jo.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}