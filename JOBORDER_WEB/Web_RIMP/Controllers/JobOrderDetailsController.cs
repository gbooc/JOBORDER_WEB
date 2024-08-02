using db.Data.Dashboard;
using db.Data.JobOrder;
using db.Database;
using db.Global;
using db.Repository;
using db.Repository.Employee;
using db.Repository.JobOrder;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_RIMP.Models;

namespace Web_RIMP.Controllers
{
    [Authorize]
    public class JobOrderDetailsController : Controller
    {
        #region Global
        RepositoryInitialization repository = new RepositoryInitialization();

        public JobOrderDetailsController()
        {
            this.repository._jo = new JobOrder(new JOEntities());
            this.repository._employee = new Employee(new JOEntities());
            this.repository._monthly_report = new JobOrderReport(new JOEntities());
            this.repository._dashboard = new Dashboard(new JOEntities());
            this.repository._jo_classification = new JobOrderClassification(new JOEntities());
        }

        public JobOrderDetailsController(IJoborder jo, IEmployee employee, JobOrderReport report, IDashboard dashboard, IJobOrderClassification classification)
        {
            this.repository._jo = jo;
            this.repository._employee = employee;
            this.repository._monthly_report = report;
            this.repository._dashboard = dashboard;
            this.repository._jo_classification = classification;
        }

        #endregion

        public ActionResult MyJobOrder(int? page, string Name, string Search)
        {
            //Override --Login User
            if (string.IsNullOrEmpty(Name))
            {
                Name = Request.Cookies["Fullname"].Value.ToString();
                ViewBag.FullName = Request.Cookies["Fullname"].Value.ToString();
                ViewBag.IdPicture = Request.Cookies["IdPicture"].Value.ToString();
                ViewBag.Department = Request.Cookies["Department"].Value.ToString();
                ViewBag.EmpId = Request.Cookies["EmpID"].Value.ToString();

            }
            else // Assignee(APC)
            {

                var Employee = repository._employee.GetEmployees()
                                         .Where(e => e.emp_name.Trim().Equals(Name.Trim()))
                                         .FirstOrDefault();
                string cleanId = Employee.emp_id.Insert(3, "-");

                ViewBag.FullName = Employee.emp_name;
                ViewBag.IdPicture = "http://kpsmsvm:90/Content/images/" + cleanId.Substring(1, cleanId.Length - 1).Trim() + ".bmp";
                ViewBag.Department = Employee.dept_name;
                ViewBag.EmpId = Employee.emp_id;
            }

            var JO = repository._jo.GetMyJobOrder(Name)
                                .Select(j => new JobOrderDetailsModel
                                {
                                    JODetailsID = j.details_id,
                                    JobNumber = j.job_no,
                                    JoTypeDetails = j.app,
                                    Status = j.status,
                                    Assignee = j.assignee,
                                    DateAssigned = j.date_assigned.ToString(),
                                    Datefiled = Convert.ToDateTime(j.date_filed).ToString("yyyy-MM-dd"),
                                    DateTarget = Convert.ToDateTime(j.date_target).ToString("yyyy-MM-dd"),
                                    DateStarted = Convert.ToDateTime(j.date_started).ToString("yyyy-MM-dd"),
                                    DateEnded = Convert.ToDateTime(j.date_ended).ToString("yyyy-MM-dd"),
                                })
                                .OrderByDescending(j => j.Status)
                                .ToList();

            var Department = Request.Cookies["Department"].Value.ToString();
            //Breakdown of items of each hw jo category
            ViewBag.HWSummary = repository._jo.GetMyJOSummaryHW(Name).ToList();
            ViewBag.SWSummary = repository._jo.GetMyJOSummarySW(Name).ToList();

            ViewBag.HWGraphSummary = repository._jo.GetGraphSummaryHW(Name).ToList();

            ViewBag.DepartmentJO = repository._jo_classification.MyJOAllDepartment(Department)
                                                                .OrderByDescending(x => x.status)
                                                                .Select(x => new MyJODeptModel 
                                                                {
                                                                JONumber = x.job_no,
                                                                Requestor = x.requested_by,
                                                                Assignee = x.assignee,
                                                                Status = x.status,
                                                                ProgressRate = x.progress_rate,
                                                                Details = x.details,
                                                                DetailsID = x.details_id,
                                                                DateTarget = Convert.ToDateTime(x.date_target).ToString("yyyy-MM-dd"),
                                                                DateAssigned = Convert.ToDateTime(x.date_assigned).ToString("yyyy-MM-dd")
                                                                })
                                                                .ToList();

            return View(JO);
        }

