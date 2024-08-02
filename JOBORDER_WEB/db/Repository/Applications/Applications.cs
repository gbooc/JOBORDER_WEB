using db.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.Repository.Applications
{
    public class Applications : IApplications, IDisposable
    {
        private JOEntities _jo;

        public Applications(JOEntities jo)
        {
            this._jo = jo;
        }
        public IEnumerable<tbl_applications> GetAllApplications()
        {
            return _jo.tbl_applications.ToList();
        }

        public tbl_applications GetApplicationsByID(int jo_id)
        {
            return _jo.tbl_applications.Find(jo_id);
        }

        public void InsertApplication(tbl_applications app)
        {
            _jo.tbl_applications.Add(app);
        }

        public void UpdateApplication(tbl_applications app)
        {
            _jo.Entry(app).State = EntityState.Modified;
        }

        public void DeleteApplication(int jo_id)
        {
            tbl_applications app = _jo.tbl_applications.Find(jo_id);
            _jo.tbl_applications.Remove(app);
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
