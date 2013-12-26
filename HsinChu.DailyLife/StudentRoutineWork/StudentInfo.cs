using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JHSchool.Data;
using JHSchool.Behavior.BusinessLogic;
using FISCA.UDT;

namespace HsinChu.DailyLife.StudentRoutineWork
{
    class StudentInfo
    {
        public Dictionary<string, StudentDataObj> DicStudent = new Dictionary<string, StudentDataObj>();

        private List<JHStudentRecord> ListStudent = new List<JHStudentRecord>();
        private List<JHPhoneRecord> ListPhone = new List<JHPhoneRecord>();
        private List<JHParentRecord> ListParent = new List<JHParentRecord>();

        private List<JHAddressRecord> ListAddress = new List<JHAddressRecord>();
        private List<JHMeritRecord> ListMerit = new List<JHMeritRecord>();
        private List<JHDemeritRecord> ListDeMerit = new List<JHDemeritRecord>();

        private List<JHMoralScoreRecord> ListMoralScore = new List<JHMoralScoreRecord>();
        private List<JHUpdateRecordRecord> ListUpdataRecord = new List<JHUpdateRecordRecord>();
        private List<JHSemesterHistoryRecord> ListJHSemesterHistory = new List<JHSemesterHistoryRecord>();

        private List<SLRecord> ListSLRRecord = new List<SLRecord>();

        private AccessHelper _accessHelper = new AccessHelper();

        private List<AutoSummaryRecord> ListAutoSummary = new List<AutoSummaryRecord>();

        /// <summary>
        /// 學生資料整理器
        /// </summary>
        public StudentInfo()
        {
            SetStudentBoxs();

            NewStudentData();

            SetPhone();
            SetParent();
            SetAddress();
            SetMeritList();
            SetDemeritList();
            SetMoralScoreList();
            SetUpdataList();
            SetSemesterHistory();

            SetAutoSummary(); //缺曠/獎懲統計

            SetSLRList();

            foreach (string each in DicStudent.Keys)
            {
                DicStudent[each].SetupData();
            }

        }

        /// <summary>
        /// 取得所有資料
        /// </summary>
        private void SetStudentBoxs()
        {
            List<string> StudentIDList = K12.Presentation.NLDPanels.Student.SelectedSource;
            ListStudent = JHStudent.SelectByIDs(StudentIDList); //取得學生
            ListStudent.Sort(new Comparison<JHStudentRecord>(ParseStudent));

            ListPhone = JHPhone.SelectByStudentIDs(StudentIDList); //取得電話資料
            ListParent = JHParent.SelectByStudentIDs(StudentIDList); //取得監護人資料

            ListAddress = JHAddress.SelectByStudentIDs(StudentIDList); //取得地址資料
            ListMerit = JHMerit.SelectByStudentIDs(StudentIDList); //取得獎勵資料(一對多)
            ListDeMerit = JHDemerit.SelectByStudentIDs(StudentIDList); //取得懲戒資料(一對多)
            ListMoralScore = JHMoralScore.SelectByStudentIDs(StudentIDList); //取得日常生活表現資料(一對多)
            ListUpdataRecord = JHUpdateRecord.SelectByStudentIDs(StudentIDList); //取得異動資料(一對多)
            ListJHSemesterHistory = JHSemesterHistory.SelectByStudentIDs(StudentIDList); //取得學生學期歷程

            ListAutoSummary = AutoSummary.Select(StudentIDList, null);
            ListAutoSummary.Sort(SortSchoolYearSemester);

            //服務學習記錄
            string qu = string.Join("','", StudentIDList);
            ListSLRRecord = _accessHelper.Select<SLRecord>(string.Format("ref_student_id in ('{0}')", qu));
        }

        private int SortSchoolYearSemester(AutoSummaryRecord X, AutoSummaryRecord Y)
        {
            string SortX = X.SchoolYear.ToString() + X.Semester.ToString();
            string SortY = Y.SchoolYear.ToString() + Y.Semester.ToString();
            return SortX.CompareTo(SortY);
        }

        /// <summary>
        /// new一個學生資料模型
        /// </summary>
        private void NewStudentData()
        {
            foreach (JHStudentRecord stud in ListStudent)
            {
                if (!DicStudent.ContainsKey(stud.ID))
                {
                    DicStudent.Add(stud.ID, new StudentDataObj());
                    DicStudent[stud.ID].StudentRecord = stud;
                }
            }        
        }

        /// <summary>
        /// 服務學習時數
        /// </summary>
        private void SetSLRList()
        {
            foreach (SLRecord slr in ListSLRRecord)
            {
                if (DicStudent.ContainsKey(slr.RefStudentID))
                {
                    DicStudent[slr.RefStudentID].ListSLR.Add(slr);
                }
            }

        }

