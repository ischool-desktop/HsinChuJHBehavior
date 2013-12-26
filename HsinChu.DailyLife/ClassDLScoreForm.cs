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
using K12.Data.Configuration;
using FISCA.LogAgent;

namespace HsinChu.DailyLife
{
    public partial class ClassDLScoreForm : FISCA.Presentation.Controls.BaseForm
    {
        private bool isDataLoaded = false; //to see if the data is loaded .
        private Dictionary<DataGridViewCell, bool> inputErrors = new Dictionary<DataGridViewCell, bool>();     //用來記錄 日常行為表現 是否有格子輸入不正確的值
        private string _dl = string.Empty; //日常生活代碼替換判斷用

        List<string> ECheckList = new List<string>(); //努力程度判斷用(list)
        private Dictionary<string, string> EffortList = new Dictionary<string, string>();  //努力程度代碼
        private Dictionary<string, string> Morality = new Dictionary<string, string>(); //日常生活表現具體建議代碼
        Dictionary<string, string> dic = new Dictionary<string, string>(); //愛整潔

        List<JHStudentRecord> Students = new List<JHStudentRecord>();
        private bool ValueCheck = false;
        private string CheckValidating = "";
        private int SelectIndex = 0; //設定ComBox變更前的index
        private List<string> StudentIDs = new List<string>();
        private Dictionary<string, List<JHMoralScoreRecord>> DicMSRList = new Dictionary<string, List<JHMoralScoreRecord>>();

        private JHClassRecord jhCR = new JHClassRecord(); //Log使用


        //    <TextScore>
        //    <DailyBehavior Name="日常行為表現">
        //        <Item Name="愛整潔" Index="指標" Degree="表現程度"/>
        //        <Item Name="有禮貌" Index="指標" Degree="表現程度"/>
        //        <Item Name="守秩序" Index="指標" Degree="表現程度"/>
        //        <Item Name="責任心" Index="指標" Degree="表現程度"/>
        //        <Item Name="公德心" Index="指標" Degree="表現程度"/>
        //        <Item Name="友愛關懷" Index="指標" Degree="表現程度"/>
        //        <Item Name="團隊合作" Index="指標" Degree="表現程度"/>
        //    </DailyBehavior>

        //    <DailyLifeRecommend Name="日常生活表現具體建議" Description="建議內容"></DailyLifeRecommend>

        //    <OtherRecommend Name="其他表現建議" Description="建議內容"></OtherRecommend>
        //    </TextScore>

        public ClassDLScoreForm()
        {

            InitializeComponent();

            this.isDataLoaded = false;

            //JHSchool.Evaluation.MoralScore.Instance.SyncDataBackground(StudentIDs);     //begin to load data ...  
            //JHSchool.Evaluation.MoralScore.Instance.ItemUpdated += new EventHandler<ItemUpdatedEventArgs>(Instance_ItemUpdated);    //after data is loaded, then invoke this function.


            RefreshStudentData(); //建立該班級的學生清單

            StudentIDs.Clear();

            foreach (JHStudentRecord each in Students)
            {
                StudentIDs.Add(each.ID);
            }

            //沒有AsKeyList啦...
            //List<string> StudentIDs = Students.AsKeyList();  

            List<JHMoralScoreRecord> MSRList = JHMoralScore.SelectByStudentIDs(StudentIDs);

            //日常生活表現具體建議使用
            Morality = new Dictionary<string, string>();

            ReflashEffortList(); //努力程度對照表
            ReflashMorality(); //日常行為對照表
            ReflashDic(); //表現程度對照表
        }

