using db.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.Data.Telephone
{
    public interface ITelephoneDowntime 
    {
        IEnumerable<sp_tel_downtime_details_Result> GetTelephoneDowntimeDetails();
    }
}
