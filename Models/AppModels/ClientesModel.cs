using GymApp.Controllers;
using GymApp.Models.Entities;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GymApp.Models.AppModels
{
    public class ClientesModel
    {
        Log log = new Log();
        public String vErrOracle { get; set; }

        public List<EntClientes> ListadoDatos()
        {
            GlobalDB db = new GlobalDB();

            String Query = "";
            var Clientes = new List<EntClientes>();

            try
            {
                Query = "SELECT * FROM TBL_CLIENTE ORDER BY 1";

                string connectionString = db.getOracleConnectionStr();
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    using (OracleCommand command = new OracleCommand(Query, connection))
                    {
                        try
                        {
                            connection.Open();

                            using(OracleDataReader dr = command.ExecuteReader())
                            {
                                log.AddToLog("CargarClientes", "Generando listado de clientes [" + Query + "]");
                                while (dr.Read())
                                {
                                    var Cliente = new EntClientes
                                    {
                                        IdCliente = dr.IsDBNull(0) ? "" : dr.GetString(0),
                                        Nombre1 = dr.IsDBNull(1) ? "" : dr.GetString(1),
                                        Nombre2 = dr.IsDBNull(2) ? "" : dr.GetString(2),
                                        Apellido1 = dr.IsDBNull(3) ? "" : dr.GetString(3),
                                        Apellido2 = dr.IsDBNull(4) ? "" : dr.GetString(4),
                                        Apellido3 = dr.IsDBNull(5) ? "" : dr.GetString(5),
                                        Nit = dr.IsDBNull(6) ? 0 : dr.GetInt32(6),
                                        Telefono = dr.IsDBNull(7) ? 0 : dr.GetInt32(7),
                                        Dpi = dr.IsDBNull(8) ? 0 : dr.GetInt64(8),
                                        FechaCreacion = dr.IsDBNull(9) ? "" : dr.GetDateTime(9).ToString("dd/MM/yyyy"),
                                        Direccion = dr.IsDBNull(10) ? "" : dr.GetString(10),
                                        Genero = dr.IsDBNull(11) ? "" : dr.GetString(11),
                                        Estado = dr.IsDBNull(12) ? "" : dr.GetString(12),
                                        Usuario = dr.IsDBNull(13) ? "" : dr.GetString(13),
                                        Email = dr.IsDBNull(14) ? "" : dr.GetString(14)
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
                log.AddToLog("CargarClientes", "Error al obtener clientes " + ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }

            return Clientes;
        }

        public int InsertarDatos(EntClientes c)
        {
            GlobalDB db = new GlobalDB();
            int vResult = 0;
            String Query = "";
            try
            {
                Query = ("INSERT INTO TBL_CLIENTE (ID_CLIENTE, NOMBRE, NOMBRE2, APELLIDO, APELLIDO2, APELLIDO_CASADA, NIT, TELEFONO, DPI, FECHA_CREACION, DIRECCION, ID_GENERO, ESTADO, USUARIO, EMAIL) " +
                    "VALUES(SEC_ID_CLIENTE, :NOMBRE1, :NOMBRE2, :APELLIDO, :APELLIDO2, :APELLIDO_CASADA, :NIT, :TELEFONO, :DPI, TRUNC(SYSDATE), :DIRECCION, :ID_GENERO, 'A', :USUARIO, :EMAIL)");

                GlobalDBParamObjectList param = new GlobalDBParamObjectList();
                param.Add(new GlobalDBParamObject("NOMBRE", c.Nombre1, OracleDbType.Varchar2));
                param.Add(new GlobalDBParamObject("NOMBRE2", c.Nombre2, OracleDbType.Varchar2));
                param.Add(new GlobalDBParamObject("APELLIDO", c.Apellido1, OracleDbType.Varchar2));
                param.Add(new GlobalDBParamObject("APELLIDO2", c.Apellido2, OracleDbType.Varchar2));
                param.Add(new GlobalDBParamObject("APELLIDO_CASADA", c.Apellido3, OracleDbType.Varchar2));
                param.Add(new GlobalDBParamObject("NIT", c.Nit, OracleDbType.Int64));
                param.Add(new GlobalDBParamObject("TELEFONO", c.Telefono, OracleDbType.Int64));
                param.Add(new GlobalDBParamObject("DPI", c.Dpi, OracleDbType.Int64));
                param.Add(new GlobalDBParamObject("DIRECCION", c.Direccion, OracleDbType.Varchar2));
                param.Add(new GlobalDBParamObject("ID_GENERO", c.Genero, OracleDbType.Char));
                param.Add(new GlobalDBParamObject("USUARIO", c.Usuario, OracleDbType.Varchar2));
                param.Add(new GlobalDBParamObject("EMAIL", c.Email, OracleDbType.Varchar2));

                vResult = db.setQuery(Query, param);
            }
            catch (Exception e)
            {
                vResult = -100;
                log.AddToLog("CrearCliente", "Ocurrio un error al crear Cliente " + e.Message + " " + e.Source + " " + e.StackTrace);
            }

            return vResult;
        }

        public int ActualizarDatos(EntClientes c)
        {
            GlobalDB db = new GlobalDB();
            int vResult = 0;
            String Query = "";
            try
            {
                Query = ("UPDATE TBL_CLIENTE " +
                    "SET NOMBRE = :NOMBRE, NOMBRE2 = :NOMBRE2, APELLIDO = :APELLIDO, APELLIDO2 = :APELLIDO2, " +
                    "APELLIDO_CASADA = :APELLIDO_CASADA, NIT = :NIT, TELEFONO = :TELEFONO, DPI = :DPI, " +
                    "DIRECCION = :DIRECCION, ID_GENERO = :ID_GENERO, ESTADO = :ESTADO, USUARIO = :USUARIO, EMAIL = :EMAIL " + 
                    "WHERE ID_CLIENTE = :ID_CLIENTE");

                GlobalDBParamObjectList param = new GlobalDBParamObjectList();
                param.Add(new GlobalDBParamObject("NOMBRE", c.Nombre1, OracleDbType.Varchar2));
                param.Add(new GlobalDBParamObject("NOMBRE2", c.Nombre2, OracleDbType.Varchar2));
                param.Add(new GlobalDBParamObject("APELLIDO", c.Apellido1, OracleDbType.Varchar2));
                param.Add(new GlobalDBParamObject("APELLIDO2", c.Apellido2, OracleDbType.Varchar2));
                param.Add(new GlobalDBParamObject("APELLIDO_CASADA", c.Apellido3, OracleDbType.Varchar2));
                param.Add(new GlobalDBParamObject("NIT", c.Nit, OracleDbType.Int64));
                param.Add(new GlobalDBParamObject("TELEFONO", c.Telefono, OracleDbType.Int64));
                param.Add(new GlobalDBParamObject("DPI", c.Dpi, OracleDbType.Int64));
                param.Add(new GlobalDBParamObject("DIRECCION", c.Direccion, OracleDbType.Varchar2));
                param.Add(new GlobalDBParamObject("ID_GENERO", c.Genero, OracleDbType.Char));
                param.Add(new GlobalDBParamObject("ESTADO", c.Estado, OracleDbType.Char));
                param.Add(new GlobalDBParamObject("USUARIO", c.Usuario, OracleDbType.Varchar2));
                param.Add(new GlobalDBParamObject("EMAIL", c.Email, OracleDbType.Varchar2));
                param.Add(new GlobalDBParamObject("ID_CLIENTE", c.IdCliente, OracleDbType.Varchar2));

                vResult = db.setQuery(Query, param);
            }
            catch (Exception e)
            {
                vResult = -100;
                log.AddToLog("CrearCliente", "Ocurrio un error al actualizar informacion del cliente " + e.Message + " " + e.Source + " " + e.StackTrace);
            }

            return vResult;
        }

        public int EliminarDatos(string IdCliente, string Estado)
        {
            GlobalDB db = new GlobalDB();
            int vResult = 0;
            String Query = "";
            try
            {
                Query = ("UPDATE TBL_CLIENTE SET ESTADO = :ESTADO WHERE ID_CLIENTE = :ID_CLIENTE");

                GlobalDBParamObjectList param = new GlobalDBParamObjectList();
                param.Add(new GlobalDBParamObject("ESTADO", Estado, OracleDbType.Char));
                param.Add(new GlobalDBParamObject("ID_CLIENTE", IdCliente, OracleDbType.Varchar2));

                vResult = db.setQuery(Query, param);
            }
            catch (Exception e)
            {
                vResult = -100;
                log.AddToLog("CrearCliente", "Ocurrio un error al dar de baja al cliente " + e.Message + " " + e.Source + " " + e.StackTrace);
            }

            return vResult;
        }
    }
}