using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Framework.Feature;
using JHSchool.Data;
using FISCA.Presentation.Controls;
using K12.Data;
using FISCA.DSAUtil;
using FISCA.LogAgent;
using Framework;

namespace HsinChu.DailyLife
{
    public partial class DLScoreEditForm : BaseForm
    {
        //<TextScore>
        //    <DailyBehavior Name="日常行為表現">
        //        <Item Name="愛整潔" Index="....." Degree="3"/>
        //        <Item Name="守秩序" Index="....." Degree="3"/>
        //    </DailyBehavior>

        //    <DailyLifeRecommend Name="日常生活表現具體建議" Description="內容">

        //    <OtherRecommend Name="其他表現建議" Description="內容"/>
        //</TextScore>

        private JHMoralScoreRecord _editorRecord;
        private Dictionary<DataGridViewCell, bool> inputErrors = new Dictionary<DataGridViewCell, bool>();     //用來記錄 日常行為表現 是否有格子輸入不正確的值
        private Dictionary<string, string> Morality = new Dictionary<string, string>(); //日常生活表現具體建議使用
        private Dictionary<string, string> EffortList = new Dictionary<string, string>();  //努力程度代碼
        private Dictionary<string, string> dic = new Dictionary<string, string>();
        private JHStudentRecord _SR;
        private Framework.Security.FeatureAce _permission;

        //是否變更資料
        private ChangeListener DataListener { get; set; }
        bool CheckChange = false;

        private string Mode;

        string _PrimaryKey;

        public DLScoreEditForm(string PrimaryKey, Framework.Security.FeatureAce permission)
        {
            InitializeComponent();

            _permission = permission; //權限

            DataListener = new ChangeListener();
            DataListener.Add(new DataGridViewSource(dgvDailyBehavior));
            DataListener.Add(new DataGridViewSource(dgvOtherRecommend));
            DataListener.Add(new DataGridViewSource(dgvDailyLifeRecommend));
            DataListener.StatusChanged += new EventHandler<ChangeEventArgs>(DataListener_StatusChanged);

            _PrimaryKey = PrimaryKey;

            _SR = JHSchool.Data.JHStudent.SelectByID(PrimaryKey); //取得學生

            #region 學年度學期
            string schoolYear = School.DefaultSchoolYear;
            cbSchoolYear.Text = schoolYear;
            cbSchoolYear.Items.Add((int.Parse(schoolYear) - 3).ToString());
            cbSchoolYear.Items.Add((int.Parse(schoolYear) - 2).ToString());
            cbSchoolYear.Items.Add((int.Parse(schoolYear) - 1).ToString());
            cbSchoolYear.Items.Add((int.Parse(schoolYear)).ToString());
            cbSchoolYear.Items.Add((int.Parse(schoolYear) + 1).ToString());
            cbSchoolYear.Items.Add((int.Parse(schoolYear) + 2).ToString());

            string semester = School.DefaultSemester;
            cbSemester.Text = semester;
            cbSemester.Items.Add("1");
            cbSemester.Items.Add("2");
            #endregion

            this.cbSchoolYear.TextChanged += new System.EventHandler(this.cbSchoolYear_TextChanged);
            this.cbSemester.TextChanged += new System.EventHandler(this.cbSemester_TextChanged);

            JHMoralScoreRecord CheckMSR = JHMoralScore.SelectBySchoolYearAndSemester(_PrimaryKey, int.Parse(schoolYear), int.Parse(semester));
            if (CheckMSR == null)
            {
                Mode = "NEW";
                _editorRecord = new JHMoralScoreRecord();
                SyndLoad();
            }
            else
            {
                Mode = "UPDATA";
                _editorRecord = CheckMSR;

                SyndLoad();

                BindData();
            }

            DataListener.Reset();
            DataListener.ResumeListen();

        }

