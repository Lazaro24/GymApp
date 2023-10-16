using GymApp.Controllers;
using GymApp.Models.Entities;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BCrypt.Net;

namespace GymApp.Models.AppModels
{
    public class UsuarioModel
    {
        Log log = new Log();
        
        public entUsuarios Loggeo(entUsuarios user)
        {            
            GlobalDB db = new GlobalDB();
            String Query = "";
            String clave = "";

            entUsuarios loggeoUser = new entUsuarios();
            try
            {
                Query = "SELECT USUARIO, PASSWORD FROM TBL_USUARIO WHERE USUARIO = '{0}' AND ESTADO = 'A' ";
                Query = string.Format(Query, user.usuario);

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
                                while (dr.Read())
                                {
                                    loggeoUser.usuario = dr.IsDBNull(0) ? "" : dr.GetString(0);
                                    loggeoUser.password = dr.IsDBNull(1) ? "" : dr.GetString(1);
                                    clave = dr.IsDBNull(1) ? "" : dr.GetString(1);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            log.AddToLog(ex);
                        }
                    } 
                }                                

                if (!BCrypt.Net.BCrypt.Verify(user.password, clave))
                {
                    loggeoUser.encontrado = false;
                    loggeoUser.mensaje = "Contraseña incorrecta";
                    return loggeoUser;
                }                    

                loggeoUser.mensaje = "Bienvenido " + loggeoUser.usuario;
                loggeoUser.encontrado = true;
            }
            catch (Exception e)
            {
                loggeoUser.mensaje = "Error: " + e.Message;
                log.AddToLog("LoggeoUsuario", "Error loggeando al usuario " + e.Message + " " + e.Source + " " + e.StackTrace);
            }

