using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Xml;
using Framework;
using JHSchool.Data;
using Aspose.Words;
using HsinChu.DailyLife.Properties;
using FISCA.Presentation.Controls;
using FISCA.Presentation;
using JHSchool;
using FISCA.LogAgent;

namespace HsinChu.DailyLife.StudentRoutineWork
{
    public partial class X_StudentRoutineForm : BaseForm
    {
        private Run _run;
        private Document _doc;
        private Document _template;
        List<string> DaySchoolList = new List<string>();
        private List<JHStudentRecord> Applist = new List<JHStudentRecord>();

        private string SchoolName = "";

        /// <summary>
        /// 已過時功能畫面,請取用NewSRoutineForm.cs
        /// </summary>
        public X_StudentRoutineForm()
        {
            InitializeComponent();

            SchoolName = School.ChineseName;
            _doc = new Document();
            _doc.Sections.Clear(); //清空此Document

            _template = new Document(new MemoryStream(Resources.記錄表Word));
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            #region 點擊列印,呼叫背景模式

            btnSave.Enabled = false;

            MotherForm.SetStatusBarMessage("開始列印...");

            BackgroundWorker BackWorker = new BackgroundWorker();
            BackWorker.DoWork += new DoWorkEventHandler(BackWorker_DoWork);
            BackWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackWorker_RunWorkerCompleted);

            BackWorker.RunWorkerAsync();

            #endregion
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void BackWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            #region 主程式
            List<JHStudentRecord> list = JHStudent.SelectByIDs(Student.Instance.SelectedKeys);
            List<string> listKey = new List<string>();
            list.Sort(new Comparison<JHStudentRecord>(StudentComparer)); //排序
            Applist.Clear();
            Applist = list;

            foreach (JHStudentRecord eachStud in list)
            {
                listKey.Add(eachStud.ID);
            }
            Dictionary<string, string> BehaviorConfig = GetBehaviorConfig();
            Dictionary<string, JHPhoneRecord> phones = Getphones(listKey);                       //取得電話
            Dictionary<string, JHParentRecord> PR = GetParent(listKey);                          //取得監護人
            Dictionary<string, JHAddressRecord> addresses = GetAddress(listKey);                 //取得地址
            Dictionary<string, List<JHUpdateRecordRecord>> URR = GetUpdateRecordRecord(listKey); //取得異動記錄
            Dictionary<string, JHSemesterHistoryRecord> Semester = GetSemesterHR(listKey);       //取得學期歷程
            Dictionary<string, List<JHMoralScoreRecord>> moralSR1 = GetMoralScorel(listKey);     //取得學生平常表現
            Dictionary<string, List<JHMeritRecord>> MeritR = GetMerit(listKey);                  //取得獎勵資料
            Dictionary<string, List<JHDemeritRecord>> DemeritR = GetDemerit(listKey);            //取得懲戒資料

            #endregion

            //設定範本
            //Workbook workbook = new Workbook();
            //workbook.Open(new MemoryStream(KaoHsiung.DailyLife.Properties.Resources.記錄表));
            //int bookindex = workbook.Worksheets.AddCopy("範本");
            //workbook.Worksheets[bookindex].Name = "sheet1";
            //設定初始值
            //int StartPageNum = 0; //頁面開始
            //int EndPageNum = 52; //頁面結束
            //const int PageNum = 53; //頁面大小

            //#region 將範本的高度全部記下來 SetRowHeight
            //List<double> SetRowHeight = new List<double>();
            //for (int x = 0; x < PageNum; x++)
            //{
            //    SetRowHeight.Add(workbook.Worksheets["範本"].Cells.GetRowHeight(x));
            //}
            //#endregion

            //Aspose.Cells.Range SheetRange = workbook.Worksheets["範本"].Cells.CreateRange(0, 0, PageNum, 67);


