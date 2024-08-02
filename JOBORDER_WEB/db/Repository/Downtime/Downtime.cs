using db.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.Repository.Downtime
{
   public class Downtime : IDowntime, IDisposable
    {
        private JOEntities _jo;

        public Downtime(JOEntities jo)
        {
            this._jo = jo;
        }
        public IEnumerable<tbl_downtime> GetAllDowntime()
        {
            return _jo.tbl_downtime.ToList();
        }

        public tbl_downtime GetDowntimeByID(int downtime_id)
        {
            return _jo.tbl_downtime.Find(downtime_id);
        }

        public void InserDowntime(tbl_downtime downtime)
        {
            _jo.tbl_downtime.Add(downtime);
        }

        public void UpdateDowntime(tbl_downtime downtime)
        {
            _jo.Entry(downtime).State = EntityState.Modified;
        }

        public void DeleteDowntime(int downtime_id)
        {
            tbl_downtime downtime = _jo.tbl_downtime.Find(downtime_id);
            _jo.tbl_downtime.Remove(downtime);
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
