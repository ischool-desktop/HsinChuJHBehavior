using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using JHSchool;
using JHSchool.Data;
using FISCA.DSAUtil;
using K12.Data.Utility;
using System.Xml;
using FISCA.LogAgent;

namespace HsinChu.DailyLife
{
    public partial class ChangeToInitialSummary : BaseForm
    {
        JHMoralScoreRecord _JHR;
        private List<string> _periodTypes;
        private List<string> _absenceList;
        private List<string> _meritTypes;
        private string _ParKey;
        private string schoolYear;
        private string semester;
        private Dictionary<string, int> periodList;
        private Dictionary<string, int> meritList;

        public string poSchoolYear;
        public string poSemester;


        /// <summary>
        /// 多載,如果該學年度學期無日常生活表現資料
        /// </summary>
        /// <param name="ParKey"></param>
        public ChangeToInitialSummary(string parkey)
        {
            InitializeComponent();

            _ParKey = parkey;

            #region 學年度學期
            schoolYear = School.DefaultSchoolYear;
            cbSchoolYear.Items.Add((int.Parse(schoolYear) - 3).ToString());
            cbSchoolYear.Items.Add((int.Parse(schoolYear) - 2).ToString());
            cbSchoolYear.Items.Add((int.Parse(schoolYear) - 1).ToString());
            int x = cbSchoolYear.Items.Add((int.Parse(schoolYear)).ToString());
            cbSchoolYear.Items.Add((int.Parse(schoolYear) + 1).ToString());
            cbSchoolYear.Items.Add((int.Parse(schoolYear) + 2).ToString());
            cbSchoolYear.SelectedIndex = x;

            semester = School.DefaultSemester;
            int z = cbSemester.Items.Add("1");
            int y = cbSemester.Items.Add("2");
            if (semester == "1")
            {
                cbSemester.SelectedIndex = z;
            }
            else
            {
                cbSemester.SelectedIndex = y;
            }

            #endregion
        }