        private void ClassDLScoreForm_Load(object sender, EventArgs e)
        {
            //學年度
            cboSchoolYear.Text = School.DefaultSchoolYear;
            cboSchoolYear.Items.Add(int.Parse(School.DefaultSchoolYear) - 2);
            cboSchoolYear.Items.Add(int.Parse(School.DefaultSchoolYear) - 1);
            cboSchoolYear.Items.Add(int.Parse(School.DefaultSchoolYear));
            //學期
            cboSemester.Text = School.DefaultSemester;
            cboSemester.Items.Add(1);
            cboSemester.Items.Add(2);

            //取得設定值
            K12.Data.Configuration.ConfigData cd = K12.Data.School.Configuration["DLBehaviorConfig"];

            #region 錯誤防堵
            if (string.IsNullOrEmpty(cd["DailyBehavior"]))
            {
                MsgBox.Show("日常生活表現設定檔發現錯誤,請重新設定");
                return;
            }

            if (string.IsNullOrEmpty(cd["OtherRecommend"]))
            {
                MsgBox.Show("日常生活表現設定檔發現錯誤,請重新設定");
                return;
            }

            if (string.IsNullOrEmpty(cd["DailyLifeRecommend"]))
            {
                MsgBox.Show("日常生活表現設定檔發現錯誤,請重新設定");
                return;
            } 
            #endregion

            //將cboPrefs增加一個KeyValuePair物件Item,value是Xml
            if (cd.Contains("DailyBehavior"))
                cboPrefs.Items.Add(new KeyValuePair<string, string>("DailyBehavior", XmlHelper.LoadXml(cd["DailyBehavior"]).GetAttribute("Name")));

            if (cd.Contains("OtherRecommend"))
                cboPrefs.Items.Add(new KeyValuePair<string, string>("OtherRecommend", XmlHelper.LoadXml(cd["OtherRecommend"]).GetAttribute("Name")));

            if (cd.Contains("DailyLifeRecommend"))
                cboPrefs.Items.Add(new KeyValuePair<string, string>("DailyLifeRecommend", XmlHelper.LoadXml(cd["DailyLifeRecommend"]).GetAttribute("Name")));

            cboPrefs.ValueMember = "Key";
            cboPrefs.DisplayMember = "Value";

            //如果沒有內容則選擇第0個
            if (cboPrefs.Items.Count > 0)
                cboPrefs.SelectedIndex = 0;

            this.isDataLoaded = true;
            BindData();

        }

