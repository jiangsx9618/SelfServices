using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Signature.Common
{
    /// <summary>
    /// http上传下载文件帮助类
    /// </summary>
    public static class HttpUpAndDownloadFile
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="options">上传文件参数</param>
        /// <returns></returns>
        public static async Task<string> Upload(UploadOptions options)
        {
            string strBoundary = "-----------------------------------" + DateTime.Now.Ticks.ToString("x");
            string contentType = $"multipart/form-data; boundary={strBoundary}";
            byte[] line = Encoding.UTF8.GetBytes("\r\n");
            byte[] boundaryNormal = Encoding.ASCII.GetBytes("\r\n--" + strBoundary + "\r\n");
            byte[] boundaryEnd = Encoding.ASCII.GetBytes("\r\n--" + strBoundary + "--\r\n");
            var req = (HttpWebRequest)HttpWebRequest.Create(options.Url);
            req.Method = "POST";
            //对发送的数据不使用缓存  
            req.AllowWriteStreamBuffering = false;
            //允许数据分块发送
            req.SendChunked = true;
            //设置获得响应的超时时间
            req.Timeout = options.MillisecondsTimeout;
            req.KeepAlive = true;
            req.ContentType = $"multipart/form-data; boundary={strBoundary}";
            using (var stream = req.GetRequestStream())
            {
                if (options.Paras != null && options.Paras.Count > 0)
                {
                    foreach (var para in options.Paras)
                    {
                        stream.Write(boundaryNormal, 0, boundaryNormal.Length);
                        var str = "Content-Disposition: form-data; name=\"" + para.Key + "\"\r\n";
                        var bs = Encoding.UTF8.GetBytes(str);
                        stream.Write(bs, 0, bs.Length);
                        stream.Write(line, 0, line.Length);
                        str = para.Value.ToString();
                        bs = Encoding.UTF8.GetBytes(str);
                        stream.Write(bs, 0, bs.Length);
                    }
                }
                if (options.FileAbsPaths != null && options.FileAbsPaths.Count > 0)
                {
                    foreach (var fileItem in options.FileAbsPaths)
                    {
                        string filename = new FileInfo(fileItem.Value).Name;
                        using (var fs = new FileStream(fileItem.Value, FileMode.Open))
                        {
                            stream.Write(boundaryNormal, 0, boundaryNormal.Length);
                            var str = "Content-Disposition: form-data; name=\"" + fileItem.Key + "\"; filename=\"" + filename + "\"\r\n";
                            var bs = Encoding.UTF8.GetBytes(str);
                            stream.Write(bs, 0, bs.Length);
                            str = "Content-Type: application/octet-stream\r\n";
                            bs = Encoding.UTF8.GetBytes(str);
                            stream.Write(bs, 0, bs.Length);
                            stream.Write(line, 0, line.Length);
                            bs = new byte[fs.Length];
                            await fs.CopyToAsync(stream);
                        }
                    }
                }
                stream.Write(boundaryEnd, 0, boundaryEnd.Length);
            }
            //设置请求头
            foreach (var header in options.Headers)
            {
                req.Headers.Add(header.Key, header.Value);
            }
            WebResponse res = await req.GetResponseAsync();
            if (res.Headers.AllKeys.Contains("Content-Disposition"))
            {
                string filename = null;
                //服务器要求下载的文件
                try
                {
                    //如果存在响应头Content-Disposition,那么下载的文件名称,从这里面获取
                    var value = res.Headers["Content-Disposition"];
                    //attachment; filename=xxxx.pdf
                    filename = value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)[1].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries)[1];
                    filename = HttpUtility.UrlDecode(filename, Encoding.UTF8);
                }
                catch { }
                var responseStream = res.GetResponseStream();
                string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "download", DateTime.Now.ToString("yyyyMMdd"));
                if (!Directory.Exists(dir))
                {
                    lock (typeof(HttpUpAndDownloadFile))
                    {
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }
                    }
                }
                string path = Path.Combine(dir, filename ?? Guid.NewGuid().ToString().Replace("-", "").ToUpper());
                using (var fs = new FileStream(path, FileMode.Create))
                {
                    responseStream.CopyTo(fs);
                }
                return path;
            }
            else
            {
                //根据content-type处理
                var responseContentType = res.ContentType;
                if (options.ResponseHandlers.ContainsKey(responseContentType))
                {
                    var result = options.ResponseHandlers[responseContentType](responseContentType, res.GetResponseStream());
                    return result;
                }
                else
                {
                    var result = options.ResponseHandlers["default"](responseContentType, res.GetResponseStream());
                    return result;
                }
            }

        }

        private static void WriteStream(Stream stream, byte[] byteArray)
        {
            stream.Write(byteArray, 0, byteArray.Length);
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="options">下载文件参数</param>
        /// <returns></returns>
        public static async Task<string> Download(DownloadOptions options)
        {
            var req = (HttpWebRequest)HttpWebRequest.Create(options.Url);
            req.Method = options.HttpMethod.Method;
            foreach (var header in options.Headers)
            {
                req.Headers.Add(header.Key, header.Value);
            }
            var res = await req.GetResponseAsync();
            string filename = null;
            if (res.Headers.AllKeys.Contains("Content-Disposition"))
            {
                try
                {
                    //如果存在响应头Content-Disposition,那么下载的文件名称,从这里面获取
                    var value = res.Headers["Content-Disposition"];
                    //attachment; filename=xxxx.pdf
                    filename = value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)[1].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries)[1];
                    filename = HttpUtility.UrlDecode(filename, Encoding.UTF8);
                }
                catch { }
            }
            var responseStream = res.GetResponseStream();
            string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "download", DateTime.Now.ToString("yyyyMMdd"));
            if (!Directory.Exists(dir))
            {
                lock (typeof(HttpUpAndDownloadFile))
                {
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                }
            }
            string path = Path.Combine(dir, filename ?? Guid.NewGuid().ToString().Replace("-", "").ToUpper());
            using (var fs = new FileStream(path, FileMode.Create))
            {
                responseStream.CopyTo(fs);
            }
            return path;
        }
    }

    public class UploadOptions
    {
        public string Url { get; set; }
        public Dictionary<string, string> FileAbsPaths { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> Paras { set; get; } = new Dictionary<string, string>();
        public int MillisecondsTimeout { set; get; } = 5 * 60 * 1000;
        public Dictionary<string, string> Headers { set; get; } = new Dictionary<string, string>();
        public Dictionary<string, Func<string, Stream, string>> ResponseHandlers = new Dictionary<string, Func<string, Stream, string>>()
        {
            { "default",UploadOptionsDefault.DefaultResponseHandlers}
        };
    }

    public static class UploadOptionsDefault
    {
        public static Func<string, Stream, string> DefaultResponseHandlers = (contenttype, responseStream) =>
        {
            if (contenttype.Contains("application/json"))
            {
                MemoryStream ms = new MemoryStream();
                responseStream.CopyTo(ms);
                var buffer = ms.ToArray();
                string result = Encoding.UTF8.GetString(buffer);
                return result;
            }
            else
            {
                string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "download", DateTime.Now.ToString("yyyyMMdd"));
                if (!Directory.Exists(dir))
                {
                    lock (typeof(HttpUpAndDownloadFile))
                    {
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }
                    }
                }
                string path = Path.Combine(dir, Guid.NewGuid().ToString().Replace("-", "").ToUpper());
                using (var fs = new FileStream(path, FileMode.Create))
                {
                    responseStream.CopyTo(fs);
                }
                return path;
            }
        };
    }

    public class DownloadOptions
    {
        public HttpMethod HttpMethod { set; get; } = HttpMethod.Get;
        public string Url { get; set; }
        public Dictionary<string, string> Headers { set; get; } = new Dictionary<string, string>();
    }

    public static class HttpUtility
    {
        private static int HexToInt(char h)
        {
            if ((h >= '0') && (h <= '9'))
            {
                return (h - '0');
            }
            if ((h >= 'a') && (h <= 'f'))
            {
                return ((h - 'a') + 10);
            }
            if ((h >= 'A') && (h <= 'F'))
            {
                return ((h - 'A') + 10);
            }
            return -1;
        }

        private static char IntToHex(int n)
        {
            if (n <= 9)
            {
                return (char)(n + 0x30);
            }
            return (char)((n - 10) + 0x61);
        }

        private static int StrToInt(byte t)
        {
            if (t <= 58)
            {
                return t - 48;
            }
            else if (t <= 90)
            {
                return t - 65 + 10;
            }
            else
            {
                return t - 97 + 10;
            }
        }

        private static bool IsSafe(char ch)
        {
            if ((((ch >= 'a') && (ch <= 'z')) || ((ch >= 'A') && (ch <= 'Z'))) || ((ch >= '0') && (ch <= '9')))
            {
                return true;
            }
            switch (ch)
            {
                case '\'':
                case '(':
                case ')':
                case '*':
                case '-':
                case '.':
                case '_':
                case '!':
                    return true;
            }
            return false;
        }

        public static string UrlEncode(string str, Encoding encoder)
        {
            int num = 0;
            int num2 = 0;
            byte[] bytes = encoder.GetBytes(str);
            int count = bytes.Length;
            int offset = 0;
            for (int i = 0; i < count; i++)
            {
                char ch = (char)bytes[offset + i];
                if (ch == ' ')
                {
                    num++;
                }
                else if (!IsSafe(ch))
                {
                    num2++;
                }
            }
            if (num == 0 && num2 == 0)
            {
                return encoder.GetString(bytes);
            }
            byte[] buffer = new byte[count + (num2 * 2)];
            int num4 = 0;
            for (int j = 0; j < count; j++)
            {
                byte num6 = bytes[offset + j];
                char ch2 = (char)num6;
                if (IsSafe(ch2))
                {
                    buffer[num4++] = num6;
                }
                else if (ch2 == ' ')
                {
                    buffer[num4++] = 0x2b;
                }
                else
                {
                    buffer[num4++] = 0x25;
                    buffer[num4++] = (byte)IntToHex((num6 >> 4) & 15);
                    buffer[num4++] = (byte)IntToHex(num6 & 15);
                }
            }
            return encoder.GetString(buffer);
        }

        public static string UrlDecode2(string str, Encoding encoder)
        {
            int len = str.Length;
            byte[] ret = encoder.GetBytes(str);
            int i = 0;
            int p = 0;
            for (; i < len;)
            {
                if (ret[i] != 37)
                {
                    ret[p] = ret[i];
                }
                else
                {
                    ret[p] = (byte)((StrToInt(ret[i + 1]) << 4) | StrToInt(ret[i + 2]));
                    i = i + 2;
                }
                p = p + 1;
                i = i + 1;
            }

            if (p != len)
            {
                ret[p] = (byte)0;
            }
            return encoder.GetString(ret);
        }

        public static string UrlDecode(string str, Encoding encoder)
        {
            int len = str.Length;
            UrlDecoder decoder = new UrlDecoder(len, encoder);
            for (int i = 0; i < len; i++)
            {
                char ch = str[i];
                if (ch == '+')
                {
                    ch = ' ';
                }
                else if ((ch == '%') && (i < (len - 2)))
                {
                    if ((str[i + 1] == 'u') && (i < (len - 5)))
                    {
                        int num3 = HexToInt(str[i + 2]);
                        int num4 = HexToInt(str[i + 3]);
                        int num5 = HexToInt(str[i + 4]);
                        int num6 = HexToInt(str[i + 5]);
                        if (((num3 < 0) || (num4 < 0)) || ((num5 < 0) || (num6 < 0)))
                        {
                            if ((ch & 0xff80) == 0)
                            {
                                decoder.AddByte((byte)ch);
                            }
                            else
                            {
                                decoder.AddChar(ch);
                            }
                        }
                        ch = (char)((((num3 << 12) | (num4 << 8)) | (num5 << 4)) | num6);
                        i += 5;
                        decoder.AddChar(ch);
                        continue;
                    }
                    int num7 = HexToInt(str[i + 1]);
                    int num8 = HexToInt(str[i + 2]);
                    if ((num7 >= 0) && (num8 >= 0))
                    {
                        byte b = (byte)((num7 << 4) | num8);
                        i += 2;
                        decoder.AddByte(b);
                        continue;
                    }
                }
                else
                {
                    decoder.AddChar(ch);
                }
            }
            return decoder.GetString();
        }

        private class UrlDecoder
        {
            // Fields
            private int _bufferSize;
            private byte[] _byteBuffer;
            private char[] _charBuffer;
            private Encoding _encoding;
            private int _numBytes;
            private int _numChars;

            // Methods
            internal UrlDecoder(int bufferSize, Encoding encoding)
            {
                this._bufferSize = bufferSize;
                this._encoding = encoding;
                this._charBuffer = new char[bufferSize];
            }

            internal void AddByte(byte b)
            {
                if (this._byteBuffer == null)
                {
                    this._byteBuffer = new byte[this._bufferSize];
                }
                this._byteBuffer[this._numBytes++] = b;
            }

            internal void AddChar(char ch)
            {
                if (this._numBytes > 0)
                {
                    this.FlushBytes();
                }
                this._charBuffer[this._numChars++] = ch;
            }

            private void FlushBytes()
            {
                if (this._numBytes > 0)
                {
                    this._numChars += this._encoding.GetChars(this._byteBuffer, 0, this._numBytes, this._charBuffer, this._numChars);
                    this._numBytes = 0;
                }
            }

            internal string GetString()
            {
                if (this._numBytes > 0)
                {
                    this.FlushBytes();
                }
                if (this._numChars > 0)
                {
                    return new string(this._charBuffer, 0, this._numChars);
                }
                return string.Empty;
            }
        }
    }
    
}
