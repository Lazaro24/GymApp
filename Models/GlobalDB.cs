using GymApp.Controllers;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace GymApp.Models
{
    public class GlobalDB
    {
        Log log = new Log();
        OracleConnection Conn = null;
        public String vErrOracle { get; set; }

        public GlobalDB()
        {
            Conn = new OracleConnection();
        }

        public String getOracleConnectionStr()
        {
            return ConfigurationManager.ConnectionStrings["GYM"].ConnectionString;
        }

        public int setQuery(String vQuery, GlobalDBParamObjectList vParam)
        {
            OracleCommand cmd = null;
            
            int Result = 0;
            try
            {
                String vConStr = getOracleConnectionStr();

                if (Conn.State != ConnectionState.Open)
                {
                    Conn.ConnectionString = vConStr;
                    Conn.Open();
                }
                cmd = Conn.CreateCommand();
                cmd.CommandText = vQuery;

                if (vParam.Count() > 0)
                {
                    for (int iL = 0; iL < vParam.Count(); iL++)
                    {
                        cmd.Parameters.Add(vParam.get(iL).Name, vParam.get(iL).Value);
                        // Si tiene tipo de dato se le agrega
                        if (vParam.get(iL).DbType.ToString().Length > 0)
                        {
                            cmd.Parameters[vParam.get(iL).Name].OracleDbType = vParam.get(iL).DbType;
                        }
                        // Si tiene longitud de dato se le agrega
                        if (vParam.get(iL).Size.HasValue)
                        {
                            cmd.Parameters[vParam.get(iL).Name].Size = vParam.get(iL).Size.Value;
                        }
                    }
                }
                log.AddToLog(cmd, "GYM", "0");
                Result = cmd.ExecuteNonQuery();
                log.AddToLog("getQuery.Resultado", "Resultado: " + Result);

                try
                {
                    cmd.Dispose();
                }
                catch (Exception eF)
                {
                    log.AddToLog(eF);
                }
            }
            catch (Exception e)
            {
                log.AddToLog(e);
            }

            return Result;
        }
    }
}