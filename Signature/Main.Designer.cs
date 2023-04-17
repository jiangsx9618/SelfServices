using eCTDViewer4.DBControls;

namespace Signature
{
    partial class Main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.text_msg = new System.Windows.Forms.TextBox();
            this.lab_listem = new System.Windows.Forms.Label();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.显示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.清空报警ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_clerAll = new eCTDViewer4.DBControls.MyButton();
            this.btn_min = new eCTDViewer4.DBControls.MyButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // text_msg
            // 
            this.text_msg.Font = new System.Drawing.Font("宋体", 10F);
            this.text_msg.Location = new System.Drawing.Point(2, 76);
            this.text_msg.Margin = new System.Windows.Forms.Padding(4);
            this.text_msg.Multiline = true;
            this.text_msg.Name = "text_msg";
            this.text_msg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.text_msg.Size = new System.Drawing.Size(998, 560);
            this.text_msg.TabIndex = 3;
            this.text_msg.DoubleClick += new System.EventHandler(this.text_msg_DoubleClick);
            // 
            // lab_listem
            // 
            this.lab_listem.BackColor = System.Drawing.Color.Lime;
            this.lab_listem.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lab_listem.ForeColor = System.Drawing.Color.White;
            this.lab_listem.Location = new System.Drawing.Point(94, 18);
            this.lab_listem.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lab_listem.Name = "lab_listem";
            this.lab_listem.Size = new System.Drawing.Size(134, 40);
            this.lab_listem.TabIndex = 8;
            this.lab_listem.Text = "监 控 服 务";
            this.lab_listem.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "报警监控";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.显示ToolStripMenuItem,
            this.清空报警ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 64);
            // 
            // 显示ToolStripMenuItem
            // 
            this.显示ToolStripMenuItem.Name = "显示ToolStripMenuItem";
            this.显示ToolStripMenuItem.Size = new System.Drawing.Size(152, 30);
            this.显示ToolStripMenuItem.Text = "显示";
            this.显示ToolStripMenuItem.Click += new System.EventHandler(this.显示ToolStripMenuItem_Click);
            // 
            // 清空报警ToolStripMenuItem
            // 
            this.清空报警ToolStripMenuItem.Name = "清空报警ToolStripMenuItem";
            this.清空报警ToolStripMenuItem.Size = new System.Drawing.Size(152, 30);
            this.清空报警ToolStripMenuItem.Text = "清空报警";
            this.清空报警ToolStripMenuItem.Click += new System.EventHandler(this.清空报警ToolStripMenuItem_Click);
            // 
            // btn_clerAll
            // 
            this.btn_clerAll.BackColor = System.Drawing.Color.Transparent;
            this.btn_clerAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_clerAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_clerAll.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_clerAll.ForeColor = System.Drawing.Color.White;
            this.btn_clerAll.Location = new System.Drawing.Point(692, 10);
            this.btn_clerAll.Name = "btn_clerAll";
            this.btn_clerAll.Size = new System.Drawing.Size(148, 48);
            this.btn_clerAll.TabIndex = 9;
            this.btn_clerAll.Text = "清  空";
            this.btn_clerAll.UseVisualStyleBackColor = false;
            this.btn_clerAll.Click += new System.EventHandler(this.btn_clerAll_Click);
            // 
            // btn_min
            // 
            this.btn_min.BackColor = System.Drawing.Color.Transparent;
            this.btn_min.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_min.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_min.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_min.ForeColor = System.Drawing.Color.White;
            this.btn_min.Location = new System.Drawing.Point(846, 10);
            this.btn_min.Name = "btn_min";
            this.btn_min.Size = new System.Drawing.Size(144, 48);
            this.btn_min.TabIndex = 10;
            this.btn_min.Text = "隐  藏";
            this.btn_min.UseVisualStyleBackColor = true;
            this.btn_min.Click += new System.EventHandler(this.btn_min_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::SelfService.Properties.Resources.computer_protection;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(12, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(64, 64);
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1006, 643);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btn_min);
            this.Controls.Add(this.btn_clerAll);
            this.Controls.Add(this.lab_listem);
            this.Controls.Add(this.text_msg);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Main";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "报警监控";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Signature_FormClosing);
            this.Load += new System.EventHandler(this.Signature_Load);
            this.SizeChanged += new System.EventHandler(this.Signature_SizeChanged);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox text_msg;
        private System.Windows.Forms.Label lab_listem;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 显示ToolStripMenuItem;
        private MyButton btn_clerAll;
        private MyButton btn_min;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripMenuItem 清空报警ToolStripMenuItem;
    }
}

