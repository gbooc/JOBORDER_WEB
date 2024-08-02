using DataMatrix.net;
using db.Data.Dashboard;
using db.Data.JobOrder;
using db.Database;
using db.Global;
using db.Repository.APC;
using db.Repository.Applications;
using db.Repository.Employee;
using db.Repository.GanttChart;
using db.Repository.JobOrder;
using db.Repository.Priority;
using db.Repository.Status;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Web_RIMP.Models;

namespace Web_RIMP.Controllers
{
    [Authorize]
    public class JobOrdersController : Controller
    {
        #region Global Initialization

        RepositoryInitialization repository = new RepositoryInitialization();


        public JobOrdersController()
        {
            this.repository._jo_classification = new JobOrderClassification(new JOEntities());
            this.repository._employee = new Employee(new JOEntities());
            this.repository._jo_priority = new Priority(new JOEntities());
            this.repository._jo = new JobOrder(new JOEntities());
            this.repository._jostatus = new Status(new JOEntities());
            this.repository._apc = new Programmer(new JOEntities());
            this.repository._app = new Applications(new JOEntities());
            this.repository._gant_chart = new GanttChart(new JOEntities());
            this.repository._monthly_report = new JobOrderReport(new JOEntities());
            this.repository._dashboard = new Dashboard(new JOEntities());
        }

        public JobOrdersController(JobOrderClassification jo_classification, Employee emp,
                                   Priority priority, JobOrder jo, Status status, Programmer apc,
                                   Applications app, GanttChart gant_chart, JobOrderReport report,
                                   Dashboard dashboard)
        {
            this.repository._jo_classification = jo_classification;
            this.repository._employee = emp;
            this.repository._jo_priority = priority;
            this.repository._jo = jo;
            this.repository._jostatus = status;
            this.repository._apc = apc;
            this.repository._app = app;
            this.repository._gant_chart = gant_chart;
            this.repository._monthly_report = report;
            this.repository._dashboard = dashboard;
        }
        #endregion

        #region Non KPI JO

        #region All Job Orders Tab
        public ActionResult AllJobOrders()
        {
            return View();
        }

        public ActionResult ExportToExcelJO(string DateFrom, string DateTo)
        {
            string filename = "Joborder-Report-" + DateTime.Today.ToShortDateString() + ".xlsx";

            var collection = repository._jo.ExportJOToExcel(
                                        Convert.ToDateTime(Convert.ToDateTime(DateFrom).ToString("yyyy-MM-dd")),
                                        Convert.ToDateTime(Convert.ToDateTime(DateTo).ToString("yyyy-MM-dd")),
                                        1)
                                        .ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo("MyWorkbook.xlsx")))
            {
                ExcelWorksheet Sheet1 = package.Workbook.Worksheets.Add("JO Summary");
                ExcelWorksheet Sheet2 = package.Workbook.Worksheets.Add("JO Details");

                Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#4d5076");
                Color colValue = System.Drawing.ColorTranslator.FromHtml("#759dd2");

                //Header Title
                Sheet2.Cells["A1:I1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                Sheet2.Cells["A1:I1"].Style.Fill.BackgroundColor.SetColor(colFromHex);
                Sheet2.Cells["A1:I1"].Style.Font.Color.SetColor(Color.White);
                Sheet2.Cells["A1:I1"].Style.Font.Size = 20;
                Sheet2.Cells["D1:D1"].Value = "APC Job Orders Monthly Report " + Convert.ToDateTime(DateFrom).ToString("yyyy-MM");
                //END

                //Header Name Style / Color
                Sheet2.Cells["A2:I2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                Sheet2.Cells["A2:I2"].Style.Fill.BackgroundColor.SetColor(colValue);
                Sheet2.Cells["A2:I2"].Style.Font.Color.SetColor(Color.White);
                Sheet2.Cells["A2:I2"].Style.Font.Size = 15;

                Sheet2.Cells["A2"].Value = "Requesting Department";
                Sheet2.Cells["B2"].Value = "Cost Center";
                Sheet2.Cells["C2"].Value = "Job Order No.";
                Sheet2.Cells["D2"].Value = "Details";
                Sheet2.Cells["E2"].Value = "APC In-Charge";
                Sheet2.Cells["F2"].Value = "Job Order Status";
                Sheet2.Cells["G2"].Value = "Date Started";
                Sheet2.Cells["H2"].Value = "Date Ended";
                Sheet2.Cells["I2"].Value = "Minutes";
                //END

                //Add excel cell a value
                int row = 3;

                foreach (var item in collection)
                {
                    Sheet2.Cells[string.Format("A{0}", row)].Value = item.department;
                    Sheet2.Cells[string.Format("B{0}", row)].Value = item.costcenter;
                    Sheet2.Cells[string.Format("C{0}", row)].Value = item.jo_no;
                    Sheet2.Cells[string.Format("D{0}", row)].Value = item.details;
                    Sheet2.Cells[string.Format("E{0}", row)].Value = item.apc;
                    Sheet2.Cells[string.Format("F{0}", row)].Value = item.jo_status;
                    Sheet2.Cells[string.Format("G{0}", row)].Value = item.date_started;
                    Sheet2.Cells[string.Format("H{0}", row)].Value = item.date_ended;
                    Sheet2.Cells[string.Format("I{0}", row)].Value = item.hrs_mins;

                    row++;
                }

                var Graph = repository._monthly_report.GetCostCenterGraph(Convert.ToDateTime(DateFrom), Convert.ToDateTime(DateTo)).ToList();

                int? Totalminutes = 0;
                DataTable tbl = new DataTable();

                tbl.Columns.Add("Cost Center", typeof(string));
                tbl.Columns.Add("Minutes", typeof(int));
                tbl.Columns.Add("Percentage", typeof(double));

                foreach (var g in Graph)
                {

                    var total = g.total;
                    Totalminutes = g.overalltotal;

                    var tmppercentage = (Convert.ToDouble(total) / Totalminutes) * 100;
                    double percentage = Math.Round(Convert.ToDouble(tmppercentage), 1);

                    tbl.Rows.Add(g.cc_description, g.total, percentage);
                }

                Sheet1.Cells["B15"].Style.Font.Size = 15;
                Sheet1.Cells["B15"].Value = "Total Minutes: " + Totalminutes;

                //Datable
                Sheet1.Cells[1, 1].LoadFromDataTable(tbl, true, OfficeOpenXml.Table.TableStyles.Medium6);

                //Tota Cost Center BAR Chart
                ExcelChart bargraph = Sheet1.Drawings.AddChart("chart", eChartType.ColumnClustered);
                bargraph.Title.Text = "JO Cost Center Chart";
                bargraph.XAxis.Title.Text = "Cost Center"; //give label to x-axis of chart  
                bargraph.XAxis.Title.Font.Size = 10;

                bargraph.SetPosition(3, 3, 3, 4);
                bargraph.SetSize(800, 400);

                //Percentage pie graph
                ExcelChart piegraph = Sheet1.Drawings.AddChart("piechart", eChartType.Pie);
                piegraph.Title.Text = "Cost Center Percenatage";

                piegraph.SetPosition(24, 3, 3, 25);
                piegraph.SetSize(800, 400);

                var ser1 = bargraph.Series.Add(Sheet1.Cells["B2:B14"], Sheet1.Cells["A2:A14"]);
                var ser2 = piegraph.Series.Add(Sheet1.Cells["C2:C14"], Sheet1.Cells["A2:A14"]);

                ser1.Header = "Cost Center";
                ser2.Header = "Percentage";

                Sheet1.Cells["A:AZ"].AutoFitColumns();
                Sheet2.Cells["A:AZ"].AutoFitColumns();

                Sheet1.Cells[Sheet1.Dimension.Address].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                Sheet1.Cells[Sheet1.Dimension.Address].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                Sheet2.Cells[Sheet2.Dimension.Address].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                Sheet2.Cells[Sheet2.Dimension.Address].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + filename);
                Response.BinaryWrite(package.GetAsByteArray());
                Response.End();
                //-- END --
            }
            return View();

        }

