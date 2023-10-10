using GymApp.Controllers;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace GymApp.Models
{
    public class GlobalDB
    {
        Log _log = new Log();
        public OracleConnection Conn { get; set; }
        public OracleDataReader Reader { get; set; }

        public OracleConnection Conectar()
        {
            try
            {
                if (Conn == null)
                {
                    string ConnectTo = ConfigurationManager.ConnectionStrings["GYM"].ConnectionString;
                    _log.AddToLog("Conectar", "Abriendo conexion con BD");
                    Conn = new OracleConnection(ConnectTo);
                    Conn.Open();
                }
            }
            catch (Exception e)
            {
                Conn = null;
                _log.AddToLog("Conectar", "Error al conectar " + e.Message + " " + e.Source + " " + e.StackTrace);
            }

            return Conn;
        }

        public void Desconectar(OracleConnection conn)
        {
            try
            {
                if (conn != null)
                {
                    conn.Close();
                    _log.AddToLog("Conectar", "Cerrando conexion con BD ");
                }
            }
            catch (Exception e)
            {
                conn = null;
                _log.AddToLog("Desconectar", "Error al desconectar " + e.Message + " " + e.Source + " " + e.StackTrace);
            }
        }

        public OracleDataReader DataReader(String query, OracleConnection conexion)
        {
            try
            {
                OracleCommand comando = new OracleCommand(query, Conn);
                _log.AddToLog("DataReader", "Ejecutando Query " + query + " Conexion ");
                Reader = comando.ExecuteReader();
            }
            catch (Exception e)
            {
                Reader = null;
                _log.AddToLog("DataReader", "Error en DataReader " + e.Message + " " + e.Source + " " + e.StackTrace);
            }

            return Reader;
        }

        public void ReaderClose(OracleDataReader dr)
        {
            try
            {
                _log.AddToLog("ReaderClose", "Cerrando DataReader ");
                dr.Close();
            }
            catch (Exception e)
            {
                dr = null;
                _log.AddToLog("ReaderClose", "Error en ReaderClose " + e.Message + " " + e.Source + " " + e.StackTrace);
            }
        }

        public int executeQuery(String Query, OracleConnection conn)
        {
            int ResultCommand = 0;
            try
            {
                _log.AddToLog("executeQuery", "Ejecutando query (" + Query + ")");
                OracleCommand command = new OracleCommand(Query, conn);
                ResultCommand = command.ExecuteNonQuery();
                _log.AddToLog("executeQuery", "Query result -> " + ResultCommand);
            }
            catch (Exception e)
            {
                ResultCommand = -100;
                _log.AddToLog("executeQuery", "Error en ejecucion " + e.Message + " " + e.Source + " " + e.StackTrace);
            }
            return ResultCommand;
        }
    }
}