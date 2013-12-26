namespace HsinChu.DailyLife
{
    partial class DailyLifeInspect
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
            this.components = new System.ComponentModel.Container();
            this.listViewEx1 = new JHSchool.Behavior.Legacy.ListViewEx();
            this.ColID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColClass = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColTeacher = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.btnPrint = new DevComponents.DotNetBar.ButtonX();
            this.btnRefresh = new DevComponents.DotNetBar.ButtonX();
            this.btnExit = new DevComponents.DotNetBar.ButtonX();
            this.txtSchoolYear = new DevComponents.DotNetBar.LabelX();
            this.txtSemester = new DevComponents.DotNetBar.LabelX();
            this.cbViewNotClass = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.txtSelect = new DevComponents.DotNetBar.LabelX();
            this.cbxSelect = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.intSchoolYear = new DevComponents.Editors.IntegerInput();
            this.intSemester = new DevComponents.Editors.IntegerInput();
            this.txtHelp = new DevComponents.DotNetBar.LabelX();
            this.txtTempCount = new DevComponents.DotNetBar.LabelX();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.intSchoolYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.intSemester)).BeginInit();
            this.SuspendLayout();
            // 
            // listViewEx1
            // 
            this.listViewEx1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.listViewEx1.Border.Class = "ListViewBorder";
            this.listViewEx1.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.listViewEx1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColID,
            this.ColClass,
            this.ColTeacher});
            this.listViewEx1.ContextMenuStrip = this.contextMenuStrip1;
            this.listViewEx1.FullRowSelect = true;
            this.listViewEx1.HideSelection = false;
            this.listViewEx1.Location = new System.Drawing.Point(11, 85);
            this.listViewEx1.Name = "listViewEx1";
            this.listViewEx1.Size = new System.Drawing.Size(619, 290);
            this.listViewEx1.TabIndex = 0;
            this.listViewEx1.UseCompatibleStateImageBehavior = false;
            this.listViewEx1.View = System.Windows.Forms.View.Details;
            // 
            // ColID
            // 
            this.ColID.Text = "班級ID";
            this.ColID.Width = 0;
            // 
            // ColClass
            // 
            this.ColClass.Text = "班級";
            this.ColClass.Width = 80;
            // 
            // ColTeacher
            // 
            this.ColTeacher.Text = "班導師";
            this.ColTeacher.Width = 90;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem1,
            this.ToolStripMenuItem2});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(163, 48);
            // 
            // ToolStripMenuItem1
            // 
            this.ToolStripMenuItem1.Name = "ToolStripMenuItem1";
            this.ToolStripMenuItem1.Size = new System.Drawing.Size(162, 22);
            this.ToolStripMenuItem1.Text = "加入[班級]待處理";
            this.ToolStripMenuItem1.Click += new System.EventHandler(this.ToolStripMenuItem1_Click);
            // 
            // ToolStripMenuItem2
            // 
            this.ToolStripMenuItem2.Name = "ToolStripMenuItem2";
            this.ToolStripMenuItem2.Size = new System.Drawing.Size(162, 22);
            this.ToolStripMenuItem2.Text = "清空[班級]待處理";
            this.ToolStripMenuItem2.Click += new System.EventHandler(this.ToolStripMenuItem2_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrint.AutoSize = true;
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnPrint.Location = new System.Drawing.Point(11, 381);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(83, 25);
            this.btnPrint.TabIndex = 1;
            this.btnPrint.Text = "匯出";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.BackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnRefresh.Location = new System.Drawing.Point(474, 381);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 25);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "重新整理";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnExit
            // 
            this.btnExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExit.Location = new System.Drawing.Point(555, 381);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 25);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "關閉";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // txtSchoolYear
            // 
            this.txtSchoolYear.AutoSize = true;
            this.txtSchoolYear.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.txtSchoolYear.BackgroundStyle.Class = "";
            this.txtSchoolYear.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtSchoolYear.Location = new System.Drawing.Point(14, 16);
            this.txtSchoolYear.Name = "txtSchoolYear";
            this.txtSchoolYear.Size = new System.Drawing.Size(47, 21);
            this.txtSchoolYear.TabIndex = 6;
            this.txtSchoolYear.Text = "學年度";
            // 
            // txtSemester
            // 
            this.txtSemester.AutoSize = true;
            this.txtSemester.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.txtSemester.BackgroundStyle.Class = "";
            this.txtSemester.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtSemester.Location = new System.Drawing.Point(157, 16);
            this.txtSemester.Name = "txtSemester";
            this.txtSemester.Size = new System.Drawing.Size(34, 21);
            this.txtSemester.TabIndex = 7;
            this.txtSemester.Text = "學期";
            // 
            // cbViewNotClass
            // 
            this.cbViewNotClass.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbViewNotClass.AutoSize = true;
            this.cbViewNotClass.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.cbViewNotClass.BackgroundStyle.Class = "";
            this.cbViewNotClass.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.cbViewNotClass.Location = new System.Drawing.Point(456, 51);
            this.cbViewNotClass.Name = "cbViewNotClass";
            this.cbViewNotClass.Size = new System.Drawing.Size(174, 21);
            this.cbViewNotClass.TabIndex = 8;
            this.cbViewNotClass.Text = "僅顯示未完成輸入之班級";
            this.cbViewNotClass.CheckedChanged += new System.EventHandler(this.cbViewNotClass_CheckedChanged);
            // 
            // txtSelect
            // 
            this.txtSelect.AutoSize = true;
            this.txtSelect.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.txtSelect.BackgroundStyle.Class = "";
            this.txtSelect.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtSelect.Location = new System.Drawing.Point(14, 51);
            this.txtSelect.Name = "txtSelect";
            this.txtSelect.Size = new System.Drawing.Size(101, 21);
            this.txtSelect.TabIndex = 9;
            this.txtSelect.Text = "請選擇評量項目";
            // 
            // cbxSelect
            // 
            this.cbxSelect.DisplayMember = "Text";
            this.cbxSelect.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbxSelect.FormattingEnabled = true;
            this.cbxSelect.ItemHeight = 19;
            this.cbxSelect.Location = new System.Drawing.Point(121, 49);
            this.cbxSelect.Name = "cbxSelect";
            this.cbxSelect.Size = new System.Drawing.Size(158, 25);
            this.cbxSelect.TabIndex = 10;
            this.cbxSelect.SelectedValueChanged += new System.EventHandler(this.cbxSelect_SelectedValueChanged);
            // 
            // intSchoolYear
            // 
            this.intSchoolYear.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.intSchoolYear.BackgroundStyle.Class = "DateTimeInputBackground";
            this.intSchoolYear.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.intSchoolYear.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.intSchoolYear.Location = new System.Drawing.Point(69, 14);
            this.intSchoolYear.MaxValue = 999;
            this.intSchoolYear.MinValue = 90;
            this.intSchoolYear.Name = "intSchoolYear";
            this.intSchoolYear.ShowUpDown = true;
            this.intSchoolYear.Size = new System.Drawing.Size(80, 25);
            this.intSchoolYear.TabIndex = 11;
            this.intSchoolYear.Value = 90;
            // 
            // intSemester
            // 
            this.intSemester.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.intSemester.BackgroundStyle.Class = "DateTimeInputBackground";
            this.intSemester.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.intSemester.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.intSemester.Location = new System.Drawing.Point(199, 14);
            this.intSemester.MaxValue = 2;
            this.intSemester.MinValue = 1;
            this.intSemester.Name = "intSemester";
            this.intSemester.ShowUpDown = true;
            this.intSemester.Size = new System.Drawing.Size(80, 25);
            this.intSemester.TabIndex = 12;
            this.intSemester.Value = 1;
            // 
            // txtHelp
            // 
            this.txtHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtHelp.AutoSize = true;
            this.txtHelp.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.txtHelp.BackgroundStyle.Class = "";
            this.txtHelp.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtHelp.Location = new System.Drawing.Point(115, 382);
            this.txtHelp.Name = "txtHelp";
            this.txtHelp.Size = new System.Drawing.Size(230, 21);
            this.txtHelp.TabIndex = 14;
            this.txtHelp.Text = "說明：本功能僅檢查(一般)狀態之學生";
            // 
            // txtTempCount
            // 
            this.txtTempCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTempCount.AutoSize = true;
            this.txtTempCount.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.txtTempCount.BackgroundStyle.Class = "";
            this.txtTempCount.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtTempCount.Location = new System.Drawing.Point(476, 16);
            this.txtTempCount.Name = "txtTempCount";
            this.txtTempCount.Size = new System.Drawing.Size(101, 21);
            this.txtTempCount.TabIndex = 15;
            this.txtTempCount.Text = "待處理班級數: 0";
            // 
            // DailyLifeInspect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(642, 416);
            this.Controls.Add(this.intSemester);
            this.Controls.Add(this.txtTempCount);
            this.Controls.Add(this.intSchoolYear);
            this.Controls.Add(this.cbxSelect);
            this.Controls.Add(this.txtSelect);
            this.Controls.Add(this.cbViewNotClass);
            this.Controls.Add(this.txtSemester);
            this.Controls.Add(this.txtHelp);
            this.Controls.Add(this.txtSchoolYear);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.listViewEx1);
            this.MaximizeBox = true;
            this.Name = "DailyLifeInspect";
            this.Text = "評等輸入狀況檢查";
            this.Load += new System.EventHandler(this.DailyLifeInspect_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.intSchoolYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.intSemester)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btnPrint;
        private DevComponents.DotNetBar.ButtonX btnRefresh;
        private DevComponents.DotNetBar.ButtonX btnExit;
        private DevComponents.DotNetBar.LabelX txtSchoolYear;
        private DevComponents.DotNetBar.LabelX txtSemester;
        private DevComponents.DotNetBar.Controls.CheckBoxX cbViewNotClass;
        private DevComponents.DotNetBar.LabelX txtSelect;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbxSelect;
        private System.Windows.Forms.ColumnHeader ColClass;
        private System.Windows.Forms.ColumnHeader ColTeacher;
        private JHSchool.Behavior.Legacy.ListViewEx listViewEx1;
        private DevComponents.Editors.IntegerInput intSchoolYear;
        private DevComponents.Editors.IntegerInput intSemester;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem2;
        private DevComponents.DotNetBar.LabelX txtHelp;
        private System.Windows.Forms.ColumnHeader ColID;
        private DevComponents.DotNetBar.LabelX txtTempCount;
    }
}