        public DLScoreEditForm(JHMoralScoreRecord editor, Framework.Security.FeatureAce permission)
        {
            #region 建構子
            InitializeComponent();

            _PrimaryKey = editor.RefStudentID;

            Mode = "UPDATA";

            _permission = permission; //權限

            DataListener = new ChangeListener();
            DataListener.Add(new DataGridViewSource(dgvDailyBehavior));
            DataListener.Add(new DataGridViewSource(dgvOtherRecommend));
            DataListener.Add(new DataGridViewSource(dgvDailyLifeRecommend));
            DataListener.StatusChanged += new EventHandler<ChangeEventArgs>(DataListener_StatusChanged);

            _editorRecord = editor;

            _SR = JHStudent.SelectByID(_editorRecord.RefStudentID); //取得學生

            cbSchoolYear.Text = _editorRecord.SchoolYear.ToString();
            cbSchoolYear.Enabled = false;
            cbSemester.Text = _editorRecord.Semester.ToString();
            cbSemester.Enabled = false;

            SyndLoad();

            BindData();

            DataListener.Reset();
            DataListener.ResumeListen();

            #endregion

        }


        void DataListener_StatusChanged(object sender, ChangeEventArgs e)
        {
            CheckChange = (e.Status == ValueStatus.Dirty);
        }

        /// <summary>
        /// 初始化表格規格
        /// </summary>
        private void SyndLoad()
        {
            dgvDailyBehavior.Rows.Clear();
            dgvOtherRecommend.Rows.Clear();
            dgvDailyLifeRecommend.Rows.Clear();

            dgvOtherRecommend.Rows.Add();
            dgvDailyLifeRecommend.Rows.Add();


            K12.Data.Configuration.ConfigData cd = K12.Data.School.Configuration["DLBehaviorConfig"];

            if (!string.IsNullOrEmpty(cd["DailyBehavior"]))
            {
                XmlElement dailyBehavior = K12.Data.XmlHelper.LoadXml(cd["DailyBehavior"]);
                tabControl1.Tabs["tabItem1"].Text = dailyBehavior.GetAttribute("Name");

                foreach (XmlElement item in dailyBehavior.SelectNodes("Item"))
                    dgvDailyBehavior.Rows.Add(item.GetAttribute("Name"), item.GetAttribute("Index"), "");
            }

            if (!string.IsNullOrEmpty(cd["OtherRecommend"]))
            {
                XmlElement OtherRecommend = K12.Data.XmlHelper.LoadXml(cd["OtherRecommend"]);
                tabControl1.Tabs["tabItem2"].Text = OtherRecommend.GetAttribute("Name");
            }

            if (!string.IsNullOrEmpty(cd["DailyLifeRecommend"]))
            {
                XmlElement dailyLifeRecommend = K12.Data.XmlHelper.LoadXml(cd["DailyLifeRecommend"]);
                tabControl1.Tabs["tabItem5"].Text = dailyLifeRecommend.GetAttribute("Name");
            }


            if (_SR.Class == null)
            {
                this.Text = string.Format("日常生活表現評量   (  {0} 號  {1}  )", _SR.SeatNo, _SR.Name);
            }
            else
            {
                this.Text = string.Format("日常生活表現評量   (  {0} 班 {1} 號  {2}  )", _SR.Class.Name, _SR.SeatNo, _SR.Name);
            }

            //日常生活表現具體建議使用
            ReflashMorality();
            //努力程度
            ReflashEffortList();
            //日常行為表現
            ReflashDic();

            //權限
            btnSave.Visible = _permission.Editable;
            dgvDailyLifeRecommend.ReadOnly = !_permission.Editable;
            dgvDailyBehavior.ReadOnly = !_permission.Editable;
            dgvOtherRecommend.ReadOnly = !_permission.Editable;

        }