            foreach (JHStudentRecord student in list)
            {
                Document Doc = (Document)_template.Clone(true);
                _run = new Run(Doc);
                DocumentBuilder builder = new DocumentBuilder(Doc);

                #region 取得資料
                List<string> Address = new List<string>(new string[] { JoinAddress(addresses[student.ID].Permanent), JoinAddress(addresses[student.ID].Mailing) }); //PopAddress(student.ID); //取得地址資訊
                List<string> URRecord = PopUpdateRecord(URR[student.ID]);                                    //取得新生異動
                Dictionary<string, string> ClassAndSeatNO = PopClassAndSeatNO(Semester[student.ID]);         //取得學期歷程(班級/座號)
                Dictionary<string, string> SchoolYearInfo = GetYearSeasom(Semester[student.ID]);             //取得學期歷程(學年度/學期)
                Dictionary<string, Dictionary<string, string>> MoralSR = PopMoralSR(moralSR1[student.ID]);   //學年學期<表現,Value>
                Dictionary<string, Dictionary<string, string>> _Summary1 = GetSunny(moralSR1[student.ID]);
                DaySchoolList.Clear();
                foreach (string each in _Summary1.Keys)
                {
                    if (!DaySchoolList.Contains(each))
                        DaySchoolList.Add(each);
                }
                List<string> DayList = PopDay(Semester[student.ID]);

                Dictionary<string, Dictionary<string, string>> _Summary2 = GetSunny2(moralSR1[student.ID]);


                List<JHMeritRecord> MeritList = new List<JHMeritRecord>();
                if (MeritR.Count != 0)
                {
                    if (MeritR.ContainsKey(student.ID))
                    {
                        MeritList = MeritR[student.ID];
                        MeritList.Sort(new Comparison<JHMeritRecord>(MeritComparer)); //排序獎勵資料
                    }
                }

                List<JHDemeritRecord> DemeritList = new List<JHDemeritRecord>();
                if (DemeritR.Count != 0)
                {
                    if (DemeritR.ContainsKey(student.ID))
                    {
                        DemeritList = DemeritR[student.ID];
                        DemeritList.Sort(new Comparison<JHDemeritRecord>(DemeritComparer)); //排序懲戒資料
                    }
                }
                #endregion

                List<string> name = new List<string>();
                List<string> value = new List<string>();

                #region 基本資料
                name.Add("學校名稱");
                value.Add(SchoolName);

                name.Add("學號");
                value.Add(student.StudentNumber);

                name.Add("姓名");
                value.Add(student.Name);

                name.Add("性別");
                value.Add(student.Gender);

                name.Add("身分證");
                value.Add(student.IDNumber);

                name.Add("生日");
                value.Add(SetupBirthday(student.Birthday.ToString()));

                name.Add("出生地"); //暫解GetBirthPlace
                value.Add(student.BirthPlace);

                name.Add("監護人");
                value.Add(PR[student.ID].Custodian.Name);

                name.Add("戶籍地址");
                value.Add(Address[0]);

                name.Add("電話1");
                value.Add(phones[student.ID].Permanent);

                name.Add("緊急"); //沒有
                value.Add("");

                name.Add("通訊地址");
                value.Add(Address[1]);

                name.Add("電話2");
                value.Add(phones[student.ID].Contact);

                name.Add("畢業國小");
                value.Add(URRecord[0]);

                name.Add("入學核准日");
                value.Add(URRecord[1]);

                name.Add("入學核准文");
                value.Add(URRecord[2]);

                foreach (string MorEach in BehaviorConfig.Keys)
                {
                    name.Add(MorEach);
                    value.Add(BehaviorConfig[MorEach]);
                }
                #endregion

                #region 班座處理
                if (ClassAndSeatNO.ContainsKey("11"))
                {
                    name.Add("11");
                    value.Add(ClassAndSeatNO["11"]);
                }
                if (ClassAndSeatNO.ContainsKey("12"))
                {
                    name.Add("12");
                    value.Add(ClassAndSeatNO["12"]);
                }
                if (ClassAndSeatNO.ContainsKey("21"))
                {
                    name.Add("21");
                    value.Add(ClassAndSeatNO["21"]);
                }
                if (ClassAndSeatNO.ContainsKey("22"))
                {
                    name.Add("22");
                    value.Add(ClassAndSeatNO["22"]);
                }
                if (ClassAndSeatNO.ContainsKey("31"))
                {
                    name.Add("31");
                    value.Add(ClassAndSeatNO["31"]);
                }
                if (ClassAndSeatNO.ContainsKey("32"))
                {
                    name.Add("32");
                    value.Add(ClassAndSeatNO["32"]);
                }
                #endregion

                #region 日常生活表現
                builder.MoveToMergeField("一"); //移動到(MergeField)一
                Cell cell_1 = (Cell)builder.CurrentParagraph.ParentNode; //取得該Cell

                builder.MoveToMergeField("二"); //移動到(MergeField)二
                Cell cell_2 = (Cell)builder.CurrentParagraph.ParentNode; //取得該Cell

                int RowIndex = 1;
                foreach (string MoralEach in MoralSR.Keys)
                {
                    if (RowIndex > 6) //6個Row
                        break;

                    Write(cell_1, MoralEach);
                    Write(cell_2, MoralEach);
                    int x = 1;
                    int y = 1;
                    foreach (string EachInSR in MoralSR[MoralEach].Values)
                    {
                        if (x <= 7) //第一排
                        {
                            Write(GetMoveRightCell(cell_1, x), EachInSR);
                            x++;
                        }
                        else if (y <= 2) //第二排
                        {
                            Write(GetMoveRightCell(cell_2, y), EachInSR);
                            y++;
                        }
                    }

                    cell_1 = GetMoveDownCell(cell_1, 1);
                    cell_2 = GetMoveDownCell(cell_2, 1);
                    RowIndex++;
                }
                #endregion

                #region 缺曠統計資料

                builder.MoveToMergeField("三"); //移動到(MergeField)三
                Cell cell_3 = (Cell)builder.CurrentParagraph.ParentNode; //取得該Cell

                int BreakY = 1;
                foreach (string each in _Summary1.Keys)
                {
                    if (BreakY > 6)
                        break;

                    Write(cell_3, each);
                    int x = 2;

                    foreach (string Teach in _Summary1[each].Keys)
                    {
                        if (x > 11)
                            break;

                        Write(GetMoveRightCell(cell_3, x), _Summary1[each][Teach]);
                        x++;
                    }
                    cell_3 = GetMoveDownCell(cell_3, 1);
                    BreakY++;
                }



                #endregion

                #region 獎懲統計資料

                builder.MoveToMergeField("四"); //移動到(MergeField)四
                Cell cell_4 = (Cell)builder.CurrentParagraph.ParentNode; //取得該Cell

                int BreakT = 1;
                foreach (string each in _Summary2.Keys)
                {
                    if (BreakT > 6)
                        break;

                    Write(cell_4, each);
                    int x = 1;

                    foreach (string Teach in _Summary2[each].Keys)
                    {
                        if (x > 6)
                            break;

                        Write(GetMoveRightCell(cell_4, x), _Summary2[each][Teach]);
                        x++;
                    }
                    cell_4 = GetMoveDownCell(cell_4, 1);
                    BreakT++;
                }




                #endregion

                #region 獎懲明細
                builder.MoveToMergeField("五"); //移動到(MergeField)五
                Cell cell_5 = (Cell)builder.CurrentParagraph.ParentNode; //取得該Cell
                int XonCount = 0;
                foreach (JHMeritRecord eachMR in MeritList)
                {
                    if (XonCount > 14)
                        break;

                    Write(cell_5, eachMR.SchoolYear.ToString() + "/" + eachMR.Semester.ToString());
                    string WriteNow = "大功:" + eachMR.MeritA + "," + "小功:" + eachMR.MeritB + "," + "嘉獎:" + eachMR.MeritC;
                    Write(GetMoveRightCell(cell_5, 1), WriteNow);
                    Write(GetMoveRightCell(cell_5, 2), eachMR.Reason);
                    cell_5 = GetMoveDownCell(cell_5, 1); //下移一格

                    XonCount++;
                }

                builder.MoveToMergeField("六"); //移動到(MergeField)五
                Cell cell_6 = (Cell)builder.CurrentParagraph.ParentNode; //取得該Cell
                int YonCount = 0;
                foreach (JHDemeritRecord eachDMR in DemeritList)
                {
                    if (YonCount > 14)
                        break;

                    Write(cell_6, eachDMR.SchoolYear.ToString() + "/" + eachDMR.Semester.ToString());
                    string WriteNow = "大過:" + eachDMR.DemeritA + "," + "小過:" + eachDMR.DemeritB + "," + "警告:" + eachDMR.DemeritC;
                    Write(GetMoveRightCell(cell_6, 1), WriteNow);
                    Write(GetMoveRightCell(cell_6, 2), eachDMR.Reason);

                    cell_6 = GetMoveDownCell(cell_6, 1); //下移一格

                    YonCount++;
                }

                #endregion

                #region 應到日數

                builder.MoveToMergeField("九"); //移動到(MergeField)五
                Cell cell_9 = (Cell)builder.CurrentParagraph.ParentNode; //取得該Cell
                int CountCell9 = 0;
                foreach (string eachDay in DayList)
                {
                    if (CountCell9 > 5)
                        break;

                    Write(cell_9, eachDay);
                    cell_9 = GetMoveDownCell(cell_9, 1); //下移一格
                    CountCell9++;
                }

                #endregion

                Doc.MailMerge.Execute(name.ToArray(), value.ToArray());
                Doc.MailMerge.DeleteFields();
                _doc.Sections.Add(_doc.ImportNode(Doc.FirstSection, true));

                #region 註解掉


                //頁面起始設定
                //workbook.Worksheets[bookindex].Cells.CreateRange(StartPageNum, 0, EndPageNum, 67).Copy(SheetRange);

                //#region 將欄位高度全部設定
                //int x = StartPageNum;
                //foreach (double each in SetRowHeight)
                //{
                //    workbook.Worksheets[bookindex].Cells.SetRowHeight(x, each);
                //    x++;
                //}
                //#endregion

                //#region 搜集資訊

                //地址 Address(戶籍地址&通訊地址)
                //新生異動 URRecord(畢業國小,入學核准日期,入學核准文號)
                //班座資料 ClassAndSeatNO(31,307/1)
                //學期歷程 SchoolYearInfo(31,97/1)
                //日常生活表現 MoralSR(97/1,明細list<string>)
                //出缺席統計 _Summary1(97/1,明細)
                //獎懲記錄統計 _Summary2(97/1,明細)
                //獎勵明細 MeritList(明細)
                //懲戒明細 DemeritList(明細)

                ////取得學年度清單
                //List<string> SchoolYearList = GetSchoolYear(Semester[student.ID]);

                //List<string> Address = new List<string>(new string[] { JoinAddress(addresses[student.ID].Permanent), JoinAddress(addresses[student.ID].Mailing) }); //PopAddress(student.ID); //取得地址資訊
                //List<string> URRecord = PopUpdateRecord(URR[student.ID]);                                    //取得新生異動
                //Dictionary<string, string> ClassAndSeatNO = PopClassAndSeatNO(Semester[student.ID]);         //取得學期歷程(班級/座號)
                //Dictionary<string, string> SchoolYearInfo = GetYearSeasom(Semester[student.ID]);             //取得學期歷程(學年度/學期)
                //Dictionary<string, Dictionary<string, string>> MoralSR = PopMoralSR(moralSR1[student.ID]);   //學年學期<表現,Value>
                //Dictionary<string, Dictionary<string, string>> _Summary1 = GetSunny(moralSR1[student.ID]);
                //Dictionary<string, Dictionary<string, string>> _Summary2 = GetSunny2(moralSR1[student.ID]);

                //List<JHMeritRecord> MeritList = new List<JHMeritRecord>();
                //if (MeritR.Count != 0)
                //{
                //    MeritList = MeritR[student.ID];
                //    MeritList.Sort(new Comparison<JHMeritRecord>(MeritComparer)); //排序獎勵資料
                //}

                //List<JHDemeritRecord> DemeritList = new List<JHDemeritRecord>();
                //if (DemeritR.Count != 0)
                //{
                //    DemeritList = DemeritR[student.ID];
                //    DemeritList.Sort(new Comparison<JHDemeritRecord>(DemeritComparer)); //排序懲戒資料
                //}

                //#endregion

                //workbook.Worksheets[bookindex].Cells[StartPageNum + 3, 4].PutValue(student.StudentNumber); //學號
                //workbook.Worksheets[bookindex].Cells[StartPageNum + 3, 20].PutValue(student.Name);         //姓名
                //workbook.Worksheets[bookindex].Cells[StartPageNum + 3, 35].PutValue(student.Gender);       //性別

                //workbook.Worksheets[bookindex].Cells[StartPageNum + 4, 4].PutValue(student.IDNumber); //身分證
                //workbook.Worksheets[bookindex].Cells[StartPageNum + 4, 20].PutValue(student.Birthday); //生日
                //workbook.Worksheets[bookindex].Cells[StartPageNum + 4, 35].PutValue(""); //出生地

                //workbook.Worksheets[bookindex].Cells[StartPageNum + 5, 4].PutValue(PR[student.ID].Custodian.Name); //監護人
                //workbook.Worksheets[bookindex].Cells[StartPageNum + 5, 20].PutValue(Address[0]); //戶籍地址
                //workbook.Worksheets[bookindex].Cells[StartPageNum + 5, 47].PutValue(phones[student.ID].Permanent); //電話1
                //workbook.Worksheets[bookindex].Cells[StartPageNum + 6, 4].PutValue(""); //緊急聯絡人
                //workbook.Worksheets[bookindex].Cells[StartPageNum + 6, 20].PutValue(Address[0]); //聯絡地址

                //workbook.Worksheets[bookindex].Cells[StartPageNum + 6, 47].PutValue(phones[student.ID].Contact); //電話2

                ////新生
                //workbook.Worksheets[bookindex].Cells[StartPageNum + 7, 4].PutValue(URRecord[0]);
                //workbook.Worksheets[bookindex].Cells[StartPageNum + 7, 32].PutValue(URRecord[1]);
                //workbook.Worksheets[bookindex].Cells[StartPageNum + 7, 50].PutValue(URRecord[2]);

                //#region 填入班級座號
                //if (ClassAndSeatNO.ContainsKey("11") || ClassAndSeatNO.ContainsKey("71"))
                //    workbook.Worksheets[bookindex].Cells[StartPageNum + 3, 49].PutValue(ClassAndSeatNO["11"]);

                //if (ClassAndSeatNO.ContainsKey("12") || ClassAndSeatNO.ContainsKey("72"))
                //    workbook.Worksheets[bookindex].Cells[StartPageNum + 4, 49].PutValue(ClassAndSeatNO["12"]);

                //if (ClassAndSeatNO.ContainsKey("21") || ClassAndSeatNO.ContainsKey("81"))
                //    workbook.Worksheets[bookindex].Cells[StartPageNum + 3, 56].PutValue(ClassAndSeatNO["21"]);

                //if (ClassAndSeatNO.ContainsKey("22") || ClassAndSeatNO.ContainsKey("82"))
                //    workbook.Worksheets[bookindex].Cells[StartPageNum + 4, 56].PutValue(ClassAndSeatNO["22"]);

                //if (ClassAndSeatNO.ContainsKey("31") || ClassAndSeatNO.ContainsKey("91"))
                //    workbook.Worksheets[bookindex].Cells[StartPageNum + 3, 63].PutValue(ClassAndSeatNO["31"]);

                //if (ClassAndSeatNO.ContainsKey("32") || ClassAndSeatNO.ContainsKey("92"))
                //    workbook.Worksheets[bookindex].Cells[StartPageNum + 4, 63].PutValue(ClassAndSeatNO["32"]);
                //#endregion

                //#region 填入學年學期 SchoolYearInfo
                ////if (SchoolYearInfo.ContainsKey("11"))
                ////{
                ////    workbook.Worksheets[bookindex].Cells[StartPageNum + 10, 0].PutValue(SchoolYearInfo["11"]);
                ////    workbook.Worksheets[bookindex].Cells[StartPageNum + 17, 0].PutValue(SchoolYearInfo["11"]);
                ////    workbook.Worksheets[bookindex].Cells[StartPageNum + 25, 0].PutValue(SchoolYearInfo["11"]);
                ////    workbook.Worksheets[bookindex].Cells[StartPageNum + 33, 0].PutValue(SchoolYearInfo["11"]);
                ////}

                ////if (SchoolYearInfo.ContainsKey("12"))
                ////{
                ////    workbook.Worksheets[bookindex].Cells[StartPageNum + 11, 0].PutValue(SchoolYearInfo["12"]);
                ////    workbook.Worksheets[bookindex].Cells[StartPageNum + 18, 0].PutValue(SchoolYearInfo["12"]);
                ////    workbook.Worksheets[bookindex].Cells[StartPageNum + 26, 0].PutValue(SchoolYearInfo["12"]);
                ////    workbook.Worksheets[bookindex].Cells[StartPageNum + 34, 0].PutValue(SchoolYearInfo["12"]);
                ////}
                ////if (SchoolYearInfo.ContainsKey("21"))
                ////{
                ////    workbook.Worksheets[bookindex].Cells[StartPageNum + 12, 0].PutValue(SchoolYearInfo["21"]);
                ////    workbook.Worksheets[bookindex].Cells[StartPageNum + 19, 0].PutValue(SchoolYearInfo["21"]);
                ////    workbook.Worksheets[bookindex].Cells[StartPageNum + 27, 0].PutValue(SchoolYearInfo["21"]);
                ////    workbook.Worksheets[bookindex].Cells[StartPageNum + 35, 0].PutValue(SchoolYearInfo["21"]);
                ////}
                ////if (SchoolYearInfo.ContainsKey("22"))
                ////{
                ////    workbook.Worksheets[bookindex].Cells[StartPageNum + 13, 0].PutValue(SchoolYearInfo["22"]);
                ////    workbook.Worksheets[bookindex].Cells[StartPageNum + 20, 0].PutValue(SchoolYearInfo["22"]);
                ////    workbook.Worksheets[bookindex].Cells[StartPageNum + 28, 0].PutValue(SchoolYearInfo["22"]);
                ////    workbook.Worksheets[bookindex].Cells[StartPageNum + 36, 0].PutValue(SchoolYearInfo["22"]);
                ////}
                ////if (SchoolYearInfo.ContainsKey("31"))
                ////{
                ////    workbook.Worksheets[bookindex].Cells[StartPageNum + 14, 0].PutValue(SchoolYearInfo["31"]);
                ////    workbook.Worksheets[bookindex].Cells[StartPageNum + 21, 0].PutValue(SchoolYearInfo["31"]);
                ////    workbook.Worksheets[bookindex].Cells[StartPageNum + 29, 0].PutValue(SchoolYearInfo["31"]);
                ////    workbook.Worksheets[bookindex].Cells[StartPageNum + 37, 0].PutValue(SchoolYearInfo["31"]);
                ////}
                ////if (SchoolYearInfo.ContainsKey("32"))
                ////{
                ////    workbook.Worksheets[bookindex].Cells[StartPageNum + 15, 0].PutValue(SchoolYearInfo["32"]);
                ////    workbook.Worksheets[bookindex].Cells[StartPageNum + 22, 0].PutValue(SchoolYearInfo["32"]);
                ////    workbook.Worksheets[bookindex].Cells[StartPageNum + 30, 0].PutValue(SchoolYearInfo["32"]);
                ////    workbook.Worksheets[bookindex].Cells[StartPageNum + 38, 0].PutValue(SchoolYearInfo["32"]);
                ////}
                //#endregion

                //#region 填入表現 MoralSR

                //int MorCount1 = 10;
                //int MorCount2 = 17;
                //foreach (string each in MoralSR.Keys)
                //{
                //    if (MoralSR[each].Count == 0)
                //    {
                //        MorCount1++;
                //        MorCount2++;
                //        continue;
                //    }

                //    workbook.Worksheets[bookindex].Cells[StartPageNum + MorCount1, 0].PutValue(each);
                //    workbook.Worksheets[bookindex].Cells[StartPageNum + MorCount1, 1].PutValue(MoralSR[each]["愛整潔"]);
                //    workbook.Worksheets[bookindex].Cells[StartPageNum + MorCount1, 7].PutValue(MoralSR[each]["有禮貌"]);
                //    workbook.Worksheets[bookindex].Cells[StartPageNum + MorCount1, 13].PutValue(MoralSR[each]["守秩序"]);
                //    workbook.Worksheets[bookindex].Cells[StartPageNum + MorCount1, 19].PutValue(MoralSR[each]["責任心"]);
                //    workbook.Worksheets[bookindex].Cells[StartPageNum + MorCount1, 25].PutValue(MoralSR[each]["公德心"]);
                //    workbook.Worksheets[bookindex].Cells[StartPageNum + MorCount1, 31].PutValue(MoralSR[each]["友愛關懷"]);
                //    workbook.Worksheets[bookindex].Cells[StartPageNum + MorCount1, 37].PutValue(MoralSR[each]["團隊合作"]);
                //    workbook.Worksheets[bookindex].Cells[StartPageNum + MorCount1, 43].PutValue(MoralSR[each]["日常生活表現具體建議"]);

                //    workbook.Worksheets[bookindex].Cells[StartPageNum + MorCount2, 0].PutValue(each);
                //    workbook.Worksheets[bookindex].Cells[StartPageNum + MorCount2, 1].PutValue(MoralSR[each]["團體活動表現"]);
                //    workbook.Worksheets[bookindex].Cells[StartPageNum + MorCount2, 23].PutValue(MoralSR[each]["公共服務表現"]);
                //    workbook.Worksheets[bookindex].Cells[StartPageNum + MorCount2, 45].PutValue(MoralSR[each]["校內外特殊表現"]);
                //    MorCount1++;
                //    MorCount2++;
                //}

                //#endregion

                //#region 出缺席統計(定位)
                //Dictionary<string, int> NdicX = new Dictionary<string, int>();
                //int Nnum = 25;
                //foreach (string Neach in SchoolYearInfo.Values)
                //{
                //    if (Nnum == 31)
                //        break;
                //    NdicX.Add(Neach, Nnum);
                //    Nnum++;
                //}
                //Dictionary<string, int> NdicY = new Dictionary<string, int>();
                //NdicY.Add("事假(集會)", 7);
                //NdicY.Add("病假(集會)", 13);
                //NdicY.Add("曠課(集會)", 19);
                //NdicY.Add("公假(集會)", 25);
                //NdicY.Add("喪假(集會)", 31);
                //NdicY.Add("事假(一般)", 37);
                //NdicY.Add("病假(一般)", 43);
                //NdicY.Add("曠課(一般)", 49);
                //NdicY.Add("公假(一般)", 55);
                //NdicY.Add("喪假(一般)", 61);
                //#endregion

                //#region 出缺席統計(範本)
                ////<Summary>
                ////    <AttendanceStatistics>
                ////        <Absence Name="曠課" PeriodType="一般" Count="5"/>
                ////        <Absence Name="事假" PeriodType="集會" Count="2"/>
                ////    </AttendanceStatistics>
                ////    <DisciplineStatistics>
                ////        <Demerit A="" B="" C=""/>
                ////        <Merit A="" B="" C=""/>
                ////    </DisciplineStatistics>
                ////</Summary>
                //#endregion

                //#region 填入出缺席統計 _Summary1
                //foreach (string eachX in _Summary1.Keys)
                //{
                //    foreach (string eachY in _Summary1[eachX].Keys)
                //    {
                //        if (NdicY.ContainsKey(eachY) && NdicX.ContainsKey(eachX))
                //        {
                //            workbook.Worksheets[bookindex].Cells[NdicX[eachX], NdicY[eachY]].PutValue(_Summary1[eachX][eachY]);
                //        }
                //    }
                //}
                //#endregion

                //#region 獎懲統計值(定位)
                //Dictionary<string, int> KdicX = new Dictionary<string, int>();
                //int Knum = 33;
                //foreach (string Keach in SchoolYearInfo.Values)
                //{
                //    if (Knum == 39)
                //        break;
                //    KdicX.Add(Keach, Nnum);
                //    Knum++;
                //}
                //Dictionary<string, int> KdicY = new Dictionary<string, int>();
                //KdicY.Add("大功", 1);
                //KdicY.Add("小功", 12);
                //KdicY.Add("嘉獎", 23);
                //KdicY.Add("大過", 34);
                //KdicY.Add("小過", 45);
                //KdicY.Add("警告", 56);
                //#endregion

                //#region 填入獎懲統計值 _Summary2
                //foreach (string eachX in _Summary2.Keys)
                //{
                //    foreach (string eachY in _Summary2[eachX].Keys)
                //    {
                //        if (NdicY.ContainsKey(eachY) && NdicX.ContainsKey(eachX))
                //        {
                //            workbook.Worksheets[bookindex].Cells[KdicX[eachX], KdicY[eachY]].PutValue(_Summary2[eachX][eachY]);
                //        }
                //    }
                //}

                //#endregion

                //#region 填入獎懲明細

                //int MeritNumX = 0;
                //int MeritNumY = 1;
                //int MeritNumZ = 10;
                //int MeritNumT = 41;

                //foreach (JHMeritRecord MeritEach in MeritList)
                //{
                //    workbook.Worksheets[bookindex].Cells[MeritNumT, MeritNumX].PutValue(MeritEach.SchoolYear + "/" + MeritEach.Semester);
                //    workbook.Worksheets[bookindex].Cells[MeritNumT, MeritNumY].PutValue("大功:" + MeritEach.MeritA + "," + "小功:" + MeritEach.MeritB + "," + "嘉獎:" + MeritEach.MeritC);
                //    workbook.Worksheets[bookindex].Cells[MeritNumT, MeritNumZ].PutValue("事由:" + MeritEach.Reason);
                //    MeritNumT++;
                //    if (MeritNumT == 53)
                //    {
                //        MeritNumX = 33;
                //        MeritNumY = 36;
                //        MeritNumZ = 45;
                //        MeritNumT = 41;
                //    }
                //}
                //foreach (JHDemeritRecord DemeritEach in DemeritList)
                //{
                //    workbook.Worksheets[bookindex].Cells[MeritNumT, MeritNumX].PutValue(DemeritEach.SchoolYear + "/" + DemeritEach.Semester);
                //    workbook.Worksheets[bookindex].Cells[MeritNumT, MeritNumY].PutValue("大過:" + DemeritEach.DemeritA + "," + "小過:" + DemeritEach.DemeritB + "," + "警告:" + DemeritEach.DemeritC);
                //    workbook.Worksheets[bookindex].Cells[MeritNumT, MeritNumZ].PutValue("事由:" + DemeritEach.Reason);
                //    MeritNumT++;
                //    if (MeritNumT == 53)
                //    {
                //        MeritNumX = 33;
                //        MeritNumY = 36;
                //        MeritNumZ = 45;
                //        MeritNumT = 41;
                //    }
                //}

                //#endregion

                ////頁面列印分隔線
                //workbook.Worksheets[bookindex].HPageBreaks.Add(PageNum, 0);

                ////定位下一名學生起始/結束
                //StartPageNum += PageNum;
                //EndPageNum += PageNum;

                #endregion
            }
            e.Result = _doc;
        }

