using db.Data.JobOrder;
using db.Database;
using db.Global;
using db.Repository;
using db.Repository.APC;
using db.Repository.Employee;
using db.Repository.JobOrder;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using PagedList;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Web_RIMP.Models;

namespace Web_RIMP.Controllers
{
    public class ReportsController : Controller
    {
        #region Global
        RepositoryInitialization repository = new RepositoryInitialization();

        public ReportsController()
        {
            this.repository._jo = new JobOrder(new JOEntities());
            this.repository._employee = new Employee(new JOEntities());
            this.repository._jo_classification = new JobOrderClassification(new JOEntities());
            this.repository._apc = new Programmer(new JOEntities());
            this.repository._dreport = new DailyReport(new JOEntities());
            this.repository._monthly_report = new JobOrderReport(new JOEntities());
        }

        public ReportsController(IJoborder jo, IEmployee employee, IJobOrderClassification classification, IProgrammer apc)
        {
            this.repository._jo = jo;
            this.repository._employee = employee;
            this.repository._jo_classification = classification;
            this.repository._apc = apc;

        }

        #endregion

        #region Daily Report

        public ActionResult AllDailyReport(int? page)
        {
            var EmpID = Request.Cookies["EmpID"].Value.ToString();

            ViewBag.Apc = repository._apc.GetAPC()
                                    .Where(x => x.emp_id.Trim() != "g930192" && x.emp_id != "J0061")
                                    .OrderBy(x => x.emp_name)
                                    .Select(x => x.emp_name)
                                    .ToList();

            return View(GetReportByName(EmpID).ToPagedList(page ?? 1, 12));
        }

        public List<DailyReportModel> GetReportByName(string ApcID)
        {
            return repository._apc.GetAllReports()
                                  .Where(x => x.apc_id.Trim() == ApcID.Trim())
                                  .Select(x => new DailyReportModel
                                  {
                                      Date = x.date.ToString("yyyy-MM-dd"),
                                      TimeStart = x.time_start,
                                      TimeEnd = x.time_end,
                                      Mins = x.mins,
                                      Activity = x.work_description,
                                      CostCenter = x.cc_description,
                                      Company = x.company,
                                      JobNo = x.jobno
                                  })
                                  .OrderByDescending(x => x.Date).ThenBy(x => x.TimeStart)
                                  .ToList();
        }

