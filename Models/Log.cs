using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace GymApp.Controllers
{
    public class Log
    {
        private static string GetCaller(int level = 1)
        {
            String Callers = "";

            //level=1
            var m = new StackTrace().GetFrame(level).GetMethod();
            // .Name is the name only, .FullName includes the namespace
            var className = m.DeclaringType.FullName;
            //the method/function name you are looking for.
            var methodName = m.Name;
            //returns a composite of the namespace, class and method name.
            Callers = className + "->" + methodName;

            return Callers;
        }

        private Boolean WriteLog(String vModule, String vMessage)
        {
            Boolean vResult = false;

            try
            {
                // Obtener el path para colocar el archivo de log
                //string vPath = System.AppDomain.CurrentDomain.BaseDirectory;
                string vPath = ConfigurationManager.AppSettings["LogApp"].ToString();

                if (vPath != null)
                {
                    string FileName = "logApplication_" + DateNow() + ".txt";
                    // Generando archivo de log
                    using (StreamWriter sw = new StreamWriter(vPath + @"\" + FileName, true))
                    {
                        sw.WriteLine(
                                "[" + DateTimeNow() + "]" +
                                "[" + vModule + "]" +
                                "[" + vMessage.Replace(Environment.NewLine, "") + "]"
                            );
                    }
                }
                vResult = true;
            }
            catch (Exception ex)
            {
                vResult = false;
            }

            return vResult;
        }

        public Boolean AddToLog(String vModule, String vMessage)
        {
            Boolean vResult = false;
            try
            {
                vModule = GetCaller() + "|" + vModule;
                vResult = WriteLog(vModule, vMessage);
            }
            catch (Exception e)
            {
                vResult = false;
            }

            return vResult;
        }

        public Boolean AddToLog(OracleCommand cmd)
        {
            Boolean vResult = false;

            string vParam = "";

            if (cmd.Parameters.Count > 0)
            {
                for (int i = 0; i < cmd.Parameters.Count; i++)
                {
                    vParam += cmd.Parameters[i].ParameterName + "=" + ((cmd.Parameters[i].Value == null) ? "" : cmd.Parameters[i].Value.ToString()) + ", ";
                }
                vParam = vParam.Substring(0, vParam.Length - 1);
            }

            string vMessage = "Type: " + cmd.CommandType + "; Text: " + cmd.CommandText + "; Parameters: " + vParam;
            string vModule = GetCaller();

            try
            {
                vResult = WriteLog(vModule, vMessage);
            }
            catch (Exception e)
            {
                vResult = false;
            }

            return vResult;
        }

        public Boolean AddToLog(Exception vEx)
        {
            Boolean vResp = false;
            String vMessage = "Message: " + vEx.Message + "; Source: " + vEx.Source + "; StackTrace: " + vEx.StackTrace + "; InnerException: " + vEx.InnerException;
            String vModule = GetCaller() + "|" + vEx.Source;

            try
            {
                vResp = WriteLog(vModule, vMessage);
            }
            catch (Exception e)
            {
                vResp = false;
            }

            return vResp;
        }

        public Boolean AddToLog(OracleCommand cmd, String vDatabase, String vDBNumber)
        {
            bool vResult;
            string vParameters = "";

            if (cmd.Parameters.Count > 0)
            {
                for (int i = 0; i < cmd.Parameters.Count; i++)
                {
                    vParameters += cmd.Parameters[i].ParameterName + "=" + ((cmd.Parameters[i].Value == null) ? "" : cmd.Parameters[i].Value.ToString()) + " , ";
                }
                vParameters = vParameters.Substring(0, vParameters.Length - 1);
            }

            String vMessage = "Ejecucion Command ::: Type: " + cmd.CommandType + "; Text: " + cmd.CommandText + "; Parameters: " + vParameters + "; DataBase: " + vDatabase + "; IdTransaccion: " + vDBNumber;
            String vModule = GetCaller();

            try
            {
                //vRecorded = ToDB(vModule,vMessage + ";  Comments" + vComments, vDatabase, vDBNumber);
                //if(!vRecorded) {
                vResult = WriteLog(vModule, vMessage);
                //}
            }
            catch (Exception e)
            {
                vResult = false;
            }

            return vResult;
        }

        public String DateNow()
        {
            DateTime Dt = DateTime.Now;
            return Dt.ToString("ddMMyyyy");
        }
        public String TimeNow()
        {
            DateTime Dt = DateTime.Now;
            return Dt.ToString("HHmmss");
        }
        public String DateTimeNow()
        {
            DateTime Dt = DateTime.Now;
            return Dt.ToString("ddMMyyyyHHmmss");
        }
    }
}