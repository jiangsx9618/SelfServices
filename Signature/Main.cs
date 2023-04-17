using Signature.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ScrollBar;

namespace Signature
{
    public partial class Main : Form
    {
        public static Main marnForm = null;
        public Main()
        {
            InitializeComponent();
            marnForm = this;
        }
        #region《界面日志刷新》
        /// <summary>
        /// 启动状态更新·
        /// </summary>
        /// <param name="falg"></param>
        /// <param name="type"> 0 本地监听服务启动 1签字版连接</param>
        private  void setFlag(bool falg,int type)
        {
            try
            {
                lab_listem.BackColor = (falg == true ? Color.LimeGreen : Color.Red);
            }
            catch  { }
        }
       static  int num = 0;
        /// <summary>
        /// 消息框更新
        /// </summary>
        /// <param name="msg"></param>
        public static void setMsg(string msg)
        {
            try 
            {
                marnForm.Invoke((EventHandler)delegate
                {
                    if (num>100)
                    {
                        num = 0;

                        marnForm.text_msg.Clear();
                    }
                    marnForm.text_msg.AppendText(msg);
                    num++;

                });
            }
            catch(Exception ex)
            {
                CommonApi.WriteLog("界面信息更新异常,"+ex.ToString());
            }
        }
        #endregion

        private void Signature_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
            //配置读取
            CommonApi.readConfig();
            //日志初始化
            CommonApi.initLogThread();
            //
            SendMsgApi.InitMsgThread();
            //启动本地监听服务
            setFlag(WebApi.StartService(),0);
        }
        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                //还原窗体显示    
                WindowState = FormWindowState.Normal;
                //激活窗体并给予它焦点
                this.Activate();
                //任务栏区显示图标
                this.ShowInTaskbar = true;
                //托盘区图标隐藏
                notifyIcon1.Visible = false;
            }
        }

        private void Signature_SizeChanged(object sender, EventArgs e)
        {
            //判断是否选择的是最小化按钮
            if (WindowState == FormWindowState.Minimized)
            {
                //隐藏任务栏区图标
                this.ShowInTaskbar = false;
                //图标显示在托盘区
                notifyIcon1.Visible = true;
            }
        }

        private void Signature_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("是否确认退出？选择'确定‘退出程序", "操作提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                formClose();
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void 显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
        }
        private void formClose()
        {
            this.Dispose();
            this.notifyIcon1.Dispose();
            System.Environment.Exit(0);
        }
        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否确认退出程序？", "退出", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                formClose();
            }
        }
        private void text_msg_DoubleClick(object sender, EventArgs e)
        {

        }
        //定义一个委托  MessageEntity是消息实体类
        delegate void ShowMessageCallback(Info_SendMsg message);

        //委托方法
        private void ShowMessageSmall(Info_SendMsg message)
        {
            //消息框Form
            MessagForm ms = new MessagForm(message);
            ms.Show();
        }

        //调用委托
        public  void AddSendMsg(Info_SendMsg message)
        {
            try
            {
                //创建委托
                ShowMessageCallback wt = new ShowMessageCallback(ShowMessageSmall);
                //这段代码在主窗体类里面写着，this指主窗体
                this.Invoke(wt, new Object[] { message });
            } catch { }

        }
        public  void ClearForm()
        {
            try
            {

                Action action = () =>
                {
                    for (int i = 0; i < SendMsgApi.list_form.Count; i++)
                    {
                        if (null != SendMsgApi.list_form[i])
                        {
                            SendMsgApi.list_form[i].CloseForm(false);
                        };
                    }
                    SendMsgApi.list_form.Clear();
                    text_msg.Clear();
                };
                Invoke(action);
            }
            catch { }
        }

        private void btn_min_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void btn_clerAll_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void 清空报警ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearForm();
        }
    }
}
