namespace HsinChu.DailyLife.日常生活表現總表.Calculation
{
    partial class GraduationPredictReportForm
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
            this.chCondition3 = new System.Windows.Forms.CheckBox();
            this.chCondition4 = new System.Windows.Forms.CheckBox();
            this.chCondition5 = new System.Windows.Forms.CheckBox();
            this.chCondition6 = new System.Windows.Forms.CheckBox();
            this.gpDaily = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.btnPrint = new DevComponents.DotNetBar.ButtonX();
            this.gpDaily.SuspendLayout();
            this.SuspendLayout();
            // 
            // chCondition3
            // 
            this.chCondition3.AutoSize = true;
            this.chCondition3.BackColor = System.Drawing.Color.Transparent;
            this.chCondition3.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.chCondition3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(133)))));
            this.chCondition3.Location = new System.Drawing.Point(39, 42);
            this.chCondition3.Name = "chCondition3";
            this.chCondition3.Size = new System.Drawing.Size(170, 21);
            this.chCondition3.TabIndex = 4;
            this.chCondition3.Tag = "AbsenceAmountEach";
            this.chCondition3.Text = "缺課節數超過指定節數。";
            this.chCondition3.UseVisualStyleBackColor = false;
            // 
            // chCondition4
            // 
            this.chCondition4.AutoSize = true;
            this.chCondition4.BackColor = System.Drawing.Color.Transparent;
            this.chCondition4.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.chCondition4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(133)))));
            this.chCondition4.Location = new System.Drawing.Point(39, 73);
            this.chCondition4.Name = "chCondition4";
            this.chCondition4.Size = new System.Drawing.Size(209, 21);
            this.chCondition4.TabIndex = 5;
            this.chCondition4.Tag = "AbsenceAmountEachFraction";
            this.chCondition4.Text = "缺課節數超過總節數指定比例。";
            this.chCondition4.UseVisualStyleBackColor = false;
            // 
            // chCondition5
            // 
            this.chCondition5.AutoSize = true;
            this.chCondition5.BackColor = System.Drawing.Color.Transparent;
            this.chCondition5.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.chCondition5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(133)))));
            this.chCondition5.Location = new System.Drawing.Point(39, 104);
            this.chCondition5.Name = "chCondition5";
            this.chCondition5.Size = new System.Drawing.Size(144, 21);
            this.chCondition5.TabIndex = 6;
            this.chCondition5.Tag = "DemeritAmountEach";
            this.chCondition5.Text = "懲戒次數合計超次。";
            this.chCondition5.UseVisualStyleBackColor = false;
            // 
            // chCondition6
            // 
            this.chCondition6.AutoSize = true;
            this.chCondition6.BackColor = System.Drawing.Color.Transparent;
            this.chCondition6.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.chCondition6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(133)))));
            this.chCondition6.Location = new System.Drawing.Point(39, 135);
            this.chCondition6.Name = "chCondition6";
            this.chCondition6.Size = new System.Drawing.Size(209, 21);
            this.chCondition6.TabIndex = 7;
            this.chCondition6.Tag = "DailyBehavior";
            this.chCondition6.Text = "日常行為表現指標未符合標準。";
            this.chCondition6.UseVisualStyleBackColor = false;
            // 
            // gpDaily
            // 
            this.gpDaily.BackColor = System.Drawing.Color.Transparent;
            this.gpDaily.CanvasColor = System.Drawing.SystemColors.Control;
            this.gpDaily.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.gpDaily.Controls.Add(this.labelX2);
            this.gpDaily.Controls.Add(this.chCondition3);
            this.gpDaily.Controls.Add(this.chCondition4);
            this.gpDaily.Controls.Add(this.chCondition6);
            this.gpDaily.Controls.Add(this.chCondition5);
            this.gpDaily.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.gpDaily.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.gpDaily.Location = new System.Drawing.Point(9, 13);
            this.gpDaily.Name = "gpDaily";
            this.gpDaily.Size = new System.Drawing.Size(312, 194);
            // 
            // 
            // 
            this.gpDaily.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.gpDaily.Style.BackColorGradientAngle = 90;
            this.gpDaily.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.gpDaily.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gpDaily.Style.BorderBottomWidth = 1;
            this.gpDaily.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.gpDaily.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gpDaily.Style.BorderLeftWidth = 1;
            this.gpDaily.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gpDaily.Style.BorderRightWidth = 1;
            this.gpDaily.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gpDaily.Style.BorderTopWidth = 1;
            this.gpDaily.Style.CornerDiameter = 4;
            this.gpDaily.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.gpDaily.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.gpDaily.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.gpDaily.TabIndex = 3;
            this.gpDaily.Text = "日常生活表現";
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            this.labelX2.Location = new System.Drawing.Point(14, 10);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(116, 21);
            this.labelX2.TabIndex = 9;
            this.labelX2.Text = "0學年度　第0學期";
            // 
            // btnPrint
            // 
            this.btnPrint.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnPrint.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.btnPrint.Location = new System.Drawing.Point(246, 217);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 23);
            this.btnPrint.TabIndex = 8;
            this.btnPrint.Text = "列印";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // GraduationPredictReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 246);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.gpDaily);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "GraduationPredictReportForm";
            this.Text = "及格設定畫面";
            this.Load += new System.EventHandler(this.GraduationPredictReportForm_Load);
            this.gpDaily.ResumeLayout(false);
            this.gpDaily.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chCondition3;
        private System.Windows.Forms.CheckBox chCondition4;
        private System.Windows.Forms.CheckBox chCondition5;
        private System.Windows.Forms.CheckBox chCondition6;
        private DevComponents.DotNetBar.Controls.GroupPanel gpDaily;
        private DevComponents.DotNetBar.ButtonX btnPrint;
        private DevComponents.DotNetBar.LabelX labelX2;
    }
}