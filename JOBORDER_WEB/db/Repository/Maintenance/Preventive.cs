using db.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.Repository.Maintenance
{
    public class Preventive : IPreventive, IDisposable
    {
        private JOEntities _jo;

        public Preventive(JOEntities jo)
        {
            this._jo = jo;
        }

        #region Preventive Maintenance
        public IEnumerable<tbl_preventive_maintenance> GetAllPreventives()
        {
            return _jo.tbl_preventive_maintenance.ToList();
        }

        public tbl_preventive_maintenance GetPreventiveById(int preventive_id)
        {
            return _jo.tbl_preventive_maintenance.Find(preventive_id);
        }

        public void InsertPreventive(tbl_preventive_maintenance preventive)
        {
            _jo.tbl_preventive_maintenance.Add(preventive);
            Save();
        }

        public void UpdatePreventive(tbl_preventive_maintenance preventive)
        {
            _jo.Entry(preventive).State = EntityState.Modified;
        }

        public void DeletePreventive(int preventive_id)
        {
            tbl_preventive_maintenance preventive = _jo.tbl_preventive_maintenance.Find(preventive_id);
            _jo.tbl_preventive_maintenance.Remove(preventive);
        }

        public IEnumerable<sp_get_preventive_maintenance_Result> GetPreventiveSummary()
        {
            return _jo.sp_get_preventive_maintenance().ToList();
        }
        #endregion

        #region Services
        public IEnumerable<tbl_preventive_services> GetAllPreventivesServices()
        {
            return _jo.tbl_preventive_services.ToList();
        }

        public tbl_preventive_services GetPreventivesServicesById(int preventive_id)
        {
            return _jo.tbl_preventive_services.Find(preventive_id);
        }

        public void InsertPreventivesServices(tbl_preventive_services preventive)
        {
            _jo.tbl_preventive_services.Add(preventive);
            Save();
        }

        public void UpdatePreventivesServices(tbl_preventive_services preventive)
        {
            _jo.Entry(preventive).State = EntityState.Modified;
        }

        public void DeletePreventivesServices(int preventive_id)
        {
            tbl_preventive_services preventive = _jo.tbl_preventive_services.Find(preventive_id);
            _jo.tbl_preventive_services.Remove(preventive);
        }
        #endregion

        #region PC
        public IEnumerable<tbl_preventive_pc> GetAllPreventivesPC()
        {
            return _jo.tbl_preventive_pc.ToList();
        }

        public tbl_preventive_pc GetPreventivesPCById(int preventive_id)
        {
            return _jo.tbl_preventive_pc.Find(preventive_id);
        }

        public void InsertPreventivesPC(tbl_preventive_pc preventive)
        {
            _jo.tbl_preventive_pc.Add(preventive);
            Save();
        }

        public void UpdatePreventivesPC(tbl_preventive_pc preventive)
        {
            _jo.Entry(preventive).State = EntityState.Modified;
        }

        public void DeletePreventivesPC(int preventive_id)
        {
            tbl_preventive_pc preventive = _jo.tbl_preventive_pc.Find(preventive_id);
            _jo.tbl_preventive_pc.Remove(preventive);
        }

        #endregion

        #region diagnosis
        public IEnumerable<tbl_preventive_diagnose> GetAllDiagnoses()
        {
            return _jo.tbl_preventive_diagnose.ToList();
        }

        public tbl_preventive_diagnose GetDiagnosesById(int preventive_id)
        {
            return _jo.tbl_preventive_diagnose.Find(preventive_id);
        }

        public void InsertDiagnoses(tbl_preventive_diagnose preventive)
        {
            _jo.tbl_preventive_diagnose.Add(preventive);
            Save();
        }

        public void UpdateDiagnoses(tbl_preventive_diagnose preventive)
        {
            _jo.Entry(preventive).State = EntityState.Modified;
        }

        public void DeleteDiagnoses(int preventive_id)
        {
            tbl_preventive_diagnose preventive = _jo.tbl_preventive_diagnose.Find(preventive_id);
            _jo.tbl_preventive_diagnose.Remove(preventive);
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