            return loggeoUser;
        }

        public Boolean setUpdatePassword(string usuario, string clave)
        {
            GlobalDB db = new GlobalDB();
            int vResult = 0;
            string cmdText = "";
            var cryptPass = BCrypt.Net.BCrypt.HashPassword(clave);
            bool update = false;

            try
            {
                cmdText = "UPDATE TBL_USUARIO SET PASSWORD = :PASSWORD WHERE USUARIO = :USUARIO ";

                GlobalDBParamObjectList param = new GlobalDBParamObjectList();
                param.Add(new GlobalDBParamObject("PASSWORD", cryptPass, OracleDbType.Varchar2));
                param.Add(new GlobalDBParamObject("USUARIO", usuario, OracleDbType.Varchar2));

                vResult = db.setQuery(cmdText, param);

                if (vResult > 0)
                {
                    update = true;
                }
            }
            catch (Exception e)
            {
                log.AddToLog(e);
            }

            return update;
        }

        public String RecuperaPassword(String usuario, string clave)
        {
            GlobalDB db = new GlobalDB();
            string vResult = "OK|Contraseña actualizada con exito!";
            string commandText = "";

            try
            {
                commandText = (
                    "SELECT TU.USUARIO, TU.ID_EMPLEADO, TE.NOMBRE1, TE.CORREO " +
                    "FROM TBL_USUARIO TU " +
                    "INNER JOIN TBL_EMPLEADOS TE ON TE.ID_EMPLEADO = TU.ID_EMPLEADO " +
                    "WHERE TE.ESTADO = 'A' " +
                    "AND TU.USUARIO = :USUARIO "
                    );
                
                string connectionString = db.getOracleConnectionStr();
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    using (OracleCommand command = new OracleCommand(commandText, connection))
                    {
                        try
                        {
                            command.Parameters.Add("USUARIO", OracleDbType.Varchar2).Value = usuario;

                            connection.Open();

                            using (OracleDataReader dr = command.ExecuteReader())
                            {
                                log.AddToLog("CargarUsuario", "Generando datos del usuario [" + commandText + "]");

                                if (!dr.Read())
                                {
                                    return "ERROR|No se encontro el usuario ingresado";
                                }

                                if(!setUpdatePassword(usuario, clave))
                                {
                                    return "ERROR|No se pudo realizar la actualización de la contraseña";
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
            catch (Exception e)
            {
                log.AddToLog(e);
            }

            return vResult;
        }

        public List<entUsuarios> ListadoDatos()
        {
            GlobalDB db = new GlobalDB();

            String Query = "";
            var Usuarios = new List<entUsuarios>();

            try
            {
                Query = "SELECT TU.USUARIO, TU.FECHA_CREACION, TU.ESTADO, " +
                    "   TE.ID_EMPLEADO, " +
                    "   (TE.NOMBRE1 || ' ' || TE.NOMBRE2 || ' ' || TE.APELLIDO1 || ' ' || TE.APELLIDO2 || ' ' || TE.APELLIDO3) NOMBRE_COMPLETO, " +
                    "   TE.DPI, TU.PASSWORD " +
                    "FROM TBL_USUARIO TU " +
                    "RIGHT JOIN TBL_EMPLEADOS TE ON TE.ID_EMPLEADO = TU.ID_EMPLEADO " +
                    "WHERE TE.ESTADO = 'A'";

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
                                log.AddToLog("CargarUsuarios", "Generando listado de usuarios [" + Query + "]");
                                while (dr.Read())
                                {
                                    var Usuario = new entUsuarios
                                    {
                                        usuario = dr.IsDBNull(0) ? "" : dr.GetString(0),
                                        fecha_creacion = dr.IsDBNull(1) ? "" : dr.GetDateTime(1).ToString("dd/MM/yyyy"),
                                        estado = dr.IsDBNull(2) ? "" : dr.GetString(2),
                                        id_empleado = dr.IsDBNull(3) ? 0 : dr.GetInt64(3),
                                        nombre_completo = dr.IsDBNull(4) ? "" : dr.GetString(4),
                                        dpi = dr.IsDBNull(5) ? 0 : dr.GetInt64(5),
                                        password = dr.IsDBNull(6) ? "" : dr.GetString(6)
                                    };
                                    Usuarios.Add(Usuario);
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
                log.AddToLog("CargarUsuarios", "Error al obtener usuarios " + ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }

            return Usuarios;
        }

        public int InsertarDatos(entUsuarios c)
        {
            GlobalDB db = new GlobalDB();
            int vResult = 0;
            String Query = "";
            try
            {
                Query = ("INSERT INTO TBL_USUARIO(USUARIO, PASSWORD, FECHA_CREACION, ESTADO, ID_EMPLEADO) " +
                    "VALUES(:USUARIO, :PASSWORD, TRUNC(SYSDATE), 'A', :ID_EMPLEADO)");

                GlobalDBParamObjectList param = new GlobalDBParamObjectList();
                param.Add(new GlobalDBParamObject("USUARIO", c.usuario, OracleDbType.Varchar2));
                param.Add(new GlobalDBParamObject("PASSWORD", BCrypt.Net.BCrypt.HashPassword(c.password), OracleDbType.Varchar2));
                param.Add(new GlobalDBParamObject("ID_EMPLEADO", c.id_empleado, OracleDbType.Int64));

                vResult = db.setQuery(Query, param);
            }
            catch (Exception e)
            {
                vResult = -100;
                log.AddToLog("CrearUsuario", "Ocurrio un error al crear usuario " + e.Message + " " + e.Source + " " + e.StackTrace);
            }

            return vResult;
        }

        public int ActualizarDatos(entUsuarios c)
        {
            GlobalDB db = new GlobalDB();
            int vResult = 0;
            String Query = "";
            try
            {
                Query = ("UPDATE TBL_USUARIO " +
                    "SET ID_EMPLEADO = :ID_EMPLEADO " +
                    "WHERE USUARIO = :USUARIO ");

                GlobalDBParamObjectList param = new GlobalDBParamObjectList();
                param.Add(new GlobalDBParamObject("ID_EMPLEADO", c.id_empleado, OracleDbType.Int64));
                param.Add(new GlobalDBParamObject("USUARIO", c.usuario, OracleDbType.Varchar2));

                vResult = db.setQuery(Query, param);
            }
            catch (Exception e)
            {
                vResult = -100;
                log.AddToLog("ActualizarUsuario", "Ocurrio un error al actualizar informacion del usuario " + e.Message + " " + e.Source + " " + e.StackTrace);
            }

            return vResult;
        }

        public int EliminarDatos(string usuario)
        {
            GlobalDB db = new GlobalDB();
            int vResult = 0;
            String Query = "";
            try
            {
                Query = ("UPDATE TBL_USUARIO SET ESTADO = 'I' WHERE USUARIO = :USUARIO");

                GlobalDBParamObjectList param = new GlobalDBParamObjectList();
                param.Add(new GlobalDBParamObject("USUARIO", usuario, OracleDbType.Varchar2));

                vResult = db.setQuery(Query, param);
            }
            catch (Exception e)
            {
                vResult = -100;
                log.AddToLog("EliminarUsuario", "Ocurrio un error al dar de baja al usuario " + e.Message + " " + e.Source + " " + e.StackTrace);
            }

            return vResult;
        }
    }
}