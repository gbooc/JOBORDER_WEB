using db.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.Repository.Telephone
{
    public class Telephone : ITelephone, IDisposable
    {
        private JOEntities _jo;

        public Telephone(JOEntities jo)
        {
            this._jo = jo;
        }
        public IEnumerable<tbl_telephone> GetAllTelephone()
        {
            return _jo.tbl_telephone.ToList();
        }

        public tbl_telephone GetTelephoneByID(int Telephone_id)
        {
            return _jo.tbl_telephone.Find(Telephone_id);
        }

        public void InserTelephone(tbl_telephone Telephone)
        {
            _jo.tbl_telephone.Add(Telephone);
        }

        public void UpdateTelephone(tbl_telephone Telephone)
        {
            _jo.Entry(Telephone).State = EntityState.Modified;
        }

        public void DeleteTelephone(int Telephone_id)
        {
            tbl_telephone Telephone = _jo.tbl_telephone.Find(Telephone_id);
            _jo.tbl_telephone.Remove(Telephone);
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
