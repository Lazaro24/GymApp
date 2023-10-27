using GymApp.Models.Entities;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GymApp.Models.AppModels
{
    public class ReportesModel
    {
        Log log = new Log();
        public String vErrOracle { get; set; }

        public List<entPagos> Reporte(string id_cliente, string concepto, string fecha_inicio, string fecha_final, string tipo_reporte)
        {
            GlobalDB db = new GlobalDB();

            String Query = "";
            var ReportePagos = new List<entPagos>();

            try
            {
                Query = "select tcp.CORRELATIVO, tcp.ID_CLIENTE, " +
                    "(tc.NOMBRE || ' ' || tc.NOMBRE2 || ' ' || tc.APELLIDO || ' ' || tc.APELLIDO2 || ' ' || tc.APELLIDO_CASADA) as nombre_completo, " +
                    "    tc.DPI, tc.NIT, tc.TELEFONO, tcp.FECHA_EMISION, tcp.ID_CONCEPTO, tc2.DESCRIPCION, tc2.MONTO, tcp.FECHA_PAGO, " +
                    "    tcp.USUARIO, tcp.FECHA_ANULACION, tcp.ID_ESTADO_PAGO " +
                    "from TBL_CONTROL_PAGOS tcp " +
                    "inner join TBL_CLIENTE tc on tc.ID_CLIENTE = tcp.ID_CLIENTE " +
                    "inner join TBL_CONCEPTO tc2 on tc2.ID_CONCEPTO = tcp.ID_CONCEPTO " +
                    "where TCP.ID_CONCEPTO = '" + concepto + "' ";

                //// realizar busqueda por cliente
                if (id_cliente.Length > 0)
                {
                    Query += " and tcp.id_cliente = '" + id_cliente + "' ";
                }

                //// Si concepto es inscripciones por defecto toma fecha pago
                if (concepto == "I")
                {
                    Query += " and trunc(tcp.fecha_pago) between to_date('" + fecha_inicio + "', 'dd/MM/yyyy') AND to_date('" + fecha_final + "', 'dd/MM/yyyy')  ";
                }
                else
                {
                    // tipo_reporte 1=Pendientes, 2=Pagados
                    if (tipo_reporte == "1")
                    {
                        Query += " and tcp.id_estado_pago = 1 and trunc(tcp.fecha_emision) between to_date('" + fecha_inicio + "', 'dd/MM/yyyy') AND to_date('" + fecha_final + "', 'dd/MM/yyyy') ";
                    }
                    else
                    {
                        Query += "  and tcp.id_estado_pago = 2 and trunc(tcp.FECHA_PAGO) between to_date('" + fecha_inicio + "', 'dd/MM/yyyy') AND to_date('" + fecha_final + "', 'dd/MM/yyyy') ";
                    }
                }

                string connectionString = db.getOracleConnectionStr();
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    using (OracleCommand command = new OracleCommand(Query, connection))
                    {
                        try
                        {
                            connection.Open();
                            log.AddToLog("ReportePagos", "Generando listado de pagos {" + Query + "}");

                            using (OracleDataReader dr = command.ExecuteReader())
                            {
                                if (dr.HasRows)
                                {
                                    while (dr.Read())
                                    {
                                        var Pago = new entPagos
                                        {
                                            correlativo = dr.IsDBNull(0) ? 0 : dr.GetInt64(0),
                                            id_cliente = dr.IsDBNull(1) ? "" : dr.GetString(1),
                                            nombre_completo = dr.IsDBNull(2) ? "" : dr.GetString(2),
                                            dpi = dr.IsDBNull(3) ? 0 : dr.GetInt64(3),
                                            nit = dr.IsDBNull(4) ? 0 : dr.GetInt64(4),
                                            telefono = dr.IsDBNull(5) ? 0 : dr.GetInt64(5),
                                            fecha_emision = dr.IsDBNull(6) ? "" : dr.GetDateTime(6).ToString("dd/MM/yyyy"),
                                            fecha_pago = dr.IsDBNull(10) ? "" : dr.GetDateTime(10).ToString("dd/MM/yyyy"),
                                            id_concepto = dr.IsDBNull(7) ? "" : dr.GetString(7),
                                            concepto = dr.IsDBNull(8) ? "" : dr.GetString(8),
                                            monto = dr.IsDBNull(9) ? new decimal(0.00) : dr.GetDecimal(9),
                                            id_estado_pago = dr.IsDBNull(13) ? 0 : dr.GetInt64(13)
                                        };
                                        ReportePagos.Add(Pago);
                                    }
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
                log.AddToLog("ReportesPagos", "Error al obtener pagos para el reporte " + ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }

            return ReportePagos;
        }
    }
}