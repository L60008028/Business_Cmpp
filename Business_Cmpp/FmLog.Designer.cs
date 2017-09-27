namespace Business_Cmpp
{
    partial class FmLog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.cboxIsShowLog = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // rtbLog
            // 
            this.rtbLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbLog.BackColor = System.Drawing.SystemColors.Window;
            this.rtbLog.Location = new System.Drawing.Point(0, 0);
            this.rtbLog.MaxLength = 32767;
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.Size = new System.Drawing.Size(630, 272);
            this.rtbLog.TabIndex = 0;
            this.rtbLog.Text = "";
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(555, 276);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(72, 16);
            this.checkBox1.TabIndex = 1;
            this.checkBox1.Text = "自动滚动";
            this.checkBox1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // cboxIsShowLog
            // 
            this.cboxIsShowLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cboxIsShowLog.AutoSize = true;
            this.cboxIsShowLog.Checked = true;
            this.cboxIsShowLog.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cboxIsShowLog.Location = new System.Drawing.Point(463, 276);
            this.cboxIsShowLog.Name = "cboxIsShowLog";
            this.cboxIsShowLog.Size = new System.Drawing.Size(72, 16);
            this.cboxIsShowLog.TabIndex = 1;
            this.cboxIsShowLog.Text = "是否显示";
            this.cboxIsShowLog.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cboxIsShowLog.UseVisualStyleBackColor = true;
            this.cboxIsShowLog.CheckedChanged += new System.EventHandler(this.cboxIsShowLog_CheckedChanged);
            // 
            // FmLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(630, 295);
            this.Controls.Add(this.cboxIsShowLog);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.rtbLog);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FmLog";
            this.Text = "FmLog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox cboxIsShowLog;
    }
}