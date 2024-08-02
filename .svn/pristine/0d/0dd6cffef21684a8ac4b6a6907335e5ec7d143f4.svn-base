using db.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.Repository.Priority
{
    public class Priority : IPriority, IDisposable
    {

        private JOEntities _jo;

        public Priority(JOEntities jo)
        {
            this._jo = jo;
        }

        public IEnumerable<tbl_priority> GetAllJOPPriority()
        {
            return _jo.tbl_priority.ToList();
        }

        public void InsertPriority(tbl_priority priority)
        {
            _jo.tbl_priority.Add(priority);
        }

        public void UpdatePriority(tbl_priority priority)
        {
            _jo.Entry(priority).State = EntityState.Modified;
        }

        public void DeletePriority(int priority)
        {
            tbl_priority app = _jo.tbl_priority.Find(priority);
            _jo.tbl_priority.Remove(app);
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
