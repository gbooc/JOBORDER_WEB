using db.Data.Dashboard;
using db.Data.JobOrder;
using db.Database;
using db.Global;
using db.Repository.APC;
using db.Repository.JobOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Web_RIMP.Models;

namespace Web_RIMP.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        #region Global Initialization

        RepositoryInitialization repository = new RepositoryInitialization();

        public DashboardController()
        {
            this.repository._apc = new Programmer(new JOEntities());
            this.repository._dashboard = new Dashboard(new JOEntities());
            this.repository._jo = new JobOrder(new JOEntities());
            this.repository._jo_classification = new JobOrderClassification(new JOEntities());
        }

        public DashboardController(IProgrammer apc, IDashboard dashboard, IJoborder jo, IJobOrderClassification classificatoin)
        {
            this.repository._apc = apc;
            this.repository._dashboard = dashboard;
            this.repository._jo = jo;
            this.repository._jo_classification = classificatoin;
        }

        #endregion
      
        // GET: Dashboard
        public ActionResult Dashboard()
        {
            try
            {
                var dashboard = (from c in repository._dashboard.JOPercentage()
                                 select new DashboardModel
                                 {
                                    Total = c.total,
                                    Completed  = (int)c.completed,
                                    Ongoing    = (int)c.ongoing,
                                    Unassigned = (int)c.pending,
                                    Rejected   = (int)c.rejected,
                                    PercentageCompleted  = (int)c.prcntage_completed,
                                    PercentageOngoing    = (int)c.prcntage_ongoing,
                                    PercentageUnassigned = (int)c.prcntage_unassigned,
                                    PercentageRejected   = (int)c.prcntage_rejected,
                                    Overdue              = (int)c.overdue
                                 }).FirstOrDefault();

                return View(dashboard);
            }
            catch
            {

                Session.Abandon();
                Response.Cookies.Clear();

                FormsAuthentication.SignOut();
                Session.Abandon();

                // Clear authentication cookie
                HttpCookie rFormsCookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
                rFormsCookie.Expires = DateTime.Now.AddYears(-1);
                Response.Cookies.Add(rFormsCookie);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}