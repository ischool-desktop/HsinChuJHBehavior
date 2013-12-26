namespace HsinChu.DailyLife
{
    partial class HsinChuDailyLifeForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnSave = new DevComponents.DotNetBar.ButtonX();
            this.btnExit = new DevComponents.DotNetBar.ButtonX();
            this.gpDailyBehavior = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.dgvDailyBehavior = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lnkDailyBehavior = new System.Windows.Forms.LinkLabel();
            this.txtDailyBehavior = new System.Windows.Forms.TextBox();
            this.gpDailyLifeRecommend = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.lnkDailyLifeRecommend = new System.Windows.Forms.LinkLabel();
            this.txtDailyLifeRecommend = new System.Windows.Forms.TextBox();
            this.lnkOtherRecommend = new System.Windows.Forms.LinkLabel();
            this.gpOtherRecommend = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.txtOtherRecommend = new System.Windows.Forms.TextBox();
            this.gpDailyBehavior.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDailyBehavior)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSave.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.btnSave.Location = new System.Drawing.Point(585, 286);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(84, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "儲存";
            this.btnSave.Click += new System.EventHandler(this.butSave_Click);
            // 
            // btnExit
            // 
            this.btnExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExit.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.btnExit.Location = new System.Drawing.Point(680, 286);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(84, 23);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "關閉";
            this.btnExit.Click += new System.EventHandler(this.butExit_Click);
            // 
            // gpDailyBehavior
            // 
            this.gpDailyBehavior.BackColor = System.Drawing.Color.Transparent;
            this.gpDailyBehavior.CanvasColor = System.Drawing.SystemColors.Control;
            this.gpDailyBehavior.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.gpDailyBehavior.Controls.Add(this.dgvDailyBehavior);
            this.gpDailyBehavior.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.gpDailyBehavior.Location = new System.Drawing.Point(12, 7);
            this.gpDailyBehavior.Name = "gpDailyBehavior";
            this.gpDailyBehavior.Size = new System.Drawing.Size(755, 180);
            // 
            // 
            // 
            this.gpDailyBehavior.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.gpDailyBehavior.Style.BackColorGradientAngle = 90;
            this.gpDailyBehavior.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.gpDailyBehavior.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gpDailyBehavior.Style.BorderBottomWidth = 1;
            this.gpDailyBehavior.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.gpDailyBehavior.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gpDailyBehavior.Style.BorderLeftWidth = 1;
            this.gpDailyBehavior.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gpDailyBehavior.Style.BorderRightWidth = 1;
            this.gpDailyBehavior.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gpDailyBehavior.Style.BorderTopWidth = 1;
            this.gpDailyBehavior.Style.Class = "";
            this.gpDailyBehavior.Style.CornerDiameter = 4;
            this.gpDailyBehavior.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.gpDailyBehavior.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.gpDailyBehavior.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.gpDailyBehavior.StyleMouseDown.Class = "";
            this.gpDailyBehavior.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.gpDailyBehavior.StyleMouseOver.Class = "";
            this.gpDailyBehavior.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.gpDailyBehavior.TabIndex = 0;
            this.gpDailyBehavior.Text = "日常行為表現";
            // 
            // dgvDailyBehavior
            // 
            this.dgvDailyBehavior.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDailyBehavior.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.column2});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDailyBehavior.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvDailyBehavior.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgvDailyBehavior.Location = new System.Drawing.Point(6, 7);
            this.dgvDailyBehavior.Name = "dgvDailyBehavior";
            this.dgvDailyBehavior.RowTemplate.Height = 24;
            this.dgvDailyBehavior.Size = new System.Drawing.Size(738, 136);
            this.dgvDailyBehavior.TabIndex = 0;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "項目";
            this.Column1.Name = "Column1";
            this.Column1.Width = 150;
            // 
            // column2
            // 
            this.column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.column2.HeaderText = "指標";
            this.column2.Name = "column2";
            // 
            // lnkDailyBehavior
            // 
            this.lnkDailyBehavior.AutoSize = true;
            this.lnkDailyBehavior.BackColor = System.Drawing.Color.Transparent;
            this.lnkDailyBehavior.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.lnkDailyBehavior.Location = new System.Drawing.Point(694, 9);
            this.lnkDailyBehavior.Name = "lnkDailyBehavior";
            this.lnkDailyBehavior.Size = new System.Drawing.Size(60, 17);
            this.lnkDailyBehavior.TabIndex = 6;
            this.lnkDailyBehavior.TabStop = true;
            this.lnkDailyBehavior.Text = "修改名稱";
            this.lnkDailyBehavior.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkDailyBehavior_LinkClicked);
            // 
            // txtDailyBehavior
            // 
            this.txtDailyBehavior.Location = new System.Drawing.Point(16, 7);
            this.txtDailyBehavior.Name = "txtDailyBehavior";
            this.txtDailyBehavior.Size = new System.Drawing.Size(150, 22);
            this.txtDailyBehavior.TabIndex = 7;
            this.txtDailyBehavior.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDailyBehavior_KeyDown);
            // 
            // gpDailyLifeRecommend
            // 
            this.gpDailyLifeRecommend.BackColor = System.Drawing.Color.Transparent;
            this.gpDailyLifeRecommend.CanvasColor = System.Drawing.SystemColors.Control;
            this.gpDailyLifeRecommend.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.gpDailyLifeRecommend.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.gpDailyLifeRecommend.Location = new System.Drawing.Point(394, 203);
            this.gpDailyLifeRecommend.Name = "gpDailyLifeRecommend";
            this.gpDailyLifeRecommend.Size = new System.Drawing.Size(373, 59);
            // 
            // 
            // 
            this.gpDailyLifeRecommend.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.gpDailyLifeRecommend.Style.BackColorGradientAngle = 90;
            this.gpDailyLifeRecommend.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.gpDailyLifeRecommend.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gpDailyLifeRecommend.Style.BorderBottomWidth = 1;
            this.gpDailyLifeRecommend.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.gpDailyLifeRecommend.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gpDailyLifeRecommend.Style.BorderLeftWidth = 1;
            this.gpDailyLifeRecommend.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gpDailyLifeRecommend.Style.BorderRightWidth = 1;
            this.gpDailyLifeRecommend.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gpDailyLifeRecommend.Style.BorderTopWidth = 1;
            this.gpDailyLifeRecommend.Style.Class = "";
            this.gpDailyLifeRecommend.Style.CornerDiameter = 4;
            this.gpDailyLifeRecommend.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.gpDailyLifeRecommend.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.gpDailyLifeRecommend.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.gpDailyLifeRecommend.StyleMouseDown.Class = "";
            this.gpDailyLifeRecommend.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.gpDailyLifeRecommend.StyleMouseOver.Class = "";
            this.gpDailyLifeRecommend.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.gpDailyLifeRecommend.TabIndex = 14;
            this.gpDailyLifeRecommend.Text = "綜合評語";
            // 
            // lnkDailyLifeRecommend
            // 
            this.lnkDailyLifeRecommend.AutoSize = true;
            this.lnkDailyLifeRecommend.BackColor = System.Drawing.Color.Transparent;
            this.lnkDailyLifeRecommend.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.lnkDailyLifeRecommend.Location = new System.Drawing.Point(682, 206);
            this.lnkDailyLifeRecommend.Name = "lnkDailyLifeRecommend";
            this.lnkDailyLifeRecommend.Size = new System.Drawing.Size(60, 17);
            this.lnkDailyLifeRecommend.TabIndex = 15;
            this.lnkDailyLifeRecommend.TabStop = true;
            this.lnkDailyLifeRecommend.Text = "修改名稱";
            this.lnkDailyLifeRecommend.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkDailyLifeRecommend_LinkClicked);
            // 
            // txtDailyLifeRecommend
            // 
            this.txtDailyLifeRecommend.Location = new System.Drawing.Point(398, 203);
            this.txtDailyLifeRecommend.Name = "txtDailyLifeRecommend";
            this.txtDailyLifeRecommend.Size = new System.Drawing.Size(151, 22);
            this.txtDailyLifeRecommend.TabIndex = 16;
            this.txtDailyLifeRecommend.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDailyLifeRecommend_KeyDown);
            // 
            // lnkOtherRecommend
            // 
            this.lnkOtherRecommend.AutoSize = true;
            this.lnkOtherRecommend.BackColor = System.Drawing.Color.Transparent;
            this.lnkOtherRecommend.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.lnkOtherRecommend.Location = new System.Drawing.Point(298, 206);
            this.lnkOtherRecommend.Name = "lnkOtherRecommend";
            this.lnkOtherRecommend.Size = new System.Drawing.Size(60, 17);
            this.lnkOtherRecommend.TabIndex = 18;
            this.lnkOtherRecommend.TabStop = true;
            this.lnkOtherRecommend.Text = "修改名稱";
            this.lnkOtherRecommend.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkOtherRecommend_LinkClicked);
            // 
            // gpOtherRecommend
            // 
            this.gpOtherRecommend.BackColor = System.Drawing.Color.Transparent;
            this.gpOtherRecommend.CanvasColor = System.Drawing.SystemColors.Control;
            this.gpOtherRecommend.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.gpOtherRecommend.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.gpOtherRecommend.Location = new System.Drawing.Point(12, 203);
            this.gpOtherRecommend.Name = "gpOtherRecommend";
            this.gpOtherRecommend.Size = new System.Drawing.Size(371, 59);
            // 
            // 
            // 
            this.gpOtherRecommend.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.gpOtherRecommend.Style.BackColorGradientAngle = 90;
            this.gpOtherRecommend.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.gpOtherRecommend.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gpOtherRecommend.Style.BorderBottomWidth = 1;
            this.gpOtherRecommend.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.gpOtherRecommend.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gpOtherRecommend.Style.BorderLeftWidth = 1;
            this.gpOtherRecommend.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gpOtherRecommend.Style.BorderRightWidth = 1;
            this.gpOtherRecommend.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gpOtherRecommend.Style.BorderTopWidth = 1;
            this.gpOtherRecommend.Style.Class = "";
            this.gpOtherRecommend.Style.CornerDiameter = 4;
            this.gpOtherRecommend.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.gpOtherRecommend.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.gpOtherRecommend.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.gpOtherRecommend.StyleMouseDown.Class = "";
            this.gpOtherRecommend.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.gpOtherRecommend.StyleMouseOver.Class = "";
            this.gpOtherRecommend.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.gpOtherRecommend.TabIndex = 17;
            this.gpOtherRecommend.Text = "其他表現";
            // 
            // txtOtherRecommend
            // 
            this.txtOtherRecommend.Location = new System.Drawing.Point(16, 203);
            this.txtOtherRecommend.Name = "txtOtherRecommend";
            this.txtOtherRecommend.Size = new System.Drawing.Size(151, 22);
            this.txtOtherRecommend.TabIndex = 19;
            this.txtOtherRecommend.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtOtherRecommend_KeyDown);
            // 
            // HsinChuDailyLifeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 321);
            this.Controls.Add(this.lnkOtherRecommend);
            this.Controls.Add(this.gpOtherRecommend);
            this.Controls.Add(this.txtOtherRecommend);
            this.Controls.Add(this.lnkDailyLifeRecommend);
            this.Controls.Add(this.gpDailyLifeRecommend);
            this.Controls.Add(this.lnkDailyBehavior);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.gpDailyBehavior);
            this.Controls.Add(this.txtDailyBehavior);
            this.Controls.Add(this.txtDailyLifeRecommend);
            this.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "HsinChuDailyLifeForm";
            this.Text = "日常生活表現評量設定";
            this.gpDailyBehavior.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDailyBehavior)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btnSave;
        private DevComponents.DotNetBar.ButtonX btnExit;
        private DevComponents.DotNetBar.Controls.GroupPanel gpDailyBehavior;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgvDailyBehavior;
        private System.Windows.Forms.LinkLabel lnkDailyBehavior;
        private System.Windows.Forms.TextBox txtDailyBehavior;
        private DevComponents.DotNetBar.Controls.GroupPanel gpDailyLifeRecommend;
        private System.Windows.Forms.LinkLabel lnkDailyLifeRecommend;
        private System.Windows.Forms.TextBox txtDailyLifeRecommend;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn column2;
        private System.Windows.Forms.LinkLabel lnkOtherRecommend;
        private DevComponents.DotNetBar.Controls.GroupPanel gpOtherRecommend;
        private System.Windows.Forms.TextBox txtOtherRecommend;
    }
}