        private void BindData()
        {
            #region 更新資料

            if (_editorRecord == null || _editorRecord.TextScore == null || _editorRecord.TextScore.InnerXml == "") return;

            //DailyBehavior
            //<DailyBehavior Name="日常行為表現">
            //    <Item Name="愛整潔" Index="....." Degree="3"/>
            //    <Item Name="守秩序" Index="....." Degree="3"/>
            //</DailyBehavior>
            XmlElement node1 = (XmlElement)_editorRecord.TextScore.SelectSingleNode("DailyBehavior");

            if (node1 != null)
            {
                foreach (XmlElement item in node1.SelectNodes("Item"))
                {
                    foreach (DataGridViewRow row in dgvDailyBehavior.Rows)
                    {
                        if (row.Cells[0].Value.ToString() == item.GetAttribute("Name"))
                        {
                            row.Cells[1].Value = item.GetAttribute("Index");
                            row.Cells[2].Value = item.GetAttribute("Degree");
                        }
                    }
                }
            }

            //OtherRecommend
            //<OtherRecommend Name="其他表現建議" Description=".....">
            XmlElement node2 = (XmlElement)_editorRecord.TextScore.SelectSingleNode("OtherRecommend");

            if (node2 != null)
                dgvOtherRecommend.Rows[0].Cells[0].Value = node2.GetAttribute("Description");

            //DailyLifeRecommend
            //<DailyLifeRecommend Name="日常生活表現具體建議" Description=".....">
            XmlElement node3 = (XmlElement)_editorRecord.TextScore.SelectSingleNode("DailyLifeRecommend");

            if (node3 != null)
                dgvDailyLifeRecommend.Rows[0].Cells[0].Value = node3.GetAttribute("Description");

            #endregion
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Mode == "NEW")
            {
                List<JHMoralScoreRecord> MSRList = JHMoralScore.SelectByStudentIDs(new string[] { _SR.ID });

                foreach (JHMoralScoreRecord each in MSRList)
                {
                    if (each.SchoolYear.ToString() == cbSchoolYear.Text && each.Semester.ToString() == cbSemester.Text)
                    {
                        FISCA.Presentation.Controls.MsgBox.Show("該學年度學期資料已經存在,無法新增");
                        return;
                    }
                }

                newSaveData();
            }
            else if (Mode == "UPDATA")
            {
                updataSaveData();
            }
            else
            {
                return;
            }
        }

        private void newSaveData()
        {
            SaveData(); //逗出XML

            _editorRecord.RefStudentID = _SR.ID;
            _editorRecord.SchoolYear = int.Parse(cbSchoolYear.Text);
            _editorRecord.Semester = int.Parse(cbSemester.Text);

            try
            {
                string xyz = MoralScore.Insert(_editorRecord);

                List<JHMoralScoreRecord> list = JHMoralScore.SelectByStudentIDs(new string[] { _editorRecord.RefStudentID });
                foreach (JHMoralScoreRecord each in list)
                {
                    if (each.SchoolYear.ToString() == _editorRecord.SchoolYear.ToString() && each.Semester.ToString() == _editorRecord.Semester.ToString())
                    {
                        _editorRecord = each;
                    }
                }
            }
            catch
            {
                FISCA.Presentation.Controls.MsgBox.Show("新增資料發生錯誤");
                return;
            }

            //新增後轉變為更新模式

            ApplicationLog.Log("日常生活表現模組.資料項目", "新增日常生活表現資料", "student", _editorRecord.Student.ID, "學生「" + _editorRecord.Student.Name + "」學年度「" + _editorRecord.SchoolYear.ToString() + "」學期「" + _editorRecord.Semester.ToString() + "」，已新增一筆日常生活表現資料。");

            FISCA.Presentation.Controls.MsgBox.Show("新增資料成功");
            cbSchoolYear.Enabled = false;
            cbSemester.Enabled = false;
            Mode = "UPDATA";
            CheckChange = false;
        }

        private void updataSaveData()
        {
            SaveData(); //逗出XML

            try
            {
                MoralScore.Update(_editorRecord);
            }
            catch
            {
                FISCA.Presentation.Controls.MsgBox.Show("更新資料發生錯誤");
                return;
            }

            ApplicationLog.Log("日常生活表現模組.資料項目", "修改日常生活表現資料", "student", _editorRecord.Student.ID, "學生「" + _editorRecord.Student.Name + "」學年度「" + _editorRecord.SchoolYear.ToString() + "」學期「" + _editorRecord.Semester.ToString() + "」，已修改一筆日常生活表現資料。");

            FISCA.Presentation.Controls.MsgBox.Show("更新資料成功");
            cbSchoolYear.Enabled = false;
            cbSemester.Enabled = false;
            CheckChange = false;
        }

