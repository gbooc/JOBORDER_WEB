using db.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.Repository.Applications
{
    public interface IApplications: IDisposable
    {
        IEnumerable<tbl_applications> GetAllApplications();
        tbl_applications GetApplicationsByID(int jo_id);
        void InsertApplication(tbl_applications app);
        void DeleteApplication(int jo_id);
        void UpdateApplication(tbl_applications jo);
        void Save();
    }
}
