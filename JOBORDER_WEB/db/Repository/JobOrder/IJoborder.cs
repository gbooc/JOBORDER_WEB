using db.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.Repository.JobOrder
{
    public interface IJoborder : IDisposable
    {
        #region Jobroder Request
        IEnumerable<tbl_joborder> GetAllJobOrder();
        tbl_joborder GetJobOrderById(int jo_id);
        void InsertJobOrder(tbl_joborder jo);
        void DeleteJobOrder(int jo_id);
        void UpdateJobOrder(tbl_joborder jo);
        // END
        #endregion

        #region JobOrder Details
        IEnumerable<tbl_joborder_details> GetAllJobOrderDetails();
        tbl_joborder_details GetJobOrderDetailsById(int jo_id);
        void InsertJobOrderDetails(tbl_joborder_details jo);
        void DeleteJobOrderDetails(int jo_id);
        void UpdateJobOrderDetails(tbl_joborder_details jo);

        IEnumerable<tbl_sub_assignees> GetAllAssigneess();
        tbl_sub_assignees GetAssignessByID(int jo_id);
        void InsertAssignees(tbl_sub_assignees jo);
        void DeleteAssignees(int jo_id);
        void UpdateAssignees(tbl_sub_assignees jo);
        IEnumerable<sp_all_assignee_Result> SP_GetAllAssignee(int Assignee);
  
        #endregion

        #region Joborder type
        IEnumerable<tbl_joborder_category> GetAllCategories();
        tbl_joborder_category GetCategoryById(int category);
        void InsertCategory(tbl_joborder_category category);
        void DeleteCategory(int categroy);
        void UpdateCategory(tbl_joborder_category jo);
        #endregion

        #region Joborder category type
        IEnumerable<tbl_category_type> GetAllCategoriesType();
        tbl_category_type GetCategoryTypeById(int category);
        void InsertCategoryType(tbl_category_type category);
        void DeleteCategoryType(int categroy);
        void UpdateCategoryType(tbl_category_type jo);
        #endregion

        #region My Joborder
        IEnumerable<sp_get_my_jo_Result> GetMyJobOrder(string param);
        IEnumerable<sp_get_myjo_summary_hw_Result> GetMyJOSummaryHW(string Person);
        IEnumerable<sp_get_myjo_graph_hw_Result> GetGraphSummaryHW(string Person);
        IEnumerable<sp_get_myjo_summary_sw_Result> GetMyJOSummarySW(string Person);
        #endregion

        #region export to excel
        IEnumerable<sp_export_excel_Result> ExportJOToExcel(DateTime DateFrom, DateTime DateTo, int FlgExport);
        #endregion

        #region Time Allocation
        IEnumerable<tbl_allocated_time> GetAllocatedTime();
        tbl_allocated_time GetAllocatedTimeById(int jo_id);
        void InsertAllocatedTime(tbl_allocated_time jo);
        void DeleteAllocatedTimeBy(int jo_id);
        void UpdateAllocatedTimeBy(tbl_allocated_time jo);
        #endregion

        #region Job Order Cost Centers

        #region tbl_sbu_costcenter
        IEnumerable<tbl_sbu_costcenter> GetAllCostCenter();
        tbl_sbu_costcenter GetCostCenterById(int jo_id);
        void InsertCostCenter(tbl_sbu_costcenter jo);
        void DeleteCostCenter(int jo_id);
        void UpdateCostCenter(tbl_sbu_costcenter jo);
        #endregion

        #region tbl_cc_category
        IEnumerable<tbl_cc_category> GetAllCCCategory();
        tbl_cc_category GetCCCategoryById(int jo_id);
        void InsertCCCategory(tbl_cc_category jo);
        void DeleteCCCategory(int jo_id);
        void UpdateCCCategory(tbl_cc_category jo);
        #endregion

        #region tbl_sbu
        IEnumerable<tbl_sbu> GetAllSBU();
        tbl_sbu GetSBUById(int jo_id);
        void InsertSBU(tbl_sbu jo);
        void DeleteSBU(int jo_id);
        void UpdateSBU(tbl_sbu jo);
        #endregion

        #region tbl_sbu_division
        IEnumerable<tbl_sbu_division> GetAllSBUDivision();
        tbl_sbu_division GetSBUDivisionById(int jo_id);
        void InsertSBUDivision(tbl_sbu_division jo);
        void DeleteSBUDivision(int jo_id);
        void UpdateSBUDivision(tbl_sbu_division jo);
        #endregion

        #region tbl_jo_cc_details
        IEnumerable<tbl_jo_cc_details> GetAllDivisionDetails();
        tbl_jo_cc_details GetDivisionDetailsById(int jo_id);
        void InsertDivisionDetails(tbl_jo_cc_details jo);
        void DeleteDivisionDetails(int jo_id);
        void UpdateDivisionDetails(tbl_jo_cc_details jo);
        #endregion

        #region tbl_costcenter_meaning
        IEnumerable<tbl_costcenter_meaning> GetCCMeaning();
        tbl_costcenter_meaning GetCCMeaningById(int jo_id);
        void InsertCCMeaning(tbl_costcenter_meaning jo);
        void DeleteCCMeaning(int jo_id);
        void UpdateCCMeaning(tbl_costcenter_meaning jo);
        #endregion

        #region tbl_managers_email
        IEnumerable<tbl_managers_email> GetAllManagers();
        tbl_managers_email GetManagersById(int jo_id);
        void InsertManagers(tbl_managers_email jo);
        void DeleteManagers(int jo_id);
        void UpdateManagers(tbl_managers_email jo);
        #endregion

        IEnumerable<sp_get_sbu_description_Result> SP_GetCostCenter(int ID);
        IEnumerable<sp_assignee_ongoing_jo_Result> SP_GetOngoingJOAssignee(string Assignee);
        #endregion

        #region JO Availability

        IEnumerable<tbl_jo_creation_availability> JOAvailability();
        tbl_jo_creation_availability JOAvailabilityById(int jo_id);
        void InsertJOAvailability(tbl_jo_creation_availability jo);
        void DeleteJOAvailability(int jo_id);
        void UpdateJOAvailability(tbl_jo_creation_availability jo);

        #endregion

        #region Job Order Problems/Issues

        IEnumerable<tbl_problems> JOProblems();
        tbl_problems JOProblemsById(int jo_id);
        void InsertJOProblems(tbl_problems jo);
        void DeleteJOProblems(int jo_id);
        void UpdateJOProblems(tbl_problems jo);
        IEnumerable<sp_sw_jo_issues_Result> SP_sw_jo_issues(int DetailsID);
        IEnumerable<sp_jo_sw_stages_and_issues_Result> SP_SW_Stages_And_Issues();

        #endregion
        void Save();
    }
}
