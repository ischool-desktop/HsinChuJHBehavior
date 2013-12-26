﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using JHSchool.Evaluation.Mapping;

namespace JHSchool.Evaluation.Calculation.GraduationConditions
{
    internal class DailyBehaviorEval : IEvaluative
    {
        private EvaluationResult _result;
        private int _amount = 0;
        private int _degree = 0;

        private PerformanceDegreeMapper _mapper;

        public DailyBehaviorEval(XmlElement element)
        {
            _result = new EvaluationResult();

            _amount = Framework.Int.Parse(element.GetAttribute("項目"));
            _degree = Framework.Int.Parse(element.GetAttribute("表現程度"));

            //<條件 Checked="False" Type="DailyBehavior" 表現程度="1" 項目="4"/>

            _mapper = new PerformanceDegreeMapper();
        }

        #region IEvaluative 成員

        public Dictionary<string, bool> Evaluate(IEnumerable<StudentRecord> list)
        {
            _result.Clear();

            Dictionary<string, bool> passList = new Dictionary<string, bool>();

            list.SyncSemesterHistoryCache();
            Dictionary<string, List<Data.JHMoralScoreRecord>> morals = new Dictionary<string, List<JHSchool.Data.JHMoralScoreRecord>>();
            foreach (Data.JHMoralScoreRecord moral in Data.JHMoralScore.SelectByStudentIDs(list.AsKeyList()))
            {
                if (!morals.ContainsKey(moral.RefStudentID))
                    morals.Add(moral.RefStudentID, new List<JHSchool.Data.JHMoralScoreRecord>());
                morals[moral.RefStudentID].Add(moral);
            }

            // 取得學生目前班級年級
            Dictionary<string, int> studGrYearDic = new Dictionary<string, int>();
            foreach (JHSchool.Data.JHStudentRecord stud in JHSchool.Data.JHStudent.SelectByIDs(list.AsKeyList()))
            {
                if (stud.Class != null)
                    if (stud.Class.GradeYear.HasValue)
                        studGrYearDic.Add(stud.ID, stud.Class.GradeYear.Value);
            }

            bool checkInsShi = false;

            // 取得學生學期歷程
            Dictionary<string, JHSchool.Data.JHSemesterHistoryRecord> studHisRecDic = new Dictionary<string, JHSchool.Data.JHSemesterHistoryRecord>();
            foreach (JHSchool.Data.JHSemesterHistoryRecord rec in JHSchool.Data.JHSemesterHistory.SelectByStudentIDs(list.AsKeyList()))
            {
                checkInsShi = true;
                K12.Data.SemesterHistoryItem shi = new K12.Data.SemesterHistoryItem();
                shi.SchoolYear = UIConfig._UserSetSHSchoolYear;
                shi.Semester = UIConfig._UserSetSHSemester;
                if (studGrYearDic.ContainsKey(rec.RefStudentID))
                    shi.GradeYear = studGrYearDic[rec.RefStudentID];

                foreach (K12.Data.SemesterHistoryItem shiItem in rec.SemesterHistoryItems)
                    if (shiItem.SchoolYear == shi.SchoolYear && shiItem.Semester == shi.Semester)
                        checkInsShi = false;
                if (checkInsShi)
                    rec.SemesterHistoryItems.Add(shi);

                studHisRecDic.Add(rec.RefStudentID, rec);
            }

            foreach (StudentRecord student in list)
            {
                passList.Add(student.ID, true);

                Dictionary<SemesterInfo, int> gyMapping = new Dictionary<SemesterInfo, int>();
                if (studHisRecDic.ContainsKey(student.ID))
                {
                    foreach (K12.Data.SemesterHistoryItem shi in studHisRecDic[student.ID].SemesterHistoryItems)
                    {
                        SemesterInfo info = new SemesterInfo();
                        info.SchoolYear = shi.SchoolYear;
                        info.Semester = shi.Semester;
                        if (!gyMapping.ContainsKey(info))
                        {
                            gyMapping.Add(info, shi.GradeYear);
                        }
                        else
                        {
                            FISCA.Presentation.Controls.MsgBox.Show("學生：" + studHisRecDic[student.ID].Student.Name + "　學期歷程重覆");
                        }
                    }
                }

                //foreach (SemesterHistoryRecord record in student.GetSemesterHistories())
                //{
                //    SemesterInfo info = new SemesterInfo();
                //    info.SchoolYear = record.SchoolYear;
                //    info.Semester = record.Semester;
                //    if (!gyMapping.ContainsKey(info))
                //        gyMapping.Add(info, record.GradeYear);
                //}

                if (!morals.ContainsKey(student.ID)) continue;

                Dictionary<SemesterInfo, int> counter = new Dictionary<SemesterInfo, int>();
                Dictionary<SemesterInfo, bool> semsHasRecord = new Dictionary<SemesterInfo, bool>();
                foreach (SemesterInfo info in gyMapping.Keys)
                {
                    if (!counter.ContainsKey(info))
                    {
                        counter.Add(info, 0);
                        semsHasRecord.Add(info, false);
                    }
                }

                foreach (Data.JHMoralScoreRecord record in morals[student.ID])
                {
                    SemesterInfo info = new SemesterInfo();
                    info.SchoolYear = record.SchoolYear;
                    info.Semester = record.Semester;
                    foreach (XmlElement itemElement in record.TextScore.SelectNodes("DailyBehavior/Item"))
                    {
                        //<Item Degree="" Index="" Name="有禮貌"/>
                        string name = itemElement.GetAttribute("Name");
                        int degree = _mapper.GetDegreeByDescription(itemElement.GetAttribute("Degree"));

                        if (counter.ContainsKey(info))
                        {
                            if (degree <= _degree)
                                counter[info]++;
                            semsHasRecord[info] = true;
                        }
                    }
                }

                List<ResultDetail> resultList = new List<ResultDetail>();
                foreach (SemesterInfo info in counter.Keys)
                {
                    if (!gyMapping.ContainsKey(info)) continue;
                    if (counter[info] >= _amount)
                    {
                        ResultDetail rd = new ResultDetail(student.ID, "" + gyMapping[info], "" + info.Semester);
                        rd.AddMessage("日常行為不符合畢業規範");
                        rd.AddDetail("日常行為不符合畢業規範(累計" + counter[info] + "項)");
                        resultList.Add(rd);
                    }
                    else if (semsHasRecord[info] == false)
                    {
                        ResultDetail rd = new ResultDetail(student.ID, "" + gyMapping[info], "" + info.Semester);
                        rd.AddMessage("日常行為表現資料缺漏");
                        rd.AddDetail("日常行為表現資料缺漏");
                        resultList.Add(rd);
                    }
                }

                if (resultList.Count > 0)
                {
                    _result.Add(student.ID, resultList);
                    passList[student.ID] = false;
                }
            }

            return passList;
        }

        public EvaluationResult Result
        {
            get { return _result; }
        }

        #endregion
    }
}
