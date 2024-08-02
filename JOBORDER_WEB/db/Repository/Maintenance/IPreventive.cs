using db.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.Repository.Maintenance
{
    public interface IPreventive : IDisposable
    {
        //Preventive maintenance
        IEnumerable<tbl_preventive_maintenance> GetAllPreventives();
        tbl_preventive_maintenance GetPreventiveById(int id);
        void InsertPreventive(tbl_preventive_maintenance preventive);
        void DeletePreventive(int preventive);
        void UpdatePreventive(tbl_preventive_maintenance preventive);

        //preventive pc
        IEnumerable<tbl_preventive_pc> GetAllPreventivesPC();
        tbl_preventive_pc GetPreventivesPCById(int id);
        void InsertPreventivesPC(tbl_preventive_pc preventive);
        void DeletePreventivesPC(int preventive);
        void UpdatePreventivesPC(tbl_preventive_pc preventive);

        //services
        IEnumerable<tbl_preventive_services> GetAllPreventivesServices();
        tbl_preventive_services GetPreventivesServicesById(int id);
        void InsertPreventivesServices(tbl_preventive_services preventive);
        void DeletePreventivesServices(int preventive);
        void UpdatePreventivesServices(tbl_preventive_services preventive);

        //diagnosis
        IEnumerable<tbl_preventive_diagnose> GetAllDiagnoses();
        tbl_preventive_diagnose GetDiagnosesById(int id);
        void InsertDiagnoses(tbl_preventive_diagnose preventive);
        void DeleteDiagnoses(int preventive);
        void UpdateDiagnoses(tbl_preventive_diagnose preventive);

        void Save();

        IEnumerable<sp_get_preventive_maintenance_Result> GetPreventiveSummary();

    }
}
