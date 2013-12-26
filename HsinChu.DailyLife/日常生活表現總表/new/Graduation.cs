using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JHSchool.Evaluation.Calculation.GraduationConditions;
using JHSchool.Data;

namespace JHSchool.Evaluation.Calculation
{
    public class Graduation
    {
        private IEvaluateFactory _factory;
        private Dictionary<string, IEvaluative> _evals;
        private EvaluationResult _result;

        private static Graduation _instance;
        public static Graduation Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Graduation();
                return _instance;
            }
        }

        private Graduation()
        {
            _factory = new EvaluateFactory();
            _evals = _factory.CreateEvaluativeEntities();
            _result = new EvaluationResult();
        }

        /// <summary>
        /// 檢查學生學期歷程
        /// 改寫當學年度、學期、年級加入判斷
        /// </summary>
        /// <param name="students"></param>
        /// <returns></returns>
        public List<StudentRecord> CheckSemesterHistories(IEnumerable<StudentRecord> students)
        {
            // 檢查是否新增學期歷程
            bool chkInsertShi = false;

            // 取得目前學生班級年級
            Dictionary<string,int> studGrYearDic = new Dictionary<string,int> ();
            foreach (JHStudentRecord stud in JHStudent.SelectByIDs(students.AsKeyList()))
                if(stud.Class !=null )
                    if(stud.Class.GradeYear.HasValue)
                        studGrYearDic.Add(stud.ID,stud.Class.GradeYear.Value );


            // 取得學生學期歷程
            Dictionary<string, JHSemesterHistoryRecord> studentSemesterHistoryRecordDict = new Dictionary<string, JHSemesterHistoryRecord>();
            foreach (JHSemesterHistoryRecord record in JHSemesterHistory.SelectByStudentIDs(students.AsKeyList()))
            {
                chkInsertShi = true;
                K12.Data.SemesterHistoryItem shi = new K12.Data.SemesterHistoryItem ();
                shi.SchoolYear = UIConfig._UserSetSHSchoolYear;
                shi.Semester = UIConfig._UserSetSHSemester;
                
                if (studGrYearDic.ContainsKey(record.RefStudentID))
                    shi.GradeYear = studGrYearDic[record.RefStudentID];

                // 檢查是否已經有同學年度學期學期歷程
                foreach (K12.Data.SemesterHistoryItem shiItem in record.SemesterHistoryItems)
                {
                    if (shiItem.SchoolYear == UIConfig._UserSetSHSchoolYear && shiItem.Semester == UIConfig._UserSetSHSemester)
                    {
                        chkInsertShi = false;
                        break;
                    }
                }

                // 加入當學年度學期判斷用
                if (chkInsertShi)
                    record.SemesterHistoryItems.Add(shi);

                studentSemesterHistoryRecordDict.Add(record.RefStudentID, record);
                //record.SemesterHistoryItems.
                // 判斷學生是否有學期歷程
                //if (!studentSemesterHistoryRecordDict.ContainsKey(record.RefStudentID))
                //    studentSemesterHistoryRecordDict.Add(record.RefStudentID, record);
               
            }

            List<StudentRecord> errorList = new List<StudentRecord>();

            foreach (StudentRecord student in students)
            {
                if (!studentSemesterHistoryRecordDict.ContainsKey(student.ID))
                {
                    errorList.Add(student);
                    continue;
                }

                Dictionary<SemesterData, int> semsMapping = new Dictionary<SemesterData, int>();
                foreach (K12.Data.SemesterHistoryItem item in studentSemesterHistoryRecordDict[student.ID].SemesterHistoryItems)
                {
                    SemesterData semesterData = new SemesterData(item.GradeYear, item.SchoolYear, item.Semester);
                    if (!semsMapping.ContainsKey(semesterData))
                    {
                        int index = (semesterData.GradeYear - 1) * 2 + semesterData.Semester;
                        semsMapping.Add(semesterData, index - 1);
                    }
                }

                if (semsMapping.Count <= 0)
                {
                    errorList.Add(student);
                    continue;
                }

                SemesterDataCollection sortSemesterData = new SemesterDataCollection(semsMapping.Keys);
                sortSemesterData = sortSemesterData.GetGradeYearSemester();

                //SemesterData firstSemesterData = sortSemesterData[0];
                //SemesterData lastSemesterData = sortSemesterData[sortSemesterData.Count - 1];
                //int firstIndex = semsMapping[firstSemesterData];
                //int lastIndex = semsMapping[lastSemesterData];

                //List<int> serialList = new List<int>();
                //for (int i = firstIndex; i <= lastIndex; i++)
                //    serialList.Add(i);
                //foreach (int index in semsMapping.Values)
                //{
                //    if (serialList.Contains(index))
                //        serialList.Remove(index);
                //}
                //if (serialList.Count > 0)
                //    errorList.Add(student);


                #region 均泰寫學期力成判斷方式

                // 放入學期歷程資料
                List<string> SemsChk = new List<string>();
                // 放入成績
                List<string> SemsScoreChk = new List<string>();

                foreach (SemesterData sd in semsMapping.Keys)
                    SemsChk.Add(sd.SchoolYear.ToString ()+ sd.Semester.ToString () + sd.GradeYear.ToString ());

                foreach (SemesterData sd in sortSemesterData)
                    SemsScoreChk.Add(sd.SchoolYear.ToString () + sd.Semester.ToString () + sd.GradeYear.ToString ());

                int errorCount = 0;
                foreach (string str in SemsScoreChk)
                    if(!SemsChk.Contains(str))
                        errorCount ++;

                if(errorCount >0)
                    errorList.Add(student );

                #endregion


                // ㄚ寶寫原來判斷方式
                //if (!semsMapping.ContainsKey(current))
                //    errorList.Add(student);
                //else
                //{
                //    int count = semsMapping[current];
                //    List<int> list = new List<int>();
                //    for (int i = 0; i <= count; i++)
                //        list.Add(i);
                //    foreach (int index in semsMapping.Values)
                //    {
                //        if (list.Contains(index))
                //            list.Remove(index);
                //    }
                //    if (list.Count > 0)
                //        errorList.Add(student);
                //}
            }

            return errorList;
        }

        //private List<SemesterHistoryRecord> ReadSemesterHistory(List<string> studentIDs)
        //{
        //    FunctionSpliter<string, SemesterHistoryRecord> selectData = new FunctionSpliter<string, SemesterHistoryRecord>(500, Util.MaxThread);
        //    selectData.Function = delegate(List<string> p)
        //    {
        //        return K12.Data.SemesterHistory.SelectByStudentIDs(p);
        //    };
        //    return selectData.Execute(studentIDs);
        //}

        /// <summary>
        /// 進行畢業判斷
        /// </summary>
        /// <param name="students"></param>
        /// <returns></returns>
        public Dictionary<string, bool> Evaluate(IEnumerable<StudentRecord> students)
        {
            Dictionary<string, bool> passList = new Dictionary<string, bool>();
            List<EvaluationResult> resultList = new List<EvaluationResult>();

            Dictionary<string, List<StudentRecord>> valid_students = new Dictionary<string, List<StudentRecord>>();
            foreach (StudentRecord student in students)
            {
                passList.Add(student.ID, true);
                ScoreCalcRuleRecord rule = student.GetScoreCalcRuleRecord();
                if (rule != null)
                {
                    if (!valid_students.ContainsKey(rule.ID))
                        valid_students.Add(rule.ID, new List<StudentRecord>());
                    valid_students[rule.ID].Add(student);
                }
            }

            foreach (string rule_id in valid_students.Keys)
            {
                if (_evals.ContainsKey(rule_id))
                {
                    Dictionary<string, bool> evalPassList = _evals[rule_id].Evaluate(valid_students[rule_id]);
                    foreach (string id in evalPassList.Keys)
                        passList[id] = evalPassList[id];
                    resultList.Add(_evals[rule_id].Result);
                }
            }

            if (resultList.Count > 0) MergeResults(passList, resultList);
            return passList;
        }

        private void MergeResults(Dictionary<string, bool> passList, IEnumerable<EvaluationResult> resultList)
        {
            EvaluationResult merged = new EvaluationResult();

            foreach (EvaluationResult result in resultList)
            {
                foreach (string student_id in result.Keys)
                {
                    if (passList[student_id]) continue;
                    merged.MergeResults(student_id, result[student_id]);
                }
            }

            _result = merged;
        }

        public EvaluationResult Result
        {
            get { return _result; }
        }

        public void Refresh()
        {
            _evals = _factory.CreateEvaluativeEntities();
            //_result.Clear();
        }

        internal void SetFactory(IEvaluateFactory factory)
        {
            _factory = factory;
        }

        public void TestDrive()
        {
            //List<StudentRecord> list = Student.Instance.SelectedList.GetInSchoolStudents();

            //List<StudentRecord> error = CheckSemesterHistories(list);
            //if (error.Count > 0)
            //{
            //    JHSchool.Evaluation.Calculation.GraduationForm.ErrorViewer viewer = new JHSchool.Evaluation.Calculation.GraduationForm.ErrorViewer();
            //    viewer.SetHeader("學生");
            //    foreach (StudentRecord student in error)
            //        viewer.SetMessage(student, new List<string>(new string[] { "學期歷程不完整" }));
            //    viewer.ShowDialog();
            //}
            //else
            //{
            //    Dictionary<string, bool> passList = Evaluate(list);
            //    EvaluationResult result = Result;

            //    int a = 0;
            //}
        }
    }
}
