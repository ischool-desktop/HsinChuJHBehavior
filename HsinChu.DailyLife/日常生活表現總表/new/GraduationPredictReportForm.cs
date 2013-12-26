using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Framework;
using Aspose.Cells;
using System.IO;
using JHSchool.Evaluation.Calculation;
using JHSchool.Evaluation.Calculation.GraduationConditions;
using JHSchool.Evaluation.StudentExtendControls.Ribbon.GraduationPredictReportControls;
using JHSchool;
using JHSchool.Evaluation;


namespace HsinChu.DailyLife.日常生活表現總表.Calculation
{
    public partial class GraduationPredictReportForm : FISCA.Presentation.Controls.BaseForm
    {
        private List<StudentRecord> _errorList;
        /// <summary>
        /// 已通過畢業標準之學生為True
        /// </summary>
        public Dictionary<string, bool> _passList;
        /// <summary>
        /// 學生未達畢業標準訊息
        /// </summary>
        public EvaluationResult _result;
        private List<StudentRecord> _students;

        private MultiThreadBackgroundWorker<StudentRecord> _historyWorker, _inspectWorker;

        string _SchoolYear;
        string _Semester;

        /// <summary>
        /// 傳入判斷之學生清單
        /// </summary>
        /// <param name="students"></param>
        public GraduationPredictReportForm(List<StudentRecord> students,string SchoolYear,string Semester)
        {
            InitializeComponent();

            labelX2.Text = SchoolYear + "學年度　第" + Semester + "學期";

            _SchoolYear = SchoolYear;
            _Semester = Semester;

            _students = students;
            _errorList = new List<StudentRecord>();
            _passList = new Dictionary<string, bool>();
            _result = new EvaluationResult();

            InitializeWorkers();
        }

        private void InitializeWorkers()
        {
            //檢查學期歷程
            _historyWorker = new MultiThreadBackgroundWorker<StudentRecord>();
            _historyWorker.Loading = MultiThreadLoading.Light;
            _historyWorker.PackageSize = _students.Count; //暫解
            _historyWorker.AutoReportsProgress = true;
            _historyWorker.DoWork += new EventHandler<PackageDoWorkEventArgs<StudentRecord>>(HistoryWorker_DoWork);
            _historyWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(HistoryWorker_RunWorkerCompleted);

            //進行畢業判斷
            _inspectWorker = new MultiThreadBackgroundWorker<StudentRecord>();
            _inspectWorker.Loading = MultiThreadLoading.Light;
            _inspectWorker.PackageSize = _students.Count; //暫解
            _inspectWorker.AutoReportsProgress = true;
            _inspectWorker.DoWork += new EventHandler<PackageDoWorkEventArgs<StudentRecord>>(InspectWorker_DoWork);
            _inspectWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(InspectWorker_RunWorkerCompleted);
        }

        #region HistoryWorker Event
        private void HistoryWorker_DoWork(object sender, PackageDoWorkEventArgs<StudentRecord> e)
        {
            try
            {
                List<JHSchool.StudentRecord> error_list = e.Argument as List<JHSchool.StudentRecord>;
                //檢查學期歷程
                error_list.AddRange(Graduation.Instance.CheckSemesterHistories(e.Items));
            }
            catch (Exception ex)
            {
                SmartSchool.ErrorReporting.ReportingService.ReportException(ex);
                MsgBox.Show("檢查學期歷程時發生錯誤。" + ex.Message);
            }
        }

        private void HistoryWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //progressBar.Value = 100;

            if (e.Error != null)
            {
                SmartSchool.ErrorReporting.ReportingService.ReportException(e.Error);
                MsgBox.Show("檢查學期歷程時發生錯誤。" + e.Error.Message);
                return;
            }

            if (_errorList.Count > 0)
            {

                ErrorViewer viewer = new ErrorViewer();
                viewer.SetHeader("學生");
                foreach (StudentRecord student in _errorList)
                    viewer.SetMessage(student, new List<string>(new string[] { "學期歷程不完整" }));
                viewer.ShowDialog();
                return;
            }
            else
            {
                //lblProgress.Text = "畢業資格審查中…";
                //FISCA.LogAgent.ApplicationLog.Log("學務系統.報表", "列印報表", "列印日常生活表現總表");

                _inspectWorker.RunWorkerAsync(_students, new object[] { _passList, _result });
            }
        }
        #endregion

