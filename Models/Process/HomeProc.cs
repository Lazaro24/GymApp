using GymApp.Controllers;
using GymApp.Models.Entities;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GymApp.Models.Process
{
    public class HomeProc
    {
        Log _log = new Log();
        GlobalDB db = null;
        OracleDataReader dr = null;

        private OracleConnection conn { get; set; }

        public List<HomeEntity> CargarDashboard()
        {
            db = new GlobalDB();
            String Query = "";
            var Homes = new List<HomeEntity>();

            try
            {
                conn = db.Conectar();

                if(conn != null)
                {
                    Query = "SELECT " +
                            "   (SELECT count(*) FROM TBL_CONTROL_PAGOS WHERE ID_ESTADO_PAGO = 2), " +
                            "   (SELECT count(*) FROM TBL_CONTROL_PAGOS WHERE ID_ESTADO_PAGO = 1), " +
                            "   (SELECT count(*) FROM TBL_CLIENTE)" +                            
                            "FROM DUAL";
                }

                dr = db.DataReader(Query, conn);
                _log.AddToLog("CargandoDashboard", "Generando listado de operaciones [" + Query + "]");
                while (dr.Read())
                {
                    var home = new HomeEntity
                    {
                        Pagos = dr.IsDBNull(0) ? 0 : dr.GetInt32(0),
                        PagosPendientes = dr.IsDBNull(0) ? 0 : dr.GetInt32(1),
                        Clientes = dr.IsDBNull(0) ? 0 : dr.GetInt32(2)
                    };
                    Homes.Add(home);
                }
                db.ReaderClose(dr);
                db.Desconectar(conn);
            }
            catch (Exception ex)
            {
                Homes = null;
                _log.AddToLog("CargarClientes", "Error al obtener clientes " + ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }

            return Homes;
        }
    }
}