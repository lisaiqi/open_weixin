using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using log4net;
using System.IO;

namespace ServerUtil.LogHelper
{
    public class Log4NetHelper
    {
        private static log4net.ILog _ilogInfo;
        private static log4net.ILog _ilogError;
        private static log4net.ILog _ilogLogin; //login log

        public static void initLog4net()
        {
            _ilogInfo = LogManager.GetLogger("loginfo");
            _ilogError = LogManager.GetLogger("logerror");
            _ilogLogin = LogManager.GetLogger("loginfo_login");
        }

        public static void logInfo_login(string as_describe)
        {
            try
            {
                if (_ilogInfo == null)
                {
                    _ilogInfo = LogManager.GetLogger("loginfo");                    
                }
                _ilogLogin.Info(as_describe);
            }
            catch (Exception)
            {
            }
            
        }

        public static void logInfo(string as_describe)
        {
            try
            {
                if (_ilogInfo == null)
                {
                    _ilogInfo = LogManager.GetLogger("loginfo");
                }
                _ilogInfo.Info(as_describe);
            }
            catch (Exception)
            {
            }
            
        }
        public static void logError(string as_describe, Exception ex)
        {
            try
            {
                if (_ilogError == null)
                {
                    _ilogError = LogManager.GetLogger("logerror");
                }                    
                _ilogError.Error(as_describe, ex);
            }
            catch (Exception)
            {
            }
            
        }


        public static void logText(string filename, string as_describe)
        {

            string ls_path = System.AppDomain.CurrentDomain.BaseDirectory;
            string ls_FileNamePart = DateTime.Now.ToString("yyyyMMdd");
            FileStream fs = new FileStream(string.Format("{0}/log/LogText/{1}.txt", ls_path, filename + ls_FileNamePart), FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(as_describe+"\r\n");
            sw.Flush();
            sw.Close();
            fs.Close();
        }




    }
}