        #endregion

        #region New Job Order Tab    
        public ActionResult CreateJobOrder()
        {
            List<CreateJobOrderModel> NewJobOrder = new List<CreateJobOrderModel>();

            try
            {
                //Job Order cards values
                var JOCounts = repository._jo_classification.SPOnGoingJO().FirstOrDefault();

                //All unassigned JO
                NewJobOrder = repository._jo_classification.SPJoUnassignedList()
                                                            .Where(x => x.status == "Created")
                                                           .Select(x => new CreateJobOrderModel()
                                                           {
                                                               JobOrder = x.job_no,
                                                               Requestor = x.requested_by,
                                                               DateCreated = x.date_filed,
                                                               Status = x.status
                                                           })
                                                           .ToList();

                //To get sbu / costcenter
                var EmpFunction = repository._employee.GetRegisteredEmployees()
                                                      .Join(repository._jo.GetAllCCCategory(),
                                                            e => e.cc_category_id, c => c.cc_category_id, (e, c) => new { e, c })
                                                      .GroupBy(y => new
                                                      {
                                                          y.c.cc_category_id,
                                                          y.c.description,
                                                          y.c.selection,
                                                          y.e.emp_id
                                                      })
                                                      .Where(y => y.Key.emp_id.Trim() == Request.Cookies["EmpID"].Value.ToString().Trim())
                                                      .FirstOrDefault();

                if (EmpFunction.Key.cc_category_id == 1)
                    //camera only
                    ViewBag.SBU = repository._jo.GetAllSBU().Where(x => x.sbu_id == 1).ToList();
                else // Manufacturing
                    ViewBag.SBU = repository._jo.GetAllSBU()
                                                    .ToList();

                ViewBag.CCCategory = EmpFunction.Key.selection;
                ViewBag.Sections = repository._jo.GetAllCostCenter().Where(x => x.dept_id.Trim() == Request.Cookies["Dept_ID"].Value.ToString()).ToList();

                //end of sbu

                //pending joborder counts
                ViewBag.UnassignedCount = JOCounts.unassignedjo;
                ViewBag.AssignedCount = JOCounts.assignedjo;
                ViewBag.HardwareJoCount = JOCounts.hwjo;
                ViewBag.SoftwareJoCount = JOCounts.swjo;

                //Approval Managers
                ViewBag.Managers = repository._employee.GetEmployeeManagers()
                                          .Where(x => x.dept_id.Trim().Equals(Request.Cookies["Dept_ID"].Value.ToString().Trim()))
                                          .Select(x => new CreateJobOrderModel
                                          {
                                              ManagerID = x.emp_id,
                                              Manager = x.emp_name
                                          }).ToList();

                //Joob Order Disable/Enable
                var JOAvailability = repository._jo.JOAvailability().FirstOrDefault();
                ViewBag.JOAvailability = JOAvailability.can_create;
                ViewBag.JOAvailabilityMessage = JOAvailability.message;
                ViewBag.JOTempAccess = JOAvailability.temp_access.Trim();

            }
            catch
            {

            }
            return View(NewJobOrder);
        }