        private void ChangeToInitialSummary_Load(object sender, EventArgs e)
        {
            dataGridViewX1.Rows.Clear();
            dataGridViewX3.Rows.Clear();

            _periodTypes = GetPeriodTypeItems();
            _periodTypes.Sort();
            _absenceList = GetAbsenceItems();
            _meritTypes = GetMeritTypes();

            #region 將節次類別,填入欄位

            periodList = new Dictionary<string, int>();

            foreach (string periodType in _periodTypes)
            {
                foreach (string each in _absenceList)
                {
                    string columnName = periodType + each;
                    int periodIndex = dataGridViewX3.Columns.Add(columnName, columnName);
                    dataGridViewX3.Columns[columnName].Width = 60;
                    dataGridViewX3.Columns[columnName].Tag = periodType + ":" + each;
                    periodList.Add(columnName, periodIndex);
                }
            }
            #endregion

            #region 塞入獎懲之預設值
            meritList = new Dictionary<string, int>();
            foreach (string merit in _meritTypes)
            {
                int columnIndex = dataGridViewX1.Columns.Add(merit, merit);

                dataGridViewX1.Columns[columnIndex].Width = 87;

                meritList.Add(merit, columnIndex);
            }
            #endregion

            dataGridViewX1.Rows.Add();
            dataGridViewX3.Rows.Add();

            BridData(_ParKey, int.Parse(schoolYear), int.Parse(semester));

        }
        /// <summary>
        /// 依傳入之學生ID,學年度,學期,以更新畫面內容
        /// </summary>
        /// <param name="id">學生ID</param>
        /// <param name="x">學年度</param>
        /// <param name="y">學期</param>
        private void BridData(string id,int x,int y)
        {
            //先清空
            dataGridViewX1.Rows.Clear();
            dataGridViewX3.Rows.Clear();

            //再Add新Row
            dataGridViewX1.Rows.Add();
            dataGridViewX3.Rows.Add();

            //取得預設學期之資料
            JHMoralScoreRecord moralScoreRecord = JHMoralScore.SelectBySchoolYearAndSemester(id, x, y);

            #region BridData
            if (moralScoreRecord != null)
            {
                XmlElement NewXml = moralScoreRecord.InitialSummary;

                if (NewXml.InnerXml != "")
                {
                    XmlNode now1 = NewXml.SelectSingleNode("AttendanceStatistics");
                    foreach (XmlElement each in now1.SelectNodes("Absence"))
                    {
                        string x1 = each.GetAttribute("PeriodType");
                        string x2 = each.GetAttribute("Name");
                        string x3 = each.GetAttribute("Count");

                        if (periodList.ContainsKey(x1 + x2))
                        {
                            dataGridViewX3.Rows[0].Cells[periodList[x1 + x2]].Value = x3;
                        }
                    }

                    XmlNode now2 = NewXml.SelectSingleNode("DisciplineStatistics/Merit");
                    XmlElement BING = (XmlElement)now2;
                    dataGridViewX1.Rows[0].Cells[meritList["大功"]].Value = BING.GetAttribute("A");
                    dataGridViewX1.Rows[0].Cells[meritList["小功"]].Value = BING.GetAttribute("B");
                    dataGridViewX1.Rows[0].Cells[meritList["嘉獎"]].Value = BING.GetAttribute("C");

                    now2 = NewXml.SelectSingleNode("DisciplineStatistics/Demerit");
                    BING = (XmlElement)now2;
                    dataGridViewX1.Rows[0].Cells[meritList["大過"]].Value = BING.GetAttribute("A");
                    dataGridViewX1.Rows[0].Cells[meritList["小過"]].Value = BING.GetAttribute("B");
                    dataGridViewX1.Rows[0].Cells[meritList["警告"]].Value = BING.GetAttribute("C");
                }
                else //資料是空的,不處理
                {

                }
            }
            else //如果此筆資料不存在,則不處理
            {

            } 
            #endregion
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            #region Save
            if (CheckErrorText())
            {
                JHMoralScoreRecord moralScoreRecord = JHMoralScore.SelectBySchoolYearAndSemester(_ParKey, int.Parse(cbSchoolYear.Text), int.Parse(cbSemester.Text));

                if (moralScoreRecord != null)
                {
                    //兜資料
                    DSXmlHelper newInitialSummary = new DSXmlHelper("InitialSummary");

                    newInitialSummary.AddElement("AttendanceStatistics");

                    #region 缺曠
                    foreach (string each1 in _periodTypes) //一般集會
                    {
                        foreach (string each2 in _absenceList)
                        {

                            string PeriodTypesAndAbsenceList = "" + dataGridViewX3.Rows[0].Cells[periodList[each1 + each2]].Value;
                            if (PeriodTypesAndAbsenceList == "") //空值就離開
                                continue;

                            int IntPeriod;
                            if (!int.TryParse(PeriodTypesAndAbsenceList, out IntPeriod)) //不是數字就離開
                                continue;

                            XmlElement xxx = newInitialSummary.AddElement("AttendanceStatistics", "Absence");
                            xxx.SetAttribute("PeriodType", each1);
                            xxx.SetAttribute("Name", each2);

                            string yyy = PeriodTypesAndAbsenceList;
                            xxx.SetAttribute("Count", yyy);
                        }
                    } 
                    #endregion

                    #region 獎勵
                    newInitialSummary.AddElement("DisciplineStatistics");
                    XmlElement zzz = newInitialSummary.AddElement("DisciplineStatistics", "Merit");
                    string a = (string)dataGridViewX1.Rows[0].Cells[meritList["大功"]].Value;
                    string b = (string)dataGridViewX1.Rows[0].Cells[meritList["小功"]].Value;
                    string c = (string)dataGridViewX1.Rows[0].Cells[meritList["嘉獎"]].Value;
                    int meritInt;
                    if (a == null) //如果是空值
                    {
                        zzz.SetAttribute("A", "0");
                    }
                    else if (int.TryParse(a, out meritInt)) //如果是數字
                    {
                        zzz.SetAttribute("A", a);
                    }

                    if (b == null) //如果是空值
                    {
                        zzz.SetAttribute("B", "0");
                    }
                    else if (int.TryParse(b, out meritInt)) //如果是數字
                    {
                        zzz.SetAttribute("B", b);
                    }

                    if (c == null) //如果是空值
                    {
                        zzz.SetAttribute("C", "0");
                    }
                    else if (int.TryParse(c, out meritInt)) //如果是數字
                    {
                        zzz.SetAttribute("C", c);
                    }  
                    #endregion

                    #region 懲戒
                    XmlElement kkk = newInitialSummary.AddElement("DisciplineStatistics", "Demerit");
                    string u = (string)dataGridViewX1.Rows[0].Cells[meritList["大過"]].Value;
                    string g = (string)dataGridViewX1.Rows[0].Cells[meritList["小過"]].Value;
                    string p = (string)dataGridViewX1.Rows[0].Cells[meritList["警告"]].Value;
                    int demeritInt;
                    if (u == null) //如果是空值
                    {
                        kkk.SetAttribute("A", "0");
                    }
                    else if (int.TryParse(u, out demeritInt)) //如果是數字
                    {
                        kkk.SetAttribute("A", u);
                    }

                    if (g == null) //如果是空值
                    {
                        kkk.SetAttribute("B", "0");
                    }
                    else if (int.TryParse(g, out demeritInt)) //如果是數字
                    {
                        kkk.SetAttribute("B", g);
                    }

                    if (p == null) //如果是空值
                    {
                        kkk.SetAttribute("C", "0");
                    }
                    else if (int.TryParse(p, out demeritInt)) //如果是數字
                    {
                        kkk.SetAttribute("C", p);
                    }
                    #endregion


                    //因為有資料所以傳入 moralScoreRecord.InitialSummary
                    moralScoreRecord.InitialSummary = newInitialSummary.BaseElement;

                    try
                    {
                        JHMoralScore.Update(moralScoreRecord);

                        poSchoolYear = moralScoreRecord.SchoolYear.ToString();
                        poSemester = moralScoreRecord.Semester.ToString();
                    }
                    catch
                    {
                        MsgBox.Show("更新資料錯誤");
                        return;
                    }

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("詳細資料：");
                    if (moralScoreRecord.Student.Class != null)
                    {
                        sb.AppendLine("學生「" + moralScoreRecord.Student.Name + "」班級「" + moralScoreRecord.Student.Class.Name + "」座號「" + moralScoreRecord.Student.SeatNo + "」學號「" + moralScoreRecord.Student.StudentNumber + "」。");
                    }
                    else
                    {
                        sb.AppendLine("學生「" + moralScoreRecord.Student.Name + "」學號「" + moralScoreRecord.Student.StudentNumber + "」。");
                    }
                    sb.AppendLine("學年度「" + moralScoreRecord.SchoolYear.ToString() + "」學期「" + moralScoreRecord.Semester.ToString() + "」。");

                    ApplicationLog.Log("日常生活表現模組.期中轉入補登", "更新期中轉入補登", "student", moralScoreRecord.Student.ID, "已更新「期中轉入補登」。\n" + sb.ToString());

                }
                else //如果是空的
                {
                    //兜資料
                    DSXmlHelper newInitialSummary = new DSXmlHelper("InitialSummary");

                    newInitialSummary.AddElement("AttendanceStatistics");

                    #region 缺曠

                    foreach (string each1 in _periodTypes) //一般集會
                    {
                        foreach (string each2 in _absenceList)
                        {
                            string PeriodTypesAndAbsenceList = (string)dataGridViewX3.Rows[0].Cells[periodList[each1 + each2]].Value;
                            if (PeriodTypesAndAbsenceList == "") //空值就離開
                                continue;

                            int IntPeriod;
                            if (!int.TryParse(PeriodTypesAndAbsenceList, out IntPeriod)) //不是數字就離開
                                continue;

                            XmlElement xxx = newInitialSummary.AddElement("AttendanceStatistics", "Absence");
                            xxx.SetAttribute("PeriodType", each1);
                            xxx.SetAttribute("Name", each2);

                            string yyy = PeriodTypesAndAbsenceList;
                            xxx.SetAttribute("Count", yyy);
                        }
                    }
                    #endregion

                    #region 獎勵
                    newInitialSummary.AddElement("DisciplineStatistics");
                    XmlElement zzz = newInitialSummary.AddElement("DisciplineStatistics", "Merit");

                    string a = (string)dataGridViewX1.Rows[0].Cells[meritList["大功"]].Value;
                    string b = (string)dataGridViewX1.Rows[0].Cells[meritList["小功"]].Value;
                    string c = (string)dataGridViewX1.Rows[0].Cells[meritList["嘉獎"]].Value;
                    int meritInt;
                    if (a == null) //如果是空值
                    {
                        zzz.SetAttribute("A", "0");
                    }
                    else if (int.TryParse(a, out meritInt)) //如果是數字
                    {
                        zzz.SetAttribute("A", a);
                    }

                    if (b == null) //如果是空值
                    {
                        zzz.SetAttribute("B", "0");
                    }
                    else if (int.TryParse(b, out meritInt)) //如果是數字
                    {
                        zzz.SetAttribute("B", b);
                    }

                    if (c == null) //如果是空值
                    {
                        zzz.SetAttribute("C", "0");
                    }
                    else if (int.TryParse(c, out meritInt)) //如果是數字
                    {
                        zzz.SetAttribute("C", c);
                    }
                    #endregion

                    #region 懲戒

                    XmlElement kkk = newInitialSummary.AddElement("DisciplineStatistics", "Demerit");
                    string u = (string)dataGridViewX1.Rows[0].Cells[meritList["大過"]].Value;
                    string g = (string)dataGridViewX1.Rows[0].Cells[meritList["小過"]].Value;
                    string p = (string)dataGridViewX1.Rows[0].Cells[meritList["警告"]].Value;
                    int demeritInt;
                    if (u == null) //如果是空值
                    {
                        kkk.SetAttribute("A", "0");
                    }
                    else if (int.TryParse(u, out demeritInt)) //如果是數字
                    {
                        kkk.SetAttribute("A", u);
                    }

                    if (g == null) //如果是空值
                    {
                        kkk.SetAttribute("B", "0");
                    }
                    else if (int.TryParse(g, out demeritInt)) //如果是數字
                    {
                        kkk.SetAttribute("B", g);
                    }

                    if (p == null) //如果是空值
                    {
                        kkk.SetAttribute("C", "0");
                    }
                    else if (int.TryParse(p, out demeritInt)) //如果是數字
                    {
                        kkk.SetAttribute("C", p);
                    }
                    #endregion

                    //因為無資料所以建立此資料
                    JHMoralScoreRecord newMoralScoreRecord = new JHMoralScoreRecord();
                    newMoralScoreRecord.RefStudentID = _ParKey;
                    newMoralScoreRecord.SchoolYear = int.Parse(cbSchoolYear.Text);
                    newMoralScoreRecord.Semester = int.Parse(cbSemester.Text);

                    //將資料填入 moralScoreRecord.InitialSummary
                    newMoralScoreRecord.InitialSummary = newInitialSummary.BaseElement;

                    try
                    {
                        JHMoralScore.Insert(newMoralScoreRecord);
                        poSchoolYear = cbSchoolYear.Text;
                        poSemester = cbSemester.Text;
                    }
                    catch
                    {
                        MsgBox.Show("新增資料錯誤");
                        return;
                    }

                    JHStudentRecord studentRecord = JHStudent.SelectByID(_ParKey);

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("詳細資料：");
                    if (studentRecord.Class != null)
                    {
                        sb.AppendLine("學生「" + studentRecord.Name + "」班級「" + studentRecord.Class.Name + "」座號「" + studentRecord.SeatNo + "」學號「" + studentRecord.StudentNumber + "」。");
                    }
                    else
                    {
                        sb.AppendLine("學生「" + studentRecord.Name + "」學號「" + studentRecord.StudentNumber + "」。");
                    }
                    sb.AppendLine("學年度「" + cbSchoolYear.Text + "」學期「" + cbSemester.Text + "」。");

                    ApplicationLog.Log("日常生活表現模組.期中轉入補登", "新增期中轉入補登", "student", studentRecord.ID, "已新增「期中轉入補登」。\n" + sb.ToString());
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                FISCA.Presentation.Controls.MsgBox.Show("資料有誤,請檢查資料正確性!");
            } 
            #endregion
        }

        public static List<string> GetAbsenceItems()
        {
            #region 取得假別項目
            string targetService = "SmartSchool.Config.GetList";

            List<string> list = new List<string>();

            DSXmlHelper helper = new DSXmlHelper("GetListRequest");
            helper.AddElement("Field");
            helper.AddElement("Field", "Content", "");
            helper.AddElement("Condition");
            helper.AddElement("Condition", "Name", "假別對照表");

            DSRequest req = new DSRequest(helper.BaseElement);
            DSResponse rsp = DSAServices.CallService(targetService, req);

            foreach (XmlElement element in rsp.GetContent().GetElements("List/AbsenceList/Absence"))
            {
                list.Add(element.GetAttribute("Name"));
            }
            return list;
            #endregion
        }

        public static List<string> GetPeriodTypeItems()
        {
            #region 取得節次類型
            string targetService = "SmartSchool.Config.GetList";

            List<string> list = new List<string>();

            DSXmlHelper helper = new DSXmlHelper("GetListRequest");
            helper.AddElement("Field");
            helper.AddElement("Field", "Content", "");
            helper.AddElement("Condition");
            helper.AddElement("Condition", "Name", "節次對照表");

            DSRequest req = new DSRequest(helper.BaseElement);
            DSResponse rsp = DSAServices.CallService(targetService, req);

            foreach (XmlElement element in rsp.GetContent().GetElements("List/Periods/Period"))
            {
                string type = element.GetAttribute("Type");

                if (!list.Contains(type))
                    list.Add(type);
            }
            return list;
            #endregion
        }

        public static List<string> GetMeritTypes()
        {
            #region 取得獎懲清單

            List<string> list = new List<string>();

            list.Add("大功");
            list.Add("小功");
            list.Add("嘉獎");
            list.Add("大過");
            list.Add("小過");
            list.Add("警告");

            return list;

            #endregion
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbSchoolYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSchoolYear.Text != "" && cbSemester.Text != "" && dataGridViewX1.ColumnCount != 0 && dataGridViewX3.ColumnCount != 0)
            {
                MsgBox.Show("期中轉入資料，應輸入於該學生轉入之「學年度/學期」。\n" + "系統建議為(" + School.DefaultSchoolYear + "學年度 第" + School.DefaultSemester + "學期)");
                BridData(_ParKey, int.Parse(cbSchoolYear.Text), int.Parse(cbSemester.Text));
            }
        }