        private void SaveData()
        {
            #region 更新儲存
            DSXmlHelper helper = new DSXmlHelper("TextScore");

            //DailyBehavior
            //<DailyBehavior Name="日常行為表現">
            //    <Item Name="愛整潔" Index="....." Degree="3"/>
            //    <Item Name="守秩序" Index="....." Degree="3"/>
            //</DailyBehavior>
            helper.AddElement("DailyBehavior").SetAttribute("Name", tabControl1.Tabs["tabItem1"].Text);
            foreach (DataGridViewRow row in dgvDailyBehavior.Rows)
            {
                if (row.IsNewRow) continue;

                XmlElement node = helper.AddElement("DailyBehavior", "Item");
                node.SetAttribute("Name", "" + row.Cells[0].Value);
                node.SetAttribute("Index", "" + row.Cells[1].Value);
                node.SetAttribute("Degree", "" + row.Cells[2].Value);
            }

            //OtherRecommend
            //<OtherRecommend Name="其他表現建議" Description=".....">
            XmlElement bnode = helper.AddElement("OtherRecommend");
            bnode.SetAttribute("Name", tabControl1.Tabs["tabItem2"].Text);
            bnode.SetAttribute("Description", "" + dgvOtherRecommend.Rows[0].Cells[0].Value);


            //DailyLifeRecommend
            //<DailyLifeRecommend Name="日常生活表現具體建議" Description=".....">
            XmlElement anode = helper.AddElement("DailyLifeRecommend");
            anode.SetAttribute("Name", tabControl1.Tabs["tabItem5"].Text);
            anode.SetAttribute("Description", "" + dgvDailyLifeRecommend.Rows[0].Cells[0].Value);

            _editorRecord.TextScore = helper.BaseElement;

            #endregion
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (CheckChange)
            {
                DialogResult dr = FISCA.Presentation.Controls.MsgBox.Show("資料已經修改是否關閉?", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
                if (dr == DialogResult.Yes)
                {
                    this.Close();
                }
                else
                {
                    return;
                }
            }
            else
            {
                this.Close();
            }
        }

        /// <summary>
        /// 檢查儲存按鈕是否可以按。當格子裡沒有錯誤的值時才Enabled。
        /// </summary>       
        private void CheckSaveButtonEnabled()
        {
            this.btnSave.Enabled = !this.inputErrors.ContainsValue(false);
        }

        //<DailyBehavior Name="日常行為表現">
        //    <Item Name="愛整潔" Index="....."/>
        //    <PerformanceDegree>
        //        <Mapping Degree="4" Desc="完全符合"/>
        //        <Mapping Degree="3" Desc="大部份符合"/>
        //        <Mapping Degree="2" Desc="部份符合"/>
        //    </PerformanceDegree>
        //</DailyBehavior>

        private void ReflashMorality()
        {
            #region 日常生活表現具體建議使用
            DSResponse dsrsp = Config.GetMoralCommentCodeList();
            Morality.Clear();
            foreach (XmlElement var in dsrsp.GetContent().GetElements("Morality"))
            {
                Morality.Add(var.GetAttribute("Code"), var.GetAttribute("Comment"));
            }
            #endregion
        }

        private void ReflashEffortList()
        {
            #region 努力程度對照表
            EffortList.Clear();
            K12.Data.Configuration.ConfigData cd = K12.Data.School.Configuration["努力程度對照表"];
            if (!string.IsNullOrEmpty(cd["xml"]))
            {
                XmlElement element = K12.Data.XmlHelper.LoadXml(cd["xml"]);

                foreach (XmlElement each in element.SelectNodes("Effort"))
                {
                    EffortList.Add(each.GetAttribute("Code"), each.GetAttribute("Name"));
                }
            }
            #endregion
        }

        private void ReflashDic()
        {
            #region 日常行為表現
            dic.Clear();
            K12.Data.Configuration.ConfigData cd = K12.Data.School.Configuration["DLBehaviorConfig"];
            XmlElement node = K12.Data.XmlHelper.LoadXml(cd["DailyBehavior"]);
            foreach (XmlElement item in node.SelectNodes("PerformanceDegree/Mapping"))
            {
                if (!dic.ContainsKey(item.GetAttribute("Degree")))
                {
                    dic.Add(item.GetAttribute("Degree"), item.GetAttribute("Desc"));
                }
                else
                {
                    FISCA.Presentation.Controls.MsgBox.Show("表現程度代碼表,代碼重覆");
                }
            }
            #endregion
        }

        private void dgvDailyBehavior_CurrentCellDirtyStateChanged_1(object sender, EventArgs e)
        {
            #region 日常行為表現資料替換
            dgvDailyBehavior.EndEdit();

            string score = "" + dgvDailyBehavior.CurrentCell.Value;
            bool isMatched = false;

            if (dic.ContainsKey(score)) //如果資料存在key
            {
                dgvDailyBehavior.CurrentCell.Value = dic[score];
                isMatched = true;
            }
            else if (dic.ContainsValue(score)) //如果資料存在value
            {
                dgvDailyBehavior.CurrentCell.Value = score;
                isMatched = true;
            }
            else
            {
                isMatched = false;
            }

            if (string.IsNullOrEmpty(score))
                isMatched = true;

            if (!isMatched)
                dgvDailyBehavior.CurrentCell.Style.BackColor = Color.Pink;
            else
                dgvDailyBehavior.CurrentCell.Style.BackColor = Color.White;

            if (!inputErrors.ContainsKey(dgvDailyBehavior.CurrentCell))
                inputErrors.Add(dgvDailyBehavior.CurrentCell, true);

            inputErrors[dgvDailyBehavior.CurrentCell] = isMatched;
            this.CheckSaveButtonEnabled();

            dgvDailyBehavior.BeginEdit(false);
            #endregion
        }

        private void dgvDailyLifeRecommend_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            #region 日常生活表現具體建議

            if (dgvDailyLifeRecommend.Rows.Count > 0)
            {
                string daliy = "";
                List<string> listNow = new List<string>();

                if (("" + dgvDailyLifeRecommend.Rows[0].Cells[0].Value).ToString().Contains(','))
                {
                    listNow.AddRange(("" + dgvDailyLifeRecommend.Rows[0].Cells[0].Value).ToString().Split(','));
                }
                else
                {
                    listNow.Add("" + dgvDailyLifeRecommend.Rows[0].Cells[0].Value);
                }

                foreach (string each in listNow)
                {
                    if (daliy == "") //如果是空的
                    {
                        if (Morality.ContainsKey(each))
                        {
                            daliy += Morality[each];
                        }
                        else
                        {
                            daliy += each;
                        }
                    }
                    else //如果不是空的
                    {
                        if (Morality.ContainsKey(each))
                        {
                            daliy += "," + Morality[each];
                        }
                        else
                        {
                            daliy += "," + each;
                        }
                    }
                }
                dgvDailyLifeRecommend.Rows[0].Cells[0].Value = daliy;
            }

            #endregion
        }

