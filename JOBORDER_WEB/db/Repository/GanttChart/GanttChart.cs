using db.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.Repository.GanttChart
{
    public class GanttChart : IGanttChart,IDisposable
    {
        private JOEntities _gant_chart;

        public GanttChart(JOEntities jo)
        {
            this._gant_chart = jo;
        }
        public IEnumerable<tbl_gantt_chart> GetAllGanttChart()
        {
            return _gant_chart.tbl_gantt_chart.ToList();
        }
        public IEnumerable<sp_get_all_actual_standard_time_Result> GetSWActualStandard(string param1, string param2)
        {
            return _gant_chart.sp_get_all_actual_standard_time(param1, param2).ToList();
        }

        public tbl_gantt_chart GetGanttChartsByID(int gant_chart_id)
        {
            return _gant_chart.tbl_gantt_chart.Find(gant_chart_id);
        }

        public void InsertGanttChart(tbl_gantt_chart gant_chart)
        {
            _gant_chart.tbl_gantt_chart.Add(gant_chart);
        }

        public void UpdateGanttChart(tbl_gantt_chart gant_chart)
        {
            _gant_chart.Entry(gant_chart).State = EntityState.Modified;
        }

        public void DeleteGanttChart(int gant_chart_id)
        {
            tbl_gantt_chart gant_chart = _gant_chart.tbl_gantt_chart.Find(gant_chart_id);
            _gant_chart.tbl_gantt_chart.Remove(gant_chart);
        }

        public void Save()
        {
            _gant_chart.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _gant_chart.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
