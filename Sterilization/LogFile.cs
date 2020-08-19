using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Sterilization
{
    public class LogFile
    {
        private string m_exePath = string.Empty;


        public LogFile()
        {
        }

        public void LogMessge(string logMessage)
        {
            LogWrite(logMessage);
        }
        public void LogWrite(string logMessage)
        {
            m_exePath = "C:\\gplstestlog";
            try
            {
                using (StreamWriter w = File.AppendText(m_exePath + "\\" + "log.txt"))
                {
                    Log(logMessage, w);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Log(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("\r\nLog Entry : ");
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                txtWriter.WriteLine("  :{0}", logMessage);
                txtWriter.WriteLine("---------------------------------------------------------------------------------------------");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}