using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Animation;

namespace eCTDViewer4.DBControls
{
    public partial class MyButton : Button
    {
        public MyButton()
        {
            InitializeComponent();
        }
        /// <summary>
        /// fix 35638 去除image btn时焦点黑框
        /// </summary>
        protected override bool ShowFocusCues
        {
            get { return false; }
        }
        bool roundCorner = true;
        Color crBorderPainting = Color.FromArgb(34, 75, 143);
        int radius = 50;
        protected override void OnPaint(PaintEventArgs pe)
        {
            if (!roundCorner)
            {
                base.OnPaint(pe);
                return;
            }
            Graphics g = pe.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            //g.SmoothingMode = SmoothingMode.HighQuality;
            //g.CompositingQuality = CompositingQuality.HighQuality;
            //g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            Rectangle rect = this.ClientRectangle;
            Brush brhBorder = new SolidBrush(crBorderPainting);
            Brush brhRect = new SolidBrush(BackColor);
            Brush b0 = new SolidBrush(this.Parent.BackColor);
            Brush bfont = new SolidBrush(ForeColor);
            try
            {
                g.Clear(this.Parent.BackColor);
                int borderSize = FlatAppearance.BorderSize;
                try
                {
                    GraphicsPath path = CreateRoundRect(rect.Left, rect.Top, rect.Left + rect.Width - borderSize, rect.Top + rect.Height - borderSize);
                    g.FillPath(brhBorder, path);
                    path.Dispose();
                    path = CreateRoundRect(rect.Left + borderSize / 2f, rect.Top + borderSize / 2f, rect.Left + rect.Width - borderSize * 2, rect.Top + rect.Height - borderSize * 2);
                    g.FillPath(brhRect, path);
                    path.Dispose();
                }
                catch (Exception e)
                {
                    Console.WriteLine("FillPath:" + e.Message);
                }
                if (this.Text != string.Empty)
                {
                    StringFormat sf = StringFormat.GenericTypographic;
                    sf.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
                    SizeF sizeoftext = g.MeasureString(this.Text, Font);
                    float tx = (float)((this.Width - sizeoftext.Width) / 2.0);
                    float ty = (float)((this.Height - sizeoftext.Height) / 2.0);
                    g.DrawString(this.Text, Font, bfont, tx, ty);
                }
            }
            finally
            {
                b0.Dispose();
                brhBorder.Dispose();
                brhRect.Dispose();
                bfont.Dispose();
            }
        }
        private GraphicsPath CreateRoundRect(float rleft, float rtop, float rwidth, float rheight)
        {
            float r = radius;
            if (rwidth < rheight)
            {
                if (radius > rwidth / 2f)
                    r = rwidth / 2f;
            }
            else
            {
                if (radius > rheight / 2f)
                    r = rheight / 2f;
            }

            GraphicsPath path;
            RectangleF rectRow = new RectangleF(rleft, rtop + r, rwidth, rheight - r * 2);
            RectangleF rectColumn = new RectangleF(rleft + r, rtop, rwidth - r * 2, rheight);
            path = new GraphicsPath(FillMode.Winding);
            path.AddRectangle(rectRow);
            path.AddRectangle(rectColumn);
            //左上
            path.AddEllipse(rleft, rtop, r * 2, r * 2);
            //右上
            path.AddEllipse(rleft + rwidth - r * 2, rtop, r * 2, r * 2);
            //左下
            path.AddEllipse(rleft, rtop + rheight - r * 2, r * 2, r * 2);
            //右下
            path.AddEllipse(rleft + rwidth - r * 2, rtop + rheight - r * 2, r * 2, r * 2);
            return path;
        }
        private Color crBorderActive = Color.Orange;
        private Color crRectActive = Color.FromArgb(74,137,200);
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            this.BackColor = crRectActive;
            this.FlatAppearance.BorderColor = crBorderActive;
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.BackColor = crBorderPainting;
            this.FlatAppearance.BorderColor = crBorderActive;
        }
    }
}
