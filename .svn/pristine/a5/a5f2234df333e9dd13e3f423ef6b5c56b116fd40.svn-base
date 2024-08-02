using db.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.Repository.Status
{
    public interface IStatus : IDisposable
    {
        IEnumerable<tbl_status> GetAllStatus();
        void InsertStatus(tbl_status status);
        void DeleteStatus(int status);
        void UpdateStatus(tbl_status status);

        IEnumerable<tbl_status_timeline> StatusTimeline();
        void InsertStatusTimeline(tbl_status_timeline status);
        void DeleteStatusTimeline(int status);
        void UpdateStatusTimeline(tbl_status_timeline status);
        void Save();
    }
}
