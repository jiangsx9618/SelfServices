using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using Newtonsoft.Json.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace Signature.Common
{
    class WebApi
    {

        static string checkUrl = "checkStart";
        static string sendMsgUrl = "sendMsg";
        private static HttpListener httpPostRequest = new HttpListener();
        /// <summary>
        /// 启动监听服务
        /// </summary>
        public static bool StartService()
        {
            bool flag = false;
            try 
            {
                httpPostRequest.Prefixes.Add(CommonApi.configParam.localUrl);
                httpPostRequest.Start();
                Thread ThrednHttpPostRequest = new Thread(new ThreadStart(HttpPostRequestHandle));
                ThrednHttpPostRequest.Start();
                flag = true;
            }
            catch(Exception ex)
            {
                CommonApi.WriteLog("监听服务启动异常,"+ex.ToString());
            }
            return flag;
        }
        /// <summary>
        /// 监听请求
        /// </summary>
        private static void HttpPostRequestHandle()
        {
            while (true)
            {
                try
                {
                    HttpListenerContext requestContext = httpPostRequest.GetContext();
                    Thread threadsub = new Thread(new ParameterizedThreadStart((requestcontext) =>
                    {
                        HttpListenerContext request = (HttpListenerContext)requestcontext;
                        //接收Post参数
                        //Stream stream = request.Request.InputStream;
                        //System.IO.StreamReader reader = new System.IO.StreamReader(stream, Encoding.UTF8);
                       
                        //string body = reader.ReadToEnd();
                        //接收get请求
                        bool flag = false;
                        if (CommonApi.ContainsString(request.Request.RawUrl, checkUrl))
                        {
                            flag = true;
                            CommonApi.WriteLog("接收到信息[" + checkUrl + "]");

                        }
                        else if (CommonApi.ContainsString(request.Request.RawUrl, sendMsgUrl)) 
                        {
                            flag = true;
                            Stream stream = request.Request.InputStream;
                            StreamReader reader = new StreamReader(stream, Encoding.UTF8);

                            string content = reader.ReadToEnd();
                            CommonApi.WriteLog("接收到信息[" + content + "]");
                            SendMsgApi.SetSendMsg(content);
                        }
                        //Response  
                        request.Response.StatusCode = 200;
                        request.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                        request.Response.ContentType = "application/json";
                        requestContext.Response.ContentEncoding = Encoding.UTF8;
                        string result = flag ? "操作成功!" : "操作失败!";
                        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(new { success = flag, msg = result }));
                        request.Response.ContentLength64 = buffer.Length;
                        var output = request.Response.OutputStream;
                        output.Write(buffer, 0, buffer.Length);
                        output.Close();
                    }));
                    threadsub.Start(requestContext);
                }
                catch (Exception ex)
                {
                    CommonApi.WriteLog("http接收异常=====" + ex.ToString());
                }

            }
        }
        /// <summary>
        /// 返回状态枚举
        /// </summary>
        public enum code
        {
            sucess = 200,
            error = 201,
        };
    }
   
}
