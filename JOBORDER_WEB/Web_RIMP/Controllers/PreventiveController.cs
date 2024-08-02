using db.Database;
using db.Global;
using db.Repository.APC;
using db.Repository.Maintenance;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using PagedList;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Web_RIMP.Models;

namespace Web_RIMP.Controllers
{
    public class PreventiveController : Controller
    {

        #region Global Initialization

        RepositoryInitialization repository = new RepositoryInitialization();
      
       
        public PreventiveController()
        {
            this.repository._apc = new Programmer(new JOEntities());
            this.repository._preventive = new Preventive(new JOEntities());
        }

        #endregion

        // GET: Preventive
        public ActionResult PreventiveMaintenance(int? page)
        {
            if (Request.Cookies["Dept_ID"].Value.ToString() != "09")
                return RedirectToAction("NotAuthorized", "Home");

            var Summary = repository._preventive.GetPreventiveSummary()
                                                .Select(x => new PreventiveModels
                                                {
                                                    Year = x.preventive_yr,
                                                    PlannedMonth = x.preventive_month,
                                                    Percentage   = x.percentage.ToString(),
                                                    Status       = x.status,
                                                    ActualPreventive   = x.actual_preventive.ToString(),
                                                    StandardPreventive = x.standard_preventive.ToString()
                                                })
                                                .ToList();

            ViewBag.Diagnosis = repository._preventive.GetAllPreventivesServices();
            ViewBag.APC       = repository._apc.GetAPC().Where(x => x.status.Equals("Active")).ToList();

            return View(Summary.ToPagedList(page ?? 1, 50));
        }

        public ActionResult GetAllComputers(string FilePath, string Plan)
        {
            List<PreventiveModels> Preventive = new List<PreventiveModels>();

            // Get the file we are going to process
            var existingFile = new FileInfo(FilePath);
            // Open and read the XlSX file.
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(existingFile))
            {
                // Get the work book in the file
                var workBook = package.Workbook;
                if (workBook != null)
                {
                    if (workBook.Worksheets.Count > 0)
                    {
                        bool Go = true;
                        int Col = 2;
                        int Designation = 0;
                        string Incharge = "";

                        var currentWorksheet = workBook.Worksheets.First();

                        //Excluding sir alfred and sir inoue
                        var Apc = repository._apc.GetAPC()
                                                 .Where(x => x.status.Equals("Active") &&
                                                        (!x.emp_id.Trim().Equals("g930192") &&
                                                         !x.emp_id.Trim().Equals("J0061") &&
                                                         !x.emp_id.Trim().Equals("g091672")))
                                                 .Select(x => x.emp_name.Trim())
                                                 .ToArray();

                        var ApcCount = Apc.Count();

                        while (Go)
                        {
                            PreventiveModels Model = new PreventiveModels();

                            try
                            {
                                object TmpPc = currentWorksheet.Cells[Col, 1].Value;
                                object Owner = currentWorksheet.Cells[Col, 7].Value;

                                if (Designation >= ApcCount)
                                    Designation = 0;

                                Incharge = Apc[Designation].Trim();

                                if (TmpPc != null)
                                {
                                    string DirtyPC = JsonConvert.SerializeObject(TmpPc);
                                    string PC = Regex.Replace(DirtyPC.Substring(0, DirtyPC.Length - 1), "[^a-zA-Z0-9_.,-]+", "", RegexOptions.Compiled);

                                    //Standardize department naming convention
                                    if (PC.ToLower().Contains("apc"))
                                        Model.Department = "APC";
                                    else if (PC.ToLower().Contains("actg"))
                                        Model.Department = "CMD-ACTG";
                                    else if (PC.ToLower().Contains("adm"))
                                        Model.Department = "CMD-ADMIN";
                                    else if (PC.ToLower().Contains("cpd"))
                                        Model.Department = "PD";
                                    else if (PC.ToLower().Contains("cqc"))
                                        Model.Department = "QMD-QC";
                                    else if (PC.ToLower().Contains("csd"))
                                        Model.Department = "CSD";
                                    else if (PC.ToLower().Contains("ctd"))
                                        Model.Department = "CTD";
                                    else if (PC.ToLower().Contains("efs"))
                                        Model.Department = "ESFD";
                                    else if (PC.ToLower().Contains("ppc"))
                                        Model.Department = "PPC";
                                    else if (PC.ToLower().Contains("pqa"))
                                        Model.Department = "QMD-PQA";
                                    else
                                        Model.Department = "PRODUCTION";

                                    //Check if pc already exist, only add the new one | data control
                                    var PCID = repository._preventive.GetAllPreventivesPC()
                                               .Where(x => x.computer_name.Trim().Equals(PC))
                                               .Select(x => x.pc_id)
                                               .FirstOrDefault();

                                    Model.ExistingPCID = PCID.ToString();
                                    Model.Equipment = PC.ToUpper().Contains("KPD") ? "Computer" : "Laptop";
                                    Model.ComputerName = PC;
                                    Model.PCOwner = JsonConvert.SerializeObject(Owner);
                                    Model.Incharge = Incharge;
                                    Model.Status = "On-going";
                                    Model.PlannedMonth = Plan;
                                    Model.Year = DateTime.Today.Year.ToString();
                                    Preventive.Add(Model);

                                    Incharge = "";
                                    Col++;
                                    Designation++;
                                }
                                else
                                    Go = false;
                            }
                            catch (Exception ex)
                            {
                                Model.Message = ex.ToString();
                            }
                        }
                    }
                }
            }