        private void BindData()
        {
            #region 更新畫面

            //如果Item是空的
            if (cboPrefs.SelectedItem == null) return;

            //清空Columns
            dgvMoralScore.Columns.Clear();
            //清空Rows
            dgvMoralScore.Rows.Clear();

            //加回基本欄位
            dgvMoralScore.Columns.Add(colRecordID);
            dgvMoralScore.Columns.Add(colStuID);
            dgvMoralScore.Columns.Add(colSeatNo);
            dgvMoralScore.Columns.Add(colStuName);
            dgvMoralScore.Columns.Add(colStuNumber);

            //取得設定值
            K12.Data.Configuration.ConfigData cd = School.Configuration["DLBehaviorConfig"];

            //選擇的Item
            switch (((KeyValuePair<string, string>)cboPrefs.SelectedItem).Key)
            {
                case "DailyBehavior":
                    if (cd.Contains("DailyBehavior"))
                    {
                        //取得此欄位內容
                        XmlElement node = XmlHelper.LoadXml(cd["DailyBehavior"]);
                        //每個欄位名稱
                        foreach (XmlElement item in node.SelectNodes("Item"))
                            dgvMoralScore.Columns.Add(item.GetAttribute("Name"), item.GetAttribute("Name"));
                    }
                    break;
                case "OtherRecommend":
                    if (cd.Contains("OtherRecommend"))
                    {
                        //取得此欄位內容
                        XmlElement node = XmlHelper.LoadXml(cd["OtherRecommend"]);
                        //將Column增加一欄,並將新增的欄位記下
                        int colIndex = dgvMoralScore.Columns.Add(node.GetAttribute("Name"), node.GetAttribute("Name"));
                        //登記寬度
                        dgvMoralScore.Columns[colIndex].Width = 400;
                        //記住目前選擇的名稱
                        _dl = node.GetAttribute("Name");
                    }
                    break;
                case "DailyLifeRecommend":
                    if (cd.Contains("DailyLifeRecommend"))
                    {
                        //取得此欄位內容
                        XmlElement node = XmlHelper.LoadXml(cd["DailyLifeRecommend"]);
                        //將Column增加一欄,並將新增的欄位記下
                        int colIndex = dgvMoralScore.Columns.Add(node.GetAttribute("Name"), node.GetAttribute("Name"));
                        //登記寬度
                        dgvMoralScore.Columns[colIndex].Width = 400;
                        //記住目前選擇的名稱
                        _dl = node.GetAttribute("Name");
                    }
                    break;
            }

            //更新學生資料
            RefreshStudentData();
            //建立以學生ID為KEY的字典
            RefreshMoralScoreData();

            //取得學生Record
            foreach (StudentRecord stuRecord in Students)
            {
                //字串陣列數 數字 用
                List<string> headers = new List<string>();
                //如果第一欄為Null就是預設新增
                headers.Add("Null");
                headers.Add(stuRecord.ID);
                headers.Add(stuRecord.SeatNo.ToString());
                headers.Add(stuRecord.Name);
                headers.Add(stuRecord.StudentNumber);

                if (DicMSRList.ContainsKey(stuRecord.ID))
                {
                    foreach (MoralScoreRecord msRecord in DicMSRList[stuRecord.ID])
                    {
                        if (msRecord.SchoolYear == int.Parse(cboSchoolYear.Text) && msRecord.Semester == int.Parse(cboSemester.Text))
                        {
                            headers[0] = msRecord.ID;
                            XmlElement node;

                            if (((KeyValuePair<string, string>)cboPrefs.SelectedItem).Key == "DailyBehavior")
                            {
                                node = msRecord.TextScore.SelectSingleNode("DailyBehavior") as XmlElement;
                                if (node != null)
                                {
                                    foreach (XmlElement item in node.SelectNodes("Item"))
                                    {
                                        headers.Add(item.GetAttribute("Degree"));
                                    }
                                }
                            }

                            #region (註)高雄使用
                            //if (((KeyValuePair<string, string>)cboPrefs.SelectedItem).Key == "GroupActivity")
                            //{
                            //    node = msRecord.TextScore.SelectSingleNode("GroupActivity") as XmlElement;
                            //    if (node != null)
                            //    {
                            //        foreach (XmlElement item in node.SelectNodes("Item"))
                            //        {
                            //            headers.Add(item.GetAttribute("Degree"));
                            //            headers.Add(item.GetAttribute("Description"));
                            //        }
                            //    }
                            //}

                            //if (((KeyValuePair<string, string>)cboPrefs.SelectedItem).Key == "PublicService")
                            //{
                            //    node = msRecord.TextScore.SelectSingleNode("PublicService") as XmlElement;
                            //    if (node != null)
                            //    {
                            //        foreach (XmlElement item in node.SelectNodes("Item"))
                            //        {
                            //            headers.Add(item.GetAttribute("Description"));
                            //        }
                            //    }
                            //}

                            //if (((KeyValuePair<string, string>)cboPrefs.SelectedItem).Key == "SchoolSpecial")
                            //{
                            //    node = msRecord.TextScore.SelectSingleNode("SchoolSpecial") as XmlElement;
                            //    if (node != null)
                            //    {
                            //        foreach (XmlElement item in node.SelectNodes("Item"))
                            //        {
                            //            headers.Add(item.GetAttribute("Description"));
                            //        }
                            //    }
                            //} 
                            #endregion

                            if (((KeyValuePair<string, string>)cboPrefs.SelectedItem).Key == "OtherRecommend")
                            {

                                node = msRecord.TextScore.SelectSingleNode("OtherRecommend") as XmlElement;

                                if (node != null)
                                    headers.Add(node.GetAttribute("Description"));
                            }

                            if (((KeyValuePair<string, string>)cboPrefs.SelectedItem).Key == "DailyLifeRecommend")
                            {
                                node = msRecord.TextScore.SelectSingleNode("DailyLifeRecommend") as XmlElement;

                                if (node != null)
                                    //dgvMoralScore.Rows[rowIndex].Cells[colIndex].Value = node.GetAttribute("Description");
                                    headers.Add(node.GetAttribute("Description"));
                            }
                        }
                    }
                }
                //增加幾個空 Rows
                dgvMoralScore.Rows.Add(headers.ToArray()); //??
            }

            #region 依字典內容,比對學年度/學期將值填入
            //int rowIndex = 0;

            //foreach (string each in DicMSRList.Keys)
            //{
            //    foreach (MoralScoreRecord msRecord in DicMSRList[each])
            //    {
            //        int colIndex = 5;

            //        if (msRecord.SchoolYear == int.Parse(cboSchoolYear.Text) && msRecord.Semester == int.Parse(cboSemester.Text))
            //        {
            //            dgvMoralScore.Rows[rowIndex].Cells[0].Value = msRecord.ID;

            //            XmlElement node;

            //            if (((KeyValuePair<string, string>)cboPrefs.SelectedItem).Key == "DailyBehavior")
            //            {

            //            }

            //            if (((KeyValuePair<string, string>)cboPrefs.SelectedItem).Key == "DailyLifeRecommend")
            //            {
            //                node = msRecord.TextScore.SelectSingleNode("DailyLifeRecommend") as XmlElement;

            //                if (node != null)
            //                    dgvMoralScore.Rows[rowIndex].Cells[colIndex].Value = node.GetAttribute("Description");
            //            }

            //            if (((KeyValuePair<string, string>)cboPrefs.SelectedItem).Key == "OtherRecommend")
            //            {
            //                node = msRecord.TextScore.SelectSingleNode("OtherRecommend") as XmlElement;

            //                if (node != null)
            //                    dgvMoralScore.Rows[rowIndex].Cells[colIndex].Value = node.GetAttribute("Description");
            //            }
            //        }
            //    }
            //    rowIndex++;
            //}
            #endregion

            #endregion
        }

