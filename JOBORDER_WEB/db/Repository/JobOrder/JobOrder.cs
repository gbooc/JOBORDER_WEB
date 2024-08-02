using db.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.Repository.JobOrder
{
    public class JobOrder : IJoborder, IDisposable
    {
        private JOEntities _jo;

        public JobOrder(JOEntities jo)
        {
            this._jo = jo;
        }
        #region Job Order
        public IEnumerable<tbl_joborder> GetAllJobOrder()
        {
            return _jo.tbl_joborder.ToList();
        }

        public tbl_joborder GetJobOrderById(int jo_id)
        {
            return _jo.tbl_joborder.Find(jo_id);
        }

        public void InsertJobOrder(tbl_joborder jo)
        {
            _jo.tbl_joborder.Add(jo);
        }

        public void DeleteJobOrder(int jo_id)
        {
            tbl_joborder result = _jo.tbl_joborder.Find(jo_id);
            _jo.tbl_joborder.Remove(result);
        }

        public void UpdateJobOrder(tbl_joborder jo)
        {
            _jo.Entry(jo).State = EntityState.Modified;
        }

        #endregion

        #region Job Order Details

        public IEnumerable<tbl_joborder_details> GetAllJobOrderDetails()
        {
            return _jo.tbl_joborder_details.ToList();
        }

        public tbl_joborder_details GetJobOrderDetailsById(int jo_id)
        {
            return _jo.tbl_joborder_details.Find(jo_id);
        }

        public void InsertJobOrderDetails(tbl_joborder_details jo)
        {
            _jo.tbl_joborder_details.Add(jo);
        }

        public void DeleteJobOrderDetails(int jo_id)
        {
            tbl_joborder_details result = _jo.tbl_joborder_details.Find(jo_id);
            _jo.tbl_joborder_details.Remove(result);
        }

        public void UpdateJobOrderDetails(tbl_joborder_details jo)
        {
            _jo.Entry(jo).State = EntityState.Modified;
        }

        public IEnumerable<tbl_sub_assignees> GetAllAssigneess()
        {
            return _jo.tbl_sub_assignees.ToList();
        }

        public tbl_sub_assignees GetAssignessByID(int jo_id)
        {
            return _jo.tbl_sub_assignees.Find(jo_id);
        }

        public void InsertAssignees(tbl_sub_assignees jo)
        {
            _jo.tbl_sub_assignees.Add(jo);
        }

        public void DeleteAssignees(int jo_id)
        {
            tbl_sub_assignees result = _jo.tbl_sub_assignees.Find(jo_id);
            _jo.tbl_sub_assignees.Remove(result);
        }

        public void UpdateAssignees(tbl_sub_assignees jo)
        {
            _jo.Entry(jo).State = EntityState.Modified;
        }

        public IEnumerable<sp_all_assignee_Result> SP_GetAllAssignee(int ID)
        {
            return _jo.sp_all_assignee(ID).ToList();
        }

        #endregion

        #region Joborder Category
        public IEnumerable<tbl_joborder_category> GetAllCategories()
        {
            return _jo.tbl_joborder_category.ToList();
        }

        public tbl_joborder_category GetCategoryById(int jo_id)
        {
            return _jo.tbl_joborder_category.Find(jo_id);
        }

        public void InsertCategory(tbl_joborder_category jo)
        {
            _jo.tbl_joborder_category.Add(jo);
        }

        public void DeleteCategory(int jo_id)
        {
            tbl_joborder_category result = _jo.tbl_joborder_category.Find(jo_id);
            _jo.tbl_joborder_category.Remove(result);
        }

        public void UpdateCategory(tbl_joborder_category jo)
        {
            _jo.Entry(jo).State = EntityState.Modified;
        }
        #endregion

        #region Joborder Category Type
        public IEnumerable<tbl_category_type> GetAllCategoriesType()
        {
            return _jo.tbl_category_type.ToList();
        }

        public tbl_category_type GetCategoryTypeById(int jo_id)
        {
            return _jo.tbl_category_type.Find(jo_id);
        }

        public void InsertCategoryType(tbl_category_type jo)
        {
            _jo.tbl_category_type.Add(jo);
        }

        public void DeleteCategoryType(int jo_id)
        {
            tbl_category_type result = _jo.tbl_category_type.Find(jo_id);
            _jo.tbl_category_type.Remove(result);
        }

        public void UpdateCategoryType(tbl_category_type jo)
        {
            _jo.Entry(jo).State = EntityState.Modified;
        }


        #endregion

        #region My JobOrder
        public IEnumerable<sp_get_my_jo_Result> GetMyJobOrder(string Param)
        {
            return _jo.sp_get_my_jo(Param).ToList();
        }
        public IEnumerable<sp_get_myjo_summary_hw_Result> GetMyJOSummaryHW(string Person)
        {
            return _jo.sp_get_myjo_summary_hw(Person).ToList();
        }
        public IEnumerable<sp_get_myjo_graph_hw_Result> GetGraphSummaryHW(string Person)
        {
            return _jo.sp_get_myjo_graph_hw(Person).ToList();
        }
        public IEnumerable<sp_get_myjo_summary_sw_Result> GetMyJOSummarySW(string Person)
        {
            return _jo.sp_get_myjo_summary_sw(Person).ToList();
        }
        #endregion

        #region Export to excel

        public IEnumerable<sp_export_excel_Result> ExportJOToExcel(DateTime DateFrom, DateTime DateTo,int FlgExport)
        {
            return _jo.sp_export_excel(DateFrom,DateTo,FlgExport).ToList();
        }


        #endregion

        #region Common
        public void Save()
        {
            _jo.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _jo.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Allocated time
        public IEnumerable<tbl_allocated_time> GetAllocatedTime()
        {
            return _jo.tbl_allocated_time.ToList();
        }

        public tbl_allocated_time GetAllocatedTimeById(int jo_id)
        {
            return _jo.tbl_allocated_time.Find(jo_id);
        }

        public void InsertAllocatedTime(tbl_allocated_time jo)
        {
            _jo.tbl_allocated_time.Add(jo);
        }

        public void DeleteAllocatedTimeBy(int jo_id)
        {
            tbl_allocated_time result = _jo.tbl_allocated_time.Find(jo_id);
            _jo.tbl_allocated_time.Remove(result);
        }

        public void UpdateAllocatedTimeBy(tbl_allocated_time jo)
        {
            _jo.Entry(jo).State = EntityState.Modified;
        }

        #endregion

        #region Cost Center

        #region SBU COST CENTER
        public IEnumerable<tbl_sbu_costcenter> GetAllCostCenter()
        {
            return _jo.tbl_sbu_costcenter.ToList();
        }

        public tbl_sbu_costcenter GetCostCenterById(int jo_id)
        {
            return _jo.tbl_sbu_costcenter.Find(jo_id);
        }

        public void InsertCostCenter(tbl_sbu_costcenter jo)
        {
            _jo.tbl_sbu_costcenter.Add(jo);
        }

        public void DeleteCostCenter(int jo_id)
        {
            tbl_sbu_costcenter result = _jo.tbl_sbu_costcenter.Find(jo_id);
            _jo.tbl_sbu_costcenter.Remove(result);
        }

        public void UpdateCostCenter(tbl_sbu_costcenter jo)
        {
            _jo.Entry(jo).State = EntityState.Modified;
        }
        #endregion

        #region CC CATEGORY
        public IEnumerable<tbl_cc_category> GetAllCCCategory()
        {
            return _jo.tbl_cc_category.ToList();
        }

        public tbl_cc_category GetCCCategoryById(int jo_id)
        {
            return _jo.tbl_cc_category.Find(jo_id);
        }

        public void InsertCCCategory(tbl_cc_category jo)
        {
            _jo.tbl_cc_category.Add(jo);
        }

        public void DeleteCCCategory(int jo_id)
        {
            tbl_cc_category result = _jo.tbl_cc_category.Find(jo_id);
            _jo.tbl_cc_category.Remove(result);
        }

        public void UpdateCCCategory(tbl_cc_category jo)
        {
            _jo.Entry(jo).State = EntityState.Modified;
        }

        #endregion

        #region SBU
        public IEnumerable<tbl_sbu> GetAllSBU()
        {
            return _jo.tbl_sbu.ToList();
        }

        public tbl_sbu GetSBUById(int jo_id)
        {
            return _jo.tbl_sbu.Find(jo_id);
        }

        public void InsertSBU(tbl_sbu jo)
        {
            _jo.tbl_sbu.Add(jo);
        }

        public void DeleteSBU(int jo_id)
        {
            tbl_sbu result = _jo.tbl_sbu.Find(jo_id);
            _jo.tbl_sbu.Remove(result);
        }

        public void UpdateSBU(tbl_sbu jo)
        {
            _jo.Entry(jo).State = EntityState.Modified;
        }

        #endregion

        #region SBU DIVISION
        public IEnumerable<tbl_sbu_division> GetAllSBUDivision()
        {
            return _jo.tbl_sbu_division.ToList();
        }

        public tbl_sbu_division GetSBUDivisionById(int jo_id)
        {
            return _jo.tbl_sbu_division.Find(jo_id);
        }

        public void InsertSBUDivision(tbl_sbu_division jo)
        {
            _jo.tbl_sbu_division.Add(jo);
        }

        public void DeleteSBUDivision(int jo_id)
        {
            tbl_sbu_division result = _jo.tbl_sbu_division.Find(jo_id);
            _jo.tbl_sbu_division.Remove(result);
        }

        public void UpdateSBUDivision(tbl_sbu_division jo)
        {
            _jo.Entry(jo).State = EntityState.Modified;
        }
        #endregion
        #region SBU CC Details
        public IEnumerable<tbl_jo_cc_details> GetAllDivisionDetails()
        {
            return _jo.tbl_jo_cc_details.ToList();
        }

        public tbl_jo_cc_details GetDivisionDetailsById(int jo_id)
        {
            return _jo.tbl_jo_cc_details.Find(jo_id);
        }

        public void InsertDivisionDetails(tbl_jo_cc_details jo)
        {
            _jo.tbl_jo_cc_details.Add(jo);
        }

        public void DeleteDivisionDetails(int jo_id)
        {
            tbl_jo_cc_details result = _jo.tbl_jo_cc_details.Find(jo_id);
            _jo.tbl_jo_cc_details.Remove(result);
        }

        public void UpdateDivisionDetails(tbl_jo_cc_details jo)
        {
            _jo.Entry(jo).State = EntityState.Modified;
        }
        #endregion

        #region tbl_managers
        public IEnumerable<tbl_managers_email> GetAllManagers()
        {
            return _jo.tbl_managers_email.ToList();
        }

        public tbl_managers_email GetManagersById(int jo_id)
        {
            return _jo.tbl_managers_email.Find(jo_id);
        }

        public void InsertManagers(tbl_managers_email jo)
        {
            _jo.tbl_managers_email.Add(jo);
        }

        public void DeleteManagers(int jo_id)
        {
            tbl_managers_email result = _jo.tbl_managers_email.Find(jo_id);
            _jo.tbl_managers_email.Remove(result);
        }

        public void UpdateManagers(tbl_managers_email jo)
        {
            _jo.Entry(jo).State = EntityState.Modified;
        }
        #endregion

        #region tbl_jo_cc_details
        public IEnumerable<tbl_costcenter_meaning> GetCCMeaning()
        {
            return _jo.tbl_costcenter_meaning.ToList();
        }

        public tbl_costcenter_meaning GetCCMeaningById(int jo_id)
        {
            return _jo.tbl_costcenter_meaning.Find(jo_id);
        }

        public void InsertCCMeaning(tbl_costcenter_meaning jo)
        {
            _jo.tbl_costcenter_meaning.Add(jo);
        }

        public void DeleteCCMeaning(int jo_id)
        {
            tbl_costcenter_meaning result = _jo.tbl_costcenter_meaning.Find(jo_id);
            _jo.tbl_costcenter_meaning.Remove(result);
        }

        public void UpdateCCMeaning(tbl_costcenter_meaning jo)
        {
            _jo.Entry(jo).State = EntityState.Modified;
        }
        #endregion

        public IEnumerable<sp_get_sbu_description_Result> SP_GetCostCenter(int ID)
        {
            return _jo.sp_get_sbu_description(ID).ToList();
        }

        public IEnumerable<sp_assignee_ongoing_jo_Result> SP_GetOngoingJOAssignee(string Assignee)
        {
            return _jo.sp_assignee_ongoing_jo(Assignee).ToList();
        }
        #endregion

        #region JO Availability

        public IEnumerable<tbl_jo_creation_availability> JOAvailability()
        {
            return _jo.tbl_jo_creation_availability.ToList();
        }

        public tbl_jo_creation_availability JOAvailabilityById(int jo_id)
        {
            return _jo.tbl_jo_creation_availability.Find(jo_id);
        }

        public void InsertJOAvailability(tbl_jo_creation_availability jo)
        {
            _jo.tbl_jo_creation_availability.Add(jo);
        }

        public void DeleteJOAvailability(int jo_id)
        {
            tbl_jo_creation_availability result = _jo.tbl_jo_creation_availability.Find(jo_id);
            _jo.tbl_jo_creation_availability.Remove(result);
        }

        public void UpdateJOAvailability(tbl_jo_creation_availability jo)
        {
            _jo.Entry(jo).State = EntityState.Modified;
        }

        #endregion

        #region JO Problems

        public IEnumerable<tbl_problems> JOProblems()
        {
            return _jo.tbl_problems.ToList();
        }

        public tbl_problems JOProblemsById(int jo_id)
        {
            return _jo.tbl_problems.Find(jo_id);
        }

        public void InsertJOProblems(tbl_problems jo)
        {
            _jo.tbl_problems.Add(jo);
        }

        public void DeleteJOProblems(int jo_id)
        {
            tbl_problems result = _jo.tbl_problems.Find(jo_id);
            _jo.tbl_problems.Remove(result);
        }

        public void UpdateJOProblems(tbl_problems jo)
        {
            _jo.Entry(jo).State = EntityState.Modified;
        }
        public IEnumerable<sp_sw_jo_issues_Result> SP_sw_jo_issues(int DetailsID)
        {
            return _jo.sp_sw_jo_issues(DetailsID).ToList();
        }
        public IEnumerable<sp_jo_sw_stages_and_issues_Result> SP_SW_Stages_And_Issues()
        {
            return _jo.sp_jo_sw_stages_and_issues().ToList();
        }
        #endregion

    }
}
