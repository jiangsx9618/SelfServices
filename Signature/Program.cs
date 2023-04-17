using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace Signature
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //获取当前进程的ID  
            int pId = Process.GetCurrentProcess().Id;
            bool isRun = false;
            foreach (Process p in Process.GetProcessesByName("Signature"))//应用程序名称，在任务管理器中可查看应用程序名称
            {
                //取得当前程序的进程，进行比较  
                if (System.Reflection.Assembly.GetExecutingAssembly().Location.ToLower() == p.MainModule.FileName.ToLower())
                {
                    if (pId != p.Id)
                    {
                        isRun = true;
                        break;
                    }
                }
            }
            if (isRun == true)
            {
                MessageBox.Show("程序已启动，请勿重复开启");
                Application.Exit();
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}
