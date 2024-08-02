using db.Data.Server;
using db.Database;
using db.Global;
using db.Repository.Downtime;
using db.Repository.DowntimeCause;
using db.Repository.Server;
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
    public class ServerController : Controller
    {
        #region Global Initialization

        RepositoryInitialization repository = new RepositoryInitialization();

        public ServerController()
        {
            this.repository._server = new Server(new JOEntities());
            this.repository._downtime = new Downtime(new JOEntities());
            this.repository._downtime_cause = new DowntimeCause(new JOEntities());
            this.repository._server_downtime = new ServerDowntime(new JOEntities());
        }

        public ServerController(IServer server, IDowntime downtime, IDowntimeCause downtime_cause, IServerDowntime server_downtime)
        {
            this.repository._server = server;
            this.repository._downtime = downtime;
            this.repository._downtime_cause = downtime_cause;
            this.repository._server_downtime = server_downtime;
        }

        #endregion

        // GET: Server
        public ActionResult Server()
        {
            if (Request.Cookies["Dept_ID"].Value.ToString() != "09")
                return RedirectToAction("NotAuthorized", "Home");

            var servers = GetAllServerDowntime();

            var DowntimeDate = repository._downtime.GetAllDowntime()
                                         .Where(d => d.downtime_type_id == 1)
                                          .Select(d => new DowntimeModel
                                          {
                                              DowntimeYearAndMonth = Convert.ToDateTime(d.datestarted_of_downtime).ToString("MMM yyyy"),
                                              Month = Convert.ToDateTime(d.datestarted_of_downtime).ToString("MM")
                                          })
                                          .OrderByDescending(d => d.Month)
                                          .ToList();


            ViewBag.DowntimeCause = repository._downtime_cause.GetAllDowntimeCause()
                                               .Where(d => d.downtime_type_id == 1)
                                                .Select(d => new DowntimeModel
                                                {
                                                    DowntimeCauseID = d.downtime_cause_id,
                                                    DowntimeCause = d.hardware_description
                                                }).ToList();

            ViewBag.DowntimeMinutes = repository._downtime.GetAllDowntime()
                                                 .Where(d => Convert.ToDateTime(d.datestarted_of_downtime).ToString("MMM yyyy").Equals(DateTime.Today.ToString("MMM yyyy"))
                                                                               && d.downtime_type_id == 1
                                                 )
                                                .Select(d => new DowntimeModel
                                                {
                                                    DowntimeMinutes = d.total_mins_of_downtime,
                                                    CodeNumber = d.code_number
                                                })
                                                 .OrderBy(d => d.CodeNumber)
                                                 .ToList();

            ViewBag.DowntimeMonthAndYear = DowntimeDate.GroupBy(d => d.DowntimeYearAndMonth)
                                                        .Select(d => d.FirstOrDefault())
                                                        .ToList();

            ViewBag.AllServerDowntime = GetDowntimeByMonth(DateTime.Today);

            return View(servers);
        }

        public ActionResult SearchMonthlyDowntime(DateTime Date)
        {
            var result = repository._downtime.GetAllDowntime()
                                              .Where(d => Convert.ToDateTime(d.datestarted_of_downtime).ToString("MMM yyyy")
                                                                            .Equals(Date.ToString("MMM yyyy"))
                                              ).Select(d => new DowntimeModel
                                              {
                                                  DowntimeMinutes = d.total_mins_of_downtime,
                                                  CodeNumber = d.code_number
                                              })
                                              .OrderBy(d => d.CodeNumber)
                                              .ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveServerDowntime(int CauseID, string CodeNumber,
                                               string DateStarted, string TimeStarted,
                                               string DateEnded, string TimeEnded, string CauseDetails)
        {
            string result = "Success";
            try
            {
                var DowntimeDateStarted = Convert.ToDateTime(DateStarted + " " + TimeStarted);
                var DowntimeDateEnded = Convert.ToDateTime(DateEnded + " " + TimeEnded);

                int DowntimeMinutes = Convert.ToInt32(DowntimeDateEnded.Subtract(DowntimeDateStarted).TotalMinutes);


                repository._tbl_downtime.downtime_type_id = 1;
                repository._tbl_downtime.datestarted_of_downtime = DowntimeDateStarted;
                repository._tbl_downtime.dateended_of_downtime = DowntimeDateEnded;
                repository._tbl_downtime.total_mins_of_downtime = DowntimeMinutes;
                repository._tbl_downtime.downtime_cause_id = CauseID;
                repository._tbl_downtime.code_number = CodeNumber;
                repository._tbl_downtime.downtime_cause_details = CauseDetails;

                repository._downtime.InserDowntime(repository._tbl_downtime);
                repository._downtime.Save();
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetServerDowntimeDetails(string ServernName, string DateFilter)
        {
            var result = repository._server_downtime.GetServerDowntimeDetails()
                                                    .Where(d => Convert.ToDateTime(d.datestarted).ToString("MMM yyyy").Equals(Convert.ToDateTime(DateFilter).ToString("MMM yyyy")))
                                                    .Where(d => d.server_name.Equals(ServernName))
                                                    .Select(d => new DowntimeModel
                                                    {
                                                        DateStarted = d.datestarted.ToString(),
                                                        DateEnded = d.dateended.ToString(),
                                                        DowntimeMinutes = d.dowtime_mins,
                                                        DowntimeCause = d.downtime_cause,
                                                        DowntimeCauseDetails = d.downtime_cause_details
                                                    }).ToList();

            return Json(result.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchServerName(string Term)
        {
            var data = GetAllServerDowntime()
                                 .Where(d => d.DowntimeName.ToLower().Trim().Contains(Term.ToLower().Trim()) 
                                 && !d.DowntimeName.Equals(""))
                                 .Select(d => new DowntimeModel
                                 { 
                                      CodeNumber = d.CodeNumber,
                                      DowntimeName = d.DowntimeName
                                 }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReloadServerDowntime(DateTime DateMonth)
        {
            var Result = GetDowntimeByMonth(DateMonth);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        public List<DowntimeModel> GetAllServerDowntime()
        {
            return repository._server.GetAllServer()
                                      .Select(s => new DowntimeModel
                                      {
                                          ID = s.server_id,
                                          DowntimeName = s.server_name,
                                          CodeNumber = s.server_code_number,
                                          DateStarted = DateTime.Today.ToShortDateString(),
                                          TimeStarted = DateTime.Now.ToShortTimeString(),
                                          DateEnded = DateTime.Today.ToShortDateString(),
                                          TimeEnded = DateTime.Now.ToShortTimeString(),
                                      }).ToList();
        }
        public List<DowntimeModel> GetDowntimeByMonth(DateTime DateMonth)
        {
            return repository._server_downtime.GetServerDowntimeDetails()
                                                    .Where(d => Convert.ToDateTime(d.datestarted).ToString("MMM yyyy").Equals(Convert.ToDateTime(DateMonth).ToString("MMM yyyy")))
                                                    .Select(d => new DowntimeModel
                                                    {
                                                        DateStarted = d.datestarted.ToString(),
                                                        DateEnded = d.dateended.ToString(),
                                                        DowntimeMinutes = d.dowtime_mins,
                                                        DowntimeCause = d.downtime_cause,
                                                        DowntimeCauseDetails = d.downtime_cause_details,
                                                        DowntimeName = d.server_name,

                                                    }).ToList();
        }
    }
}