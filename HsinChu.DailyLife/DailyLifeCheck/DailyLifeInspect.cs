using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using K12.Data;
using System.Xml;
using JHSchool.Data;
using Aspose.Cells;
using K12.Presentation;

namespace HsinChu.DailyLife
{
    public partial class DailyLifeInspect : BaseForm
    {
        private Dictionary<string, List<string>> DicTitle = new Dictionary<string, List<string>>();
        List<ColumnHeader> ReMoveColumn = new List<ColumnHeader>();

        /// <summary>
        /// 班級清單
        /// </summary>
        List<ListViewItem> ListViewCollection = new List<ListViewItem>();

        /// <summary>
        /// 背景模式處理器
        /// </summary>
        BackgroundWorker BGW = new BackgroundWorker();

        /// <summary>
        /// 背景模式狀態
        /// </summary>
        bool nowRunBackWorker = false;

        /// <summary>
        /// 資料處理器
        /// </summary>
        DataSelect ds;

        int _SchoolYear;
        int _Semester;

        public DailyLifeInspect()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DailyLifeInspect_Load(object sender, EventArgs e)
        {
            BGW.DoWork += new DoWorkEventHandler(BGW_DoWork);
            BGW.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BGW_RunWorkerCompleted);

            intSchoolYear.Value = int.Parse(School.DefaultSchoolYear);
            intSemester.Value = int.Parse(School.DefaultSemester);


            txtTempCount.Text = "待處理班級數: " + NLDPanels.Class.TempSource.Count;

            //建立畫面樣式
            ColumnSetup();

            cbxSelect.SelectedIndex = 0;

            //開始背景模式
            BingForm();

            this.intSchoolYear.ValueChanged += new EventHandler(SchoolYearSemster_ValueChanged);
            this.intSemester.ValueChanged += new EventHandler(SchoolYearSemster_ValueChanged);

        }

        /// <summary>
        /// 建立Column
        /// </summary>
        private void ColumnSetup()
        {
            #region ColumnSetup

            K12.Data.Configuration.ConfigData cd = K12.Data.School.Configuration["DLBehaviorConfig"];

            #region 日常行為表現
            if (!string.IsNullOrEmpty(cd["DailyBehavior"]))
            {
                if (cd.Contains("DailyBehavior"))
                {
                    XmlElement dailyBehavior = XmlHelper.LoadXml(cd["DailyBehavior"]);

                    cbxSelect.Items.Add(dailyBehavior.GetAttribute("Name"));
                    DicTitle.Add(dailyBehavior.GetAttribute("Name"), new List<string>());

                    foreach (XmlElement each in dailyBehavior.SelectNodes("Item"))
                    {
                        DicTitle[dailyBehavior.GetAttribute("Name")].Add(each.GetAttribute("Name"));
                        //ReMoveColumn.Add(listViewEx1.Columns.Add(each.GetAttribute("Name")));

                    }
                }
            }
            #endregion

            #region 其他表現
            if (!string.IsNullOrEmpty(cd["OtherRecommend"]))
            {
                if (cd.Contains("OtherRecommend"))
                {
                    XmlElement OtherRecommend = XmlHelper.LoadXml(cd["OtherRecommend"]);

                    cbxSelect.Items.Add(OtherRecommend.GetAttribute("Name"));
                    DicTitle.Add(OtherRecommend.GetAttribute("Name"), new List<string>());
                    DicTitle[OtherRecommend.GetAttribute("Name")].Add(OtherRecommend.GetAttribute("Name"));

                }
            }
            #endregion

            #region 日常生活表現具體建議
            if (!string.IsNullOrEmpty(cd["DailyLifeRecommend"]))
            {
                if (cd.Contains("DailyLifeRecommend"))
                {
                    XmlElement dailyLifeRecommend = XmlHelper.LoadXml(cd["DailyLifeRecommend"]);

                    cbxSelect.Items.Add(dailyLifeRecommend.GetAttribute("Name"));
                    DicTitle.Add(dailyLifeRecommend.GetAttribute("Name"), new List<string>());
                    DicTitle[dailyLifeRecommend.GetAttribute("Name")].Add(dailyLifeRecommend.GetAttribute("Name"));

                }
            }
            #endregion

            #endregion
        }

