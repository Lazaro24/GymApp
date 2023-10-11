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
        Log log = new Log();
        
        public List<HomeEntity> CargarDashboard()
        {
            GlobalDB db = new GlobalDB();
            String Query = "";

            var Homes = new List<HomeEntity>();
            try
            {
                Query = "SELECT " +
                        "   (SELECT count(*) FROM TBL_CONTROL_PAGOS WHERE ID_ESTADO_PAGO = 2), " +
                        "   (SELECT count(*) FROM TBL_CONTROL_PAGOS WHERE ID_ESTADO_PAGO = 1), " +
                        "   (SELECT count(*) FROM TBL_CLIENTE)" +                            
                        "FROM DUAL";

                log.AddToLog("CargandoDashboard", "Generando listado de operaciones [" + Query + "]");
                
                string connectionString = db.getOracleConnectionStr();
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    using (OracleCommand command = new OracleCommand(Query, connection))
                    {
                        try
                        {
                            connection.Open();

                            using (OracleDataReader dr = command.ExecuteReader())
                            {
                                log.AddToLog("CargarClientes", "Generando listado de clientes [" + Query + "]");
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
                            }
                        }
                        catch (Exception ex)
                        {
                            log.AddToLog(ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Homes = null;
                log.AddToLog("CargarClientes", "Error al obtener clientes " + ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }

            return Homes;
        }
    }
}