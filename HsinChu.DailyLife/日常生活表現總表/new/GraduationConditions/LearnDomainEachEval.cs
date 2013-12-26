using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Framework;

namespace JHSchool.Evaluation.Calculation.GraduationConditions
{
    internal class LearnDomainEachEval : IEvaluative
    {
        private EvaluationResult _result;
        private int _domain_count = 0;
        private decimal _score = 0;

        public LearnDomainEachEval(XmlElement element)
        {
            _result = new EvaluationResult();

            _domain_count = int.Parse(element.GetAttribute("學習領域"));
            string degree = element.GetAttribute("等第");

            //ConfigData cd = School.Configuration["等第對照表"];
            //if (!string.IsNullOrEmpty(cd["xml"]))
            //{
            //    XmlElement xml = XmlHelper.LoadXml(cd["xml"]);
            //    XmlElement scoreMapping = (XmlElement)xml.SelectSingleNode("ScoreMapping[@Name=\"" + degree + "\"]");
            //    decimal d;
            //    if (scoreMapping != null && decimal.TryParse(scoreMapping.GetAttribute("Score"), out d))
            //        _score = d;
            //}

            JHSchool.Evaluation.Mapping.DegreeMapper mapper = new JHSchool.Evaluation.Mapping.DegreeMapper();
            decimal? d = mapper.GetScoreByDegree(degree);
            if (d.HasValue) _score = d.Value;

            //<條件 Checked="True" Type="LearnDomainEach" 學習領域="2" 等第="丙"/>
        }

        #region IEvaluative 成員

        public Dictionary<string, bool> Evaluate(IEnumerable<StudentRecord> list)
        {
            _result.Clear();

            Dictionary<string, bool> passList = new Dictionary<string, bool>();

            Dictionary<string, SemesterHistoryUtility> shList = new Dictionary<string, SemesterHistoryUtility>();
            foreach (Data.JHSemesterHistoryRecord shRecord in Data.JHSemesterHistory.SelectByStudentIDs(list.AsKeyList().ToArray()))
            {
                if (!shList.ContainsKey(shRecord.RefStudentID))
                    shList.Add(shRecord.RefStudentID, new SemesterHistoryUtility(shRecord));
            }

            //list.SyncSemesterScoreCache();
            Dictionary<string, List<Data.JHSemesterScoreRecord>> studentSemesterScoreCache = new Dictionary<string, List<JHSchool.Data.JHSemesterScoreRecord>>();
            foreach (Data.JHSemesterScoreRecord record in Data.JHSemesterScore.SelectByStudentIDs(list.AsKeyList()))
            {
                if (!studentSemesterScoreCache.ContainsKey(record.RefStudentID))
                    studentSemesterScoreCache.Add(record.RefStudentID, new List<JHSchool.Data.JHSemesterScoreRecord>());
                studentSemesterScoreCache[record.RefStudentID].Add(record);
            }
            
            foreach (StudentRecord each in list)
            {
                List<ResultDetail> resultList = new List<ResultDetail>();
                SemesterHistoryUtility shUtil = shList[each.ID];

                if (studentSemesterScoreCache.ContainsKey(each.ID))
                {
                    foreach (Data.JHSemesterScoreRecord record in studentSemesterScoreCache[each.ID])
                    {
                        int count = 0;

                        foreach (K12.Data.DomainScore domain in record.Domains.Values)
                        {
                            if (domain.Score.HasValue && domain.Score.Value >= _score)
                                count++;
                        }

                        if (count < _domain_count)
                        {
                            ResultDetail rd = new ResultDetail(each.ID, "" + shUtil.GetGradeYear(record.SchoolYear, record.Semester), "" + record.Semester);
                            rd.AddMessage("學期領域成績不符合畢業規範");
                            rd.AddDetail("學期領域成績不符合畢業規範");
                            resultList.Add(rd);
                        }
                    }
                }

                if (resultList.Count > 0)
                {
                    _result.Add(each.ID, resultList);
                    passList.Add(each.ID, false);
                }
                else
                    passList.Add(each.ID, true);
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
