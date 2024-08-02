using db.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.Repository.Server
{
    public class Server : IServer, IDisposable
    {
        private JOEntities _jo;

        public Server(JOEntities jo)
        {
            this._jo = jo;
        }
        public IEnumerable<tbl_server> GetAllServer()
        {
            return _jo.tbl_server.ToList();
        }

        public tbl_server GetServerByID(int server_id)
        {
            return _jo.tbl_server.Find(server_id);
        }

        public void InserServer(tbl_server server)
        {
            _jo.tbl_server.Add(server);
        }

        public void UpdatServer(tbl_server server)
        {
            _jo.Entry(server).State = EntityState.Modified;
        }

        public void DeleteServer(int server_id)
        {
            tbl_server server = _jo.tbl_server.Find(server_id);
            _jo.tbl_server.Remove(server);
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
