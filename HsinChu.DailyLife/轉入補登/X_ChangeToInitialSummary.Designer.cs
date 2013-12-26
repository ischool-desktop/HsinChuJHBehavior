namespace HsinChu.DailyLife
{
    partial class ChangeToInitialSummary
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridViewX1 = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.btnSave = new DevComponents.DotNetBar.ButtonX();
            this.btnClose = new DevComponents.DotNetBar.ButtonX();
            this.lbHelpTxt1 = new DevComponents.DotNetBar.LabelX();
            this.gpMeritDemerit = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.gpAttendance = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.dataGridViewX3 = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.cbSchoolYear = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cbSemester = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).BeginInit();
            this.gpMeritDemerit.SuspendLayout();
            this.gpAttendance.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX3)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewX1
            // 
            this.dataGridViewX1.AllowUserToAddRows = false;
            this.dataGridViewX1.AllowUserToDeleteRows = false;
            this.dataGridViewX1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewX1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewX1.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewX1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataGridViewX1.Location = new System.Drawing.Point(5, 4);
            this.dataGridViewX1.Name = "dataGridViewX1";
            this.dataGridViewX1.RowHeadersVisible = false;
            this.dataGridViewX1.RowTemplate.Height = 24;
            this.dataGridViewX1.Size = new System.Drawing.Size(522, 95);
            this.dataGridViewX1.TabIndex = 0;
            this.dataGridViewX1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewX1_CellValueChanged);
            // 
            // btnSave
            // 
            this.btnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSave.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.btnSave.Location = new System.Drawing.Point(373, 330);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "儲存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnClose.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.btnClose.Location = new System.Drawing.Point(464, 330);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "離開";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lbHelpTxt1
            // 
            this.lbHelpTxt1.AutoSize = true;
            this.lbHelpTxt1.BackColor = System.Drawing.Color.Transparent;
            this.lbHelpTxt1.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.lbHelpTxt1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lbHelpTxt1.Location = new System.Drawing.Point(332, 10);
            this.lbHelpTxt1.Name = "lbHelpTxt1";
            this.lbHelpTxt1.Size = new System.Drawing.Size(192, 21);
            this.lbHelpTxt1.TabIndex = 4;
            this.lbHelpTxt1.Text = "請輸入期中缺曠/獎/懲統計內容";
            // 
            // gpMeritDemerit
            // 
            this.gpMeritDemerit.BackColor = System.Drawing.Color.Transparent;
            this.gpMeritDemerit.CanvasColor = System.Drawing.SystemColors.Control;
            this.gpMeritDemerit.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.gpMeritDemerit.Controls.Add(this.dataGridViewX1);
            this.gpMeritDemerit.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.gpMeritDemerit.Location = new System.Drawing.Point(9, 192);
            this.gpMeritDemerit.Name = "gpMeritDemerit";
            this.gpMeritDemerit.Size = new System.Drawing.Size(539, 130);
            // 
            // 
            // 
            this.gpMeritDemerit.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.gpMeritDemerit.Style.BackColorGradientAngle = 90;
            this.gpMeritDemerit.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.gpMeritDemerit.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gpMeritDemerit.Style.BorderBottomWidth = 1;
            this.gpMeritDemerit.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.gpMeritDemerit.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gpMeritDemerit.Style.BorderLeftWidth = 1;
            this.gpMeritDemerit.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gpMeritDemerit.Style.BorderRightWidth = 1;
            this.gpMeritDemerit.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gpMeritDemerit.Style.BorderTopWidth = 1;
            this.gpMeritDemerit.Style.CornerDiameter = 4;
            this.gpMeritDemerit.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.gpMeritDemerit.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.gpMeritDemerit.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.gpMeritDemerit.TabIndex = 5;
            this.gpMeritDemerit.Text = "期中獎懲統計";
            // 
            // gpAttendance
            // 
            this.gpAttendance.BackColor = System.Drawing.Color.Transparent;
            this.gpAttendance.CanvasColor = System.Drawing.SystemColors.Control;
            this.gpAttendance.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.gpAttendance.Controls.Add(this.dataGridViewX3);
            this.gpAttendance.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.gpAttendance.Location = new System.Drawing.Point(9, 41);
            this.gpAttendance.Name = "gpAttendance";
            this.gpAttendance.Size = new System.Drawing.Size(539, 145);
            // 
            // 
            // 
            this.gpAttendance.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.gpAttendance.Style.BackColorGradientAngle = 90;
            this.gpAttendance.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.gpAttendance.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gpAttendance.Style.BorderBottomWidth = 1;
            this.gpAttendance.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.gpAttendance.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gpAttendance.Style.BorderLeftWidth = 1;
            this.gpAttendance.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gpAttendance.Style.BorderRightWidth = 1;
            this.gpAttendance.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gpAttendance.Style.BorderTopWidth = 1;
            this.gpAttendance.Style.CornerDiameter = 4;
            this.gpAttendance.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.gpAttendance.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.gpAttendance.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.gpAttendance.TabIndex = 6;
            this.gpAttendance.Text = "期中缺曠統計";
            // 
            // dataGridViewX3
            // 
            this.dataGridViewX3.AllowUserToAddRows = false;
            this.dataGridViewX3.AllowUserToDeleteRows = false;
            this.dataGridViewX3.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewX3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewX3.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewX3.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataGridViewX3.Location = new System.Drawing.Point(5, 4);
            this.dataGridViewX3.Name = "dataGridViewX3";
            this.dataGridViewX3.RowHeadersVisible = false;
            this.dataGridViewX3.RowTemplate.Height = 24;
            this.dataGridViewX3.Size = new System.Drawing.Size(522, 110);
            this.dataGridViewX3.TabIndex = 0;
            this.dataGridViewX3.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewX3_CellValueChanged);
            // 
            // cbSchoolYear
            // 
            this.cbSchoolYear.DisplayMember = "Text";
            this.cbSchoolYear.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbSchoolYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSchoolYear.FormattingEnabled = true;
            this.cbSchoolYear.ItemHeight = 16;
            this.cbSchoolYear.Location = new System.Drawing.Point(68, 9);
            this.cbSchoolYear.Name = "cbSchoolYear";
            this.cbSchoolYear.Size = new System.Drawing.Size(69, 22);
            this.cbSchoolYear.TabIndex = 7;
            this.cbSchoolYear.SelectedIndexChanged += new System.EventHandler(this.cbSchoolYear_SelectedIndexChanged);
            // 
            // cbSemester
            // 
            this.cbSemester.DisplayMember = "Text";
            this.cbSemester.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbSemester.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSemester.FormattingEnabled = true;
            this.cbSemester.ItemHeight = 16;
            this.cbSemester.Location = new System.Drawing.Point(182, 9);
            this.cbSemester.Name = "cbSemester";
            this.cbSemester.Size = new System.Drawing.Size(78, 22);
            this.cbSemester.TabIndex = 8;
            this.cbSemester.SelectedIndexChanged += new System.EventHandler(this.cbSemester_SelectedIndexChanged);
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            this.labelX1.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.labelX1.Location = new System.Drawing.Point(17, 11);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(47, 21);
            this.labelX1.TabIndex = 9;
            this.labelX1.Text = "學年度";
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            this.labelX2.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.labelX2.Location = new System.Drawing.Point(144, 11);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(34, 21);
            this.labelX2.TabIndex = 10;
            this.labelX2.Text = "學期";
            // 
            // ChangeToInitialSummary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 362);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.cbSemester);
            this.Controls.Add(this.cbSchoolYear);
            this.Controls.Add(this.gpAttendance);
            this.Controls.Add(this.gpMeritDemerit);
            this.Controls.Add(this.lbHelpTxt1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Name = "ChangeToInitialSummary";
            this.Text = "期中轉入補登";
            this.Load += new System.EventHandler(this.ChangeToInitialSummary_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).EndInit();
            this.gpMeritDemerit.ResumeLayout(false);
            this.gpAttendance.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.Controls.DataGridViewX dataGridViewX1;
        private DevComponents.DotNetBar.ButtonX btnSave;
        private DevComponents.DotNetBar.ButtonX btnClose;
        private DevComponents.DotNetBar.LabelX lbHelpTxt1;
        private DevComponents.DotNetBar.Controls.GroupPanel gpMeritDemerit;
        private DevComponents.DotNetBar.Controls.GroupPanel gpAttendance;
        private DevComponents.DotNetBar.Controls.DataGridViewX dataGridViewX3;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbSchoolYear;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbSemester;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
    }
}