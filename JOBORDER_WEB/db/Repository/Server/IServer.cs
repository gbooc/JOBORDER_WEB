using db.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.Repository.Server
{
    public interface IServer : IDisposable
    {
        IEnumerable<tbl_server> GetAllServer();
        tbl_server GetServerByID(int server_id);
        void InserServer(tbl_server server);
        void DeleteServer(int server_id);
        void UpdatServer(tbl_server server);
        void Save();
    }
}