        private void RefreshMoralScoreData()
        {
            #region 建立以學生ID為KEY的字典
            DicMSRList.Clear();

            //取得資料
            List<JHMoralScoreRecord> MSRList = JHMoralScore.SelectByStudentIDs(StudentIDs);

            foreach (string stuID in StudentIDs)
            {
                foreach (JHMoralScoreRecord each in MSRList)
                {
                    if (each.RefStudentID == stuID)
                    {
                        if (!DicMSRList.ContainsKey(stuID))
                        {
                            DicMSRList.Add(stuID, new List<JHMoralScoreRecord>());
                            DicMSRList[stuID].Add(each);
                        }
                        else
                        {
                            DicMSRList[stuID].Add(each);
                        }
                    }

                }
            }
            #endregion
        }

        private void SaveData()
        {
            #region 儲存資料
            //new一個Editors的List
            List<MoralScoreRecord> UPdataMSR = new List<MoralScoreRecord>();
            List<MoralScoreRecord> UPInsertMSR = new List<MoralScoreRecord>();

            //更新資料清單
            RefreshMoralScoreData();

            //針對每一個Row(一個學生)
            foreach (DataGridViewRow row in dgvMoralScore.Rows)
            {
                //檢查每一個Cell
                foreach (DataGridViewCell cell in row.Cells)
                {
                    //如果顏色有問題,就return
                    if (cell.Style.BackColor == Color.Pink)
                    {
                        FISCA.Presentation.Controls.MsgBox.Show("輸入內容有誤(粉紅色區塊),請修正後再儲存");
                        return;
                    }
                }

                string SaveInstudID = row.Cells[1].Value.ToString();
                JHMoralScoreRecord editor = GetCurrentSemesterData(SaveInstudID);


                //資料不存在
                if (editor == null)
                {
                    editor = new JHMoralScoreRecord();
                    editor.RefStudentID = row.Cells[1].Value.ToString();
                    editor.SchoolYear = int.Parse(cboSchoolYear.Text);
                    editor.Semester = int.Parse(cboSemester.Text);
                }

                #region 如果日常生活表現資料是空的
                XmlElement textscore = null;
                DSXmlHelper hlptextscore = null;

                //取得設定值
                K12.Data.Configuration.ConfigData cd = K12.Data.School.Configuration["DLBehaviorConfig"];

                if (editor.TextScore == null)
                    textscore = DSXmlHelper.LoadXml("<TextScore/>");
                else
                    textscore = editor.TextScore;

                hlptextscore = new DSXmlHelper(textscore); 
                #endregion

                #region DailyBehavior
                if (((KeyValuePair<string, string>)cboPrefs.SelectedItem).Key == "DailyBehavior")
                {
                    //設定值內有DailyBehavior的話
                    if (cd.Contains("DailyBehavior"))
                    {
                        if (hlptextscore.GetElement("DailyBehavior") == null)
                            hlptextscore.AddElement("DailyBehavior");

                        hlptextscore.GetElement("DailyBehavior").RemoveAll();

                        XmlElement node = DSXmlHelper.LoadXml(cd["DailyBehavior"]);
                        hlptextscore.GetElement("DailyBehavior").SetAttribute("Name", node.GetAttribute("Name"));

                        foreach (XmlElement item in node.SelectNodes("Item"))
                        {
                            string name = item.GetAttribute("Name");
                            XmlElement anode = hlptextscore.AddElement("DailyBehavior", "Item");
                            anode.SetAttribute("Name", name);
                            anode.SetAttribute("Index", item.GetAttribute("Index"));
                        }
                    }

                    XmlElement hnode = hlptextscore.GetElement("DailyBehavior");

                    for (int i = 5; i < row.Cells.Count; i++)
                    {
                        foreach (XmlElement node in hnode.SelectNodes("Item"))
                        {
                            if (dgvMoralScore.Columns[i].Name == node.GetAttribute("Name"))
                            {
                                node.SetAttribute("Degree", "" + row.Cells[i].Value);
                            }
                        }
                    }
                } 
                #endregion

                #region OtherRecommend

                if (((KeyValuePair<string, string>)cboPrefs.SelectedItem).Key == "OtherRecommend")
                {
                    //在設定值中是否存在
                    if (cd.Contains("OtherRecommend"))
                    {
                        //如果學生沒有此欄位
                        if (hlptextscore.GetElement("OtherRecommend") == null)
                            hlptextscore.AddElement(".", "OtherRecommend");

                        //清空此Element內所有元素
                        hlptextscore.GetElement("OtherRecommend").RemoveAll();

                        //讀取設定值內容
                        XmlElement node = XmlHelper.LoadXml(cd["OtherRecommend"]);

                        //設定此Element的名稱
                        hlptextscore.GetElement("OtherRecommend").SetAttribute("Name", node.GetAttribute("Name"));
                    }

                    hlptextscore.GetElement("OtherRecommend").SetAttribute("Description", "" + row.Cells[5].Value);

                }

                #endregion

                #region DailyLifeRecommend
                if (((KeyValuePair<string, string>)cboPrefs.SelectedItem).Key == "DailyLifeRecommend")
                {
                    //在設定值中是否存在
                    if (cd.Contains("DailyLifeRecommend"))
                    {
                        //如果學生沒有此欄位
                        if (hlptextscore.GetElement("DailyLifeRecommend") == null)
                            hlptextscore.AddElement(".", "DailyLifeRecommend");

                        //清空此Element內所有元素
                        hlptextscore.GetElement("DailyLifeRecommend").RemoveAll();

                        //讀取設定值內容
                        XmlElement node = XmlHelper.LoadXml(cd["DailyLifeRecommend"]);

                        //設定此Element的名稱
                        hlptextscore.GetElement("DailyLifeRecommend").SetAttribute("Name", node.GetAttribute("Name"));
                    }

                    hlptextscore.GetElement("DailyLifeRecommend").SetAttribute("Description", "" + row.Cells[5].Value);

                } 
                #endregion
                


                editor.TextScore = textscore;

                if (IsAddRequired(editor))
                {
                    UPInsertMSR.Add(editor);
                }
                else
                {
                    UPdataMSR.Add(editor);
                }
            }

            #endregion

            try
            {
                MoralScore.Insert(UPInsertMSR);
                MoralScore.Update(UPdataMSR);
            }
            catch (Exception ex)
            {
                FISCA.Presentation.Controls.MsgBox.Show("儲存發生錯誤");
                throw ex;
            }

            ApplicationLog.Log("日常生活表現模組.評等輸入", "評等輸入", "class", jhCR.ID, "由「評等輸入」功能，將學生「日常生活表現資料」進行批次修改。\n詳細資料：\n班級「" + jhCR.Name + "」學年度「" + cboSchoolYear.Text + "」學期「" + cboSemester.Text + "」。");
            FISCA.Presentation.Controls.MsgBox.Show("儲存完成");
            ValueCheck = false;
        }

