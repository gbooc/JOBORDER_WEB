using db.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.Repository.APC
{
    public class Programmer : IProgrammer, IDisposable
    {
        private JOEntities _jo;

        #region APC
        public Programmer(JOEntities jo)
        {
            this._jo = jo;
        }

        public IEnumerable<tbl_apc_personnel> GetAPC()
        {
            return _jo.tbl_apc_personnel.ToList();
        }

        public tbl_apc_personnel GetAPCByID(int apc_id)
        {
            return _jo.tbl_apc_personnel.Find(apc_id);
        }

        public void InsertAPC(tbl_apc_personnel apc)
        {
            _jo.tbl_apc_personnel.Add(apc);
        }

        public void UpdateAPC(tbl_apc_personnel apc)
        {
            _jo.Entry(apc).State = EntityState.Modified;
        }

        public void DeleteAPC(int apc_id)
        {
            tbl_apc_personnel apc = _jo.tbl_apc_personnel.Find(apc_id);
            _jo.tbl_apc_personnel.Remove(apc);
        }
        #endregion

        #region Daily Report
        public IEnumerable<tbl_daily_report> GetAllReports()
        {
            return _jo.tbl_daily_report.ToList();
        }

        public tbl_daily_report GetReportsByID(int apc_id)
        {
            return _jo.tbl_daily_report.Find(apc_id);
        }

        public void InsertReport(tbl_daily_report apc)
        {
            _jo.tbl_daily_report.Add(apc);
        }

        public void UpdateReport(tbl_daily_report apc)
        {
            _jo.Entry(apc).State = EntityState.Modified;
        }

        public void DeleteReport(int apc_id)
        {
            tbl_daily_report apc = _jo.tbl_daily_report.Find(apc_id);
            _jo.tbl_daily_report.Remove(apc);
        }

        #endregion

        #region On Hold Messages
        public IEnumerable<tbl_onhold_message> OnHoldAllMessages()
        {
            return _jo.tbl_onhold_message.ToList();
        }

        public tbl_onhold_message OnHoldAllMessageByID(int apc_id)
        {
            return _jo.tbl_onhold_message.Find(apc_id);
        }

        public void InsertOnHoldAllMessage(tbl_onhold_message apc)
        {
            _jo.tbl_onhold_message.Add(apc);
        }

        public void UpdateOnHoldAllMessage(tbl_onhold_message apc)
        {
            _jo.Entry(apc).State = EntityState.Modified;
        }

        public void DeleteOnHoldAllMessage(int apc_id)
        {
            tbl_onhold_message apc = _jo.tbl_onhold_message.Find(apc_id);
            _jo.tbl_onhold_message.Remove(apc);
        }

        #endregion

        #region Common
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
        #endregion
    }
}
