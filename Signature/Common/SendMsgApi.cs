using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Documents;

namespace Signature.Common
{
    public class SendMsgApi
    {
        static Thread _ThreMsg;
        public static List<MessagForm> list_form = new List<MessagForm>();
        static Queue<Info_SendMsg> sendMsgQueue = new Queue<Info_SendMsg>();
        private static void AddMsg(Info_SendMsg info)
        {
            if (info.msgContent.Length > 0)
            {
                sendMsgQueue.Enqueue(info);
            }
        }
        /// <summary>
        /// 初始化 日志线程
        /// 消息日志记录由主线程直接写入文件改为由独立的日志线程操作 调用方式不变 通过消息队列接受和处理消息日志
        /// 由UDP日志分发服务时不会造成主线程卡顿
        /// 2019-12-4 jsx
        /// </summary>
        public static void InitMsgThread()
        {
            try
            {
                _ThreMsg = new Thread(new ThreadStart(Msghread));
                _ThreMsg.IsBackground = true;
                _ThreMsg.Start();
            }
            catch (Exception ex)
            {
            }
        }
        private static void Msghread()
        {
            while (true)
            {
                try 
                {
                    //刷新消息
                    if (sendMsgQueue.Count > 0)
                    {
                        Main.marnForm.AddSendMsg(sendMsgQueue.Dequeue());
                    }
                } 
                catch { }

                Thread.Sleep(500);
            }
        }

        public static void SetSendMsg(string msg) 
        {
            try 
            {
                if (!string.IsNullOrEmpty(msg)) 
                {
                    List<Info_SendMsg> list = new List<Info_SendMsg>();
                    list = JsonConvert.DeserializeObject<List<Info_SendMsg>>(msg);
                    if (list.Count >0) 
                    {
                        Main.marnForm.ClearForm();
                        for (int i=0;i< list.Count;i++) 
                        {
                            AddMsg(list[i]);
                        }
                    }

                }
            } catch 
            { 

            }
        }

    }
    [Serializable]
    public class Info_SendMsg 
    {
        public string id;
        public string msgContent;
        public string title;
        public int type;
        public string titleColor;
    }
}