        private void dgvDailyLifeRecommend_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvDailyLifeRecommend.EndEdit();
            dgvDailyLifeRecommend.BeginEdit(false);
        }

        private void cbSchoolYear_TextChanged(object sender, EventArgs e)
        {
            JHMoralScoreRecord CheckMSR = JHMoralScore.SelectBySchoolYearAndSemester(_PrimaryKey, int.Parse(cbSchoolYear.Text), int.Parse(cbSemester.Text));
            DataListener.SuspendListen(); //終止變更判斷
            if (CheckMSR == null)
            {
                Mode = "NEW";
                _editorRecord = new JHMoralScoreRecord();
                SyndLoad();
            }
            else
            {
                Mode = "UPDATA";
                _editorRecord = CheckMSR;

                SyndLoad();

                BindData();
            }
            DataListener.Reset();
            DataListener.ResumeListen();
            inputErrors.Clear();
            btnSave.Enabled = true;
        }

        private void cbSemester_TextChanged(object sender, EventArgs e)
        {
            JHMoralScoreRecord CheckMSR = JHMoralScore.SelectBySchoolYearAndSemester(_PrimaryKey, int.Parse(cbSchoolYear.Text), int.Parse(cbSemester.Text));
            DataListener.SuspendListen(); //終止變更判斷
            if (CheckMSR == null)
            {
                Mode = "NEW";
                _editorRecord = new JHMoralScoreRecord();
                SyndLoad();
            }
            else
            {
                Mode = "UPDATA";
                _editorRecord = CheckMSR;

                SyndLoad();

                BindData();
            }
            DataListener.Reset();
            DataListener.ResumeListen();
            inputErrors.Clear();
            btnSave.Enabled = true;
        }

    }
}
