using db.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.Repository.Status
{
    public class Status : IStatus, IDisposable
    {
        private JOEntities _jo;

        public Status(JOEntities jo)
        {
            this._jo = jo;
        }

        public IEnumerable<tbl_status> GetAllStatus()
        {
            return _jo.tbl_status.ToList();
        }

        public void InsertStatus(tbl_status Status)
        {
            _jo.tbl_status.Add(Status);
        }

        public void UpdateStatus(tbl_status Status)
        {
            _jo.Entry(Status).State = EntityState.Modified;
        }

        public void DeleteStatus(int Status)
        {
            tbl_status app = _jo.tbl_status.Find(Status);
            _jo.tbl_status.Remove(app);
        }

        public IEnumerable<tbl_status_timeline> StatusTimeline()
        {
            return _jo.tbl_status_timeline.ToList();
        }

        public void InsertStatusTimeline(tbl_status_timeline Status)
        {
            _jo.tbl_status_timeline.Add(Status);
        }

        public void UpdateStatusTimeline(tbl_status_timeline Status)
        {
            _jo.Entry(Status).State = EntityState.Modified;
        }

        public void DeleteStatusTimeline(int Status)
        {
            tbl_status_timeline app = _jo.tbl_status_timeline.Find(Status);
            _jo.tbl_status_timeline.Remove(app);
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