        public ActionResult SoftwaresByDept(string Department)
        {
            var Softwares = repository._jo_classification.DepartmentsSoftware(Department)
                                      .Select(x => new CreateJobOrderModel
                                      {
                                          Sw_Application = x.application_name,
                                          Sw_CodeNumber = x.code_number,
                                          Sw_Programmer = x.emp_name,
                                          Sw_UserDepartment = x.user_department
                                      })
                                      .ToList();

            return Json(Softwares, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SearchSystem(string System)
        {

            var DeptRequestor = repository._app.GetAllApplications()
                                               .Where(x => x.application_name.Trim() == System.Trim())
                                               .Select(x => x.user_department)
                                               .FirstOrDefault();

            var Softwares = repository._jo_classification.DepartmentsSoftware(DeptRequestor)
                                      .Select(x => new CreateJobOrderModel
                                      {
                                          Sw_Application = x.application_name,
                                          Sw_CodeNumber = x.code_number,
                                          Sw_Programmer = x.emp_name,
                                          Sw_UserDepartment = x.user_department
                                      })
                                      .Where(x => x.Sw_Application.Trim() == System.Trim())
                                      .FirstOrDefault();

            return Json(Softwares, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSBUDivision(int SBUId)
        {
            var Division = repository._jo.SP_GetCostCenter(SBUId).FirstOrDefault();
            return Json(Division, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveJobOrder(string CostCenter, string WorkDetails, string Purpose, string RequestedBy, string NotedBy, string ApprovedBy)
        {
            DateTime today = DateTime.Today;

            //Job Order Details
            repository._tbl_joborder_details.type_id = 0;
            repository._tbl_joborder_details.status_id = 1;
            repository._tbl_joborder_details.sw_codenumber = "";
            repository._tbl_joborder_details.details = WorkDetails;
            repository._tbl_joborder_details.purpose = Purpose;
            repository._tbl_joborder.date_filed = DateTime.Now;
            repository._jo.InsertJobOrderDetails(repository._tbl_joborder_details);
            repository._jo.Save();
            //END


            //Get Job Order details ID
            var detailsid = repository._jo.GetAllJobOrderDetails().Max(d => d.details_id);


            //JOb Order
            repository._tbl_joborder.job_no = "APCJO" + today.ToString("yyyymmdd") + detailsid;
            repository._tbl_joborder.details_id = Convert.ToInt32(detailsid);
            repository._tbl_joborder.priority_id = 3;
            repository._tbl_joborder.date_needed = today;
            repository._tbl_joborder.requested_by = RequestedBy;
            repository._tbl_joborder.noted_by = NotedBy;
            repository._tbl_joborder.approved_by = ApprovedBy;

            repository._jo.InsertJobOrder(repository._tbl_joborder);
            repository._jo.Save();
            //End

            //Cost Center
            var createdjoborder = repository._jo.GetAllJobOrder()
                                                 .Where(x => x.details_id == Convert.ToInt32(detailsid))
                                                 .FirstOrDefault();
            string JobNo = createdjoborder.job_no;

            string[] SplitCC = CostCenter.Split('-');

            List<string> NamesList = new List<string>(CostCenter.Length);
            NamesList.AddRange(SplitCC);

            repository._tbl_jo_cc_details.jobno_id = createdjoborder.jobno_id;
            repository._tbl_jo_cc_details.category = NamesList[0];
            repository._tbl_jo_cc_details.sbu = NamesList[1];
            repository._tbl_jo_cc_details.division = NamesList[2];
            repository._tbl_jo_cc_details.cost_center = NamesList[3];
            repository._tbl_jo_cc_details.description = NamesList[1] + "-" + NamesList[2] + "-" + NamesList[3];

            repository._jo.InsertDivisionDetails(repository._tbl_jo_cc_details);
            repository._jo.Save();

            //Generate data matrix
            DmtxImageEncoder encoder = new DmtxImageEncoder();
            DmtxImageEncoderOptions options = new DmtxImageEncoderOptions();
            options.ModuleSize = 10;
            options.MarginSize = 10;
            options.BackColor = Color.White;
            options.ForeColor = Color.Black;
            options.Scheme = DmtxScheme.DmtxSchemeAsciiGS1;
            Bitmap encodedBitmap = encoder.EncodeImage(JobNo, options);
            encodedBitmap.RotateFlip(RotateFlipType.Rotate180FlipXY);

            System.IO.MemoryStream ms = new MemoryStream();

            string folderPath = Server.MapPath("~/Barcodes/");
            repository._tbl_joborder.path_barcode = folderPath + JobNo + ".png";

            encodedBitmap.Save(repository._tbl_joborder.path_barcode, System.Drawing.Imaging.ImageFormat.Png);

            //Job Order Disable/Enable
            var JOAvailability = repository._jo.JOAvailability().FirstOrDefault();

            if (JOAvailability.can_create == "No")
            {
                JOAvailability.temp_access = "";
                repository._jo.UpdateJOAvailability(JOAvailability);
                repository._jo.Save();
            }

            return Json(JobNo, JsonRequestBehavior.AllowGet);
        }


        public ActionResult DeleteJO(string JobNo)
        {
            string FolderPath = Server.MapPath("~/Content/img/Barcodes/");

            string FileName = FolderPath + JobNo + ".png";

            if (System.IO.File.Exists(FileName))
                System.IO.File.Delete(FileName);

            return View();
        }


        public void EmailForApproval(string EmailType, string ApprovalName, string JONumber, string ApprovalType, string RequestedBy, string Details, DateTime? DateFiled)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("automailer.rimp@ph.ricoh-imaging.com");

            string headerMail = "";
            string Reciepients = "";

            switch (EmailType)
            {
                case "RequestorApproval":


                    mailMessage.To.Add("grace.booc@ph.ricoh-imaging.com");  // Reciepients
                                                                            //  mailMessage.CC.Add("grace.booc@ph.ricoh-imaging.com"); // Managers
                                                                            //   mailMessage.CC.Add("alfred.balundo@ph.ricoh-imaging.com,grace.booc@ph.ricoh-imaging.com," + CC); // Managers

                    mailMessage.Subject = "Job Order Request Approval";

                    headerMail = @"Hi " + ApprovalName.Trim() + ",<br><br>" +
                                        "Good day there.<br><br>" +
                                        "New Job Order was created by " + RequestedBy.Trim() + " last " + Convert.ToDateTime(DateFiled).ToString("yyyy-MM-dd dddd") + ".<br>" +
                                        "JO system already digitalized the creation and approval of JO, thus for this to be forwarded to APC<br>" +
                                        "it needs your approval by clicking the <u>Approved</u> link or to disapprove/hold the JO, click the <u>Deferred</u> link below.<br><br>" +
                                        "Job Order Details: <br><br>" +
                                         Details +
                                        "<br><br>" +
                                        "<a href = 'http://localhost:51172/JobOrders/JOApproval?IsApprove=1&JONumber=" + JONumber + "&ApprovalType=" + ApprovalType + "&Source=From Email'>Approved JO</a>  |  " +
                                        "<a href = '#Approved'>Deferred JO</a><br><br>" +
                                        "Click this <a href='http://http://localhost:51172/JobOrders/JobOrderDetails?JoNumber=" + JONumber + "'>Link</a> to view the details.";

                    break;
                case "ApcApproval":


                    mailMessage.To.Add(Reciepients);  // Reciepients
                                                      //  mailMessage.CC.Add("grace.booc@ph.ricoh-imaging.com"); // Managers
                                                      //   mailMessage.CC.Add("alfred.balundo@ph.ricoh-imaging.com,grace.booc@ph.ricoh-imaging.com," + CC); // Managers

                    mailMessage.Subject = "Job Order Request Approval";

                    headerMail = @"Hi,<br><br>" +
                                        "Good day there.<br><br>" +
                                        "New approved job order was created by [requestor] last [date], <br>" +
                                        "signed and noted by [NAME] and approved by [NAME].<br>" +
                                        "See below for this JO details.<br><br>" +
                                        "[insert details]";
                    break;
            }

            mailMessage.Body = headerMail;
            mailMessage.IsBodyHtml = true;
            SmtpClient SmtpServer = new SmtpClient();
            SmtpServer.Port = 25;
            SmtpServer.Host = "mail.ricoh.co.jp";
            SmtpServer.Send(mailMessage);
            mailMessage.Dispose();
        }
        public ActionResult SearchSoftware(string Term)
        {
            var data = repository._app.GetAllApplications()
                                 .Where(d => d.application_name.ToLower()
                                                               .StartsWith(Term.ToLower()) &&
                                             d.is_active == "Active"
                                 ).GroupBy(x => new
                                 {
                                     x.application_name,
                                     x.code_number
                                 })
                                 .Select(d => d.FirstOrDefault());

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AllHardwareCategory()
        {
            var data = repository._jo.GetAllCategoriesType()
                                 .Where(d => d.is_active == "Active")
                                 .ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Job Order Details

        [AllowAnonymous]
        [HttpGet]
        public ActionResult JobOrderDetails(string JoNumber)
        {
            var JODetails = repository._jo_classification.SPJODetails(JoNumber)
                                                         .Select(x => new JobOrderDetailsModel
                                                         {
                                                             JoID = x.jobno_id,
                                                             JODetailsID = x.details_id,
                                                             JobNumber = x.job_no,
                                                             Requestor = x.requested_by,
                                                             Department = x.dept_name,
                                                             JO_Costcenter = x.cc_description,
                                                             JoType = x.details,
                                                             StandardTime = 0,
                                                             ActualTime = x.actual_time,
                                                             Assignee = x.assignee,
                                                             DateAssigned = Convert.ToDateTime(x.date_assigned).ToString("yyyy-MM-dd"),
                                                             APCLocalNumber = x.tel_num,
                                                             DateTarget = Convert.ToDateTime(x.date_target).ToString("yyyy-MM-dd"),
                                                             JOHandled = x.johandled,
                                                             DateEnded = x.date_ended.ToString("yyyy-MM-dd") == DateTime.Today.ToString("yyyy-MM-dd") ? "" : x.date_ended.ToString("yyyy-MM-dd"),
                                                             ProgressRate = x.progress_rate,
                                                             Diagnosis = x.diagnosis,
                                                             ActionTaken = x.action_taken,
                                                             ReasonOfDelay = x.reason_delay,
                                                             Datefiled = x.date_filed.ToString(),
                                                             RequestorLocal = x.req_contact,
                                                             WorkDetails = x.request_details,
                                                             WorkPurpose = x.request_purpose,
                                                             NotedBy = x.noted_by,
                                                             ApprovedBy = x.approved_by,
                                                             DeptID = x.dept_id,
                                                             TypeID = (int)x.type_id,
                                                             Status = x.status.Trim(),
                                                             DateStarted = Convert.ToDateTime(x.date_started).ToString("yyyy-MM-dd"),
                                                             NumOfDays = (int)x.development_days,
                                                             RequirementsMeetings = x.documents_remarks,
                                                             ProgrammerPic = string.IsNullOrEmpty(x.apc_id) || x.apc_id == "0" ? "~/Content/img/idcard1.png" : "http://kpsmsvm:90/Content/images/" + x.apc_id.Insert(3, "-").TrimStart('g').Trim() + ".bmp"
                                                         })
                                                         .FirstOrDefault();
            ViewBag.Managers = repository._employee.GetEmployeeManagers()
                                         .Where(x => x.dept_id.Trim().Equals(JODetails.DeptID.Trim()))
                                         .Select(x => new CreateJobOrderModel
                                         {
                                             ManagerID = x.emp_id,
                                             Manager = x.emp_name
                                         }).ToList();

            ViewBag.AllocatedTime = repository._jo_classification
                                              .SPTimeAllocation((int)JODetails.JODetailsID)
                                              .Select(x => new TimeAllocation
                                              {
                                                  DetailsID = (int)x.details_id,
                                                  Date = Convert.ToDateTime(x.date).ToString("yyyy-MM-dd"),
                                                  Day = x.day,
                                                  TimeRendered = x.mins.ToString(),
                                                  Remarks = x.work_description
                                              })
                                              .ToList();

            ViewBag.JOIssues = repository._jo.SP_sw_jo_issues((int)JODetails.JODetailsID).ToList();

            return View(JODetails);
        }


        public ActionResult UpdateJobOrderDetails(int JobNoID, int DetailsID, string Details, string Purpose, string NotedBy,
                                                  string ApprovedBy, int? StatusID, int? Progress, string Diagnosis, string ActionTaken,
                                                  string ReasonOfDelay, int ActualTime, string DocumentMeetings)
        {

            var Joborder = repository._jo.GetJobOrderById(JobNoID);
            var JobOrderDetails = repository._jo.GetJobOrderDetailsById(DetailsID);

            //Joborder
            Joborder.approved_by = ApprovedBy;
            Joborder.noted_by = NotedBy;

            repository._jo.UpdateJobOrder(Joborder);
            repository._jo.Save();

            //end

            //joborder details
            JobOrderDetails.status_id = StatusID;
            JobOrderDetails.progress_rate = Progress;
            JobOrderDetails.details = Details;
            JobOrderDetails.purpose = Purpose;
            JobOrderDetails.diagnosis = Diagnosis;
            JobOrderDetails.action_taken = ActionTaken;
            JobOrderDetails.reason_delay = ReasonOfDelay;
            JobOrderDetails.actual_time = ActualTime;
            JobOrderDetails.documents_remarks = DocumentMeetings;

            if (StatusID == 12)
                JobOrderDetails.date_ended = DateTime.Now;

            repository._jo.UpdateJobOrderDetails(JobOrderDetails);
            repository._jo.Save();
            //END

            UpdateStatusTimeLine(DetailsID, (int)StatusID);
            return Json(JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateAllocationTime(string FieldName, string FieldValue, int ID)
        {
            var TimeAllocation = repository._jo.GetAllocatedTimeById(ID);
            var HasError = "";

            try
            {
                switch (FieldName)
                {
                    case "Remarks": TimeAllocation.work_description = FieldValue; break;
                    case "Time": TimeAllocation.mins = Convert.ToInt32(FieldValue) * 60; break;
                }

                repository._jo.UpdateAllocatedTimeBy(TimeAllocation);
                repository._jo.Save();
            }
            catch (Exception ex)
            {
                HasError = ex.ToString();
            }

            return Json(HasError, JsonRequestBehavior.AllowGet);
        }

        public void UploadIcon(string strBase64, string filename)
        {
            string folderPath = Server.MapPath("~/Content/SystemIcon/");

            string imagePath = folderPath + filename + ".png";
            byte[] data = System.Convert.FromBase64String(strBase64);

            MemoryStream ms = new MemoryStream(data);
            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
            img.Save(imagePath, System.Drawing.Imaging.ImageFormat.Jpeg);

            var Path = repository._app.GetAllApplications().Where(a => a.code_number == filename).FirstOrDefault();

            Path.icon_path = filename;
            repository._app.UpdateApplication(Path);
            repository._app.Save();

        }

        public ActionResult AddTimeAllocation(int DetailsID, DateTime DateRendered, int MinRendered, string Remarks)
        {
            string DayOfDate = DateRendered.ToString("dddd");

            repository._tbl_allocated_time.details_id = DetailsID;
            repository._tbl_allocated_time.date = DateRendered;
            repository._tbl_allocated_time.mins = MinRendered;
            repository._tbl_allocated_time.day = DayOfDate;
            repository._tbl_allocated_time.work_description = Remarks;


            repository._jo.InsertAllocatedTime(repository._tbl_allocated_time);
            repository._jo.Save();

            var TimeAllocation = repository._tbl_allocated_time;

            return Json(JsonRequestBehavior.AllowGet);
        }

        public ActionResult DisplayAllAssignee(int DetailsID)
        {
            var APC = repository._jo.SP_GetAllAssignee(DetailsID).ToList();

            return Json(APC, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ManageAssignee(int DetailsID, string Assignee)
        {
            string Result = "";

            try
            {
                var JoAssignees = repository._jo.GetAllAssigneess()
                                                .Where(x => x.details_id == DetailsID && x.emp_name == Assignee)
                                                .FirstOrDefault();


                if (JoAssignees == null)
                {

                    repository._tbl_sub_assignees.details_id = DetailsID;
                    repository._tbl_sub_assignees.emp_name = Assignee;

                    repository._jo.InsertAssignees(repository._tbl_sub_assignees);
                    repository._jo.Save();
                }
                else
                {
                    var RemoveAssignee = repository._jo.GetAssignessByID(JoAssignees.a_id);

                    repository._jo.DeleteAssignees(RemoveAssignee.a_id);
                    repository._jo.Save();
                }
            }
            catch (Exception ex)
            {
                Result = ex.ToString();
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddIssue(int? DetailsID, DateTime? Date, string Issue, string Status, string TransType)
        {
            string Message = "";
            try
            {
                if (TransType == "Add")
                {
                    repository._tbl_problems.details_id = DetailsID;
                    repository._tbl_problems.date_happen = Date;
                    repository._tbl_problems.problem = Issue;
                    repository._tbl_problems.status = Status;

                    repository._jo.InsertJOProblems(repository._tbl_problems);
                    repository._jo.Save();

                }
                else
                {
                    var Problem = repository._jo.JOProblemsById(Convert.ToInt32(TransType));

                    Problem.status = Status;

                    repository._jo.UpdateJOProblems(Problem);
                    repository._jo.Save();
                }
            }
            catch (Exception msg)
            {
                Message = msg.ToString();

            }
            return Json(Message, JsonRequestBehavior.AllowGet);

        }

        #endregion


        #region JO Attachments

        public ActionResult JobOrderScope(string jonumber)
        {
            var scope = (from s in repository._jo.GetAllJobOrder()
                         join d in repository._jo.GetAllJobOrderDetails() on s.details_id equals d.details_id
                         where s.job_no.Trim().Equals(jonumber.Trim())
                         select d).FirstOrDefault();

            var hasScope = scope.has_scope == null ? "" : scope.has_scope;
            return Json(hasScope, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UploadScope()
        {
            string result = "";
            string fileName = "";

            try
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase file = Request.Files[i];
                    int fileSize = file.ContentLength;
                    fileName = Path.GetFileName(file.FileName);
                    string mimeType = file.ContentType;
                    System.IO.Stream fileContent = file.InputStream;

                    string folderPath = Server.MapPath("~/JobOrder Scope/") + fileName;

                    if (System.IO.File.Exists(folderPath))
                    {
                        //Delete existing scope and add new one
                        result = ReplaceScope(folderPath);
                        file.SaveAs(folderPath);
                    }
                    else
                    {
                        //Add new scope and update db sw_scope
                        file.SaveAs(folderPath);
                        result = UpdateScope(fileName.Substring(0, fileName.IndexOf('.')));
                    }
                    result = folderPath;
                }
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }

            return Json(result + Request.Files.Count + " files");
        }

        public string ReplaceScope(string FolderPath)
        {
            System.IO.File.Delete(FolderPath);

            return "Removed";
        }

        public string UpdateScope(string FileName)
        {
            //Get the job order
            var scope = (from s in repository._jo.GetAllJobOrder()
                         join d in repository._jo.GetAllJobOrderDetails() on s.details_id equals d.details_id
                         where s.job_no.Trim().Equals(FileName.Trim())
                         select d).FirstOrDefault();

            //Add flag to job order has scope field
            scope.has_scope = "1";
            repository._jo.UpdateJobOrderDetails(scope);
            repository._jo.Save();

            return "Updated";
        }

        public ActionResult DownloadScope(string JobNumber)
        {
            string filePath = Server.MapPath("~/JobOrder Scope/") + JobNumber + ".docx";
            if (System.IO.File.Exists(filePath))
            {
                WebClient req = new WebClient();

                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.Buffer = true;
                Response.AddHeader("Content-Disposition", "attachment;filename=" + JobNumber + ".extension");
                byte[] data = req.DownloadData(Server.MapPath("~/JobOrder Scope/") + JobNumber + ".docx");
                Response.BinaryWrite(data);
                Response.End();
            }

            return View();
        }
        #endregion

        #region Gantt chart
        public ActionResult AddGanttChart(int? DetailsID, string Title, DateTime DateStarted, DateTime DateEnded, string WorkPackage, string TaskNo)
        {
            var result = "";
            try
            {
                repository._tbl_gantt_chart.details_id = DetailsID.ToString();
                repository._tbl_gantt_chart.functions = Title;
                repository._tbl_gantt_chart.date_started = DateStarted;
                repository._tbl_gantt_chart.date_ended = DateEnded;
                repository._tbl_gantt_chart.work_package = WorkPackage;
                repository._tbl_gantt_chart.branches = TaskNo;


                repository._gant_chart.InsertGanttChart(repository._tbl_gantt_chart);
                repository._gant_chart.Save();
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadGanttChart(int DetailsID)
        {
            var Result = repository._jo.GetAllocatedTime()
                                   .Where(r => r.details_id == DetailsID && !String.IsNullOrEmpty(r.work_description))
                                   .Select(r => new GanttChartModel
                                   {
                                       //WorkPackage = r.work_package,
                                       //Branches = r.branches,
                                       Function = r.work_description,
                                       DateStarted = Convert.ToDateTime(r.date).ToString("yyyy-MM-dd"),
                                       //  DateEnded = Convert.ToDateTime(r.date_ended).ToString("yyyy-MM-dd")
                                   }).ToList();

            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ClosedSWJO(string DetailsID, string Type)
        {
            var Result = repository._gant_chart.GetSWActualStandard(DetailsID, Type).ToList();

            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public void AutoGanttChart(string Status, int? DetailsID)
        {
            var TmpStatus = "";
            var TaskNo = "";
            switch (Status)
            {
                case "3":
                    TmpStatus = "Testing";
                    TaskNo = "C2";
                    break;
                case "4":
                    TmpStatus = "Debugging";
                    TaskNo = "C3";
                    break;
                case "5":
                    TmpStatus = "Orientation";
                    TaskNo = "D1";
                    break;
                case "6":
                    TmpStatus = "Closed";
                    TaskNo = "D2";
                    break;
            }
            if (TmpStatus != "")
                AddGanttChart(DetailsID, TmpStatus, Convert.ToDateTime(DateTime.Today.ToShortDateString()), Convert.ToDateTime(DateTime.Today.ToShortDateString()), TmpStatus, TaskNo);

            if (Status != "3" && TmpStatus != "")
            {
                var GanttChart = repository._gant_chart.GetAllGanttChart()
                                           .Where(g => Convert.ToInt32(g.details_id) == DetailsID)
                                           .OrderByDescending(g => g.gant_chart_id)
                                           .Take(2)
                                           .LastOrDefault();

                GanttChart.date_ended = Convert.ToDateTime(DateTime.Today.ToShortDateString());
                repository._gant_chart.UpdateGanttChart(GanttChart);
                repository._gant_chart.Save();
            }
        }

        public ActionResult UploadExcel(FormCollection formCollection)
        {
            //var usersList = new List<User>();
            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["UploadedFile"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        //Excel Details
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;
                        //End
                        //Iterate Excel File
                        for (int rowIterator = 5; rowIterator <= noOfRow; rowIterator++)
                        {

                        }
                    }
                }
            }
            return RedirectToAction("JobOrderDetails");
        }
        #endregion

        #endregion

        #region KPI JO

        public ActionResult KPIJobOrder()
        {
            if (Request.Cookies["Dept_ID"].Value.ToString() != "09")
                return RedirectToAction("NotAuthorized", "Home");

            return View();
        }

        public ActionResult GetCompletedJO(string Year)
        {
            var Result = repository._jo_classification.KPICompletedJO(Year)
                                                      .Select(x => new KPIJobOrder
                                                      {
                                                          ClosedJOMonth = x.month_completed,
                                                          ClosedJOCount = x.total_count,

                                                          CloseEmail = x.email,
                                                          ClosedDesktop = x.dekstop,
                                                          ClosedNetwork = x.network,
                                                          CloseServer = x.server,
                                                          CloseOthers = x.others,
                                                          CloseTel = x.tel,
                                                          CloseSoftware = x.software
                                                      })
                                                      .ToList();
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAssignedJO(string Year)
        {
            var Result = repository._jo_classification.KPIMonthlyAssignedJO(Year)
                                                      .Select(x => new KPIJobOrder
                                                      {
                                                          AssignedJOMonth = x.month_completed,
                                                          AssignedJOCount = x.total_count,

                                                          AssignedEmail = x.email,
                                                          AssignedDesktop = x.dekstop,
                                                          AssignedNetwork = x.network,
                                                          AssignedServer = x.server,
                                                          AssignedOthers = x.others,
                                                          AssignedTel = x.tel,
                                                          AssignedSoftware = x.software

                                                      })
                                                      .ToList();
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult KPIPerYear()
        {
            var Result = repository._jo_classification.KPIJoPerYear()
                                                      .Select(x => new KPIJobOrder
                                                      {
                                                          Year = x.jo_yr,
                                                          YearCount = x.yr_count
                                                      })
                                                      .ToList();

            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult KPIPerYearDetails()
        {
            var Result = repository._jo_classification.KPIJoPerYearDetails()
                                                      .Select(x => new KPIJobOrder
                                                      {
                                                          Year = x.jo_yr,
                                                          OnTime = x.jo_ontime,
                                                          Delayed = x.jo_delayed,
                                                          Ongoing = x.jo_ongoing,
                                                          Unassigned = x.jo_unassigned
                                                      })
                                                      .ToList();

            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult HardwarePercentage()
        {
            var Result = repository._jo_classification.KPIHardware()
                                                      .Select(x => new KPIJobOrder
                                                      {
                                                          AvgActualTime = x.avg_actualtime,
                                                          AvgStandardTime = x.avg_standard,
                                                          GoalPercentage = String.Format("{0:0.00}", x.percent_actualtime),
                                                          TotalActualTime = x.total_actual,
                                                          TotalStandardTime = x.tota_standard
                                                      })
                                                      .ToList();
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SatisfactoryRate()
        {
            var Result = repository._jo_classification.KPISatisfactoryRate()
                                    .Select(x => new KPIJobOrder
                                    {
                                        Satisfied5 = x.satisfied5.ToString(),
                                        Satisfied4 = x.satisfied4.ToString(),
                                        Neutral = x.satisfied3.ToString(),
                                        Dissatisfied2 = x.satisfied2.ToString(),
                                        Dissatisfied1 = x.satisfied1.ToString()
                                    }).ToList();

            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AllApcJo()
        {
            var Result = repository._dashboard.GetAllDevJOCount()
                                   .Select(x => new KPIJobOrder
                                   {
                                       APC = x.dev.Trim(),
                                       EmpID = !x.emp_id.Contains("gtp") ? "http://kpsmsvm:90/Content/images/" + x.emp_id.Insert(3, "-").Substring(1, x.emp_id.Length - 1).Trim() + ".bmp" : "/Content/img/imgFemale.png",
                                       TotalJO = x.total_completed.ToString(),
                                       HW = x.jo_hw.ToString(),
                                       SW = x.jo_sw.ToString()
                                   })
                                   .ToList();
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ResourceUtilization()
        {
            var Utilization = repository._jo_classification
                                        .KPIResourceUtilization()
                                        .Select(x => new KPIJobOrder
                                        {
                                            APC = x.emp_name,
                                            JOCount = x.jo_count,
                                            JOActualTime = x.jo_actual,
                                            ReportCount = x.report_count,
                                            ReportActualTime = x.report_mins
                                        })
                                        .ToList();

            return Json(Utilization, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetHWMonthlyReport(DateTime DateFilter)
        {
            if (Request.Cookies["Dept_ID"].Value.ToString() != "09")
                return RedirectToAction("NotAuthorized", "Home");

            var error = "";

            try
            {
                var HW = repository._monthly_report.GetHWMonthlyReport(DateFilter)
                    .Select(r => new DowntimeModel
                    {
                        JONumber = r.hw_codenumber,
                        ActualTime = r.actual_time,
                        StandardTime = r.standard_time,
                        MinYear = r.year_min,
                        MaxYear = r.year_max,
                        JobOrderDetails = r.details,
                        DateStarted = Convert.ToDateTime(r.datestarted).ToShortDateString(),
                        DateEnded = Convert.ToDateTime(r.dateended).ToShortDateString(),
                        Assignee = r.assignee,
                        Achieved = r.achieved.ToString(),
                        Unachieved = r.unachieved.ToString(),
                        DateTarget = Convert.ToDateTime(r.date_target).ToShortDateString()
                    }).ToList();

                return Json(HW, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                error = ex.ToString();
            }

            return Json(error, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSWMonthlyReport(DateTime DateFilter)
        {
            var SW = repository._monthly_report.GetSWMonthlyReport(DateFilter)
                                .Select(r => new DowntimeModel
                                {
                                    JONumber = r.hw_codenumber,
                                    ActualTime = r.actual_time,
                                    StandardTime = r.standard_time,
                                    MinYear = r.year_min,
                                    MaxYear = r.year_max,
                                    JobOrderDetails = r.details,
                                    DateStarted = Convert.ToDateTime(r.datestarted).ToShortDateString(),
                                    DateEnded = Convert.ToDateTime(r.dateended).ToShortDateString(),
                                    Assignee = r.assignee,
                                    Achieved = r.achieved.ToString(),
                                    Unachieved = r.unachieved.ToString(),
                                    DateTarget = Convert.ToDateTime(r.date_target).ToShortDateString()

                                }).ToList();

            return Json(SW, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetYears()
        {
            var Year = repository._jo.GetAllJobOrder()
                                     .Select(x => Convert.ToDateTime(x.date_filed).ToString("yyyy"))
                                     .FirstOrDefault();
            return Json(Year, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetMonthyReport(DateTime From, DateTime To)
        {
            var Result = repository._monthly_report.GetCostCenterGraph(From, To).ToList();

            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPieMonthlyReport(DateTime From, DateTime To)
        {
            var Graph = repository._monthly_report.GetCostCenterGraph(From, To).ToList();

            return Json(Graph, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetExportedJO(DateTime DateFrom, DateTime DateTo)
        {
            var Report = repository._jo.ExportJOToExcel(
                            Convert.ToDateTime(Convert.ToDateTime(DateFrom).ToString("yyyy-MM-dd")),
                            Convert.ToDateTime(Convert.ToDateTime(DateTo).ToString("yyyy-MM-dd")),
                            0)
                            .ToList();
            return Json(Report, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region All Unassigned, sir alfred's page

        public ActionResult AllUnassignedJO()
        {
            //returns only created jo
            var Unassigned = repository._jo_classification.SPJoUnassignedList()
                                                           .Select(x => new UnassignedJOModel()
                                                           {
                                                               JobNo = x.job_no,
                                                               Requestor = x.requested_by,
                                                               DateFiled = x.date_filed.ToString(),
                                                               Details = x.details,
                                                               Category = (int)x.type_id == 8 ? "Software" : "Hardware",
                                                               Status = x.status
                                                           })
                                                           .ToList();
            var JobAvailability = repository._jo.JOAvailability().FirstOrDefault();

            //Disable/Enable creation of JO
            ViewBag.JOAvailability = JobAvailability.can_create;
            ViewBag.JOAvailabilityMessage = JobAvailability.message;

            ViewBag.Assignee = repository._apc.GetAPC()
                                              .Where(x => x.emp_id.Trim() != "g930192" &&
                                                     x.emp_id != "J0061" &&
                                                     x.status == "Active")
                                              .ToList();

            ViewBag.Status = repository._jostatus.GetAllStatus().ToList();
            
            var Stages = repository._jo.SP_SW_Stages_And_Issues().ToList();

            return View(Unassigned);
        }

        public ActionResult APCOngoingJo()
        {
            var OngoingJO = repository._dashboard.GetAllDevJOCount()
                                                 .Select(x => new DashboardModel
                                                 {
                                                     Developer = x.dev,
                                                     DevToday = x.today_total,
                                                     DevTotal = x.overall_total,
                                                     AchievedJO = x.achieved_jo,
                                                     UnAchievedJO = x.unachieved_jo,
                                                     TelNum = x.dev_contact,
                                                     DueOngoing = x.due_ongoing,
                                                     Assessment = x.assessment
                                                 }).ToList();

            return Json(OngoingJO, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LatestAvailableAPC()
        {
            var APC = repository._jo_classification.SPLatestAvailable()
                                                   .Select(x => new UnassignedJOModel
                                                   {
                                                       ApcName = x.emp_name.Trim(),
                                                       Assignee = !string.IsNullOrEmpty(x.emp_id.Trim()) ? x.emp_id.Insert(2, "-").TrimEnd() + ".bmp" : "nopic.png",//"http://kpsmsvm:90/Content/images/" + x.emp_id.Insert(2, "-").TrimEnd() + ".bmp",
                                                       Datetarget = string.IsNullOrEmpty(x.latest_target) ? "Available " : Convert.ToDateTime(x.latest_target).ToString("yyyy-MM-dd")
                                                   })
                                                   .ToList();

            return Json(APC, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AllDeferredJO()
        {
            var Deferred = repository._jo_classification.GetListOfJobOrder()
                                   .Where(x => x.jo_status.Equals("Under Assessment") &&
                                                (String.IsNullOrEmpty(x.date_assigned.ToString()) &&
                                                String.IsNullOrEmpty(x.date_target.ToString()) &&
                                                String.IsNullOrEmpty(x.date_started.ToString()) &&
                                                String.IsNullOrEmpty(x.date_needed.ToString())) &&
                                                x.datefiled >= DateTime.Now.AddDays(-20)
                                   ).Select(x => new UnassignedJOModel
                                   {
                                       JobNo = x.jonumber,
                                       Details = x.details,
                                       Requestor = x.requestor,
                                       DateFiled = Convert.ToDateTime(x.datefiled).ToString("yyyy-MM-dd")
                                   })
                                   .OrderBy(x => x.DateFiled)
                                   .ToList();
            return Json(JsonRequestBehavior.AllowGet);
        }

        public ActionResult DepartmentOnGoingJO()
        {
            var Department = repository._monthly_report.GetJOByDepartment()
                                                       .Select(x => new JOByDepartment
                                                       {
                                                           DeptName = x.department,
                                                           Count = x.jo_count.ToString(),
                                                           DelayedJo = x.jo_delayed.ToString()

                                                       })
                                                       .ToList();

            return Json(Department, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateUnassignedJO(string JobNo, string Assignee, string Datetarget, string Message, int JOType, string CodeNumber, int StatusID)
        {
            bool Result = true; // change to false for sending email

            try
            {
                //Get joborder details id
                var JO = repository._jo.GetAllJobOrder()
                                       .Where(jo => jo.job_no == JobNo)
                                       .FirstOrDefault();

                var JODetails = repository._jo.GetAllJobOrderDetails()
                                              .Where(d => d.details_id == JO.details_id)
                                              .FirstOrDefault();

                if (!string.IsNullOrEmpty(Datetarget))
                    JODetails.date_target = Convert.ToDateTime(Datetarget);


                JODetails.assignee = Assignee; // assignee
                JODetails.date_assigned = DateTime.Today;
                JODetails.status_id = StatusID;
                JODetails.type_id = (int)JOType;
                JODetails.sw_codenumber = CodeNumber;
                JODetails.date_started = DateTime.Today;


                if (StatusID == 12)
                    JODetails.date_ended = DateTime.Today;

                repository._jo.UpdateJobOrderDetails(JODetails);
                repository._jo.Save();

                UpdateStatusTimeLine(JODetails.details_id, StatusID);

                if (StatusID == 4)
                {
                    Result = SendEmail(JobNo, DateTime.Now.ToString("MMMM dd, yyyy-dddd hh:mm tt"), string.IsNullOrEmpty(Datetarget) ? ""
                                                        : Convert.ToDateTime(Datetarget).ToString("MMMM dd, yyyy-dddd"), Message);
                }
            }
            catch (Exception ex)
            {

            }

            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public void UpdateStatusTimeLine(int DetailsID, int StatusID)
        {
            //Check timeline status - convert sp result to entity
            var TimelineStatus = repository._jo_classification.SPTimelineStatus(DetailsID)
                                                              .Select(x => new tbl_status_timeline
                                                              {
                                                                  details_id = x.details_id,
                                                                  under_assessment = x.under_assessment,
                                                                  incharge_assigned = x.incharge_assigned,
                                                                  request_documents = x.request_documents,
                                                                  development = x.development,
                                                                  pending_incharge = x.pending_incharge,
                                                                  proposal = x.proposal,
                                                                  rejected = x.rejected,
                                                                  review = x.review,
                                                                  revision = x.revision,
                                                                  testing = x.testing,
                                                                  t_id = x.t_id
                                                              })
                                                              .FirstOrDefault();

            //Check status updated
            if (TimelineStatus == null)
            {
                //Identify what status
                switch (StatusID)
                {

                    case 2: repository._tbl_status_timeline.under_assessment = DateTime.Now; break;
                    case 3: repository._tbl_status_timeline.pending_incharge = DateTime.Now; break;
                    case 4: repository._tbl_status_timeline.incharge_assigned = DateTime.Now; break;
                    case 5: repository._tbl_status_timeline.request_documents = DateTime.Today; break;
                    case 6: repository._tbl_status_timeline.review = DateTime.Now; break;
                    case 7: repository._tbl_status_timeline.rejected = DateTime.Now; break;
                    case 8: repository._tbl_status_timeline.proposal = DateTime.Now; break;
                    case 9: repository._tbl_status_timeline.revision = DateTime.Now; break;
                    case 10: repository._tbl_status_timeline.development = DateTime.Now; break;
                    case 11: repository._tbl_status_timeline.testing = DateTime.Now; break;
                }

                repository._tbl_status_timeline.details_id = DetailsID;
                repository._jostatus.InsertStatusTimeline(repository._tbl_status_timeline);

            }
            else
            {
                //Identify what status
                switch (StatusID)
                {
                    case 2: TimelineStatus.under_assessment = DateTime.Now; break;
                    case 3: TimelineStatus.pending_incharge = DateTime.Now; break;
                    case 4: TimelineStatus.incharge_assigned = DateTime.Now; break;
                    case 5: TimelineStatus.request_documents = DateTime.Now; break;
                    case 6: TimelineStatus.review = DateTime.Now; break;
                    case 7: TimelineStatus.rejected = DateTime.Now; break;
                    case 8: TimelineStatus.proposal = DateTime.Now; break;
                    case 9: TimelineStatus.revision = DateTime.Now; break;
                    case 10: TimelineStatus.development = DateTime.Now; break;
                    case 11: TimelineStatus.testing = DateTime.Now; break;
                }
                repository._jostatus.UpdateStatusTimeline(TimelineStatus);
                repository._jostatus.Save();
            }
        }

        public bool SendEmail(string JobNo, string DateAssigned, string DateTarget, string OnHoldMessage)
        {
            bool isSent = false;
            try
            {
                var EmailRecipients = repository._jo_classification.JODesignationEmail(JobNo).FirstOrDefault();

                var Reciepients = EmailRecipients.requested_by_email + EmailRecipients.assignee_email;
                var CC = EmailRecipients.approved_by_email + EmailRecipients.noted_by_email + EmailRecipients.agm_email + EmailRecipients.default_recepient;

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("automailer.rimp@ph.ricoh-imaging.com");

                //mailMessage.CC.Add("grace.booc@ph.ricoh-imaging.com"); // Managers

                mailMessage.To.Add(Reciepients.Trim().Remove(Reciepients.Length - 1));
                mailMessage.CC.Add(CC.Trim().Remove(CC.Length - 1));

                string Subject = "";

                string MessageContent = "";
                //Message Content
                // if (TransactionType == "Assign")
                // {
                Subject = "Job Order Request Confirmation";
                MessageContent = " is accepted at APC <br> on " + DateAssigned +
                                 " and is assigned to <u>" + EmailRecipients.assignee.Trim().Substring(0, EmailRecipients.assignee.Trim().Length - 3) + "</u>.<br>" +
                                 "Approved Estimated Target Date of completion is " + DateTarget +
                                 ".<br>If you have any concern, kindly contact local # <u>1220</u> or the assigned person at local # <u>" + EmailRecipients.assignee_local + "</u>." +
                                  "<br><br> Job Order Link: <a href='http://kpsweb2:1001/JobOrders/JobOrderDetails?JoNumber=" + JobNo + "#AllJo'>Link</a>" +
                                 "<br><br><br><br>** Please do not reply, this is auto generated email ** ";
                //}
                //else
                //{
                //    Subject = "On-Hold Job Order";
                //    MessageContent = OnHoldMessage;
                //}

                mailMessage.Subject = Subject;
                string headerMail = @"Hi " + EmailRecipients.requested_by.Trim().Substring(0, EmailRecipients.requested_by.Trim().Length - 3) + ",<br><br>" +
                                    "Your Job Order # " + JobNo + MessageContent;


                mailMessage.Body = headerMail;
                mailMessage.IsBodyHtml = true;
                SmtpClient SmtpServer = new SmtpClient();
                SmtpServer.Port = 25;
                SmtpServer.Host = "mail.ricoh.co.jp";
                SmtpServer.Send(mailMessage);
                mailMessage.Dispose();

                isSent = true;
            }
            catch
            {

            }
            return isSent;
        }

        public ActionResult SearchUnassignedJO(string JobNo)
        {
            var JO = repository._jo_classification.SPJoUnassignedList()
                                                      .Select(x => new UnassignedJOModel()
                                                      {
                                                          JobNo = x.job_no,
                                                          Requestor = x.requested_by,
                                                          DateFiled = x.date_filed.ToString(),
                                                          Details = x.details,
                                                      })
                                                      .Where(x => x.JobNo.Contains(JobNo))
                                                      .ToList();

            return Json(JO, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchAssigneeJOs(string Assignee)
        {
            var JO = repository._jo.SP_GetOngoingJOAssignee(Assignee.TrimStart())
                                .Select(x => new UnassignedJOModel
                                {
                                    JobNo = x.jobno,
                                    Details = x.details,
                                    Datetarget = Convert.ToDateTime(x.date_target).ToShortDateString(),
                                    JoProgress = x.progress_rate == null ? "0" : x.progress_rate.ToString(),
                                    Department = x.dept_name,
                                    DateProgress = Convert.ToDateTime(x.date_progress_updated).ToString("yyyy-MM-dd"),
                                    IsDue = x.flg_due,
                                    ReasonOfDelay = x.reason_of_due
                                })
                                .ToList();

            return Json(JO, JsonRequestBehavior.AllowGet);
        }
        public ActionResult JoByDeptDetails(string DeptName)
        {
            var Result = repository._jo_classification.GetOnGoingJOByDept()
                                                       .Where(x => x.department.Trim() == DeptName.Trim())
                                                       .Select(x => new UnassignedJOModel
                                                       {
                                                           Details = x.details,
                                                           Datetarget = Convert.ToDateTime(x.date_target).ToShortDateString(),
                                                           JoProgress = x.progress_rate == null ? "0" : x.progress_rate.ToString(),
                                                           JobNo = x.job_no,
                                                           Department = x.department,
                                                           Assignee = x.assignee,
                                                           ReasonOfDelay = x.reason_delay
                                                       })
                                                       .ToList();

            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UnassignedToExcel()
        {

            int row = 2;

            string filename = "Joborder-Report-" + DateTime.Today.ToShortDateString() + ".xlsx";

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(new FileInfo("MyWorkbook.xlsx")))
            {
                ExcelWorksheet Sheet1 = package.Workbook.Worksheets.Add("ByAssignee");

                Sheet1.Column(1).Width = 30;
                Sheet1.Column(2).Width = 25;
                Sheet1.Column(3).Width = 70;
                Sheet1.Column(4).Width = 20;
                Sheet1.Column(5).Width = 20;
                Sheet1.Column(6).Width = 20;
                Sheet1.Column(7).Width = 70;

                Sheet1.Cells["A1:E1"].Style.Font.Size = 15;

                DataTable tbl = new DataTable();

                tbl.Columns.Add("Department", typeof(string));
                tbl.Columns.Add("JO No.", typeof(string));
                tbl.Columns.Add("Details", typeof(string));
                tbl.Columns.Add("Assignee", typeof(string));
                tbl.Columns.Add("Progress", typeof(string));
                tbl.Columns.Add("Date Target", typeof(string));
                tbl.Columns.Add("Reason Of Delay", typeof(string));


                var ApcJO = repository._jo_classification.GetOnGoingJOByDept()
                                          .Select(x => new UnassignedJOModel
                                          {
                                              JobNo = x.job_no,
                                              Details = x.details,
                                              Datetarget = Convert.ToDateTime(x.date_target).ToShortDateString(),
                                              JoProgress = x.progress_rate == null ? "0" : x.progress_rate.ToString(),
                                              Department = x.department,
                                              DateProgress = Convert.ToDateTime(x.date_progress_updated).ToString("yyyy-MM-dd"),
                                              ReasonOfDelay = x.reason_delay,
                                              Assignee = x.assignee,
                                              DateTarget = Convert.ToDateTime(x.date_target).ToString("yyyy-MM-dd")
                                          })
                                          .OrderBy(x => x.Department)
                                          .ToList();

                foreach (var JO in ApcJO)
                {
                    tbl.Rows.Add(JO.Department, JO.JobNo, JO.Details, JO.Assignee, JO.JoProgress + "% Completed", JO.DateTarget, JO.ReasonOfDelay);
                }

                //tbl.DefaultView.Sort = "Department desc";
                //tbl.DefaultView.ToTable();

                Sheet1.Cells[1, 1].LoadFromDataTable(tbl, true, OfficeOpenXml.Table.TableStyles.Light13);

                //Sheet1.Cells["E1"].AutoFilter = true;
                Sheet1.Cells["A:AZ"].Style.WrapText = true;
                Sheet1.Cells[Sheet1.Dimension.Address].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                Sheet1.Cells[Sheet1.Dimension.Address].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                Sheet1.Cells[Sheet1.Dimension.Address].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                Sheet1.Cells[Sheet1.Dimension.Address].Style.Border.Left.Style = ExcelBorderStyle.Thin;

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + filename);
                Response.BinaryWrite(package.GetAsByteArray());
                Response.End();
            }
            return View();
        }

        #endregion

        #region Survey
        public void AddSurvey(string JONumber)
        {
            var Details = repository._jo.GetAllJobOrder()
                                    .Join(repository._jo.GetAllJobOrderDetails(),
                                           e => e.details_id, c => c.details_id, (e, c) => new { e, c })
                                    .GroupBy(y => new
                                    {
                                        y.e.details_id,
                                        y.e.job_no,
                                        y.e.requested_by,
                                        y.c.assignee,
                                        y.c.details
                                    })
                                    .Where(x => x.Key.job_no == JONumber)
                                    .FirstOrDefault();

            var Emails = repository._employee.GetRegisteredEmployees()
                                             .Join(repository._employee.GetEmployees(),
                                                   e => e.emp_id.Trim(), c => c.emp_id.Trim(), (e, c) => new { e, c })
                                             .GroupBy(y => new
                                             {
                                                 y.e.emp_id,
                                                 y.c.emp_name,
                                                 y.e.email_address
                                             })
                                             .Where(y => y.Key.emp_name.Trim() == Details.Key.requested_by || y.Key.emp_name.Trim() == Details.Key.assignee.Trim())
                                             .Select(y => y.Key.email_address)
                                             .ToList();


            var tmpEmails = "";
            foreach (var email in Emails)
            {
                tmpEmails += email + ",";
            }

            var Reciepients = tmpEmails.Trim().Substring(0, tmpEmails.Trim().Length - 1);

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("automailer.rimp@ph.ricoh-imaging.com");

            mailMessage.To.Add(Reciepients);

            mailMessage.Subject = "JobOrder Confirmation And Survey";

            string headerMail = @"Hi " + Details.Key.requested_by.Substring(0, Details.Key.requested_by.Trim().Length - 2) + ",<br><br>" +
                                 Details.Key.assignee.Substring(0, Details.Key.assignee.Trim().Length - 2) + ",the in-charge of this JO has already closed this Job Order please confirm and send us your feedback.<br><br>" +
                                 "JO Details: <br>" + Details.Key.details + "<br><br>" +
                                 "<b style='font-size:15px;'>Tell Us About Your Experience.</b><br><br>" +
                                 "How satisfied were you with our service for this Job Order? <br><br><br>" +
                                 "<button><a href = 'http://kpsweb2:1001/JobOrders/SurveySubmission?JoNo=" + JONumber + "&Rate=5'>[5] - Very Satisfied</a></button><br>" +
                                 "<button><a href = 'http://kpsweb2:1001/JobOrders/SurveySubmission?JoNo=" + JONumber + "&Rate=4'>[4] - Somewhat Satisfied</a></button><br>" +
                                 "<button><a href = 'http://kpsweb2:1001/JobOrders/SurveySubmission?JoNo=" + JONumber + "&Rate=3'>[3] - Neutral</a></button><br>" +
                                 "<button><a href = 'http://kpsweb2:1001/JobOrders/SurveySubmission?JoNo=" + JONumber + "&Rate=2'>[2] - Somewhat Dissatisfied</a></button><br>" +
                                 "<button><a href = 'http://kpsweb2:1001/JobOrders/SurveySubmission?JoNo=" + JONumber + "&Rate=1'>[1] - Dissatisfied</a></button>" +
                                 "<br/><br><br>" +
                                 "<b>Thank You.</b>";

            mailMessage.Body = headerMail;
            mailMessage.IsBodyHtml = true;

            SmtpClient SmtpServer = new SmtpClient();
            SmtpServer.Port = 25;
            SmtpServer.Host = "mail.ricoh.co.jp";
            SmtpServer.Send(mailMessage);
            mailMessage.Dispose();
        }

        [AllowAnonymous]
        public ActionResult SurveySubmission(string JoNo, int Rate)
        {

            var Details = repository._jo.GetAllJobOrder()
                                        .Where(x => x.job_no == JoNo && x.rate == null)
                                        .FirstOrDefault();
            if (Details != null)
            {
                Details.rate = Rate;
                Details.date_confirmed = DateTime.Today;

                repository._jo.UpdateJobOrder(Details);
                repository._jo.Save();
            }

            return View();
        }
        #endregion

        #region Others

        public ActionResult UpdateJOCreationAvailability(string Value, string NewMessage, string TempAccess)
        {
            var Message = "";

            try
            {
                var Availablility = repository._jo.JOAvailability().FirstOrDefault();

                Availablility.can_create = Value;
                Availablility.message = NewMessage;
                Availablility.temp_access = TempAccess;

                repository._jo.UpdateJOAvailability(Availablility);
                repository._jo.Save();
            }
            catch (Exception ex)
            {
                Message = ex.ToString();
            }
            return Json(Message, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchRegisteredEmployees(string Term)
        {
            var data = repository._employee.GetEmployees()
                                               .Where(x => x.emp_name.ToLower().Trim().StartsWith(Term.ToLower().Trim()))
                                                .ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }

}