        public ActionResult SearchMyJO(string Search)
        {
           var Name = Request.Cookies["Fullname"].Value.ToString();

            var Result = repository._jo.GetMyJobOrder(Name)
                                   .Where(x =>
                                               x.job_no.ToLower().Trim().Contains(Search.ToLower().Trim()) ||
                                               x.app.ToLower().Trim().Contains(Search.ToLower().Trim()) ||
                                               x.status.ToLower().Trim().Contains(Search.ToLower().Trim())
                                   )
                                   .Select(j => new JobOrderDetailsModel
                                   {
                                       JODetailsID = j.details_id,
                                       JobNumber = j.job_no,
                                       JoTypeDetails = j.app,
                                       Status = j.status,
                                       Assignee = j.assignee,
                                       DateAssigned = j.date_assigned.ToString(),
                                       Datefiled = Convert.ToDateTime(j.date_filed).ToString("yyyy-MM-dd"),
                                       DateTarget = Convert.ToDateTime(j.date_target).ToString("yyyy-MM-dd"),
                                       DateStarted = Convert.ToDateTime(j.date_started).ToString("yyyy-MM-dd"),
                                       DateEnded = Convert.ToDateTime(j.date_ended).ToString("yyyy-MM-dd"),
                                   })
                                   .OrderByDescending(j => j.Status)
                                   .ToList();

            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchMyJoByDept(string Keyword)
        {
            var Department = Request.Cookies["Department"].Value.ToString();
            var Result = repository._jo_classification.MyJOAllDepartment(Department)
                                                              .Select(x => new MyJODeptModel
                                                              {
                                                                  JONumber = x.job_no,
                                                                  Requestor = x.requested_by,
                                                                  Assignee = x.assignee,
                                                                  Status = x.status,
                                                                  ProgressRate = x.progress_rate,
                                                                  Details = x.details,
                                                                  DetailsID = x.details_id,
                                                                  DateTarget = Convert.ToDateTime(x.date_target).ToString("yyyy-MM-dd"),
                                                                  DateAssigned = Convert.ToDateTime(x.date_assigned).ToString("yyyy-MM-dd")
                                                              })
                                                              .Where(x => x.JONumber.ToLower().Contains(Keyword.ToLower()) 
                                                                    || x.Requestor.ToLower().Contains(Keyword.ToLower())
                                                                    || x.Assignee.ToLower().Contains(Keyword.ToLower())
                                                                    || x.Status.ToLower().Contains(Keyword.ToLower())
                                                                    || x.Details.ToLower().Contains(Keyword.ToLower())
                                                              )
                                                              .ToList();

            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult ResultByDepartment()
        {
            var department = repository._monthly_report.GetJOByDepartment()
                                                       .Select(x => new JOByDepartment
                                                       {
                                                           DeptName = x.department,
                                                           Count = x.jo_count.ToString()
                                                       })
                                                       .ToList();

            ViewBag.ClosedJO  = repository._monthly_report.GetAllCompletedJO().ToList();
            ViewBag.DelayedJO = repository._monthly_report.GetAllDelayedJO().ToList();
            ViewBag.OntimeJO  = repository._monthly_report.GetAllOnTimeJO().ToList();
            
            return PartialView(department);
        }
    }
}