using db.Data.Network;
using db.Database;
using db.Global;
using db.Repository.Downtime;
using db.Repository.DowntimeCause;
using db.Repository.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_RIMP.Models;

namespace Web_RIMP.Controllers
{
    [Authorize]
    public class NetworkController : Controller
    {

        #region Global Initialization

        RepositoryInitialization repository = new RepositoryInitialization();

        public NetworkController()
        {
            this.repository._network = new Network(new JOEntities());
            this.repository._downtime = new Downtime(new JOEntities());
            this.repository._downtime_cause = new DowntimeCause(new JOEntities());
            this.repository._network_downtime = new NetworkDowntime(new JOEntities());
        }

        public NetworkController(INetwork netwrok, IDowntime downtime, IDowntimeCause downtime_cause, INetworkDowntime network_downtime)
        {
            this.repository._network = netwrok;
            this.repository._downtime = downtime;
            this.repository._downtime_cause = downtime_cause;
            this.repository._network_downtime = network_downtime;
        }

        #endregion

        public ActionResult Network()
        {
            if (Request.Cookies["Dept_ID"].Value.ToString() != "09")
                return RedirectToAction("NotAuthorized", "Home");

            var network = repository._network.GetAllNetwork()
                                            .Select(s => new DowntimeModel
                                            {
                                                ID = s.network_id,
                                                CodeNumber = s.network_code_number,
                                                Location = s.location,
                                                DateStarted = DateTime.Today.ToShortDateString(),
                                                TimeStarted = DateTime.Now.ToShortTimeString(),
                                                DateEnded = DateTime.Today.ToShortDateString(),
                                                TimeEnded = DateTime.Now.ToShortTimeString()
                                            });

            ViewBag.DowntimeCause = repository._downtime_cause.GetAllDowntimeCause()
                                              .Where(d => d.downtime_type_id == 2)
                                              .Select(d => new DowntimeModel
                                              {
                                                  DowntimeCauseID = d.downtime_cause_id,
                                                  DowntimeCause = d.hardware_description
                                              })
                                              .ToList();

            ViewBag.DowntimeMinutes = repository._downtime.GetAllDowntime()
                                                 .Where(d => Convert.ToDateTime(d.datestarted_of_downtime).ToString("MMM yyyy")
                                                                                 .Equals(DateTime.Today.ToString("MMM yyyy")) &&
                                                                                 d.downtime_type_id == 2
                                                 )
                                                 .Select(d => new DowntimeModel
                                                 {
                                                     DowntimeMinutes = d.total_mins_of_downtime,
                                                     CodeNumber = d.code_number
                                                 })
                                                .OrderBy(d => d.CodeNumber)
                                                .ToList();

            var DowntimeDate = repository._downtime.GetAllDowntime()
                                         .Where(d => d.downtime_type_id == 2)
                                         .Select(d => new DowntimeModel
                                         {
                                             DowntimeYearAndMonth = Convert.ToDateTime(d.datestarted_of_downtime).ToString("MMM yyyy"),
                                             Month = Convert.ToDateTime(d.datestarted_of_downtime).ToString("MM")
                                         })
                                         .OrderByDescending(d => d.Month)
                                         .ToList();

            ViewBag.DowntimeMonthAndYear = DowntimeDate.GroupBy(d => d.DowntimeYearAndMonth)
                                                     .Select(d => d.FirstOrDefault())
                                                     .ToList();

            ViewBag.AllNetworkDowntime = GetDowntimeByMonth(DateTime.Today);

            return View(network);
        }

        public ActionResult SaveNetworkDowntime(int CauseID, string CodeNumber,
                                       string DateStarted, string TimeStarted,
                                       string DateEnded, string TimeEnded, string CauseDetails)
        {
            string result = "Success";
            try
            {
                var DowntimeDateStarted = Convert.ToDateTime(DateStarted + " " + TimeStarted);
                var DowntimeDateEnded = Convert.ToDateTime(DateEnded + " " + TimeEnded);

                int DowntimeMinutes = Convert.ToInt32(DowntimeDateEnded.Subtract(DowntimeDateStarted).TotalMinutes);


                repository._tbl_downtime.downtime_type_id = 2;
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

        public ActionResult GetNetworkDowntimeDetails(string NetworkName, string DateFilter)
        {
            var result = repository._network_downtime.GetNetworkDowntimeDetails()
                                    .Where(d => Convert.ToDateTime(d.datestarted).ToString("MMM yyyy").Equals(Convert.ToDateTime(DateFilter).ToString("MMM yyyy"))
                                     && d.network_code_number.Equals(NetworkName))
                                    .Select(d => new DowntimeModel
                                    {
                                        Location = d.downtime_location,
                                        DateStarted = d.datestarted.ToString(),
                                        DateEnded = d.dateended.ToString(),
                                        DowntimeMinutes = d.dowtime_mins,
                                        DowntimeCause = d.downtime_cause,
                                        DowntimeCauseDetails = d.downtime_cause_details
                                    }).ToList();

            return Json(result.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchDowntime(DateTime Date)
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
        public ActionResult SearchNetworkName(string Term)
        {
            var data = GetAllNetworkDowntime()
                                 .Where(d => d.DowntimeName.ToLower().Trim().Contains(Term.ToLower().Trim())
                                 && !d.DowntimeName.Equals(""))
                                 .Select(d => new DowntimeModel
                                 {
                                     CodeNumber = d.CodeNumber,
                                     DowntimeName = d.DowntimeName
                                 }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReloadNetworkDowntime(DateTime DateMonth)
        {
            var Result = GetDowntimeByMonth(DateMonth);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        public List<DowntimeModel> GetAllNetworkDowntime()
        {
            return repository._network.GetAllNetwork()
                                      .Select(s => new DowntimeModel
                                      {
                                          ID = s.network_id,
                                          DowntimeName = s.location,
                                          CodeNumber = s.network_code_number,
                                          DateStarted = DateTime.Today.ToShortDateString(),
                                          TimeStarted = DateTime.Now.ToShortTimeString(),
                                          DateEnded = DateTime.Today.ToShortDateString(),
                                          TimeEnded = DateTime.Now.ToShortTimeString(),
                                      }).ToList();
        }

        public List<DowntimeModel> GetDowntimeByMonth(DateTime DateMonth)
        {
            return repository._network_downtime.GetNetworkDowntimeDetails()
                                                    .Where(d => Convert.ToDateTime(d.datestarted).ToString("MMM yyyy").Equals(Convert.ToDateTime(DateMonth).ToString("MMM yyyy")))
                                                    .Select(d => new DowntimeModel
                                                    {
                                                        DateStarted = d.datestarted.ToString(),
                                                        DateEnded = d.dateended.ToString(),
                                                        DowntimeMinutes = d.dowtime_mins,
                                                        DowntimeCause = d.downtime_cause,
                                                        DowntimeCauseDetails = d.downtime_cause_details,
                                                        DowntimeName = d.network_code_number,
                                                        Location = d.downtime_location

                                                    }).ToList();
        }
    }
}