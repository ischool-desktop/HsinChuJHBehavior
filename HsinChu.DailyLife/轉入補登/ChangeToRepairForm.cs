using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using JHSchool.Data;
using System.Xml;
using Framework;
using JHSchool;
using FISCA.LogAgent;

namespace HsinChu.DailyLife
{
    public partial class ChangeToRepairForm : BaseForm
    {
        private string _ParKey;
        private Dictionary<string, int> DicNum = new Dictionary<string, int>(); //用來儲存每個ListView欄位定位

        public ChangeToRepairForm(string parkey)
        {
            InitializeComponent();

            _ParKey = parkey;

            JHMoralScore.AfterInsert += new EventHandler<K12.Data.DataChangedEventArgs>(JHMoralScore_AfterUpdate);
            JHMoralScore.AfterUpdate += new EventHandler<K12.Data.DataChangedEventArgs>(JHMoralScore_AfterUpdate);
            JHMoralScore.AfterDelete += new EventHandler<K12.Data.DataChangedEventArgs>(JHMoralScore_AfterUpdate);
        }

        void JHMoralScore_AfterUpdate(object sender, K12.Data.DataChangedEventArgs e)
        {
            BindData();
        }

        private void ChangeToRepairForm_Load(object sender, EventArgs e)
        {
            #region Load

            btnClear.Enabled = false;
            btnEdit.Enabled = false;

            K12.Data.Configuration.ConfigData cd = K12.Data.School.Configuration["DLBehaviorConfig"];

            //lbChangeInfo.Text = "(期中轉入補登預設" + JHSchool.School.DefaultSchoolYear + "學年度" + JHSchool.School.DefaultSemester + "學期)";

            lvPrefs.Columns.Clear();

            ColumnHeader CHSchoolYear = new ColumnHeader();
            CHSchoolYear.Text = "學年度";
            int SYNum = lvPrefs.Columns.Add(CHSchoolYear);
            DicNum.Add("學年度", SYNum);

            ColumnHeader CHSemester = new ColumnHeader();
            CHSemester.Text = "學期";
            int SeNum = lvPrefs.Columns.Add(CHSemester);
            DicNum.Add("學期", SeNum);

            if (cd.Contains("DailyBehavior"))
            {
                XmlElement dailyBehavior = XmlHelper.LoadXml(cd["DailyBehavior"]);

                foreach (XmlElement item in dailyBehavior.SelectNodes("Item"))
                {
                    ColumnHeader ColHeader = new ColumnHeader();
                    ColHeader.Text = item.GetAttribute("Name");
                    int Num = lvPrefs.Columns.Add(ColHeader);
                    DicNum.Add(item.GetAttribute("Name"), Num);
                }
            }

            BindData();

            #endregion
        }

