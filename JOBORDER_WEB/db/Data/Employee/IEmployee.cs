using db.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.Repository
{
    public interface IEmployee
    {
        //All Employee Information Without department manager
        IEnumerable<vw_Employee> GetEmployees();
        IEnumerable<tbl_employees> GetRegisteredEmployees();
        vw_Employee GetEmployeesByID(int id);
        IEnumerable<vw_Managers> GetEmployeeManagers();
        IEnumerable<vw_Emails> GetEmails();
        IEnumerable<sp_get_transaction_Result> GetCanteen(string EmpID,string From, string To);
        
    }
}