        /// <summary>
        /// 填入電話資料
        /// </summary>
        private void SetPhone()
        {
            foreach (JHPhoneRecord phone in ListPhone)
            {
                if (DicStudent.ContainsKey(phone.RefStudentID))
                {
                    DicStudent[phone.RefStudentID].PhoneContact = phone.Contact;
                    DicStudent[phone.RefStudentID].PhonePermanent = phone.Permanent;
                }
            }
        }

        /// <summary>
        /// 取得監護人資料
        /// </summary>
        private void SetParent()
        {
            foreach (JHParentRecord parent in ListParent)
            {
                if (DicStudent.ContainsKey(parent.RefStudentID))
                {
                    DicStudent[parent.RefStudentID].CustodianName = parent.Custodian.Name;
                }
            }
        }

        /// <summary>
        /// 填入地址資料
        /// </summary>
        private void SetAddress()
        {
            foreach (JHAddressRecord address in ListAddress)
            {
                if (DicStudent.ContainsKey(address.RefStudentID))
                {
                    DicStudent[address.RefStudentID].AddressMailing = JoinAddress(address.Mailing);
                    DicStudent[address.RefStudentID].AddressPermanent = JoinAddress(address.Permanent);
                }
            }
        }

        /// <summary>
        /// 取得地址資料
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private string JoinAddress(K12.Data.AddressItem address)
        {
            return address.ZipCode + address.County + address.Town + address.District + address.Area + address.Detail;
        }

        /// <summary>
        /// 填入獎勵資料(一對多)
        /// </summary>
        private void SetMeritList()
        {
            foreach (JHMeritRecord merit in ListMerit)
            {
                if (DicStudent.ContainsKey(merit.RefStudentID))
                {
                    DicStudent[merit.RefStudentID].ListMerit.Add(merit);
                }
            }
        }

        /// <summary>
        /// 填入懲戒資料(一對多)
        /// </summary>
        private void SetDemeritList()
        {
            foreach (JHDemeritRecord demerit in ListDeMerit)
            {
                if (DicStudent.ContainsKey(demerit.RefStudentID))
                {
                    DicStudent[demerit.RefStudentID].ListDeMerit.Add(demerit);
                }
            }
        }

        /// <summary>
        /// 填入日常生活表現資料
        /// </summary>
        private void SetMoralScoreList()
        {
            foreach (JHMoralScoreRecord moralScore in ListMoralScore)
            {
                if (DicStudent.ContainsKey(moralScore.RefStudentID))
                {
                    DicStudent[moralScore.RefStudentID].ListMoralScore.Add(moralScore);
                }
            }
        }

        /// <summary>
        /// 填入異動記錄
        /// </summary>
        private void SetUpdataList()
        {
            foreach (JHUpdateRecordRecord UpdateRecord in ListUpdataRecord)
            {
                if (DicStudent.ContainsKey(UpdateRecord.StudentID))
                {
                    DicStudent[UpdateRecord.StudentID].ListUpdateRecord.Add(UpdateRecord);
                }
            }
        }

        /// <summary>
        /// 填入學期歷程
        /// </summary>
        private void SetSemesterHistory()
        {
            foreach (JHSemesterHistoryRecord semesterHistory in ListJHSemesterHistory)
            {
                if (DicStudent.ContainsKey(semesterHistory.RefStudentID))
                {
                    DicStudent[semesterHistory.RefStudentID].SemesterHistory = semesterHistory;
                }
            }
        }

        /// <summary>
        /// 取得缺曠統計內容
        /// </summary>
        private void SetAutoSummary()
        {
            foreach (AutoSummaryRecord auto in ListAutoSummary)
            {
                if (DicStudent.ContainsKey(auto.RefStudentID))
                {
                    DicStudent[auto.RefStudentID].ListAutoSummary.Add(auto);
                }
            }
        }

        /// <summary>
        /// 排序功能
        /// </summary>
        static public int ParseStudent(JHStudentRecord x, JHStudentRecord y)
        {
            //取得班級名稱
            string Xstring = x.Class != null ? x.Class.Name : "";
            string Ystring = y.Class != null ? y.Class.Name : "";

            //取得座號
            int Xint = x.SeatNo.HasValue ? x.SeatNo.Value : 0;
            int Yint = y.SeatNo.HasValue ? y.SeatNo.Value : 0;
            //班級名稱加:號加座號(靠右對齊補0)
            Xstring += ":" + Xint.ToString().PadLeft(2, '0');
            Ystring += ":" + Yint.ToString().PadLeft(2, '0');

            return Xstring.CompareTo(Ystring);

        }
    }
}
