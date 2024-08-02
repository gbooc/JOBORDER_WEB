using db.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.Repository.Network
{
    public class Network : INetwork, IDisposable
    {
        private JOEntities _jo;

        public Network(JOEntities jo)
        {
            this._jo = jo;
        }
        public IEnumerable<tbl_network> GetAllNetwork()
        {
            return _jo.tbl_network.ToList();
        }

        public tbl_network GetNetworkByID(int Network_id)
        {
            return _jo.tbl_network.Find(Network_id);
        }

        public void InserNetwork(tbl_network Network)
        {
            _jo.tbl_network.Add(Network);
        }

        public void UpdatNetwork(tbl_network Network)
        {
            _jo.Entry(Network).State = EntityState.Modified;
        }

        public void DeleteNetwork(int Network_id)
        {
            tbl_network Network = _jo.tbl_network.Find(Network_id);
            _jo.tbl_network.Remove(Network);
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
