using db.Data.JobOrder;
using db.Database;
using db.Global;
using db.Repository;
using db.Repository.Employee;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services;
using System.Windows.Forms;
using Web_RIMP.Models;

namespace Web_RIMP.Controllers
{
    public class HomeController : Controller
    {
        #region Global
       
        RepositoryInitialization repository = new RepositoryInitialization();

        public HomeController()
        {
            this.repository._employee = new Employee(new JOEntities());
            this.repository._jo_classification = new JobOrderClassification(new JOEntities());
        }

        public HomeController(IEmployee employee, IJobOrderClassification classification)
        {
            this.repository._employee = employee;
            this.repository._jo_classification = classification;
        }

        #endregion


        public ActionResult Index()
        {
            if (HttpContext.User.Identity.Name == null || HttpContext.User.Identity.Name == "")
            {
                return View();
            }

            return RedirectToAction("Dashboard", "Dashboard");
        }

        [WebMethod]
        public ActionResult Login(string EmpID, string Password)
        {
            var flag = "";

            if (EmpID.Equals("gris") && Password.Equals("gris"))
            {
                flag = "gris";

                Response.Cookies.Add(new HttpCookie("Fullname", "Visitor"));
                Response.Cookies.Add(new HttpCookie("EmpID", "Visitor"));
                Response.Cookies.Add(new HttpCookie("Department", "Visitor"));
                Response.Cookies.Add(new HttpCookie("Dept_ID", "Visitor"));
                Response.Cookies.Add(new HttpCookie("IdPicture", "Visitor"));


                return Json(flag, JsonRequestBehavior.AllowGet);
            }
            //Outside ricoh | LGC
            else if (EmpID.Equals("v0002") && Password.Equals("0123456789"))
            {
                var image = "";

                FormsAuthentication.SetAuthCookie(EmpID, false);

                Response.Cookies.Add(new HttpCookie("Fullname", "DAG-UMAN, RANDY"));
                Response.Cookies.Add(new HttpCookie("EmpID", EmpID));
                Response.Cookies.Add(new HttpCookie("Department", "LGC"));
                Response.Cookies.Add(new HttpCookie("Dept_ID", "09"));
                Response.Cookies.Add(new HttpCookie("IdPicture", image));
                flag = "1";

                return Json(flag, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var TmpEmpID = EmpID.Length > 7 ? EmpID.TrimEnd('s') : EmpID; // for account with s

                //Override ID
                EmpID = TmpEmpID;

                //Check if login is allowed
                var RegisteredEmployee = repository._employee.GetRegisteredEmployees()
                                                             .Where(e => e.emp_id.Trim().Equals(EmpID.Trim()))
                                                             .Select(e => e.emp_id)
                                                             .FirstOrDefault();

                var RegisteredID = RegisteredEmployee == null ? "0" : RegisteredEmployee.Trim();

                //// Get registered employee details
                var Employee = repository._employee.GetEmployees()
                                                   .Where(x => x.emp_id.Trim().Equals(RegisteredID.Trim()))
                                                   .FirstOrDefault();
                if (Employee != null)
                {

                    if (!Employee.dept_name.Equals("LGC"))
                    {
                        using (var context = new PrincipalContext(ContextType.Domain, "pri.local"))
                        {
                            bool isValid = context.ValidateCredentials(EmpID, Password);

                            if (isValid)
                            {
                                string cleanId = Employee.emp_id.Insert(3, "-");

                                var image = "http://kpsmsvm:90/Content/images/" + cleanId.Substring(1, cleanId.Length - 1).Trim() + ".bmp";

                                FormsAuthentication.SetAuthCookie(Employee.emp_id, false);

                                Response.Cookies.Add(new HttpCookie("Fullname", Employee.emp_name));
                                Response.Cookies.Add(new HttpCookie("EmpID", Employee.emp_id));
                                Response.Cookies.Add(new HttpCookie("Department", Employee.dept_name));
                                Response.Cookies.Add(new HttpCookie("Dept_ID", Employee.dept_id));
                                Response.Cookies.Add(new HttpCookie("IdPicture", image));

                                if (Employee.emp_id.Trim() == "g930192") // Sir alred access only
                                    flag = "2";
                                else
                                    flag = "1";
                            }
                        }
                    }
                }

                return Json(flag, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            Response.Cookies.Clear();

            FormsAuthentication.SignOut();
            Session.Abandon();

            // Clear authentication cookie
            HttpCookie rFormsCookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            rFormsCookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(rFormsCookie);

            return RedirectToAction("Index");
        }

        public ActionResult NotAuthorized()
        {
            return View();
        }
        public ActionResult Canteen()
        {
            return View();
        }
        public ActionResult GetTransaction(string EmpID, DateTime From, DateTime To)
        {

            List<CanteenTransaction> TransactionList = new List<CanteenTransaction>();

            var conn_dev2 = new SqlConnection("Data Source=KPSDEV; Initial Catalog=MISDB; User ID=sa; Password=sapcpcebu");
                conn_dev2.Open();

            SqlCommand cmd = new SqlCommand("sp_get_transaction_grace", conn_dev2);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@empid", EmpID);
            cmd.Parameters.AddWithValue("@datefrom", From);
            cmd.Parameters.AddWithValue("@dateto", To);

            SqlDataReader sdr = cmd.ExecuteReader();

            while (sdr.Read())
            {

                TransactionList.Add(new CanteenTransaction
                {
                    Date   = Convert.ToDateTime(sdr["trans_date"]).ToShortDateString(),
                    Amount = sdr["amount"].ToString(),
                    EmpID  = sdr["trans_emp_id"].ToString()
                });
            }

            return Json(TransactionList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GeneralSearch(string Keyword)
        {
            var SearchResult = repository._jo_classification.SPGeneralSearch(Keyword)
                                         .ToList();

            return Json(JsonRequestBehavior.AllowGet);
        }
    }
}