        private Dictionary<string, string> GetBehaviorConfig()
        {
            #region 日常生活表現設定值
            K12.Data.Configuration.ConfigData cd = K12.Data.School.Configuration["DLBehaviorConfig"];
            Dictionary<string, string> DLBdoc = new Dictionary<string, string>();

            if (cd.Contains("DailyBehavior"))
            {
                XmlElement dailyBehavior = XmlHelper.LoadXml(cd["DailyBehavior"]);
                foreach (XmlElement item in dailyBehavior.SelectNodes("Item"))
                    DLBdoc.Add(item.GetAttribute("Name"), item.GetAttribute("Name"));
            }
            //else
            //{
            //    DLBdoc.AddRange(new string[] { "", "", "", "", "" });
            //}

            if (cd.Contains("OtherRecommend"))
            {
                XmlElement dailyLifeRecommend = XmlHelper.LoadXml(cd["OtherRecommend"]);
                DLBdoc.Add("其他表現建議", dailyLifeRecommend.GetAttribute("Name"));
            }
            else
            {
                DLBdoc.Add("其他表現建議", "");
            }

            if (cd.Contains("DailyLifeRecommend"))
            {
                XmlElement dailyLifeRecommend = XmlHelper.LoadXml(cd["DailyLifeRecommend"]);
                DLBdoc.Add("日常生活表現具體建議", dailyLifeRecommend.GetAttribute("Name"));
            }
            else
            {
                DLBdoc.Add("日常生活表現具體建議", "");
            }

            //if (cd.Contains("GroupActivity"))
            //{
            //    XmlElement groupActivity = XmlHelper.LoadXml(cd["GroupActivity"]);
            //    DLBdoc.Add("團體活動表現", groupActivity.GetAttribute("Name"));
            //}
            //else
            //{
            //    DLBdoc.Add("團體活動表現", "");
            //}

            //if (cd.Contains("PublicService"))
            //{
            //    XmlElement publicService = XmlHelper.LoadXml(cd["PublicService"]);
            //    DLBdoc.Add("公共服務表現", publicService.GetAttribute("Name"));
            //}
            //else
            //{
            //    DLBdoc.Add("公共服務表現", "");
            //}

            //if (cd.Contains("SchoolSpecial"))
            //{
            //    XmlElement schoolSpecial = XmlHelper.LoadXml(cd["SchoolSpecial"]);
            //    DLBdoc.Add("校內外特殊表現", schoolSpecial.GetAttribute("Name"));
            //}
            //else
            //{
            //    DLBdoc.Add("校內外特殊表現", "");
            //}

            return DLBdoc;
            #endregion
        }