        private static bool IsAddRequired(JHMoralScoreRecord editor)
        {
            return string.IsNullOrEmpty(editor.ID);
        }

        private JHMoralScoreRecord GetCurrentSemesterData(string SaveInstudID)
        {
            if (DicMSRList.ContainsKey(SaveInstudID))
            {
                foreach (JHMoralScoreRecord each in DicMSRList[SaveInstudID])
                {
                    if (each.SchoolYear.ToString() == cboSchoolYear.Text && each.Semester.ToString() == cboSemester.Text)
                    {
                        return each;
                    }
                }
            }

            return null;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int tryInt;
            if (!int.TryParse(cboSchoolYear.Text, out tryInt) || !int.TryParse(cboSemester.Text, out tryInt))
            {
                MessageBox.Show("學年度/學期 格式錯誤。");
                return;
            }


            SaveData();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboPrefs_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        private void cboPrefs_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (ValueCheck)
            {
                DialogResult DR = FISCA.Presentation.Controls.MsgBox.Show("放棄變更資料?", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (DR == DialogResult.Yes)
                {
                    if (this.isDataLoaded)
                        BindData();
                }
                else
                {
                    cboPrefs.SelectedIndex = SelectIndex;
                }
            }
            else
            {
                if (this.isDataLoaded)
                    BindData();
            }
            ValueCheck = false;
        }

        //<DailyBehavior Name="日常行為表現">
        //    <Item Name="愛整潔" Index="....."/>
        //    <PerformanceDegree>
        //        <Mapping Degree="4" Desc="完全符合"/>
        //        <Mapping Degree="3" Desc="大部份符合"/>
        //        <Mapping Degree="2" Desc="部份符合"/>
        //    </PerformanceDegree>
        //</DailyBehavior>
        private void dgvMoralScore_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            #region 替換代碼
            dgvMoralScore.EndEdit();

            //dgvMoralScore.Columns[dgvMoralScore.CurrentCell.ColumnIndex].Name

            if (((KeyValuePair<string, string>)cboPrefs.SelectedItem).Key == "DailyBehavior")
            {
                string score = "" + dgvMoralScore.CurrentCell.Value;
                bool isMatched = false;

                if (dic.ContainsKey(score)) //如果資料存在key
                {
                    dgvMoralScore.CurrentCell.Value = dic[score];
                    isMatched = true;
                }
                else if (dic.ContainsValue(score)) //如果資料存在value
                {
                    dgvMoralScore.CurrentCell.Value = score;
                    isMatched = true;
                }
                else
                {
                    isMatched = false;
                }

                if (string.IsNullOrEmpty(score)) isMatched = true;

                if (!isMatched)
                    dgvMoralScore.CurrentCell.Style.BackColor = Color.Pink;
                else
                    dgvMoralScore.CurrentCell.Style.BackColor = Color.White;

                if (!inputErrors.ContainsKey(dgvMoralScore.CurrentCell))
                    inputErrors.Add(dgvMoralScore.CurrentCell, true);

                inputErrors[dgvMoralScore.CurrentCell] = isMatched;
                this.CheckSaveButtonEnabled();
            }

            dgvMoralScore.BeginEdit(false);
            #endregion
        }