        public ActionResult DailyReport()
        {
            if (Request.Cookies["Dept_ID"].Value.ToString() != "09")
                return RedirectToAction("NotAuthorized", "Home");

            var Apc = Request.Cookies["Fullname"].Value.Trim().ToString();
            var EmpID = Request.Cookies["EmpID"].Value.Trim().ToString();

            //var AllOngoingJO = repository._jo.GetAllJobOrder()
            //                                   .Join(repository._jo.GetAllJobOrderDetails(),
            //                                         e => e.details_id, c => c.details_id, (e, c) => new { e, c })
            //                                  .GroupBy(y => new
            //                                  {
            //                                      y.e.details_id,
            //                                      y.e.job_no,
            //                                      y.c.assignee,
            //                                      y.c.status_id
            //                                  })
            //                                  .Where(y => !String.IsNullOrEmpty(y.Key.assignee) && y.Key.assignee.Trim().Equals(Apc) && (y.Key.status_id != 6 && y.Key.status_id != 10))
            //                                  .Select(x => x.Key.job_no)
            //                                   .ToList();

            var ReportToday = repository._apc.GetAllReports()
                                             .Where(x => x.date.ToString("yyyy-MM-dd") == DateTime.Today.ToString("yyyy-MM-dd") &&
                                                         x.apc_id.Trim().Equals(EmpID.Trim()))
                                             .Select(x => new DailyReportModel
                                             {
                                                 ID = x.r_id,
                                                 TimeStart = x.time_start,
                                                 TimeEnd = x.time_end,
                                                 Mins = x.mins,
                                                 Activity = x.work_description,
                                                 CostCenter = x.cc_description,
                                                 Company = x.company
                                             })
                                             .OrderBy(x => x.TimeStart)
                                             .ToList();


            ViewBag.CostCenter = repository._jo.GetCCMeaning().ToList();
            ViewBag.AllOngoingJO = repository._monthly_report
                                                            .GetSharedJO(Apc)
                                                            .Select(x => x.job_no)
                                                            .ToList();

            return View(ReportToday);
        }
        public ActionResult ExportReportToExcel(string EmpID, string DateSent)
        {
            var collection = GetMyJobOrder(EmpID, Convert.ToDateTime(DateSent)).ToList();
            string filename = "APCDailyReport_" + EmpID.Trim() + "_" + DateSent;

            var Apc = repository._apc.GetAPC()
                                .Where(x => x.emp_id.Trim() == EmpID.Trim())
                                .Select(x => x.emp_name).FirstOrDefault();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo(filename + ".xlsx")))
            {
                ExcelWorksheet Sheet = package.Workbook.Worksheets.Add("Report");

                Sheet.Column(3).Width = 16;
                Sheet.Column(4).Width = 25;
                Sheet.Column(5).Width = 25;
                Sheet.Column(6).Width = 18;
                Sheet.Column(7).Width = 18;
                Sheet.Column(8).Width = 25;
                Sheet.Column(9).Width = 17;
                //Sheet.Column(6).Width = 25;
                //Sheet.Column(7).Width = 15;

                Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#3E90FF");
                Color colValue = System.Drawing.ColorTranslator.FromHtml("#BFF0FB");

                //font size
                Sheet.Cells["C2:H2"].Style.Font.Size = 17;
                Sheet.Cells["C4:C4"].Style.Font.Size = 11;
                Sheet.Cells["C8:C8"].Style.Font.Size = 11;
                Sheet.Cells["F4:F4"].Style.Font.Size = 11;
                Sheet.Cells["C6:C6"].Style.Font.Size = 11;
                Sheet.Cells["D8:D8"].Style.Font.Size = 11;
                Sheet.Cells["G8:G8"].Style.Font.Size = 11;

                Sheet.Cells["D2:D2"].Style.Font.Bold = true;
                Sheet.Cells["C4:C4"].Style.Font.Bold = true;
                Sheet.Cells["C6:C6"].Style.Font.Bold = true;
                Sheet.Cells["F4:F4"].Style.Font.Bold = true;
                Sheet.Cells["C6:C6"].Style.Font.Bold = true;
                Sheet.Cells["D6:D6"].Style.Font.Bold = true;
                Sheet.Cells["G6:G6"].Style.Font.Bold = true;
                Sheet.Cells["D14:G14"].Style.Font.Bold = true;
                Sheet.Cells["F6:F6"].Style.Font.Bold = true;
                Sheet.Cells["H6:H6"].Style.Font.Bold = true;
                Sheet.Cells["I6:I6"].Style.Font.Bold = true;
                Sheet.Cells["C2:H2"].Style.Font.Bold = true;

                //bg color
                Sheet.Cells["C4:C4"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                Sheet.Cells["C4:C4"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                Sheet.Cells["C6:I6"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                Sheet.Cells["F4:F4"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                Sheet.Cells["C6:C6"].Style.Fill.PatternType = ExcelFillStyle.Solid;

                Sheet.Cells["C4:C4"].Style.Fill.BackgroundColor.SetColor(colFromHex);
                Sheet.Cells["C6:I6"].Style.Fill.BackgroundColor.SetColor(colFromHex);
                Sheet.Cells["F4:F4"].Style.Fill.BackgroundColor.SetColor(colFromHex);
                Sheet.Cells["C6:C6"].Style.Fill.BackgroundColor.SetColor(colFromHex);
                // Sheet.Cells["G6:G6"].Style.Fill.BackgroundColor.SetColor(colFromHex);

                //Header | APC Name
                Sheet.Cells["C2:H2"].Value = "DAILY STATUS REPORT - APC";
                Sheet.Cells["C4:C4"].Value = "NAME:";
                Sheet.Cells["D4:D4"].Value = Apc.Trim();
                Sheet.Cells["F4:F4"].Value = "DATE:";
                Sheet.Cells["G4:G4"].Value = DateSent;

                //Merge Text
                Sheet.Cells["C2:H2"].Merge = true;
                Sheet.Cells["C2:I2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                Sheet.Cells["C2:I2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                Sheet.Cells[9, 4, 13, 6].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                Sheet.Cells[15, 4, 20, 6].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                Sheet.Cells["G9:G14"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                Sheet.Cells["G9:G14"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                Sheet.Cells["G15:G20"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                Sheet.Cells["G15:G20"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                Sheet.Cells["D2:F2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                Sheet.Cells["D2:F2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //Borders
                Sheet.Cells[string.Format("C4:I4")].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                Sheet.Cells[string.Format("C4:I4")].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                Sheet.Cells[string.Format("C4:C8")].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                Sheet.Cells[string.Format("C4:C8")].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                Sheet.Cells[string.Format("I4:I8")].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                //END

                Sheet.Cells["C6:C6"].Value = "TIME:";
                Sheet.Cells["D6:D6"].Value = "ACTIVITY:";
                Sheet.Cells["F6:F6"].Value = "JO #:";
                Sheet.Cells["G6:G6"].Value = "SBU:";
                Sheet.Cells["H6:H6"].Value = "PROGRESS:";
                Sheet.Cells["I6:I6"].Value = "COMPANY:";

                int row = 7;
                //Add excel cell a value
                foreach (var item in collection)
                {
                    Sheet.Cells[string.Format("D{0}:E{0}", row)].Merge = true;
                 
                    Sheet.Cells[string.Format("C{0}", row)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    Sheet.Cells[string.Format("D{0}:E{0}", row)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    Sheet.Cells[string.Format("F{0}", row)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    Sheet.Cells[string.Format("G{0}", row)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    Sheet.Cells[string.Format("H{0}", row)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    Sheet.Cells[string.Format("I{0}", row)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    Sheet.Cells[string.Format("C{0}", row)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    Sheet.Cells[string.Format("D{0}:E{0}", row)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    Sheet.Cells[string.Format("F{0}", row)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    Sheet.Cells[string.Format("G{0}", row)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    Sheet.Cells[string.Format("H{0}", row)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    Sheet.Cells[string.Format("I{0}", row)].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    Sheet.Cells[string.Format("C{0}", row)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    Sheet.Cells[string.Format("D{0}:E{0}", row)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    Sheet.Cells[string.Format("F{0}", row)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    Sheet.Cells[string.Format("G{0}", row)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    Sheet.Cells[string.Format("H{0}", row)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    Sheet.Cells[string.Format("I{0}", row)].Style.Border.Left.Style = ExcelBorderStyle.Thin;

                    Sheet.Cells[string.Format("C{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    Sheet.Cells[string.Format("G{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    Sheet.Cells[string.Format("I{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    Sheet.Cells[string.Format("C{0}", row)].Value = item.time_start + " - " + item.time_end;
                    Sheet.Cells[string.Format("D{0}", row)].Value = item.work_description;
                    Sheet.Cells[string.Format("F{0}", row)].Value = item.jobno;
                    Sheet.Cells[string.Format("G{0}", row)].Value = item.cc_description;

                    if (item.jobno != null && !String.IsNullOrEmpty(item.jobno))
                    {
                        Sheet.Cells[string.Format("H{0}", row)].Value = (item.progress == null || item.progress == 0) ? "0% Completed" : item.progress + "% Completed";

                        Sheet.Cells[string.Format("C{0}:I{0}", row)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        Sheet.Cells[string.Format("C{0}:I{0}", row)].Style.Fill.BackgroundColor.SetColor(colValue);

                    }
                    else
                        Sheet.Cells[string.Format("H{0}", row)].Value = "";

                    Sheet.Cells[string.Format("I{0}", row)].Value = item.company;

                    Sheet.Cells[string.Format("D{0}:E{0}", row)].Style.WrapText = true;
                    Sheet.Row(row).Height = 50;

                    row++;
                }
                //  Sheet.Cells["A:AZ"].AutoFitColumns();
                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + filename);
                Response.BinaryWrite(package.GetAsByteArray());

                Response.End();
            }
            return View();
        }
        public List<sp_export_daily_report_Result> GetMyJobOrder(string APC, DateTime Date)
        {
            var ReportToday = repository._dreport.GetAllDailyReport(APC, Date)
                                                 .ToList();
            return ReportToday;
        }
        public ActionResult GetJODetails(string JobNo)
        {
            var JobOrder = repository._jo_classification.GetListOfJobOrder()
                           .Where(x => x.jonumber.Equals(JobNo))
                           .Select(x => new JobOrderDetailsModel
                           {
                               JoID = x.id,
                               JODetailsID = x.details_id,
                               JobNumber = x.jonumber,
                               WorkDetails = x.details,
                               DateTarget = Convert.ToDateTime(x.date_target).ToString("yyyy-MM-dd"),
                               JO_Costcenter = x.req_costcenter,
                               Requestor = x.requestor,
                               ProgressRate = x.jo_progress
                           }).FirstOrDefault();

            return Json(JobOrder, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddReport(string Empid, DateTime Date, string Day, string TimeStart, string TimeEnd, int Mins, string Activity, string JobNo, string CostCenter, string Company)
        {
            var Result = "";
            try
            {
                //Link to JO allocated time
                if (!String.IsNullOrEmpty(JobNo))
                {
                    var DetailsID = repository._jo.GetAllJobOrder().Where(x => x.job_no == JobNo).Select(x => x.details_id).FirstOrDefault();

                    repository._tbl_allocated_time.details_id = DetailsID;
                    repository._tbl_allocated_time.date = Date;
                    repository._tbl_allocated_time.day = Day;
                    repository._tbl_allocated_time.mins = Mins;
                    repository._tbl_allocated_time.work_description = Activity;

                    repository._jo.InsertAllocatedTime(repository._tbl_allocated_time);
                    repository._jo.Save();

                    var JODetails = repository._jo.GetAllJobOrderDetails()
                                               .Where(x => x.details_id == DetailsID)
                                               .FirstOrDefault();

                    if (JODetails.date_started == null)
                        JODetails.date_started = Date;

                    if (JODetails.actual_time == null)
                        JODetails.actual_time = 0;


                    var ActualTime = Mins + JODetails.actual_time;

                    JODetails.actual_time = ActualTime;

                    repository._jo.UpdateJobOrderDetails(JODetails);
                    repository._jo.Save();

                }

                //Save to daily report table
                repository._tbl_daily_report.apc_id = Empid;
                repository._tbl_daily_report.date = Date;
                repository._tbl_daily_report.day = Day;
                repository._tbl_daily_report.time_start = TimeStart;
                repository._tbl_daily_report.time_end = TimeEnd;
                repository._tbl_daily_report.work_description = Activity;
                repository._tbl_daily_report.mins = Mins;
                repository._tbl_daily_report.cc_description = CostCenter;
                repository._tbl_daily_report.company = Company;
                repository._tbl_daily_report.jobno = JobNo;

                repository._apc.InsertReport(repository._tbl_daily_report);
                repository._apc.Save();

            }
            catch (Exception ex)
            {
                Result = ex.Message.ToString();
            }

            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SendEmailReport(string EmpID)
        {
            string Result = "";
            string DateSend = DateTime.Today.ToString("yyyy-MM-dd");
            try
            {
                //Get sender email
                var Email = repository._employee.GetRegisteredEmployees()
                                                .Where(x => x.emp_id.Trim() == EmpID.Trim())
                                               .Select(x => x.email_address)
                                                .FirstOrDefault();

                var ApcName = repository._apc.GetAPC()
                                             .Where(x => x.emp_id.Trim() == EmpID)
                                             .Select(x => x.emp_name)
                                             .FirstOrDefault();

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("automailer.rimp@ph.ricoh-imaging.com");

             //    mailMessage.To.Add("grace.booc@ph.ricoh-imaging.com");
                mailMessage.To.Add("alfred.balundo@ph.ricoh-imaging.com"); //,masami.inoue@ricoh-imaging.co.jp"
                mailMessage.CC.Add(Email);

                mailMessage.Subject = "ApcDailyReport_" + ApcName.ToUpper().Trim() + "_" + DateSend;


                string headerMail = @"Hi Sir, <br>" +
                                    "Good day! <br><br>" +
                                    "Link below is my report for today. <br>" +
                                  //  "<a href='http://localhost:51172/Reports/ExportReportToExcel?EmpID=" + EmpID + "&DateSent=" + DateSend + "'>APCDailyReport_" + ApcName + "_" + DateSend + "</a>" +
                                   "<a href='http://kpsweb2:1001/Reports/ExportReportToExcel?EmpID=" + EmpID + "&DateSent=" + DateSend + "'>APCDailyReport_" + ApcName + "_" + DateSend + "</a>" +
                                    "<br><br>Thank You & Regards,<br>" + ApcName;



                mailMessage.Body = headerMail;
                mailMessage.IsBodyHtml = true;
                SmtpClient SmtpServer = new SmtpClient();
                SmtpServer.Port = 25;
                SmtpServer.Host = "mail.ricoh.co.jp";
                SmtpServer.Send(mailMessage);
                mailMessage.Dispose();
            }
            catch (Exception ex)
            {
                Result = ex.ToString();
            }
            return Json(Result,JsonRequestBehavior.AllowGet);
        }
        public ActionResult ResendEmail(string EmpID, string DateSend)
        {
            string Result = "";
            try
            {
                //Get sender email
                var Email = repository._employee.GetRegisteredEmployees()
                                                .Where(x => x.emp_id.Trim() == EmpID.Trim())
                                                .Select(x => x.email_address)
                                                .FirstOrDefault();

                var ApcName = repository._apc.GetAPC()
                                             .Where(x => x.emp_id.Trim() == EmpID)
                                             .Select(x => x.emp_name)
                                             .FirstOrDefault();

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("automailer.rimp@ph.ricoh-imaging.com");

                mailMessage.To.Add("grace.booc@ph.ricoh-imaging.com");
             //   mailMessage.To.Add("alfred.balundo@ph.ricoh-imaging.com"); //,masami.inoue@ricoh-imaging.co.jp"
                mailMessage.CC.Add(Email);

                mailMessage.Subject = "ApcDailyReport_" + ApcName.ToUpper().Trim() + "_" + DateSend;


                string headerMail = @"Hi Sir, <br>" +
                                    "Good day! <br><br>" +
                                    "Link below is my report for today. <br>" +
                                    //"<a href='http://localhost:51172/Reports/ExportReportToExcel?EmpID=" + EmpID + "&DateSent=" + DateSend + "'>APCDAILYREPORT_" + ApcName + "_" + DateSend + "</a>" +
                                    "<a href='http://kpsweb2:1001/Reports/ExportReportToExcel?EmpID=" + EmpID + "&DateSent=" + DateSend + "'>APCDailyReport_" + ApcName + "_" + DateSend + "</a>" +
                                    "<br><br>Thank You & Regards,<br>" + ApcName;



                mailMessage.Body = headerMail;
                mailMessage.IsBodyHtml = true;
                SmtpClient SmtpServer = new SmtpClient();
                SmtpServer.Port = 25;
                SmtpServer.Host = "mail.ricoh.co.jp";
                SmtpServer.Send(mailMessage);
                mailMessage.Dispose();
            }
            catch (Exception ex)
            {
                Result = ex.ToString();
            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateDailyReport(int ID, string TimeStart, string TimeEnd, string Mins, string Activity, string Costcenter, string Company)
        {

            var Report = repository._apc.GetAllReports()
                                   .Where(x => x.r_id == ID)
                                   .FirstOrDefault();

            Report.time_start = TimeStart;
            Report.time_end = TimeEnd;
            Report.mins = Convert.ToInt32(Mins);
            Report.work_description = Activity;
            Report.cc_description = Costcenter;
            Report.company = Company;

            repository._apc.UpdateReport(Report);
            repository._apc.Save();

            return Json(JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region JO Monthly report

        public ActionResult JOMonthlyReport()
        {

            return View();

        }


        #endregion
    }
}