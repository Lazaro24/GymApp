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
                    string FileName = "log_" + DateNow() + ".txt";
                    // Generando archivo de log
                    using (StreamWriter sw = new StreamWriter(vPath + @"\log\" + FileName, true))
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
            return Dt.ToString("ddMMyyyy HHmmss");
        }
    }
}