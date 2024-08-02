using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_RIMP.Models
{
    public class CreateJobOrderModel
    {
        //Job Order Information
        public string Sw_CodeNumber { get; set; }
        public string Sw_Application { get; set; }
        public string Sw_UserDepartment { get; set; }
        public string Sw_Programmer { get; set; }
        public string ManagerID { get; set; }
        public string Manager { get; set; }

        //List of unassigned jo
        public  string JobOrder { get; set; }
        public string Category { get; set; }
        public string Requestor { get; set; }
        public DateTime? DateCreated { get; set; }
        public string Status { get; set; }

        //joborder counts
        public int? UnassignedCount { get; set; }
        public int? AssignedCount { get; set; }
        public int? HardwareJoCount { get; set; }
        public int? SoftwareJoCount { get; set; }

        //SBU
        public string SBU { get; set; }
        public string CostCenter { get; set; }
        public string CostCenterSections { get; set; }
    }

    public class AllHardwareJOModel
    {
        public int ID { get; set; }
        public string CodeNumber { get; set; }
        public string JoNumber { get; set; }
        public string Details { get; set; }
        public string Assignee { get; set; }
        public string Status { get; set; }
        public int? StandardTime { get; set; }
        public string Type { get; set; }
        public string Datefiled { get; set; }
        public string DateTarget { get; set; }
    }

    public class AllSoftwareJOModel
    {
        public int ID { get; set; }
        public string JoNumber { get; set; }
        public string CodeNumber { get; set; }
        public string Status { get; set; }
        public string Assignee { get; set; }
        public string AppName { get; set; }
    }

    public class JobOrderDetailsModel
    {
        public int JoID { get; set; }
        public int? JODetailsID { get; set; }
        public string JobNumber { get; set; }
        public string CodeNumber { get; set; }
        public string WorkDetails { get; set; }
        public string WorkPurpose { get; set; }
        public string Assignee { get; set; }
        public string Datefiled { get; set; }
        public string Department { get; set; }
        public int? StandardTime { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public string Diagnosis { get; set; }
        public string ActionTaken { get; set; }
        public int? ProgressRate { get; set; }
        public string Remarks { get; set; }
        public string JoType { get; set; }
        public string JoTypeDetails { get; set; }
        public string Requestor { get; set; }
        public string DateTarget { get; set; }

        public string NotedBy { get; set; }
        public string ApprovedBy { get; set; }
        public string DateStarted { get; set; }
        public string DateEnded { get; set; }
        public int? ActualTime { get; set; }
        public string Scope { get; set; }

        public string SystemType { get; set; }
        public string RequestorLocal { get; set; }
        public string JO_Costcenter { get; set; }
        public DateTime? DateConfirmed { get; set; }
        public string DateAssigned { get; set; }
        public string ReasonOfDelay { get; set; }
        public int IsDue { get; set; }
        public string BarcodePath { get; set; }
        public string APCLocalNumber { get; set; }
        public int? JOHandled { get; set; }
        public string DeptID { get; set; }
        public string ProgrammerPic { get; set; }
        public int? NumOfDays { get; set; }
        public int TypeID { get; set; }
        public string RequirementsMeetings { get; set; }
    }

    public class JoCategories
    {
        public int CategoryID { get; set; }
        public string Type { get; set; }
        public string Details { get; set; }
        public int? StandardTime { get; set; }
        public string CodeNumber { get; set; }
        public int DetailsID { get; set; }
    }

    public class StatusModel
    {
        public int StatusId { get; set; }
        public string Status { get; set; }
    }
    public class GanttChartModel
    {
        public string WorkPackage { get; set; }
        public string Branches { get; set; }
        public string Function { get; set; }
        public string DateStarted { get; set; }
        public string DateEnded { get; set; }

    }

    public class UnassignedJOModel
    {
        public string ApcName { get; set; }
        public string JobNo { get; set; }
        public string Details { get; set; }
        public string Department { get; set; }
        public string Requestor { get; set; }
        public string Assignee { get; set; }
        public string AssigneeID { get; set; }
        public string Datetarget { get; set; }
        public int? JOCount { get; set; }
        public string DateFiled { get; set; }
        public string JoProgress { get; set; }
        public string DateProgress { get; set; }
        public int? IsDue { get; set; }
        public string ReasonOfDelay { get; set; }
        public string DateTarget { get; set; }
        public string Message { get; set; }
        public int ID { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
    }

    public class JOProgressModel
    {
        public int? ID { get; set; }
        public string Date { get; set; }
        public string Day { get; set; }
        public int? Mins { get; set; }
        public string Description { get; set; }
    }

    public class JOByDepartment
    {
        public string DeptName { get; set; }
        public string Count { get; set; }
        public string DelayedJo { get; set; }
    }
    public class CanteenTransaction
    {
        public string Date { get; set; }
        public string Amount { get; set; }
        public string EmpID { get; set; }
    }

    public class DailyReportModel
    {
        public int ID { get; set; }
        public string JobNo { get; set; }
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        public int Mins { get; set; }
        public string Activity { get; set; }
        public string CostCenter { get; set; }
        public string Company { get; set; }
        public string Date { get; set; }
    }

    public class KPIJobOrder
    {
        public string ClosedJOMonth { get; set; }
        public int? ClosedJOCount { get; set; }
        public string AssignedJOMonth { get; set; }
        public int? AssignedJOCount { get; set; }
        public string AllJOYear { get; set; }
        public string AllJOMonth { get; set; }
        public int? OnTime { get; set; }
        public int? Delayed { get; set; }
        public string Deffered { get; set; }

        public int? CloseEmail { get; set; }
        public int? CloseTel { get; set; }
        public int? ClosedDesktop { get; set; }
        public int? ClosedNetwork { get; set; }
        public int? CloseServer { get; set; }
        public int? CloseOthers { get; set; }
        public int? CloseSoftware { get; set; }
                  
        public int? AssignedEmail { get; set; }
        public int? AssignedTel { get; set; }
        public int? AssignedDesktop { get; set; }
        public int? AssignedNetwork { get; set; }
        public int? AssignedServer { get; set; }
        public int? AssignedOthers { get; set; }
        public int? AssignedSoftware { get; set; }

        public int? Year { get; set; }
        public int? YearCount { get; set; }
        public int? Ongoing { get; set; }
        public int? Unassigned { get; set; }

        public int? AvgActualTime { get; set; }
        public int? AvgStandardTime { get; set; }
        public int? TotalActualTime { get; set; }
        public int? TotalStandardTime { get; set; }
        public string GoalPercentage { get; set; }

        public string Satisfied5 { get; set; }
        public string Satisfied4 { get; set; }
        public string Neutral { get; set; }
        public string Dissatisfied2 { get; set; }
        public string Dissatisfied1 { get; set; }

        public string APC { get; set; }
        public string EmpID { get; set; }
        public string TotalJO { get; set; }
        public string HW { get; set; }
        public string SW { get; set; }

        public int? JOCount { get; set; }
        public int? JOActualTime { get; set; }
        public int? ReportCount { get; set; }
        public int? ReportActualTime { get; set; }
    }

    public class MyJODeptModel
    {
        public string JONumber { get; set; }
        public string Details { get; set; }
        public string Requestor { get; set; }
        public string Assignee { get; set; }
        public string Status { get; set; }
        public int? ProgressRate { get; set; }
        public string DateTarget { get; set; }
        public string DateAssigned { get; set; }
        public int? DetailsID { get; set; }
    }
    public class TimeAllocation
    {
        public int DetailsID { get; set; }
        public string Date { get; set; }
        public string Day { get; set; }
        public string TimeRendered { get; set; }
        public string Remarks { get; set; }
    }

}