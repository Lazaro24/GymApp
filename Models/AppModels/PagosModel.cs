using GymApp.Models.Entities;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GymApp.Models.AppModels
{
    public class PagosModel
    {
        Log log = new Log();
        public String vErrOracle { get; set; }

        public List<entFormasPago> FormasPago()
        {
            GlobalDB db = new GlobalDB();

            String Query = "";
            var FormasPago = new List<entFormasPago>();

            try
            {
                Query = "SELECT * FROM TBL_FORMA_PAGO WHERE ESTADO = 'A' ORDER BY 1";

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
                                log.AddToLog("FormasPago", "Generando listado de formas de pago {" + Query + "}");
                                while (dr.Read())
                                {
                                    var fp = new entFormasPago
                                    {
                                        id_forma_pago = dr.IsDBNull(0) ? "" : dr.GetString(0),
                                        descripcion = dr.IsDBNull(1) ? "" : dr.GetString(1)
                                    };
                                    FormasPago.Add(fp);
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
                log.AddToLog("FormasPago", "Error al obtener formas de pago " + ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }

            return FormasPago;
        }

        public List<entConcepto> Conceptos()
        {
            GlobalDB db = new GlobalDB();

            String Query = "";
            var Conceptos = new List<entConcepto>();

            try
            {
                Query = "SELECT * FROM TBL_CONCEPTO ORDER BY 1";

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
                                log.AddToLog("Conceptos", "Generando listado de conceptos {" + Query + "}");
                                while (dr.Read())
                                {
                                    var concepto = new entConcepto
                                    {
                                        id_concepto = dr.IsDBNull(0) ? "" : dr.GetString(0),
                                        descripcion = dr.IsDBNull(1) ? "" : dr.GetString(1),
                                        monto = dr.IsDBNull(2) ? new decimal(0.00) : dr.GetDecimal(2)
                                    };
                                    Conceptos.Add(concepto);
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
                log.AddToLog("FormasPago", "Error al obtener formas de pago " + ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }

            return Conceptos;
        }

        public List<entPagos> getCuotas(string id_cliente)
        {
            GlobalDB db = new GlobalDB();            
            var PagosPP = new List<entPagos>();
            String Query = "";

            try
            {
                Query = "select tcp.CORRELATIVO, tcp.ID_CLIENTE, " +
                    "(tc.NOMBRE || ' ' || tc.NOMBRE2 || ' ' || tc.APELLIDO || ' ' || tc.APELLIDO2 || ' ' || tc.APELLIDO_CASADA) as nombre_completo, " +
                    "    tc.DPI, tc.NIT, tc.TELEFONO, tcp.FECHA_EMISION, tcp.ID_CONCEPTO, tc2.DESCRIPCION, tc2.MONTO, tcp.FECHA_PAGO, " +
                    "    tcp.USUARIO, tcp.FECHA_ANULACION, tcp.ID_ESTADO_PAGO " +
                    "from TBL_CONTROL_PAGOS tcp " +
                    "inner join TBL_CLIENTE tc on tc.ID_CLIENTE = tcp.ID_CLIENTE " +
                    "inner join TBL_CONCEPTO tc2 on tc2.ID_CONCEPTO = tcp.ID_CONCEPTO " +
                    "where tcp.ID_ESTADO_PAGO = 1 AND tcp.ID_CLIENTE = :ID_CLIENTE " +
                    "ORDER BY 1 ASC ";

                string connectionString = db.getOracleConnectionStr();
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    using (OracleCommand command = new OracleCommand(Query, connection))
                    {
                        try
                        {
                            command.Parameters.Add("ID_CLIENTE", OracleDbType.Varchar2).Value = id_cliente;

                            connection.Open();
                            log.AddToLog("PagosPendientes", "Generando listado de pagos pendientes {" + Query + "}");
                            using (OracleDataReader dr = command.ExecuteReader())
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
                                        id_concepto = dr.IsDBNull(7) ? "" : dr.GetString(7),
                                        concepto = dr.IsDBNull(8) ? "" : dr.GetString(8),
                                        monto = dr.IsDBNull(9) ? new decimal(0.00) : dr.GetDecimal(9),
                                        id_estado_pago = dr.IsDBNull(13) ? 0 : dr.GetInt64(13)
                                    };
                                    PagosPP.Add(Pago);
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
                log.AddToLog("PagosPendientes", "Error al obtener pagos pendientes " + ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }

            return PagosPP;
        }

        public List<entPagos> getCliente(string tipo_busqueda, string valor)
        {
            GlobalDB db = new GlobalDB();

            String Query = "";
            var Clientes = new List<entPagos>();
            string complemento = "";

            switch (tipo_busqueda)
            {
                case "1":
                    complemento = " AND ID_CLIENTE = UPPER('" + valor + "') ";
                    break;
                case "2":
                    complemento = " AND DPI = " + valor + " ";
                    break;
                case "3":
                    // busqueda por nit
                    complemento = " AND NIT = " + valor + " ";
                    break;
                case "4":
                    // busqueda por telefono
                    complemento = " AND TELEFONO = " + valor + " ";
                    break;
                case "5":
                    // busqueda por nombre
                    complemento = " AND UPPER(NOMBRE||NOMBRE2||APELLIDO||APELLIDO2||APELLIDO_CASADA) LIKE UPPER('%" + valor + "%') ";
                    break;
                default:
                    complemento = "";
                    break;
            }

            try
            {
                Query = " SELECT ID_CLIENTE, " +
                    "(NOMBRE || ' ' || NOMBRE2 || ' ' || APELLIDO || ' ' || APELLIDO2 || ' ' || APELLIDO_CASADA) as nombre_completo, " +
                    "DPI, NIT, TELEFONO " +
                    "FROM TBL_CLIENTE WHERE ESTADO = 'A' " + complemento;

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
                                log.AddToLog("Obteniendo cliente", "extrayendo informacion de cliente {" + Query + "}");
                                while (dr.Read())
                                {
                                    var Cliente = new entPagos
                                    {
                                        id_cliente = dr.IsDBNull(0) ? "" : dr.GetString(0),
                                        nombre_completo = dr.IsDBNull(1) ? "" : dr.GetString(1),
                                        dpi = dr.IsDBNull(2) ? 0 : dr.GetInt64(2),
                                        nit = dr.IsDBNull(3) ? 0 : dr.GetInt64(3),
                                        telefono = dr.IsDBNull(4) ? 0 : dr.GetInt64(4)
                                    };
                                    Clientes.Add(Cliente);
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
                log.AddToLog("PagosPendientes", "Error al obtener pagos pendientes " + ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }

            return Clientes;
        }

        public string getCorrelativo()
        {
            string vResult = "0";

            GlobalDB db = new GlobalDB();
            string Query = "SELECT NVL(MAX(CORRELATIVO), 0) + 1 FROM TBL_CONTROL_PAGOS";

            try
            {
                using(OracleConnection conn = new OracleConnection(db.getOracleConnectionStr()))
                {
                    conn.Open();
                    using(OracleCommand cmd = new OracleCommand(Query, conn))
                    {
                        object result = cmd.ExecuteScalar();
                        vResult = (result != DBNull.Value) ? Convert.ToString(result) : "-1";
                    }
                }
            }catch(Exception e)
            {
                vResult = "-100";
                log.AddToLog("ObtenerCorrelativo", "Ocurrio un error al obtener el correlativo " + e.Message + " " + e.Source + " " + e.StackTrace);
            }

            return vResult;
        }

        public int GuardarPago(string concepto, string id_cliente, string correlativo, string usuario, string id_forma_pago, string monto, string numero_documento, string autorizacion)
        {
            GlobalDB db = new GlobalDB();
            int vResult = 0;
            String Query1 = "";
            string vCorr = correlativo;

            if (concepto == "M")
            {
                Query1 = "UPDATE TBL_CONTROL_PAGOS SET FECHA_PAGO = TRUNC(SYSDATE), USUARIO = :USUARIO, ID_ESTADO_PAGO = 2 WHERE CORRELATIVO = :CORRELATIVO";                
            }
            else
            {
                vCorr = getCorrelativo();
                Query1 = "INSERT INTO TBL_CONTROL_PAGOS(CORRELATIVO, ID_CLIENTE, FECHA_EMISION, ID_CONCEPTO, FECHA_PAGO, USUARIO, ID_ESTADO_PAGO) " +
                    "VALUES(:CORRELATIVO, :ID_CLIENTE, TRUNC(SYSDATE), :CONCEPTO, TRUNC(SYSDATE), :USUARIO, 2)";
            }
            
            String Query2 = "INSERT INTO TBL_TIPO_PAGO(CORRELATIVO, ID_FORMA_PAGO, MONTO, NUMERO_DOCUMENTO, AUTORIZACION) " +
                "VALUES(:CORRELATIVO, :ID_FORMA_PAGO, :MONTO, :NUMERO_DOCUMENTO, :AUTORIZACION)";

            OracleTransaction transaction = null;
            try
            {
                using (OracleConnection connection = new OracleConnection(db.getOracleConnectionStr()))
                {
                    connection.Open();
                    OracleCommand cmd1 = connection.CreateCommand();
                    OracleCommand cmd2 = connection.CreateCommand();

                    transaction = connection.BeginTransaction();

                    cmd1.Connection = connection;
                    cmd1.Transaction = transaction;

                    cmd2.Connection = connection;
                    cmd2.Transaction = transaction;

                    cmd1.CommandText = Query1;
                    if (concepto == "M")
                    {
                        cmd1.Parameters.Add("USUARIO", OracleDbType.Varchar2).Value = usuario;
                        cmd1.Parameters.Add("CORRELATIVO", OracleDbType.Int64).Value = correlativo;
                    }
                    else
                    {
                        cmd1.Parameters.Add("CORRELATIVO", OracleDbType.Int64).Value = vCorr;
                        cmd1.Parameters.Add("ID_CLIENTE", OracleDbType.Varchar2).Value = id_cliente;
                        cmd1.Parameters.Add("CONCEPTO", OracleDbType.Varchar2).Value = concepto;
                        cmd1.Parameters.Add("USUARIO", OracleDbType.Varchar2).Value = usuario;
                    }
                    
                    vResult = cmd1.ExecuteNonQuery();

                    if (vResult > 0)
                    {
                        cmd2.CommandText = Query2;
                        cmd2.Parameters.Add("CORRELATIVO", OracleDbType.Int64).Value = vCorr;
                        cmd2.Parameters.Add("ID_FORMA_PAGO", OracleDbType.Varchar2).Value = id_forma_pago;
                        cmd2.Parameters.Add("MONTO", OracleDbType.Decimal).Value = Decimal.Parse(monto);
                        cmd2.Parameters.Add("NUMERO_DOCUMENTO", OracleDbType.Varchar2).Value = numero_documento;
                        cmd2.Parameters.Add("AUTORIZACION", OracleDbType.Varchar2).Value = autorizacion;
                        vResult = cmd2.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
            }
            catch (Exception e)
            {
                vResult = -100;
                log.AddToLog("GuardarPago", "Ocurrio un error al guardar el pago " + e.Message + " " + e.Source + " " + e.StackTrace);

                if (transaction != null)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex)
                    {
                        log.AddToLog("GuardarPago", "Error al realizar rollback: " + ex.Message);
                    }
                }
            }

            return vResult;
        }
    }
}