        private void BindData()
        {
            lvPrefs.Items.Clear();

            #region 更新畫面內容
            List<JHMoralScoreRecord> MSRlist = new List<JHMoralScoreRecord>();

            MSRlist = JHMoralScore.SelectByStudentIDs(new List<string>() { _ParKey });

            MSRlist.Sort(new Comparison<JHMoralScoreRecord>(SchoolYearComparer));

            foreach (JHMoralScoreRecord record in MSRlist)
            {
                ListViewItem item = new ListViewItem();

                foreach (string each in DicNum.Keys)
                {
                    item.SubItems.Add("");
                }

                item.Tag = record;

                item.SubItems[DicNum["學年度"]].Text = record.SchoolYear.ToString();
                item.SubItems[DicNum["學期"]].Text = record.Semester.ToString();

                ////取得日常生活表現內容
                XmlElement Xmlnode = (XmlElement)record.TextScore.SelectSingleNode("DailyBehavior");

                if (Xmlnode == null)
                {
                    lvPrefs.Items.Add(item);
                    continue; ;
                }

                //填入值
                foreach (XmlElement snode in Xmlnode.SelectNodes("Item"))
                {
                    if (DicNum.ContainsKey(snode.GetAttribute("Name")))
                    {
                        item.SubItems[DicNum[snode.GetAttribute("Name")]].Text = snode.GetAttribute("Degree");
                    }
                }

                lvPrefs.Items.Add(item);

            } 
            #endregion
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lvPrefs.SelectedItems.Count == 0)
            {
                FISCA.Presentation.Controls.MsgBox.Show("請選擇一筆資料進行操作!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            JHMoralScoreRecord JHR = (JHMoralScoreRecord)lvPrefs.SelectedItems[0].Tag;
            ChangeToMoralScore CtoMoral = new ChangeToMoralScore(JHR);
            DialogResult dr = CtoMoral.ShowDialog();
            if (dr == DialogResult.OK)
            {
                btnClear.Enabled = false;
                btnEdit.Enabled = false;
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            ChangeToMoralScore CtoMoral = new ChangeToMoralScore(_ParKey);
            DialogResult dr = CtoMoral.ShowDialog();
            if (dr == DialogResult.OK)
            {
                btnClear.Enabled = false;
                btnEdit.Enabled = false;
            }
        }

        //private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //{
        //    #region 期中轉入補登Form
        //    ChangeToInitialSummary summary = new ChangeToInitialSummary(_ParKey);
        //    DialogResult dr = summary.ShowDialog();

        //    if (dr == DialogResult.OK)
        //    {
        //        btnClear.Enabled = false;
        //        btnEdit.Enabled = false;
        //    }

        //    if (!string.IsNullOrEmpty(summary.poSchoolYear) && !string.IsNullOrEmpty(summary.poSemester))
        //    {
        //        linkLabel1.Text = "期中轉入補登(已補登於 " + summary.poSchoolYear + " 學年度 第 " + summary.poSemester + " 學期)";
        //    }

        //    #endregion
        //}

        private void btnClear_Click(object sender, EventArgs e)
        {
            #region 刪除
            if (lvPrefs.SelectedItems.Count == 0)
            {
                FISCA.Presentation.Controls.MsgBox.Show("請選擇一筆資料進行操作!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult dr = FISCA.Presentation.Controls.MsgBox.Show("您確定要刪除該筆資料嗎?", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                JHMoralScoreRecord _editorRecord = lvPrefs.SelectedItems[0].Tag as JHMoralScoreRecord;
                try
                {
                    JHMoralScore.Delete(_editorRecord);
                }
                catch (Exception ex)
                {
                    FISCA.Presentation.Controls.MsgBox.Show("刪除資料發生錯誤!" + ex.Message);
                    return;
                }

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("詳細資料：");
                if (_editorRecord.Student.Class != null)
                {
                    sb.AppendLine("學生「" + _editorRecord.Student.Name + "」班級「" + _editorRecord.Student.Class.Name + "」座號「" + _editorRecord.Student.SeatNo + "」學號「" + _editorRecord.Student.StudentNumber + "」。");
                }
                else
                {
                    sb.AppendLine("學生「" + _editorRecord.Student.Name + "」學號「" + _editorRecord.Student.StudentNumber + "」。");
                }

                sb.AppendLine("學年度「" + _editorRecord.SchoolYear.ToString() + "」學期「" + _editorRecord.Semester.ToString() + "」");

                ApplicationLog.Log("日常生活表現模組.轉入補登", "刪除日常生活表現資料", "student", _editorRecord.Student.ID, "由「轉入補登」功能，刪除「日常生活表現」資料。\n" + sb.ToString());
                
                btnClear.Enabled = false;
                btnEdit.Enabled = false;
            
            }
            else //取消則不處理
            {
            }


            #endregion
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lvPrefs_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region ListView選擇的事件
            if (lvPrefs.SelectedItems.Count == 1)
            {
                btnClear.Enabled = true;
                btnEdit.Enabled = true;
            }
            else if (lvPrefs.SelectedItems.Count == 0)
            {
                btnClear.Enabled = false;
                btnEdit.Enabled = false;
            }
            else if (lvPrefs.SelectedItems.Count > 1)
            {
                btnClear.Enabled = false;
                btnEdit.Enabled = false;
            } 
            #endregion
        }

        private int SchoolYearComparer(JHMoralScoreRecord x, JHMoralScoreRecord y)
        {
            #region 排序
            string xx = x.SchoolYear.ToString() + x.Semester.ToString();
            string yy = y.SchoolYear.ToString() + y.Semester.ToString();
            return xx.CompareTo(yy); 
            #endregion
        }

        private void lvPrefs_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvPrefs.SelectedItems.Count == 1)
            {
                JHMoralScoreRecord JHR = (JHMoralScoreRecord)lvPrefs.SelectedItems[0].Tag;
                ChangeToMoralScore CtoMoral = new ChangeToMoralScore(JHR);
                DialogResult dr = CtoMoral.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    btnClear.Enabled = false;
                    btnEdit.Enabled = false;
                }
            }
        }
    }
}
