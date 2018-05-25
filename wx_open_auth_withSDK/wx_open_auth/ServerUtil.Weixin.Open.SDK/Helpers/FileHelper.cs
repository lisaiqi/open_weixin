 
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Weixin.Open.SDK.Helpers
{
    public class FileHelper
    {
        /// <summary>
        /// 根据完整文件路径获取FileStream
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static FileStream GetFileStream(string fileName)
        {
            FileStream fileStream = null;
            if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
            {
                fileStream = new FileStream(fileName, FileMode.Open);
            }
            return fileStream;
        }

        /// <summary>
        /// log message to file path
        /// </summary>
        /// <param name="msg">message</param>
        /// <param name="filePath">filepath: c:\\xx\\xx.xx</param>
        public static void LogToTxt(string msg)
        {
             
             string dirPath = AppDomain.CurrentDomain.BaseDirectory + "\\Log\\";
           
            string path = dirPath + DateTime.Now.ToString("yy-MM-dd") + ".txt";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            File.AppendAllText(path, msg + DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss") + "\r\n\r\n");
 
        }

    }
}
