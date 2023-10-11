using GymApp.Controllers;
using GymApp.Models.Entities;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
                Query = "SELECT USUARIO, PASSWORD, NOMBRES, APELLIDOS FROM TBL_USUARIO WHERE USUARIO = '{0}' AND ESTADO = 'A' ";
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
                                    loggeoUser.nombres = dr.IsDBNull(2) ? "" : dr.GetString(2);
                                    loggeoUser.apellidos = dr.IsDBNull(3) ? "" : dr.GetString(3);
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
    }
}