        private string SetupBirthday(string dt)
        {
            if (dt != string.Empty)
            {
                int x = dt.IndexOf(' ');
                return dt.Remove(x);
            }
            return "";


        }

        void BackWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            #region 儲存
            Document inResult = (Document)e.Result;
            btnSave.Enabled = true;

            try
            {
                SaveFileDialog SaveFileDialog1 = new SaveFileDialog();

                SaveFileDialog1.Filter = "Word (*.doc)|*.doc|所有檔案 (*.*)|*.*";
                SaveFileDialog1.FileName = "學生訓導紀錄表(新竹)";

                if (SaveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    inResult.Save(SaveFileDialog1.FileName);
                    Process.Start(SaveFileDialog1.FileName);
                    MotherForm.SetStatusBarMessage("列印完成.");

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("詳細資料：");
                    foreach (JHStudentRecord each in Applist)
                    {
                        if (each.Class != null)
                        {
                            sb.AppendLine("學生「" + each.Name + "」班級「" + each.Class.Name + "」座號「" + each.SeatNo + "」學號「" + each.StudentNumber + "」。");
                        }
                        else
                        {
                            sb.AppendLine("學生「" + each.Name + "」學號「" + each.StudentNumber + "」。");
                        }
                    }

                    ApplicationLog.Log("日常生活表現模組.學生訓導記錄表", "列印學生訓導記錄表", "已列印報表「學生訓導記錄表」共「" + Applist.Count + "」名學生。\n" + sb.ToString());
                }
                else
                {
                    FISCA.Presentation.Controls.MsgBox.Show("檔案未儲存");
                }
            }
            catch
            {
                FISCA.Presentation.Controls.MsgBox.Show("檔案儲存錯誤,請檢查檔案是否開啟中!!");
                MotherForm.SetStatusBarMessage("儲存失敗.");
            }