            return Json(Preventive.OrderBy(x => x.Incharge), JsonRequestBehavior.AllowGet);
        }
        public ActionResult SavePreventive(List<string> Preventive)
        {
            List<string> Summary = new List<string>();
            JavaScriptSerializer js = new JavaScriptSerializer();

            var output = js.Serialize(Preventive);
            var ParsePreventive = JArray.Parse(output);


            for (int i = 0; i < Preventive.Count; i++)
            {
                int JsonCount = 1;
                dynamic elements = JsonConvert.DeserializeObject(ParsePreventive[i].ToString());

                foreach (var item in elements)
                {

                    string tmp = item.ToString();

                    var messy = Regex.Replace(tmp, @"\r\n?|\\n", Environment.NewLine);
                    var rem1  = messy.Substring(messy.IndexOf(":") + 2);

                    var rem2  = rem1.Substring(1, rem1.Length - 1);
                    var clean = Regex.Replace(rem2.Substring(0, rem2.Length - 1), "[^a-zA-Z0-9_.,-]+", "", RegexOptions.Compiled);

                    switch (JsonCount)
                    {
                        //Pc
                        case 1: repository._tbl_preventive_pc.computer_name = clean; break;
                        case 2: repository._tbl_preventive_pc.equipment     = clean; break;
                        case 3: repository._tbl_preventive_pc.owner         = clean; break;
                        case 4: repository._tbl_preventive_pc.department    = clean; break;
                        //preventive maintenance
                        case 5: repository._tbl_preventive_maintenance.status           = clean; break;
                        case 6: repository._tbl_preventive_maintenance.planned_month    = clean; break;
                        case 7: repository._tbl_preventive_maintenance.preventive_year  = clean; break;
                        case 8: repository._tbl_preventive_maintenance.incharge         = clean; break;
                        case 9: repository._tbl_preventive_maintenance.pc_id            = Convert.ToInt32(clean); break;

                    }

                    JsonCount++;
                }
                if (Summary.Count == 0)
                    Summary.Add(JsonConvert.SerializeObject(elements));
             
                //New pc added, insert only the non-exisiting pc in the table, to control the data
                if (repository._tbl_preventive_maintenance.pc_id == 0)
                {
                    var PCID = repository._preventive.GetAllPreventivesPC()
                                                 .Where(x => x.computer_name.Trim().Equals(repository._tbl_preventive_pc.computer_name))
                                                 .Select(x => x.pc_id)
                                                 .FirstOrDefault();

                    repository._tbl_preventive_maintenance.pc_id = PCID;
                    repository._preventive.InsertPreventivesPC(repository._tbl_preventive_pc);
                }
                repository._tbl_preventive_maintenance.pc_id = repository._tbl_preventive_maintenance.pc_id;
                repository._preventive.InsertPreventive(repository._tbl_preventive_maintenance);
            }

            return Json(Summary, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPlanDetails(string PlanMonth, string Search)
        {
            var Details = RetrievePlanDetails(PlanMonth);

            //Override details if transaction is search
            if (!String.IsNullOrEmpty(Search))
                Details = SearchPreventiceDetails(Search, Details);
            
            return Json(Details, JsonRequestBehavior.AllowGet);
        }

        public List<PreventiveModels> RetrievePlanDetails(string PlanMonth)
        {
            var List = repository._preventive.GetAllPreventivesPC()
                                                .Join(repository._preventive.GetAllPreventives(),
                                                                            p => p.pc_id, pc => pc.pc_id, (pc, p) => new { p, pc })
                                                .GroupBy(y => new {
                                                    y.p.maintenance_id,
                                                    y.p.planned_month,
                                                    y.pc.computer_name,
                                                    y.pc.equipment,
                                                    y.pc.department,
                                                    y.p.incharge,
                                                    y.p.status
                                                })
                                                .Select(x => new PreventiveModels
                                                {
                                                    ID = x.Key.maintenance_id,
                                                    PlannedMonth = x.Key.planned_month,
                                                    ComputerName = x.Key.computer_name,
                                                    Equipment = x.Key.equipment,
                                                    Department = x.Key.department,
                                                    Incharge = x.Key.incharge,
                                                    Status = x.Key.status
                                                })
                                                .Where(x => x.PlannedMonth.Equals(PlanMonth))
                                                .OrderBy(x => x.Department)
                                                .ToList();
            return List;
        }
        public List<PreventiveModels> SearchPreventiceDetails(string SearchWord, List<PreventiveModels> Preventive)
        {
            var Result = Preventive
                                    .Where(x => x.ComputerName.ToLower().Trim().Contains(SearchWord.Trim().ToLower()) ||
                                                x.Department.ToLower().Trim().Contains(SearchWord.Trim().ToLower())   ||
                                                x.Equipment.ToLower().Trim().Contains(SearchWord.Trim().ToLower())    ||
                                                x.Incharge.ToLower().Trim().Contains(SearchWord.Trim().ToLower())     ||
                                                x.Status.ToLower().Trim().Contains(SearchWord.Trim().ToLower()))
                                    .ToList();
            return Result;
        }

        public ActionResult SavePreventiceDiagnosis(DateTime ActualTime, List<string> Diagnosis, string PreparedBy, string CheckedBy, string ApprovedBy)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            var output = js.Serialize(Diagnosis);
            var ParsePreventive = JArray.Parse(output);

            for (int i = 0; i < Diagnosis.Count; i++)
            {
                int JsonCount = 1;
                dynamic elements = JsonConvert.DeserializeObject(ParsePreventive[i].ToString());

                foreach (var item in elements)
                {

                    string tmp = item.ToString();

                    var messy = Regex.Replace(tmp, @"\r\n?|\\n", Environment.NewLine);
                    var rem1 = messy.Substring(messy.IndexOf(":") + 2);

                    var rem2 = rem1.Substring(1, rem1.Length - 1);
                    var clean = Regex.Replace(rem2.Substring(0, rem2.Length - 1), "[^a-zA-Z0-9_.,-]+", "", RegexOptions.Compiled);

                    switch (JsonCount)
                    {
                        case 1: repository._tbl_preventive_diagnose.maintenance_id = Convert.ToInt32(clean); break;
                        case 2: repository._tbl_preventive_diagnose.services_id    = Convert.ToInt32(clean); break;
                        case 3: repository._tbl_preventive_diagnose.notes          = clean; break;
                        case 4: repository._tbl_preventive_diagnose.remarks        = clean; break;
                        case 5: repository._tbl_preventive_diagnose.time_finish    = clean; break;
                    }

                    JsonCount++;
                }

                var Maintenance = repository._preventive.GetAllPreventives()
                                                        .Where(x => x.maintenance_id == repository._tbl_preventive_diagnose.maintenance_id)
                                                        .FirstOrDefault();

                Maintenance.actual_date = ActualTime;
                Maintenance.status      = "Completed";
                Maintenance.prepared_by = PreparedBy;
                Maintenance.approved_by = ApprovedBy;
                Maintenance.checked_by  = CheckedBy;

                repository._preventive.UpdatePreventive(Maintenance);
                repository._preventive.InsertDiagnoses(repository._tbl_preventive_diagnose);
            }

            return Json(JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportPreventive(string PlanMonth)
        {
            string filename = "Joborder-Report-" + DateTime.Today.ToShortDateString() + ".xlsx";

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo("MyWorkbook.xlsx")))
            {
                ExcelWorksheet Sheet = package.Workbook.Worksheets.Add("Report");

                //Header Title
                Sheet.Cells["E1:M1"].Style.Font.Size = 20;
                Sheet.Cells["A1:I1"].Value = "APC IT EQUIPMENTS AND FACILITIES PLAN";
                Sheet.Cells["A1:I1"].Merge = true;
                Sheet.Cells["A1:I1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                Sheet.Cells["A1:I1"].Style.Font.Bold = true;
                //END

                //Approvals And Legend
                Sheet.Cells["A2:Q2"].Style.Font.Size = 12;
                Sheet.Cells["A2:A2"].Value = "Department:";
                Sheet.Cells["B2:B2"].Value = "ASIA PACIFIC CENTER";
                Sheet.Cells["F2:F2"].Value = "Approved By:";
                Sheet.Cells["G2:G2"].Value = "Checked By:";
                Sheet.Cells["H2:H2"].Value = "Prepared By:";
                Sheet.Cells["B3:B3"].Value = "Updated as of:";
                Sheet.Cells["C3:C3"].Value = "Month:";

                Sheet.Cells["A4:Q4"].Style.Font.Size = 12;
                Sheet.Cells["A4:A4"].Value = "Preventives:";
                Sheet.Cells["B5:B5"].Value = "";
                Sheet.Cells["B6:B6"].Value = "";
                Sheet.Cells["B5:B5"].Value = "Insert month";
                Sheet.Cells["B6:B6"].Value = "Insert month";

                Sheet.Cells["F6:F6"].Value = "A. Balundo";
                Sheet.Cells["G6:G6"].Value = "M. Amoin";
                Sheet.Cells["H6:H6"].Value = "D. Colinares";
                //END

                //Borders
                Sheet.Cells["F2:H2"].Style.Border.Top.Style    = ExcelBorderStyle.Thin;
                Sheet.Cells["F3:H3"].Style.Border.Top.Style    = ExcelBorderStyle.Thin;
                Sheet.Cells["F5:H5"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                Sheet.Cells["F6:H6"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                Sheet.Cells["F2:F6"].Style.Border.Left.Style   = ExcelBorderStyle.Thin;
                Sheet.Cells["F2:F6"].Style.Border.Right.Style  = ExcelBorderStyle.Thin;
                Sheet.Cells["H2:H6"].Style.Border.Right.Style  = ExcelBorderStyle.Thin;
                Sheet.Cells["H2:H6"].Style.Border.Left.Style   = ExcelBorderStyle.Thin;
                Sheet.Cells["A9:D9"].Style.Border.Top.Style    = ExcelBorderStyle.Thin;
                Sheet.Cells["A10:D10"].Style.Border.Bottom.Style   = ExcelBorderStyle.Thin;
                Sheet.Cells["A8:D8"].Style.Border.Top.Style    = ExcelBorderStyle.Thin;
                Sheet.Cells["A8:D8"].Style.Border.Left.Style   = ExcelBorderStyle.Thin;
                Sheet.Cells["D8:D8"].Style.Border.Right.Style  = ExcelBorderStyle.Thin;
                Sheet.Cells["D9:D10"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                Sheet.Cells["D9:D10"].Style.Border.Left.Style  = ExcelBorderStyle.Thin;
                Sheet.Cells["B9:B10"].Style.Border.Left.Style  = ExcelBorderStyle.Thin;
                Sheet.Cells["B9:B10"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                Sheet.Cells["C9:C10"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                // END

                //Table header
                Sheet.Cells["A9:Q9"].Style.Font.Size = 12;
                
                Sheet.Cells["A9:A9"].Value  = "Computer Name";
                Sheet.Cells["A9:A10"].Merge = true;
                Sheet.Cells["A9:A10"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                Sheet.Cells["A9:A10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                Sheet.Cells["B9:B9"].Value  = "Equipment";
                Sheet.Cells["B9:B10"].Merge = true;
                Sheet.Cells["B9:B10"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                Sheet.Cells["B9:B10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                Sheet.Cells["C9:C9"].Value  = "Dept";
                Sheet.Cells["C9:C10"].Merge = true;
                Sheet.Cells["C9:C10"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                Sheet.Cells["C9:C10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                Sheet.Cells["D9:D9"].Value = "In-Charge";
                Sheet.Cells["D9:D10"].Merge = true;
                Sheet.Cells["D9:D10"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                Sheet.Cells["D9:D10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

               
                Sheet.Cells["E10:F10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                Sheet.Cells["G10:H10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                Sheet.Cells["K10:L10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                Sheet.Cells["A8:D8"].Merge = true;
                Sheet.Cells["A8:D8"].Value = "Preventive Maintenance Plan For Insert year";
                Sheet.Cells["A8:D8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                int row = 11;
                var Preventive = RetrievePlanDetails(PlanMonth);

                foreach (var item in Preventive)
                {
                    Sheet.Cells[string.Format("A{0}", row)].Value = item.ComputerName;
                    Sheet.Cells[string.Format("B{0}", row)].Value = item.Equipment;
                    Sheet.Cells[string.Format("C{0}", row)].Value = item.Department;
                    Sheet.Cells[string.Format("D{0}", row)].Value = item.Incharge;
                    
                    //border
                    Sheet.Cells[string.Format("A{0}:D{0}", row)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    Sheet.Cells[string.Format("A{0}:D{0}", row)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    Sheet.Cells[string.Format("A{0}", row)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    Sheet.Cells[string.Format("B{0}", row)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    Sheet.Cells[string.Format("C{0}", row)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    Sheet.Cells[string.Format("D{0}", row)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    row++;
                }

                Sheet.Cells["A:AZ"].AutoFitColumns();                
                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + filename);
                Response.BinaryWrite(package.GetAsByteArray());
                Response.End();

                //END
            }

                return View();

        }
    }
}