        /// <summary>
        /// 檢查儲存按鈕是否可以按。當格子裡沒有錯誤的值時才Enabled。
        /// </summary>       
        private void CheckSaveButtonEnabled()
        {
            this.btnSave.Enabled = !this.inputErrors.ContainsValue(false);
        }

        private void dgvMoralScore_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column == colSeatNo)
            {
                int x, y;

                if (!int.TryParse(("" + e.CellValue1), out x))
                    x = int.MaxValue;

                if (!int.TryParse("" + e.CellValue2, out y))
                    y = int.MaxValue;

                e.SortResult = x.CompareTo(y);
            }
            e.Handled = true;
        }

        private void dgvMoralScore_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //this.dgvMoralScore.Sort(this.colSeatNo, ListSortDirection.Ascending);
        }

        private void dgvMoralScore_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            #region 處理生活表現,熱鍵替代功能


            if (dgvMoralScore.Columns[e.ColumnIndex].Name == _dl)
            {
                DataGridViewCell cell = dgvMoralScore.Rows[e.RowIndex].Cells[e.ColumnIndex];
                //cell.Value;

                string daliy = "";
                if (cell.Value == null)
                    return;
                string NowCell = "" + cell.Value.ToString();
                List<string> listNow = new List<string>();

                if (NowCell.Contains(','))
                {
                    listNow.AddRange(NowCell.Split(','));
                }
                else
                {
                    listNow.Add(NowCell);
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
                cell.Value = daliy;
            }
            #endregion

            if (ECheckList.Contains(dgvMoralScore.Columns[e.ColumnIndex].Name))
            {
                DataGridViewCell cell = dgvMoralScore.Rows[e.RowIndex].Cells[e.ColumnIndex];

                if (cell.Value == null)
                    return;

                if (EffortList.ContainsKey("" + cell.Value))
                {
                    cell.Value = EffortList["" + cell.Value];
                    dgvMoralScore.CurrentCell.Style.BackColor = Color.White;
                }
                else if (EffortList.ContainsValue("" + cell.Value))
                {
                    dgvMoralScore.CurrentCell.Style.BackColor = Color.White;
                }
                else
                {
                    dgvMoralScore.CurrentCell.Style.BackColor = Color.Pink;
                }
            }

            //如果值不等於編輯前內容
            if (CheckValidating != "" + dgvMoralScore.Rows[e.RowIndex].Cells[e.ColumnIndex].Value)
            {
                //資料已更新
                ValueCheck = true;
            }

        }

        private void RefreshStudentData()
        {
            #region 取得一般狀態的學生
            Students.Clear();

            JHClassRecord cr = JHClass.SelectByID(JHSchool.Class.Instance.SelectedKeys[0]);

            jhCR = cr;

            List<JHStudentRecord> StudentList = cr.Students;


            foreach (JHStudentRecord each in StudentList)
            {
                if (each.Status == JHStudentRecord.StudentStatus.一般)
                {
                    Students.Add(each);
                }
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
                XmlElement element = XmlHelper.LoadXml(cd["xml"]);

                foreach (XmlElement each in element.SelectNodes("Effort"))
                {
                    EffortList.Add(each.GetAttribute("Code"), each.GetAttribute("Name"));
                }
            }
            #endregion
        }

        private void ReflashMorality()
        {
            #region 日常行為表現對照表
            DSResponse dsrsp = Config.GetMoralCommentCodeList();
            foreach (XmlElement var in dsrsp.GetContent().GetElements("Morality"))
            {
                Morality.Add(var.GetAttribute("Code"), var.GetAttribute("Comment"));
            }
            #endregion
        }

        private void ReflashDic()
        {
            #region 表現程度對照表
            K12.Data.Configuration.ConfigData cd = K12.Data.School.Configuration["DLBehaviorConfig"];
            dic = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(cd["DailyBehavior"]))
            {
                MsgBox.Show("日常生活表現設定檔發現錯誤,請重新設定");
                return;
            }

            XmlElement node = XmlHelper.LoadXml(cd["DailyBehavior"]);

            if (node.SelectNodes("PerformanceDegree/Mapping") == null)
            {
                MsgBox.Show("尚未設定表現程度對照表,將無法自動替換代碼!");
                return;
            }
            else if (node.SelectNodes("PerformanceDegree/Mapping").Count == 0)
            {
                MsgBox.Show("尚未設定表現程度對照表,將無法自動替換代碼!");
                return;
            }

            foreach (XmlElement item in node.SelectNodes("PerformanceDegree/Mapping"))
            {
                dic.Add(item.GetAttribute("Degree"), item.GetAttribute("Desc"));
            }
            #endregion
        }

        //學期切換時
        private void cboSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        //學年度切換時
        private void cboSchoolYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        //開始編輯
        private void dgvMoralScore_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            CheckValidating = "" + dgvMoralScore.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
        }

        //當使用者按住cboPrefs控制項時
        private void cboPrefs_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            SelectIndex = cboPrefs.SelectedIndex;
        }
    }
}
