using db.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.Repository.GanttChart
{
    public interface IGanttChart : IDisposable
    {
        IEnumerable<tbl_gantt_chart> GetAllGanttChart();
        tbl_gantt_chart GetGanttChartsByID(int gant_chart_id);
        void InsertGanttChart(tbl_gantt_chart gant_chart);
        void DeleteGanttChart(int gant_chart_id);
        void UpdateGanttChart(tbl_gantt_chart gant_chart);

        IEnumerable<sp_get_all_actual_standard_time_Result> GetSWActualStandard(string param1,string param2);
        void Save();
    }
}
