using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JHSchool.Data;
using FISCA.Presentation.Controls;
using System.Xml;

namespace HsinChu.DailyLife
{
    class DataSelect
    {
        private int _SchoolYear;
        private int _Semster;

        /// <summary>
        /// 學年度/學期,學生ID,JHMoralScoreRecord
        /// </summary>
        private Dictionary<string, JHMoralScoreRecord> DicMoralScoreRecord = new Dictionary<string, JHMoralScoreRecord>();
        private Dictionary<string, List<string>> _list = new Dictionary<string, List<string>>();

        public Dictionary<string, DataObj> DicClassObj = new Dictionary<string, DataObj>();

        public DataSelect(int SchoolYear, int Semster, Dictionary<string, List<string>> list)
        {
            _list = list;

            _SchoolYear = SchoolYear;
            _Semster = Semster;

            Reset();

            ObjSetup();

        }

        /// <summary>
        /// 將資料整理為Obj
        /// </summary>
        private bool ObjSetup()
        {
            #region 資料整合

            if (DicMoralScoreRecord.Count == 0)
            {
                MsgBox.Show("本學年度學期無相關資料");
                return false;
            }

            DicClassObj.Clear();

            Dictionary<string, List<JHStudentRecord>> DicTempClass = new Dictionary<string, List<JHStudentRecord>>();

            foreach (JHStudentRecord each in JHStudent.SelectAll())
            {
                if (each.Status != K12.Data.StudentRecord.StudentStatus.一般)
                    continue;

                //如果有班級
                if (each.Class == null)
                    continue;

                //如果不包含
                if (!DicTempClass.ContainsKey(each.RefClassID))
                {
                    DicTempClass.Add(each.RefClassID, new List<JHStudentRecord>());
                }
                DicTempClass[each.RefClassID].Add(each);

            }

            //取得班級清單
            List<JHClassRecord> ClassList = JHClass.SelectByIDs(DicTempClass.Keys);
            //排序
            ClassList.Sort(new Comparison<JHClassRecord>(SortClass));

            //Catch教師資料
            JHTeacher.SelectAll();

            foreach (JHClassRecord classR in ClassList)
            {

                if (!DicClassObj.ContainsKey(classR.ID))
                {
                    //傳入設定值
                    DataObj obj = new DataObj(_list);
                    obj.ClassID = classR.ID;
                    obj.ClassName = classR.Name;

                    if (classR.Teacher != null)
                    {
                        obj.TeacherName = classR.Teacher.Name;
                    }

                    if (DicTempClass.ContainsKey(classR.ID))
                    {
                        obj.ClassCount = DicTempClass[classR.ID].Count;

                        foreach (JHStudentRecord stud in DicTempClass[classR.ID])
                        {
                            #region 資料檢查式
                            if (DicMoralScoreRecord.ContainsKey(stud.ID))
                            {
                                XmlElement xml = DicMoralScoreRecord[stud.ID].TextScore;
                                foreach (XmlElement xmlNode in xml.SelectNodes("DailyBehavior"))
                                {
                                    foreach (XmlElement xmlNode2 in xmlNode.SelectNodes("Item"))
                                    {
                                        if (obj.DBList.ContainsKey(xmlNode2.GetAttribute("Name")) && xmlNode2.GetAttribute("Degree") != "")
                                        {
                                            obj.DBList[xmlNode2.GetAttribute("Name")]++;
                                        }
                                    }
                                }

                                XmlElement Otherrecommend = (XmlElement)xml.SelectSingleNode("OtherRecommend");
                                if (Otherrecommend != null)
                                {
                                    if (obj.DBList.ContainsKey(Otherrecommend.GetAttribute("Name")) && Otherrecommend.GetAttribute("Description") != "")
                                    {
                                        obj.DBList[Otherrecommend.GetAttribute("Name")]++;
                                    }
                                }


                                XmlElement recommend = (XmlElement)xml.SelectSingleNode("DailyLifeRecommend");
                                if (recommend != null)
                                {
                                    if (obj.DBList.ContainsKey(recommend.GetAttribute("Name")) && recommend.GetAttribute("Description") != "")
                                    {
                                        obj.DBList[recommend.GetAttribute("Name")]++;
                                    }
                                }
                            }
                            #endregion
                        }

                        DicClassObj.Add(classR.ID, obj);
                    }
                    else
                    {
                        DicClassObj.Add(classR.ID, obj);
                    }
                }
            }

            return true;

            #endregion
        }

        /// <summary>
        /// 重置資料,取得所有(學生ID,JHMoralScoreRecord)資料,依學年度學期分類
        /// </summary>
        public void Reset()
        {
            #region 取得所有(學生ID,JHMoralScoreRecord)資料
            DicMoralScoreRecord.Clear();

            //依班級取得學生
            List<string> StudList = new List<string>();
            foreach (JHStudentRecord each in JHStudent.SelectByClasses(JHClass.SelectAll()))
            {
                if (each.Status == K12.Data.StudentRecord.StudentStatus.一般)
                {
                    StudList.Add(each.ID);
                }
            }

            //依學生/學年度/學期篩選日常生活表現資料
            List<JHMoralScoreRecord> MSRlist = JHMoralScore.SelectByStudentIDs(StudList); //取得學生的日常生活資料

            foreach (JHMoralScoreRecord each in MSRlist)
            {
                if (each.SchoolYear == _SchoolYear && each.Semester == _Semster)
                {
                    //學生/資料
                    DicMoralScoreRecord.Add(each.RefStudentID, each);
                }

            }
            #endregion
        }

        private int SortClass(JHClassRecord x, JHClassRecord y)
        {
            return x.Name.CompareTo(y.Name);
        }
    }
}
