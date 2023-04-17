using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Threading;
using System.Xml.Serialization;

namespace Signature.Common
{
    /// <summary>
    /// 公共方法工具类
    /// jsx 2019-10-16
    /// </summary>
    class CommonApi
    {
        #region《日志处理》

        static string logPath = Application.StartupPath + "/MessageLog";
        /// <summary>
        /// windows 异常类
        /// </summary>
        static EventLog errLog = new EventLog();
        static Thread _ThreMsg;
        /// <summary>
        /// 日志信息队列日志
        /// </summary>
        static Queue<string> MSGQueue = new Queue<string>();
        /// <summary>
        /// 记录日志文件 添加至日志队列
        /// </summary>
        /// <param name="messageLog"></param>
        public static void WriteLog(string messageLog)
        {
            if (messageLog.Length > 0)
            {
                MSGQueue.Enqueue(messageLog);
            }
        }
        /// <summary>
        /// 初始化 日志线程
        /// 消息日志记录由主线程直接写入文件改为由独立的日志线程操作 调用方式不变 通过消息队列接受和处理消息日志
        /// 由UDP日志分发服务时不会造成主线程卡顿
        /// 2019-12-4 jsx
        /// </summary>
        public static void initLogThread()
        {
            try
            {
                _ThreMsg = new Thread(new ThreadStart(LogThread));
                _ThreMsg.IsBackground = true;
                _ThreMsg.Start();
            }
            catch (Exception ex)
            {
                Log("初始化日志错误," + ex.ToString());
            }
        }
        private static void LogThread()
        {
            while (true)
            {
                //刷新消息
                if (MSGQueue.Count > 0)
                {
                    Log(MSGQueue.Dequeue());
                }
                Thread.Sleep(10);
            }
        }
        /// <summary>
        /// 日志处理
        /// 系统日志
        /// </summary>
        /// <param name="msg"></param>
        private static void Log(string msg)
        {
            try
            {

                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }
                //将日志写入文件
                StreamWriter sw = new StreamWriter(logPath + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + "_Log.txt", true);
                string logmsg =DateTime.Now.ToString("") + "===" + msg + "\r\n";
                sw.Write(logmsg);
                sw.Close();
                Main.setMsg(logmsg);
            }
            catch (Exception ex)
            {
                errLog.WriteEntry("写消息日志方法异常，原因:" + ex.Message);//日志出现异常，记录到windows系统日志
            }
        }
        #endregion

        #region 《配置读取》
        public static ConfigParam configParam = new ConfigParam();
        /// <summary>
        /// 读取系统配置文件
        /// </summary>
        public static void readConfig()
        {
            try
            {
                string Config_File = Application.StartupPath + "/Config/Config.xml";
                if (File.Exists(Config_File))
                {
                    configParam = (ConfigParam)Deserialize_from_xml(Config_File, configParam.GetType());
                    clearFile();
                }
                else
                {
                    MessageBox.Show("配置文件[Config.xml]不存在");
                    WriteLog("配置文件[Config.xml]不存在");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取系统配置文件异常," + ex.ToString());
                WriteLog("读取系统配置文件异常," + ex.ToString());
            }
        }
        /// <summary>
        /// 文件初始化
        /// 清除一个月之前的文件
        /// </summary>
        private static void clearFile()
        {
            try
            {
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }
                //日志清除
                for (int i = 0; i < Directory.GetFiles(logPath).ToList().Count; i++)
                {
                    string filePath = Directory.GetFiles(logPath)[i];
                    string fileName = Path.GetFileNameWithoutExtension(filePath).Substring(0, 10);
                    DateTime start = DateTime.ParseExact(fileName, "yyyy-MM-dd", null);
                    if (DateTime.Now.Subtract(start).Days > 30)
                    {
                        File.Delete(filePath);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonApi.WriteLog("文件清除处理异常," + ex.ToString());
            }
        }

        #endregion

        #region《通用方法》
        /// <summary>
        /// 匹配A是否包含B字符
        /// 不区分大小写
        /// </summary>
        /// <param name="strA"></param>
        /// <param name="strB"></param>
        /// <returns></returns>
        public static bool ContainsString(string strA, string strB)
        {
            bool contains = false;
            if (!string.IsNullOrEmpty(strA) && !string.IsNullOrEmpty(strB))
            {
                contains = strA.IndexOf(strB, StringComparison.OrdinalIgnoreCase) >= 0;
            }
            return contains;
        }
        /// <summary>
        /// deserialize xml file to object
        /// </summary>
        /// <param name="path">the path of the xml file</param>
        /// <param name="object_type">the object type you want to deserialize</param>
        public static object Deserialize_from_xml(string path, Type object_type)
        {
            object result = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(object_type);
                using (StreamReader reader = new StreamReader(path))
                {
                    result = serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                WriteLog("deserialize from xml  error ," + ex.ToString());
            }
            return result;
        }
        #endregion
    }
}
