using db.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.Repository.APC
{
    public interface IProgrammer : IDisposable
    {
        IEnumerable<tbl_apc_personnel> GetAPC();
        tbl_apc_personnel GetAPCByID(int apc_id);
        void InsertAPC(tbl_apc_personnel apc);
        void DeleteAPC(int apc_id);
        void UpdateAPC(tbl_apc_personnel apc);

        #region Daily report
        IEnumerable<tbl_daily_report> GetAllReports();
        tbl_daily_report GetReportsByID(int apc_id);
        void InsertReport(tbl_daily_report apc);
        void DeleteReport(int apc_id);
        void UpdateReport(tbl_daily_report apc);

        #endregion

        #region On Hold Message
        IEnumerable<tbl_onhold_message> OnHoldAllMessages();
        tbl_onhold_message OnHoldAllMessageByID(int msgs_id);
        void InsertOnHoldAllMessage(tbl_onhold_message msgs);
        void DeleteOnHoldAllMessage(int msgs);
        void UpdateOnHoldAllMessage(tbl_onhold_message msgs);

        #endregion
        void Save();
    }
}