        /// <summary>
        /// 整合後的背景模式
        /// </summary>
        private void BingForm()
        {
            listViewEx1.Items.Clear();

            _SchoolYear = intSchoolYear.Value;
            _Semester = intSemester.Value;

            LockForm(false);

            this.Text = "資料處理中...";
            FISCA.Presentation.MotherForm.SetStatusBarMessage("資料處理中...");

            BGW.RunWorkerAsync();
        }

        /// <summary>
        /// 傳入bool,將特定物件關閉或開啟
        /// </summary>
        /// <param name="now"></param>
        private void LockForm(bool now)
        {
            //intSchoolYear.Enabled = now;
            //intSemester.Enabled = now;
            cbxSelect.Enabled = now;
            cbViewNotClass.Enabled = now;
            btnPrint.Enabled = now;
            btnRefresh.Enabled = now;
        }

        void BGW_DoWork(object sender, DoWorkEventArgs e)
        {
            //取得全校資訊
            //New一個資料處理器
            ds = new DataSelect(_SchoolYear, _Semester, DicTitle);
        }

        void BGW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (nowRunBackWorker)
            {
                nowRunBackWorker = false;
                BingForm();
                return;
            }

            //畫面完成
            SetupSubItems();

            SetListViewItem();

            LockForm(true);

            this.Text = "評等輸入狀況檢查";
            
            FISCA.Presentation.MotherForm.SetStatusBarMessage("");