        private void cbSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSchoolYear.Text != "" && cbSemester.Text != "" && dataGridViewX1.ColumnCount != 0 && dataGridViewX3.ColumnCount != 0)
            {
                MsgBox.Show("期中轉入資料，應輸入於該學生轉入之「學年度/學期」。\n" + "系統建議為(" + School.DefaultSchoolYear + "學年度 第" + School.DefaultSemester + "學期)");
                BridData(_ParKey, int.Parse(cbSchoolYear.Text), int.Parse(cbSemester.Text));
            }
        }

        private void dataGridViewX3_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            #region dataGridViewX3檢查機制
            DataGridViewCell _cell = dataGridViewX3.Rows[e.RowIndex].Cells[e.ColumnIndex];
            string cheng = "" + _cell.Value;

            if (cheng != "")
            {
                int indexNow;
                if (!int.TryParse(cheng, out indexNow))
                {
                    _cell.ErrorText = "資料必須為數字";

                }
                else
                {
                    _cell.ErrorText = "";
                }
            }
            #endregion
        }

        private void dataGridViewX1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            #region dataGridViewX1檢查機制
            DataGridViewCell _cell = dataGridViewX1.Rows[e.RowIndex].Cells[e.ColumnIndex];
            string cheng = "" + _cell.Value;

            if (cheng != "")
            {
                int indexNow;
                if (!int.TryParse(cheng, out indexNow))
                {
                    _cell.ErrorText = "資料必須為數字";

                }
                else
                {
                    _cell.ErrorText = "";
                }
            }
            #endregion
        }

        private bool CheckErrorText()
        {
            #region 儲存前檢查
            foreach (DataGridViewRow each in dataGridViewX3.Rows)
            {
                foreach (DataGridViewCell each2 in each.Cells)
                {
                    if (each2.ErrorText != "")
                    {
                        return false;
                    }
                }
            }

            foreach (DataGridViewRow each in dataGridViewX1.Rows)
            {
                foreach (DataGridViewCell each2 in each.Cells)
                {
                    if (each2.ErrorText != "")
                    {
                        return false;
                    }
                }
            }
            return true; 
            #endregion
        }
    }
}
