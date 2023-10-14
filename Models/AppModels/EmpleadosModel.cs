using GymApp.Models.Entities;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GymApp.Models.AppModels
{
    public class EmpleadosModel
    {
        Log log = new Log();
        public String vErrOracle { get; set; }

        public List<entEmpleados> ListadoDatos()
        {
            GlobalDB db = new GlobalDB();

            String Query = "";
            var Empleados = new List<entEmpleados>();

            try
            {
                Query = "SELECT * FROM TBL_EMPLEADOS ORDER BY 1";

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
                                log.AddToLog("CargarEmpleados", "Generando listado de empleados [" + Query + "]");
                                while (dr.Read())
                                {
                                    var Empleado = new entEmpleados
                                    {
                                        IdEmpleado = dr.IsDBNull(0) ? 0 : dr.GetInt64(0),
                                        Nombre1 = dr.IsDBNull(1) ? "" : dr.GetString(1),
                                        Nombre2 = dr.IsDBNull(2) ? "" : dr.GetString(2),
                                        Apellido1 = dr.IsDBNull(3) ? "" : dr.GetString(3),
                                        Apellido2 = dr.IsDBNull(4) ? "" : dr.GetString(4),
                                        Apellido3 = dr.IsDBNull(5) ? "" : dr.GetString(5),
                                        Dpi = dr.IsDBNull(6) ? 0 : dr.GetInt64(6),
                                        Nit = dr.IsDBNull(7) ? 0 : dr.GetInt64(7),
                                        Telefono = dr.IsDBNull(8) ? 0 : dr.GetInt64(8),                                        
                                        Direccion = dr.IsDBNull(9) ? "" : dr.GetString(9),
                                        FechaCreacion = dr.IsDBNull(10) ? "" : dr.GetDateTime(10).ToString("dd/MM/yyyy"),
                                        FechaBaja = dr.IsDBNull(11) ? "" : dr.GetDateTime(11).ToString("dd/MM/yyyy"),
                                        Genero = dr.IsDBNull(12) ? "" : dr.GetString(12),
                                        Correo = dr.IsDBNull(13) ? "" : dr.GetString(13),
                                        Estado = dr.IsDBNull(14) ? "" : dr.GetString(14)
                                    };
                                    Empleados.Add(Empleado);
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
                log.AddToLog("CargarEmpleados", "Error al obtener empleados " + ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }

            return Empleados;
        }

        public int InsertarDatos(entEmpleados c)
        {
            GlobalDB db = new GlobalDB();
            int vResult = 0;
            String Query = "";
            try
            {
                Query = ("INSERT INTO TBL_EMPLEADOS (ID_EMPLEADO, NOMBRE1, NOMBRE2, APELLIDO1, APELLIDO2, APELLIDO3, DPI, NIT, TELEFONO, DIRECCION, FECHA_CREACION, ID_GENERO, CORREO, ESTADO) " +
                    "VALUES((SELECT NVL(MAX(ID_EMPLEADO), 0) + 1 FROM TBL_EMPLEADOS), " +
                    "   UPPER(:NOMBRE1), UPPER(:NOMBRE2), UPPER(:APELLIDO1), UPPER(:APELLIDO2), UPPER(:APELLIDO3), :DPI, :NIT, :TELEFONO, UPPER(:DIRECCION), TRUNC(SYSDATE), :ID_GENERO, :CORREO, 'A')");

                GlobalDBParamObjectList param = new GlobalDBParamObjectList();
                param.Add(new GlobalDBParamObject("NOMBRE1", c.Nombre1, OracleDbType.Varchar2));
                param.Add(new GlobalDBParamObject("NOMBRE2", c.Nombre2, OracleDbType.Varchar2));
                param.Add(new GlobalDBParamObject("APELLIDO1", c.Apellido1, OracleDbType.Varchar2));
                param.Add(new GlobalDBParamObject("APELLIDO2", c.Apellido2, OracleDbType.Varchar2));
                param.Add(new GlobalDBParamObject("APELLIDO3", c.Apellido3, OracleDbType.Varchar2));
                param.Add(new GlobalDBParamObject("DPI", c.Dpi, OracleDbType.Int64));
                param.Add(new GlobalDBParamObject("NIT", c.Nit, OracleDbType.Int64));
                param.Add(new GlobalDBParamObject("TELEFONO", c.Telefono, OracleDbType.Int64));                
                param.Add(new GlobalDBParamObject("DIRECCION", c.Direccion, OracleDbType.Varchar2));
                param.Add(new GlobalDBParamObject("ID_GENERO", c.Genero, OracleDbType.Char));                
                param.Add(new GlobalDBParamObject("CORREO", c.Correo, OracleDbType.Varchar2));

                vResult = db.setQuery(Query, param);
            }
            catch (Exception e)
            {
                vResult = -100;
                log.AddToLog("CrearEmpleado", "Ocurrio un error al crear empleado " + e.Message + " " + e.Source + " " + e.StackTrace);
            }

            return vResult;
        }

        public int ActualizarDatos(entEmpleados c)
        {
            GlobalDB db = new GlobalDB();
            int vResult = 0;
            String Query = "";
            try
            {
                Query = ("UPDATE TBL_EMPLEADOS " +
                    "SET NOMBRE1 = UPPER(:NOMBRE1), NOMBRE2 = UPPER(:NOMBRE2), APELLIDO1 = UPPER(:APELLIDO1), APELLIDO2 = UPPER(:APELLIDO2), " +
                    "APELLIDO3 = UPPER(:APELLIDO3), NIT = :NIT, TELEFONO = :TELEFONO, DPI = :DPI, " +
                    "DIRECCION = UPPER(:DIRECCION), ID_GENERO = :ID_GENERO, CORREO = :CORREO " +
                    "WHERE ID_EMPLEADO = :ID_EMPLEADO");

                GlobalDBParamObjectList param = new GlobalDBParamObjectList();
                param.Add(new GlobalDBParamObject("NOMBRE1", c.Nombre1, OracleDbType.Varchar2));
                param.Add(new GlobalDBParamObject("NOMBRE2", c.Nombre2, OracleDbType.Varchar2));
                param.Add(new GlobalDBParamObject("APELLIDO1", c.Apellido1, OracleDbType.Varchar2));
                param.Add(new GlobalDBParamObject("APELLIDO2", c.Apellido2, OracleDbType.Varchar2));
                param.Add(new GlobalDBParamObject("APELLIDO3", c.Apellido3, OracleDbType.Varchar2));
                param.Add(new GlobalDBParamObject("NIT", c.Nit, OracleDbType.Int64));
                param.Add(new GlobalDBParamObject("TELEFONO", c.Telefono, OracleDbType.Int64));
                param.Add(new GlobalDBParamObject("DPI", c.Dpi, OracleDbType.Int64));
                param.Add(new GlobalDBParamObject("DIRECCION", c.Direccion, OracleDbType.Varchar2));
                param.Add(new GlobalDBParamObject("ID_GENERO", c.Genero, OracleDbType.Char));
                param.Add(new GlobalDBParamObject("CORREO", c.Correo, OracleDbType.Varchar2));
                param.Add(new GlobalDBParamObject("ID_EMPLEADO", c.IdEmpleado, OracleDbType.Varchar2));

                vResult = db.setQuery(Query, param);
            }
            catch (Exception e)
            {
                vResult = -100;
                log.AddToLog("ActualizarEmpleado", "Ocurrio un error al actualizar informacion del empleado " + e.Message + " " + e.Source + " " + e.StackTrace);
            }

            return vResult;
        }

        public int EliminarDatos(string IdEmpleado, string Estado)
        {
            GlobalDB db = new GlobalDB();
            int vResult = 0;
            String Query = "";
            try
            {
                Query = ("UPDATE TBL_EMPLEADOS SET ESTADO = :ESTADO, FECHA_BAJA = TRUNC(SYSDATE) WHERE ID_EMPLEADO = :ID_EMPLEADO");

                GlobalDBParamObjectList param = new GlobalDBParamObjectList();
                param.Add(new GlobalDBParamObject("ESTADO", Estado, OracleDbType.Char));
                param.Add(new GlobalDBParamObject("ID_EMPLEADO", IdEmpleado, OracleDbType.Varchar2));

                vResult = db.setQuery(Query, param);
            }
            catch (Exception e)
            {
                vResult = -100;
                log.AddToLog("EliminarEmpleado", "Ocurrio un error al dar de baja al empleado " + e.Message + " " + e.Source + " " + e.StackTrace);
            }

            return vResult;
        }
    }
}