            #endregion

        }

        private Dictionary<string, JHPhoneRecord> Getphones(List<string> list)
        {
            #region 取得選擇學生之電話
            Dictionary<string, JHPhoneRecord> phones = new Dictionary<string, JHPhoneRecord>();
            foreach (JHPhoneRecord eachP in JHPhone.SelectByStudentIDs(list))
            {
                phones.Add(eachP.RefStudentID, eachP);
            }
            return phones;
            #endregion
        }

        public Dictionary<string, List<JHMeritRecord>> GetMerit(List<string> list)
        {
            #region 取得獎勵
            Dictionary<string, List<JHMeritRecord>> MeritR = new Dictionary<string, List<JHMeritRecord>>();
            foreach (JHMeritRecord eachP in JHMerit.SelectByStudentIDs(list))
            {
                if (!MeritR.ContainsKey(eachP.RefStudentID))
                    MeritR.Add(eachP.RefStudentID, new List<JHMeritRecord>());
                MeritR[eachP.RefStudentID].Add(eachP);
            }
            return MeritR;
            #endregion
        }

        public Dictionary<string, List<JHDemeritRecord>> GetDemerit(List<string> list)
        {
            #region 取得懲戒
            Dictionary<string, List<JHDemeritRecord>> DemeritR = new Dictionary<string, List<JHDemeritRecord>>();
            foreach (JHDemeritRecord eachP in JHDemerit.SelectByStudentIDs(list))
            {
                if (!DemeritR.ContainsKey(eachP.RefStudentID))
                    DemeritR.Add(eachP.RefStudentID, new List<JHDemeritRecord>());
                DemeritR[eachP.RefStudentID].Add(eachP);
            }
            return DemeritR;
            #endregion
        }

        private Dictionary<string, JHParentRecord> GetParent(List<string> list)
        {
            #region 取得選擇學生之監護人(Get)
            Dictionary<string, JHParentRecord> PR = new Dictionary<string, JHParentRecord>();
            foreach (JHParentRecord eachP in JHParent.SelectByStudentIDs(list))
            {
                PR.Add(eachP.RefStudentID, eachP);
            }
            return PR;
            #endregion
        }

        private Dictionary<string, List<JHMoralScoreRecord>> GetMoralScorel(List<string> list)
        {
            #region 取得學生日常生活表現(Get)
            Dictionary<string, List<JHMoralScoreRecord>> Moral_Score1 = new Dictionary<string, List<JHMoralScoreRecord>>();

            List<JHMoralScoreRecord> Jo = JHMoralScore.SelectByStudentIDs(list);
            Jo.Sort(new Comparison<JHMoralScoreRecord>(SchoolYearComparer));

            foreach (JHMoralScoreRecord eachP in Jo) //Key,Value
            {
                if (!Moral_Score1.ContainsKey(eachP.RefStudentID))
                    Moral_Score1.Add(eachP.RefStudentID, new List<JHMoralScoreRecord>());
                Moral_Score1[eachP.RefStudentID].Add(eachP);
            }

            foreach (string each in list)
            {
                if (!Moral_Score1.ContainsKey(each))
                    Moral_Score1.Add(each, new List<JHMoralScoreRecord>());
            }

            return Moral_Score1;
            #endregion
        }

        private Dictionary<string, Dictionary<string, string>> PopMoralSR(List<JHMoralScoreRecord> _JHMoralScoreRecord)
        {
            #region 取得日常行為生活表現XmlElement
            //學年學期<類別,value>
            Dictionary<string, Dictionary<string, string>> Dic = new Dictionary<string, Dictionary<string, string>>();

            foreach (JHMoralScoreRecord _MSR in _JHMoralScoreRecord)
            {
                Dictionary<string, string> DicList = new Dictionary<string, string>();

                string SchoolYearSem = _MSR.SchoolYear.ToString() + "/" + _MSR.Semester.ToString();

                if (_MSR.TextScore.InnerXml == "")
                {
                    //Dic[eachX] = DicList;
                    continue;
                }


                XmlHelper hlp = new XmlHelper(_MSR.TextScore);

                //日常生活表現
                if (hlp.GetElements("DailyBehavior/Item") != null)
                {
                    foreach (XmlElement each1 in hlp.GetElements("DailyBehavior/Item"))
                    {
                        XmlHelper item = new XmlHelper(each1);
                        DicList.Add(item.GetString("@Name"), item.GetString("@Degree"));
                    }
                }
                else //如果沒有DailyBehavior
                {
                    DicList.Add("愛整潔", "");
                    DicList.Add("有禮貌", "");
                    DicList.Add("守秩序", "");
                    DicList.Add("責任心", "");
                    DicList.Add("公德心", "");
                    DicList.Add("友愛關懷", "");
                    DicList.Add("團隊合作", "");
                }

                //其他表現建議
                if (hlp.GetString("OtherRecommend/@Name") != string.Empty)
                {
                    DicList.Add(hlp.GetString("OtherRecommend/@Name"), hlp.GetString("OtherRecommend/@Description"));
                }
                else
                {
                    DicList.Add("其他表現建議", "");
                }

                //日常生活表現具體建議
                if (hlp.GetString("DailyLifeRecommend/@Name") != string.Empty)
                {
                    DicList.Add(hlp.GetString("DailyLifeRecommend/@Name"), hlp.GetString("DailyLifeRecommend/@Description"));
                }
                else
                {
                    DicList.Add("日常生活表現具體建議", "");
                }


                ////團體活動表現
                //string Group = "";
                //foreach (XmlElement each2 in hlp.GetElements("GroupActivity/Item"))
                //{
                //    XmlHelper item = new XmlHelper(each2);
                //    Group += item.GetString("@Name") + "：" + item.GetString("@Degree") + "，" + item.GetString("@Description") + "。";
                //}
                //if (hlp.GetString("GroupActivity/@Name") != string.Empty)
                //{
                //    DicList.Add(hlp.GetString("GroupActivity/@Name"), Group);
                //}

                ////公共服務表現
                //string PublicS = "";
                //foreach (XmlElement each3 in hlp.GetElements("PublicService/Item"))
                //{
                //    XmlHelper item = new XmlHelper(each3);
                //    PublicS += item.GetString("@Name") + "：" + item.GetString("@Description") + "。";
                //}
                //if (hlp.GetString("PublicService/@Name") != string.Empty)
                //{
                //    DicList.Add(hlp.GetString("PublicService/@Name"), PublicS);
                //}

                //string Special = "";
                //foreach (XmlElement each3 in hlp.GetElements("SchoolSpecial/Item"))
                //{
                //    XmlHelper item = new XmlHelper(each3);
                //    Special += item.GetString("@Name") + "：" + item.GetString("@Description") + "。";
                //}
                //if (hlp.GetString("SchoolSpecial/@Name") != string.Empty)
                //{
                //    DicList.Add(hlp.GetString("SchoolSpecial/@Name"), Special);
                //}


                Dic[SchoolYearSem] = DicList;
            }
            return Dic;
            #endregion
        }

        private Dictionary<string, Dictionary<string, string>> GetSunny(List<JHMoralScoreRecord> list)
        {
            #region 取得學生缺曠統計值
            Dictionary<string, Dictionary<string, string>> dic = new Dictionary<string, Dictionary<string, string>>();

            foreach (JHMoralScoreRecord each in list)
            {
                Dictionary<string, string> SummyTemp = new Dictionary<string, string>();
                SummyTemp.Add("事假(一般)", "");
                SummyTemp.Add("病假(一般)", "");
                SummyTemp.Add("曠課(一般)", "");
                SummyTemp.Add("公假(一般)", "");
                SummyTemp.Add("喪假(一般)", "");
                SummyTemp.Add("事假(集會)", "");
                SummyTemp.Add("病假(集會)", "");
                SummyTemp.Add("曠課(集會)", "");
                SummyTemp.Add("公假(集會)", "");
                SummyTemp.Add("喪假(集會)", "");

                XmlHelper hlp = new XmlHelper(each.Summary);
                foreach (XmlElement Xmleach in hlp.GetElements("AttendanceStatistics/Absence"))
                {
                    XmlHelper hlpEach = new XmlHelper(Xmleach);
                    string x = hlpEach.GetString("@Name") + "(" + hlpEach.GetString("@PeriodType") + ")";
                    if (SummyTemp.ContainsKey(x))
                    {
                        SummyTemp[x] = hlpEach.GetString("@Count");
                    }

                }

                dic.Add(each.SchoolYear + "/" + each.Semester, SummyTemp);
            }

            return dic;
            #endregion
        }

        private Dictionary<string, Dictionary<string, string>> GetSunny2(List<JHMoralScoreRecord> list)
        {
            #region 取得學生功過統計值
            Dictionary<string, Dictionary<string, string>> dic = new Dictionary<string, Dictionary<string, string>>();

            foreach (JHMoralScoreRecord each in list)
            {

                XmlHelper hlp = new XmlHelper(each.Summary);
                Dictionary<string, string> attenda = new Dictionary<string, string>();

                if (hlp.GetElement("DisciplineStatistics/Merit") == null)
                {
                    dic.Add(each.SchoolYear + "/" + each.Semester, attenda);
                    continue;
                }

                XmlHelper hlpEach1 = new XmlHelper(hlp.GetElement("DisciplineStatistics/Merit"));

                string MeritA = "";
                string MeritB = "";
                string MeritC = "";
                if (hlpEach1.GetString("@A") != "0")
                    MeritA = hlpEach1.GetString("@A");
                if (hlpEach1.GetString("@B") != "0")
                    MeritB = hlpEach1.GetString("@B");
                if (hlpEach1.GetString("@C") != "0")
                    MeritC = hlpEach1.GetString("@C");
                attenda.Add("大功", MeritA);
                attenda.Add("小功", MeritB);
                attenda.Add("嘉獎", MeritC);

                XmlHelper hlpEach2 = new XmlHelper(hlp.GetElement("DisciplineStatistics/Demerit"));
                string DemeritA = "";
                string DemeritB = "";
                string DemeritC = "";
                if (hlpEach2.GetString("@A") != "0")
                    DemeritA = hlpEach2.GetString("@A");
                if (hlpEach2.GetString("@B") != "0")
                    DemeritB = hlpEach2.GetString("@B");
                if (hlpEach2.GetString("@C") != "0")
                    DemeritC = hlpEach2.GetString("@C");
                attenda.Add("大過", DemeritA);
                attenda.Add("小過", DemeritB);
                attenda.Add("警告", DemeritC);

                dic.Add(each.SchoolYear + "/" + each.Semester, attenda);
            }

            return dic;
            #endregion
        }


        private static Dictionary<string, JHAddressRecord> GetAddress(List<string> list)
        {
            #region 取得選擇學生之戶籍地址(Get)
            Dictionary<string, JHAddressRecord> address = new Dictionary<string, JHAddressRecord>();
            foreach (JHAddressRecord eachP in JHAddress.SelectByStudentIDs(list))
            {
                address.Add(eachP.RefStudentID, eachP);
            }
            return address;
            #endregion
        }

        //組地址字串
        private string JoinAddress(K12.Data.AddressItem address)
        {
            return address.ZipCode + address.County + address.District + address.Area + address.Detail;
        }

        private static Dictionary<string, List<JHUpdateRecordRecord>> GetUpdateRecordRecord(List<string> list)
        {
            #region 取得選擇學生之新生異動(Get)
            Dictionary<string, List<JHUpdateRecordRecord>> UpdateRR = new Dictionary<string, List<JHUpdateRecordRecord>>();
            foreach (JHUpdateRecordRecord eachP in JHUpdateRecord.SelectByStudentIDs(list))
            {
                if (!UpdateRR.ContainsKey(eachP.StudentID))
                    UpdateRR.Add(eachP.StudentID, new List<JHUpdateRecordRecord>());

                UpdateRR[eachP.StudentID].Add(eachP);

                //UpdateRR.Add(eachP[0].StudentID, eachP);
            }

            foreach (string each in list)
            {
                if (!UpdateRR.ContainsKey(each))
                    UpdateRR.Add(each, new List<JHUpdateRecordRecord>());
            }

            return UpdateRR;
            #endregion
        }

        private List<string> PopUpdateRecord(List<JHUpdateRecordRecord> URR)
        {
            #region 取得新生異動List
            List<string> list = new List<string>();

            foreach (JHUpdateRecordRecord each in URR)
            {
                if (each.UpdateCode == "1")
                {

                    list.Add("");
                    list.Add(each.ADDate);
                    list.Add(each.ADNumber);
                    break;
                }
            }

            if (list.Count <= 0)
                list.AddRange(new string[] { "", "", "" });

            return list;
            #endregion
        }

        private static Dictionary<string, JHSemesterHistoryRecord> GetSemesterHR(List<string> list)
        {
            #region 取得選擇學生之學期歷程(Get)
            Dictionary<string, JHSemesterHistoryRecord> semester = new Dictionary<string, JHSemesterHistoryRecord>();
            foreach (JHSemesterHistoryRecord eachP in JHSemesterHistory.SelectByStudentIDs(list))
            {
                semester.Add(eachP.RefStudentID, eachP);
            }
            return semester;
            #endregion
        }

        private Dictionary<string, string> PopClassAndSeatNO(JHSemesterHistoryRecord SemesterDD)
        {
            #region 回傳班級座號清單(年級學期,班級/座號)->(11,203/10)
            Dictionary<string, string> dic = new Dictionary<string, string>();
            JHSemesterHistoryRecord SHR = SemesterDD;

            foreach (K12.Data.SemesterHistoryItem each in SHR.SemesterHistoryItems)
            {
                if (!dic.ContainsKey(each.GradeYear.ToString() + each.Semester.ToString())) //如果不包含此(年級/學期)
                {
                    dic.Add(each.GradeYear.ToString() + each.Semester.ToString(), each.ClassName + "/" + each.SeatNo); //新增(年級/學期,班級/座號)的Dictionary
                }
            }
            return dic;
            #endregion
        }

        private List<string> PopDay(JHSemesterHistoryRecord SemesterDD)
        {
            List<string> dic = new List<string>();
            JHSemesterHistoryRecord SHR = SemesterDD;
            foreach (string each in DaySchoolList)
            {
                bool check = false;
                foreach (K12.Data.SemesterHistoryItem each2 in SHR.SemesterHistoryItems)
                {
                    if (each == each2.SchoolYear.ToString() + "/" + each2.Semester.ToString())
                    {
                        check = true;
                        if (!dic.Contains(each))
                        {
                            dic.Add(each2.SchoolDayCount.ToString());
                        }
                    }
                }
                if (!check)
                {
                    dic.Add("");
                }
            }
            return dic;
        }

        private Dictionary<string, string> GetYearSeasom(JHSemesterHistoryRecord SemesterDD)
        {
            #region 回傳學年度學期(年級學期,學年度/學期)->(11,97/1)
            Dictionary<string, string> dic = new Dictionary<string, string>();
            JHSemesterHistoryRecord SHR = SemesterDD;

            foreach (K12.Data.SemesterHistoryItem each in SHR.SemesterHistoryItems)
            {
                if (!dic.ContainsKey(each.GradeYear + each.Semester.ToString()))
                {
                    dic.Add(each.GradeYear + each.Semester.ToString(), each.SchoolYear.ToString() + "/" + each.Semester.ToString());
                }
            }

            return dic;

            #endregion
        }

        private List<string> GetSchoolYear(JHSemesterHistoryRecord SemesterDD)
        {
            #region 回傳學年度學期
            List<string> list = new List<string>();
            JHSemesterHistoryRecord SHR = SemesterDD;

            foreach (K12.Data.SemesterHistoryItem each in SHR.SemesterHistoryItems)
            {
                if (list.Contains(each.SchoolYear + "/" + each.Semester))
                {
                    list.Add(each.SchoolYear + "/" + each.Semester);
                }
            }

            return list;

            #endregion
        }

        #region 排序功能

        //學生排序
        private int StudentComparer(JHStudentRecord x, JHStudentRecord y)
        {
            string xx = (x.Class == null ? "" : x.Class.Name) + x.SeatNo.ToString().PadLeft(3, '0');
            string yy = (y.Class == null ? "" : y.Class.Name) + y.SeatNo.ToString().PadLeft(3, '0');

            return xx.CompareTo(yy);
        }

        //獎勵排序
        private int MeritComparer(JHMeritRecord x, JHMeritRecord y)
        {
            string xx = x.SchoolYear.ToString() + x.Semester.ToString();
            string yy = y.SchoolYear.ToString() + y.Semester.ToString();
            return xx.CompareTo(yy);
        }

        //懲戒排序
        private int DemeritComparer(JHDemeritRecord x, JHDemeritRecord y)
        {
            string xx = x.SchoolYear.ToString() + x.Semester.ToString();
            string yy = y.SchoolYear.ToString() + y.Semester.ToString();
            return xx.CompareTo(yy);
        }

        private int SchoolYearComparer(JHMoralScoreRecord x, JHMoralScoreRecord y)
        {
            string xx = x.SchoolYear.ToString() + x.Semester.ToString();
            string yy = y.SchoolYear.ToString() + y.Semester.ToString();
            return xx.CompareTo(yy);
        }

        #endregion

        #region 阿寶友情贊助

        private Cell GetMoveDownCell(Cell cell, int count)
        {
            #region 以Cell為基準,向下移一格
            if (count == 0) return cell;

            Row row = cell.ParentRow;
            int col_index = row.IndexOf(cell);
            Table table = row.ParentTable;
            int row_index = table.Rows.IndexOf(row) + count;

            try
            {
                return table.Rows[row_index].Cells[col_index];
            }
            catch (Exception ex)
            {
                return null;
            }
            #endregion
        }

        private Cell GetMoveRightCell(Cell cell, int count)
        {
            #region 以Cell為基準,向右移一格
            if (count == 0) return cell;

            Row row = cell.ParentRow;
            int col_index = row.IndexOf(cell);
            Table table = row.ParentTable;
            int row_index = table.Rows.IndexOf(row);

            try
            {
                return table.Rows[row_index].Cells[col_index + count];
            }
            catch (Exception ex)
            {
                return null;
            }
            #endregion
        }

        private Cell GetMoveRightCellByNextSibling(Cell cell, int count)
        {
            #region 以Cell為基準,使用NextSibling向右移一格
            if (count == 0) return cell;

            Node node = cell;
            for (int i = 0; i < count; i++)
                node = node.NextSibling;

            try
            {
                return (Cell)node;
            }
            catch (Exception ex)
            {
                return null;
            }
            #endregion
        }

        private void Write(Cell cell, string text)
        {
            #region 寫入資料
            if (cell.FirstParagraph == null)
                cell.Paragraphs.Add(new Paragraph(cell.Document));
            cell.FirstParagraph.Runs.Clear();
            _run.Text = text;
            _run.Font.Size = 8;
            cell.FirstParagraph.Runs.Add(_run.Clone(true));
            #endregion
        }

        #endregion
    }
}
