using db.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.Repository.Network
{
    public interface INetwork : IDisposable
    {
        IEnumerable<tbl_network> GetAllNetwork();
        tbl_network GetNetworkByID(int network_id);
        void InserNetwork(tbl_network network);
        void DeleteNetwork(int network_id);
        void UpdatNetwork(tbl_network network);
        void Save();
    }
}