            listViewEx1.Focus();
        }

        /// <summary>
        /// 依cbViewNotClass狀態,篩選完成班級
        /// </summary>
        private void SetListViewItem()
        {
            if (cbViewNotClass.Checked)
            {
                foreach (ListViewItem each in ListViewCollection)
                {
                    if ((bool)each.Tag)
                    {
                        listViewEx1.Items.Add(each);
                    }
                }
            }
            else
            {
                foreach (ListViewItem each in ListViewCollection)
                {
                    listViewEx1.Items.Add(each);
                }
            }
        }

        /// <summary>
        /// 切換輸入項目時...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSelect_SelectedValueChanged(object sender, EventArgs e)
        {
            string select = (string)cbxSelect.SelectedItem;

            //清空Column
            foreach (ColumnHeader each in ReMoveColumn)
            {
                listViewEx1.Columns.Remove(each);
            }

            ReMoveColumn.Clear();

            if (DicTitle.ContainsKey(select))
            {
                List<string> list = DicTitle[select];

                foreach (string each in list)
                {
                    ReMoveColumn.Add(listViewEx1.Columns.Add(each));
                }
            }

            foreach (ColumnHeader each in ReMoveColumn)
            {
                if (cbxSelect.SelectedIndex == 0)
                {
                    each.Width = 65;
                }
                else if (cbxSelect.SelectedIndex == 1)
                {
                    each.Width = 100;
                }
                else if (cbxSelect.SelectedIndex == 2)
                {
                    each.Width = 100;
                }
                else if (cbxSelect.SelectedIndex == 3)
                {
                    each.Width = 100;
                }
                else if (cbxSelect.SelectedIndex == 4)
                {
                    each.Width = 200;
                }
            }

            if (ds != null)
            {
                SetupSubItems();
                listViewEx1.Items.Clear();
                SetListViewItem();
            }
        }

        /// <summary>
        /// 依班級建立ListViewItem物件,並且填入顏色
        /// </summary>
        private void SetupSubItems()
        {
            ListViewCollection.Clear();
            listViewEx1.Items.Clear();

            foreach (string each in ds.DicClassObj.Keys)
            {
                ListViewItem item = new ListViewItem(ds.DicClassObj[each].ClassID);
                item.SubItems.Add(ds.DicClassObj[each].ClassName);
                item.SubItems.Add(ds.DicClassObj[each].TeacherName);

                bool Check = false; //檢查

                foreach (string cbxselet in DicTitle[(string)cbxSelect.SelectedItem])
                {
                    int TopString = ds.DicClassObj[each].DBList[cbxselet];  //未輸入人數
                    int ButtonString = ds.DicClassObj[each].ClassCount; //班級總人數
                    string Insert = TopString + "/" + ButtonString;
                    if (TopString != ButtonString) //前後數字不同則標示為紅
                    {
                        if (ButtonString != 0)
                        {
                            item.SubItems.Add(Insert).ForeColor = Color.Red;
                            Check = true;
                        }
                        else //如果總人數為0
                        {
                            item.SubItems.Add(Insert);
                        }
                    }
                    else //數字相同為通過
                    {
                        item.SubItems.Add(Insert);
                    }
                }

                item.Tag = Check;

                ListViewCollection.Add(item);
                //listViewEx1.Items.Add(item);
            } 
        }

        /// <summary>
        /// 更新鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (ds != null)
            {
                if (BGW.IsBusy)
                {
                    nowRunBackWorker = true;
                }
                else
                {
                    BingForm();
                }
            } 
        }

        /// <summary>
        /// 學年度學期變更後...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SchoolYearSemster_ValueChanged(object sender, EventArgs e)
        {
            if (BGW.IsBusy)
            {
                nowRunBackWorker = true;
            }
            else
            {
                BingForm();
            } 
        }

        /// <summary>
        /// 僅顯示未完成輸入之班級
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbViewNotClass_CheckedChanged(object sender, EventArgs e)
        {
            listViewEx1.Items.Clear();
            SetListViewItem();
        }

        /// <summary>
        /// 匯出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            Workbook wb = new Workbook();

            int ColumnIndex = 0;
            int ItemIndex = 0;
            //填入標題
            foreach (ColumnHeader each in listViewEx1.Columns)
            {
                if (each.Text != "班級ID")
                {
                    wb.Worksheets[0].Cells[ItemIndex, ColumnIndex].PutValue(each.Text);
                    ColumnIndex++;
                }
            }

            ItemIndex++;

            foreach (ListViewItem each in listViewEx1.Items)
            {
                ColumnIndex = 0;
                bool ClassID = false;
                foreach (ListViewItem.ListViewSubItem subitem in each.SubItems)
                {
                    if (ClassID)
                    {
                        wb.Worksheets[0].Cells[ItemIndex, ColumnIndex].PutValue(subitem.Text);
                        ColumnIndex++;
                    }
                    else
                        ClassID = true;

                }
                ItemIndex++;
            }

            SaveFileDialog sd = new System.Windows.Forms.SaveFileDialog();
            sd.Title = "另存新檔";
            sd.FileName = "評等輸入狀況檢查(匯出).xls";
            sd.Filter = "Excel檔案 (*.xls)|*.xls|所有檔案 (*.*)|*.*";
            if (sd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    wb.Save(sd.FileName, FileFormatType.Excel2003);
                    System.Diagnostics.Process.Start(sd.FileName);

                }
                catch
                {
                    FISCA.Presentation.Controls.MsgBox.Show("指定路徑無法存取。", "建立檔案失敗", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    this.Enabled = true;
                    return;
                }
            } 
        }

        /// <summary>
        /// 加入待處理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();

            foreach (ListViewItem each in listViewEx1.SelectedItems)
            {
                list.Add(each.SubItems[0].Text);
            }

            NLDPanels.Class.AddToTemp(list);

            txtTempCount.Text = "待處理班級數: " + NLDPanels.Class.TempSource.Count; 
        }

        /// <summary>
        /// 移出待處理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            NLDPanels.Class.RemoveFromTemp(NLDPanels.Class.TempSource);

            txtTempCount.Text = "待處理班級數: " + NLDPanels.Class.TempSource.Count; 

        }

        /// <summary>
        /// 離開
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
