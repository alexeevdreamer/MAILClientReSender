using System;
using System.Reflection;
using System.IO;
using System.Text;

namespace MailReSend
{
    public class Log
    {
        static public bool Write(string _msg)
        {

            string pathFolderLog = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\ReSendMailLogs";
            bool res = true;

            try
            { 
                Directory.CreateDirectory(pathFolderLog);
                using (StreamWriter log = new StreamWriter(pathFolderLog +"\\Log_"+ DateTime.Now.ToString("yyyy-MM-dd")+".txt", true, Encoding.Unicode))
                {
                    log.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " : \t" + _msg);
                    log.Close();
                }

            }
            catch (Exception)
            {
                res = false;
            }

            return res;
        } 
    }
}
