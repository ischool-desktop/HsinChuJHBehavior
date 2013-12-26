using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JHSchool.Data;
using System.Xml;
using System.Windows.Forms;
using FISCA.DSAUtil;

namespace HsinChu.DailyLife.ClassDailyLife
{
    class MoralScoreList
    {
        List<JHStudentRecord> _StudentRe = new List<JHStudentRecord>();
        List<string> StudentID = new List<string>(); //一般狀態的學生
        List<JHMoralScoreRecord> MSList = new List<JHMoralScoreRecord>(); //一般狀態學生的日常生活表現資料
        Dictionary<string, JHMoralScoreRecord> _DicMoralScore = new Dictionary<string, JHMoralScoreRecord>();

        int _SchoolYear;
        int _Semester;


        /// <summary>
        /// 傳入學年度/學期,以取得選擇學生的日常生活表現資料
        /// </summary>
        /// <param name="SchoolYear"></param>
        /// <param name="Semester"></param>
        public MoralScoreList(int SchoolYear,int Semester)
        {
            _SchoolYear = SchoolYear;
            _Semester = Semester;
            Reset();
        }

        /// <summary>
        /// 更新物件內容
        /// </summary>
        public void Reset()
        {
            #region Reset
            //取得學生資料
            List<JHStudentRecord> StudentList = JHStudent.SelectByClasses(JHClass.SelectByIDs(JHSchool.Class.Instance.SelectedKeys));

            //篩選一般狀態的學生
            StudentID.Clear();
            StudentRe.Clear();
            foreach (JHStudentRecord each in StudentList)
            {
                if (each.Status == K12.Data.StudentRecord.StudentStatus.一般)
                {
                    StudentID.Add(each.ID); //取得JHMoralScore使用
                    StudentRe.Add(each);
                }
            }

            StudentRe.Sort(new Comparison<JHStudentRecord>(ClassStudentSort));

            //以一般生建立資料清單
            foreach (JHStudentRecord each in StudentRe)
            {
                if (!_DicMoralScore.ContainsKey(each.ID))
                {
                    _DicMoralScore.Add(each.ID, null);
                }
            }

            //依學年度學期,取得資料,並且建立資料清單
            MSList = JHMoralScore.SelectByStudentIDs(StudentID);

            foreach (JHMoralScoreRecord each in MSList)
            {
                if (_SchoolYear == each.SchoolYear && _Semester == each.Semester)
                {
                    if (_DicMoralScore.ContainsKey(each.RefStudentID))
                    {
                        _DicMoralScore[each.RefStudentID] = each;
                    }
                }
            } 
            #endregion

        }

        /// <summary>
        /// 取得JHStudentRecord清單
        /// </summary>
        public List<JHStudentRecord> StudentRe
        {
            get { return _StudentRe; }
            set { _StudentRe = StudentRe; }
        }

        /// <summary>
        /// 此學生是否擁有JHMoralScoreRecord資料
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool SelectMoral(string ID)
        {
            if (_DicMoralScore[ID] == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 以學生ID取得JHMoralScoreRecord資料
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public JHMoralScoreRecord GetMoralScore(string ID)
        {
            if (_DicMoralScore.ContainsKey(ID))
            {
                return _DicMoralScore[ID];
            }
            else
            {
                return null;
            }
        }

        private int ClassStudentSort(JHStudentRecord x,JHStudentRecord y)
        {
            string xx1 = x.Class.Name;
            string xx2 = x.SeatNo.HasValue ? x.SeatNo.Value.ToString().PadLeft(3, '0') : "000";
            string xx3 = xx1 + xx2;

            string yy1 = y.Class.Name;
            string yy2 = y.SeatNo.HasValue ? y.SeatNo.Value.ToString().PadLeft(3, '0') : "000";
            string yy3 = yy1 + yy2;

            return xx3.CompareTo(yy3);
        }
    }
}
