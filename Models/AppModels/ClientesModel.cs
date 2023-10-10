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
        GlobalDB db = null;
        OracleDataReader dr = null;

        private OracleConnection conn { get; set; }

        public List<EntClientes> CargarClientes()
        {
            db = new GlobalDB();
            String Query = "";
            var Clientes = new List<EntClientes>();

            try
            {
                conn = db.Conectar();

                if (conn != null)
                {
                    Query = "SELECT * FROM TBL_CLIENTE WHERE ESTADO = 'A' ORDER BY 2 ";
                }

                dr = db.DataReader(Query, conn);
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
                db.ReaderClose(dr);
                db.Desconectar(conn);
            }
            catch (Exception ex)
            {
                Clientes = null;
                log.AddToLog("CargarClientes", "Error al obtener clientes " + ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }

            return Clientes;
        }

        public int ActualizarCliente(String Codigo, String Nombre, String Apellido, int Telefono, String Nit, String Sexo, String Email)
        {
            db = new GlobalDB();
            String Query = "";
            int vResult = 0;

            try
            {
                conn = db.Conectar();

                if (conn != null)
                {
                    Query = "UPDATE TBL_CLIENTE " +
                            "SET NOMBRE = '" + Nombre + "', " +
                            "SET NOMBRE = '" + Nombre + "', " +"   LASTNAME = '" + Apellido + "', " +
                            "WHERE CLIENT_CODE = " + Codigo + " ";
                }

                vResult = db.executeQuery(Query, conn);
                db.Desconectar(conn);
            }
            catch (Exception e)
            {
                vResult = -100;
                log.AddToLog("ActualizarCliente", "Error al actualizar cliente " + e.Message + " " + e.Source + " " + e.StackTrace);
            }

            return vResult;
        }

        public int CrearCliente(String Nombre, String Apellido, String Telefono, String Nit, String Sexo, String Email)
        {
            int vResult = 0;
            db = new GlobalDB();
            String Query = "";

            try
            {
                conn = db.Conectar();

                if (conn != null)
                {
                    Query = "INSERT INTO TBL_CLIENT(CLIENT_CODE, NAME, LASTNAME, PHONE_NUMBER, NIT, SEXO, EMAIL, REGISTER_DATE, STATUS) " +
                            "VALUES((SELECT NVL(MAX(CLIENT_CODE), 0) + 1  FROM TBL_CLIENT), " +
                            "'" + Nombre + "', '" + Apellido + "', " + Telefono + ", '" + Nit + "', '" + Sexo + "', '" + Email + "', SYSDATE, 'A')";
                }

                vResult = db.executeQuery(Query, conn);
                db.Desconectar(conn);
            }
            catch (Exception e)
            {
                vResult = -100;
                log.AddToLog("CrearCliente", "Ocurrio un error al crear Cliente " + e.Message + " " + e.Source + " " + e.StackTrace);
            }

            return vResult;
        }

        public int EliminarCliente(string CodCliente)
        {
            db = new GlobalDB();
            String Query = "";
            int vResult = 0;

            try
            {
                conn = db.Conectar();

                if (conn != null)
                {
                    Query = "UPDATE TBL_CLIENTE " +
                            "SET ESTADO = 'I' " +
                            "WHERE ID_CLIENTE = '" + CodCliente + "' ";
                }
                log.AddToLog("EliminarCliente", "Eliminando cliente [" + Query + "]");
                vResult = db.executeQuery(Query, conn);
                db.Desconectar(conn);
            }
            catch (Exception e)
            {
                vResult = -100;
                log.AddToLog("EliminarCliente", "Error al eliminar el cliente " + e.Message + " " + e.Source + " " + e.StackTrace);
            }

            return vResult;
        }
    }
}