using db.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.Repository.Telephone
{
    public interface ITelephone : IDisposable
    {
        IEnumerable<tbl_telephone> GetAllTelephone();
        tbl_telephone GetTelephoneByID(int telephone_id);
        void InserTelephone(tbl_telephone telephone);
        void DeleteTelephone(int telephone_id);
        void UpdateTelephone(tbl_telephone telephone);
        void Save();
    }
}
