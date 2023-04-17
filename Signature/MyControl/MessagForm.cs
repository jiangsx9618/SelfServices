using Signature.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Signature
{
    public partial class MessagForm : Form
    {
        [Category("高级"), DefaultValue(true), Description("窗体是否固定大小，为true时，无法拖动边角调整窗体大小，默认true")]
        public bool FixedSize { get; set; } = true;
        bool closeFlag = true;
        public MessagForm(Info_SendMsg info)
        {
            int x = (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2;
            int y = (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2;
            if (SendMsgApi.list_form.Count > 0) 
            {
               int new_y = SendMsgApi.list_form.Last().Location.Y + 60;

                if (new_y < Screen.PrimaryScreen.WorkingArea.Height - this.Height) 
                {
                    y = new_y;
                }
            }
            this.Location = new Point(x, y);
            InitializeComponent();
            SendMsgApi.list_form.Add(this);
            string msg = info.title + Environment.NewLine + Environment.NewLine + info.msgContent;
            lab_msg.Text = msg;
            lab_msg.ForeColor = CommonApi.ContainsString(info.titleColor, "Red") ? Color.Red : Color.Black;
        }

        public void CloseForm(bool _closeFlag) 
        {
            closeFlag = _closeFlag;
            this.Close();
        }

        private void MessagForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (closeFlag) 
            {
                SendMsgApi.list_form.Remove(this);
            }
        }

        private void MessagForm_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            CloseForm(true);
        }

       public void SetWindowRegion()
        {
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            GraphicsPath FormPath = GetRoundedRectPath(rect, 30);
            this.Region = new Region(FormPath);
        }
        private GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            int diameter = radius;
            Rectangle arcRect = new Rectangle(rect.Location, new Size(diameter, diameter));
            GraphicsPath path = new GraphicsPath();
            // 左上角  
            path.AddArc(arcRect, 180, 90);
            // 右上角  
            arcRect.X = rect.Right - diameter;
            path.AddArc(arcRect, 270, 90);
            // 右下角  
            arcRect.Y = rect.Bottom - diameter;
            path.AddArc(arcRect, 0, 90);
            // 左下角  
            arcRect.X = rect.Left;
            path.AddArc(arcRect, 90, 90);
            path.CloseFigure();//闭合曲线  
            return path;
        }

        private void MessagForm_Resize(object sender, EventArgs e)
        {
            SetWindowRegion();
        }
    }
}
