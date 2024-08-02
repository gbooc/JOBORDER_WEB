using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_RIMP.Models
{
    public class DowntimeModel
    {
        public int ID { get; set; }
        public string CodeNumber { get; set; }
        public string DowntimeName { get; set; }
        public string DateStarted { get; set; }
        public string TimeStarted { get; set; }
        public string DateEnded { get; set; }
        public string TimeEnded { get; set; }
        public int DowntimeCauseID { get; set; }
        public string DowntimeCause { get; set; }
        public string DowntimeYearAndMonth { get; set; }
        public int? DowntimeMinutes { get; set; }
        public string Month { get; set; }
        public string Location { get; set; }
        public string DowntimeCauseDetails { get; set; }
        public string JONumber { get; set; }
        public int? ActualTime { get; set; }
        public int? StandardTime { get; set; }
        public int? MinYear { get; set; }
        public int? MaxYear { get; set; }
        public string JobOrderDetails { get; set; }
        public string Assignee { get; set; }
        public string Achieved { get; set; }
        public string Unachieved { get; set; }
        public string DateTarget { get; set; }
    }
}