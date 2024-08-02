using db.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.Repository.Employee
{
    public class Employee : IEmployee
    {
        private JOEntities _jo;

        public Employee(JOEntities jo)
        {
            this._jo = jo;
        }

        public IEnumerable<vw_Employee> GetEmployees()
        {
            return _jo.vw_Employee.ToList();
        }

        public IEnumerable<tbl_employees> GetRegisteredEmployees()
        {
            return _jo.tbl_employees.ToList();
        }

        public vw_Employee GetEmployeesByID(int id)
        {
            return _jo.vw_Employee.Find(id);
        }

        public IEnumerable<vw_Managers> GetEmployeeManagers()
        {
            return _jo.vw_Managers.ToList();
        }
        public IEnumerable<vw_Emails> GetEmails()
        {
            return _jo.vw_Emails.ToList();
        }
        public IEnumerable<sp_get_transaction_Result> GetCanteen(string EmpID, string From, string To)
        {
            return _jo.sp_get_transaction(EmpID,From,To).ToList();
        }
    }
}
