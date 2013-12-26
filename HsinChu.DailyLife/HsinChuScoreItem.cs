using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Framework;
using System.Xml;
using FCode = Framework.Security.FeatureCodeAttribute;
using JHSchool.Data;
using FISCA.Presentation.Controls;
using FISCA.Presentation;
using FISCA.LogAgent;

namespace HsinChu.DailyLife
{
    [FCode("JHSchool.Student.Detail0060", "日常生活表現")]
    public partial class HsinChuScoreItem : DetailContent
    {
        //<TextScore>
        //    <DailyBehavior Name="日常行為表現">
        //        <Item Name="愛整潔" Index="指標" Degree="非常符合"/>
        //        <Item Name="守秩序" Index="指標" Degree="尚可符合"/>
        //    </DailyBehavior>

        //    <DailyLifeRecommend Name="日常生活表現具體建議" Description="內容">

        //    <OtherRecommend Name="其他表現建議" Description="內容"/>
        //</TextScore>

        private List<JHMoralScoreRecord> _records = new List<JHMoralScoreRecord>();

        internal static Framework.Security.FeatureAce UserPermission;
        private Dictionary<string, int> DicNum = new Dictionary<string, int>();

        BackgroundWorker BgW = new BackgroundWorker();

        bool BkWBool = false;

        public HsinChuScoreItem()
        {
            InitializeComponent();

            Group = "日常生活表現";

            JHMoralScore.AfterDelete += new EventHandler<K12.Data.DataChangedEventArgs>(JHMoralScore_AfterDelete);
            JHMoralScore.AfterInsert += new EventHandler<K12.Data.DataChangedEventArgs>(JHMoralScore_AfterDelete);
            JHMoralScore.AfterUpdate += new EventHandler<K12.Data.DataChangedEventArgs>(JHMoralScore_AfterDelete);

            BgW.DoWork += new DoWorkEventHandler(BkW_DoWork);
            BgW.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BkW_RunWorkerCompleted);

            UserPermission = User.Acl[FCode.GetCode(GetType())];

            if (UserPermission.Editable)
            {
                btnEdit.Visible = UserPermission.Editable;
                btnNew.Visible = UserPermission.Editable;
                btnClear.Visible = UserPermission.Editable;
                btnView.Visible = false;
            }
            else
            {
                btnEdit.Visible = UserPermission.Editable;
                btnNew.Visible = UserPermission.Editable;
                btnClear.Visible = UserPermission.Editable;
                btnView.Visible = true;
            }

        }

        void JHMoralScore_AfterDelete(object sender, K12.Data.DataChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<object, K12.Data.DataChangedEventArgs>(JHMoralScore_AfterDelete), sender, e);
            }
            else
            {
                if (this.PrimaryKey != "")
                {
                    if (!BgW.IsBusy)
                        BgW.RunWorkerAsync();
                }
            }
        }


        protected override void OnPrimaryKeyChanged(EventArgs e)
        {
            this.Loading = true;

            if (BgW.IsBusy)
            {
                BkWBool = true;
            }
            else
            {
                BgW.RunWorkerAsync();
            }
        }

        void BkW_DoWork(object sender, DoWorkEventArgs e)
        {
            _records.Clear();

            _records = JHMoralScore.SelectByStudentIDs(new string[] { this.PrimaryKey });

            //string[] xyz = new string[1];
            //xyz[0] = this.PrimaryKey;
        }

        void BkW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
            if (BkWBool)
            {
                BkWBool = false;
                BgW.RunWorkerAsync();
                return;
            }

            BindData();

            this.Loading = false;
        }

        /// <summary>
        /// 更新資料項目之規格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DLScoreItem_Load(object sender, EventArgs e)
        {
            K12.Data.Configuration.ConfigData cd = K12.Data.School.Configuration["DLBehaviorConfig"];

            lvPrefs.Columns.Clear();
            DicNum.Clear();

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
        }


        /// <summary>
        /// 更新資料項目之內容
        /// </summary>
        private void BindData()
        {
            lvPrefs.Items.Clear();

            _records.Sort(new Comparison<JHMoralScoreRecord>(SchoolYearComparer));

            foreach (JHMoralScoreRecord record in _records)
            {
                if (record.TextScore == null)
                    continue;

                if (string.IsNullOrEmpty(record.TextScore.InnerXml))
                    continue;

                //學年度
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


                    //ListViewItem.ListViewSubItem TimSubitem = new ListViewItem.ListViewSubItem(item, "Text");
                    //TimSubitem.Text = snode.GetAttribute("Degree");
                    //item.SubItems.Add(TimSubitem);
                }

                lvPrefs.Items.Add(item);
            }
            

            if (lvPrefs.Items.Count <= 0)
            {
                btnEdit.Enabled = false;
            }
            else
            {
                btnEdit.Enabled = true;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lvPrefs.SelectedItems.Count != 0)
            {
                JHMoralScoreRecord _editorRecord = lvPrefs.SelectedItems[0].Tag as JHMoralScoreRecord;

                DLScoreEditForm editor = new DLScoreEditForm(_editorRecord, UserPermission);

                editor.ShowDialog();
            }
            else
            {
                FISCA.Presentation.Controls.MsgBox.Show("請選擇一個項目");

                return;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (lvPrefs.SelectedItems.Count != 0)
            {
                JHMoralScoreRecord _editorRecord = lvPrefs.SelectedItems[0].Tag as JHMoralScoreRecord;

                DialogResult DR = new DialogResult();

                DR = FISCA.Presentation.Controls.MsgBox.Show("是否要刪除此筆記錄?", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);

                if (DR == DialogResult.Yes)
                {
                    try
                    {
                        JHMoralScore.Delete(_editorRecord);
                    }
                    catch
                    {
                        FISCA.Presentation.Controls.MsgBox.Show("刪除資料發生錯誤");
                        return;
                    }
                    ApplicationLog.Log("日常生活表現模組.資料項目", "刪除日常生活表現資料", "student", _editorRecord.Student.ID, "學生「" + _editorRecord.Student.Name + "」學年度「" + _editorRecord.SchoolYear.ToString() + "」學期「" + _editorRecord.Semester.ToString() + "」，日常生活表現資料已被刪除。");
                }
                else
                {
                    return;
                }
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            DLScoreEditForm editor = new DLScoreEditForm(this.PrimaryKey, UserPermission);

            editor.ShowDialog();
        }

        private int SchoolYearComparer(JHMoralScoreRecord x, JHMoralScoreRecord y)
        {
            string xx = x.SchoolYear.ToString() + x.Semester.ToString();
            string yy = y.SchoolYear.ToString() + y.Semester.ToString();
            return xx.CompareTo(yy);
        }

        private void btnView_Click_1(object sender, EventArgs e)
        {
            btnEdit_Click(sender, e);
        }

        private void lvPrefs_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvPrefs.SelectedItems.Count == 1)
            {
                JHMoralScoreRecord _editorRecord = lvPrefs.SelectedItems[0].Tag as JHMoralScoreRecord;

                DLScoreEditForm editor = new DLScoreEditForm(_editorRecord, UserPermission);

                editor.ShowDialog();
            }
        }
    }
}