        #region InspectWorker Event
        private void InspectWorker_DoWork(object sender, PackageDoWorkEventArgs<StudentRecord> e)
        {
            try
            {
                object[] objs = e.Argument as object[];
                Dictionary<string, bool> passList = objs[0] as Dictionary<string, bool>;
                EvaluationResult result = objs[1] as EvaluationResult;

                //進行畢業判斷
                Dictionary<string, bool> list = Graduation.Instance.Evaluate(e.Items);
                foreach (string id in list.Keys)
                {
                    if (!passList.ContainsKey(id))
                        passList.Add(id, list[id]);
                }

                if (Graduation.Instance.Result.Count > 0)
                {
                    MergeResult(list, Graduation.Instance.Result, result);
                }
            }
            catch (Exception ex)
            {
                SmartSchool.ErrorReporting.ReportingService.ReportException(ex);
                MsgBox.Show("發生錯誤。" + ex.Message);
            }
        }

        private void InspectWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //progressBar.Value = 100;

            if (e.Error != null)
            {
                //btnExit.Enabled = true;
                SmartSchool.ErrorReporting.ReportingService.ReportException(e.Error);
                MsgBox.Show("審查時發生錯誤。" + e.Error.Message);
                return;
            }

            List<string> NotPassList = new List<string>();
            //篩選通過之學生
            foreach (string each in _passList.Keys)
            {
                if (!_passList[each])
                {
                    JHSchool.Data.JHStudentRecord sr = JHSchool.Data.JHStudent.SelectByID(each);
                    string GradeYear = sr.Class.GradeYear.HasValue ? sr.Class.GradeYear.Value.ToString() : "0";

                    if (_result.ContainsKey(each))
                    {
                        foreach (ResultDetail detail in _result[each])
                        {
                            if (detail.Semester == _Semester && detail.GradeYear == GradeYear)
                            {
                                NotPassList.Add(each);
                            }
                        }
                    }
                }

            }

            StringBuilder sb = new StringBuilder();
            if (NotPassList.Count > 0)
            {
                sb.AppendLine("不及格學生共" + NotPassList.Count + "名");
                sb.AppendLine("您是否要將學生加入待處理?");
                sb.AppendLine("(無論選擇[是]或[否]都將繼續資料列印動作...)");
                sb.AppendLine("");
                sb.AppendLine("說明：");
                sb.AppendLine("將不及格學生加入待處理");
                sb.AppendLine("可列印這些學生之畢業預警報表");
                sb.AppendLine("以瞭解學生不及格之原因");
                DialogResult dr = FISCA.Presentation.Controls.MsgBox.Show(sb.ToString(), MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);

                if (dr == DialogResult.Yes)
                {
                    K12.Presentation.NLDPanels.Student.AddToTemp(NotPassList);
                }
            }

            this.DialogResult = DialogResult.OK;
        }
        #endregion

        private void btnPrint_Click(object sender, EventArgs e)
        {
            _result.Clear();

            // 取得使用者選擇學年度學期
            int.TryParse(_SchoolYear, out UIConfig._UserSetSHSchoolYear);
            int.TryParse(_Semester, out UIConfig._UserSetSHSemester);

            this.Text = "開始進行不及格判斷!!";

            List<string> conditions = new List<string>();

            // 檢查畫面上日常生活表現使用者勾選
            foreach (Control ctrl in gpDaily.Controls)
            {
                if (ctrl is System.Windows.Forms.CheckBox)
                {
                    System.Windows.Forms.CheckBox chk = ctrl as System.Windows.Forms.CheckBox;
                    if (chk.Checked && !string.IsNullOrEmpty("" + chk.Tag))
                    {
                        if (!conditions.Contains("" + chk.Tag))
                            conditions.Add("" + chk.Tag);
                    }
                }
            }
            //建立條件物件
            IEvaluateFactory factory = new ConditionalEvaluateFactory(conditions);
            //選取條件物件
            Graduation.Instance.SetFactory(factory);
            //刷新狀態
            Graduation.Instance.Refresh();

            if (!_historyWorker.IsBusy)
            {
                btnPrint.Enabled = false;
                _historyWorker.RunWorkerAsync(_students, _errorList);
            }
        }

        private void MergeResult(Dictionary<string, bool> passList, EvaluationResult sourceResult, EvaluationResult targetResult)
        {
            foreach (string student_id in sourceResult.Keys)
            {
                if (passList.ContainsKey(student_id) && passList[student_id]) continue;
                targetResult.MergeResults(student_id, sourceResult[student_id]);
            }
        }

        private void GraduationPredictReportForm_Load(object sender, EventArgs e)
        {

        }

    }
}
