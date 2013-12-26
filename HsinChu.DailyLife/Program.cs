using FISCA;
using FISCA.Presentation;
using Framework;
using Framework.Security;
using HsinChu.DailyLife.ClassDailyLife;
using HsinChu.DailyLife.Properties;
using HsinChu.DailyLife.StudentRoutineWork;
using HsinChu.DailyLife.日常生活表現總表;
using JHSchool;
using JHSchool.Affair;

namespace HsinChu.DailyLife
{
    public class Program
    {
        [MainMethod()]
        public static void Main()
        {
            //毛毛蟲
            Student.Instance.AddDetailBulider(new DetailBulider<HsinChuScoreItem>()); //新竹版本

            #region 日常生活表現評等設定
            RibbonBarItem KBKeyIn = StuAdmin.Instance.RibbonBarItems["基本設定"];
            KBKeyIn["設定"]["日常生活表現評量設定"].Enable = User.Acl["JHSchool.StuAdmin.Ribbon0020"].Executable;
            KBKeyIn["設定"]["日常生活表現評量設定"].Click += delegate
            {
                HsinChuDailyLifeForm DaliyLife = new HsinChuDailyLifeForm();
                DaliyLife.ShowDialog();
            };

            RibbonBarItem StuItem = StuAdmin.Instance.RibbonBarItems["批次作業/查詢"];
            StuItem["評等輸入狀況"].Image = Properties.Resources.ink_ok_64;
            StuItem["評等輸入狀況"].Enable = User.Acl["JHSchool.StuAdmin.Ribbon00110"].Executable;
            StuItem["評等輸入狀況"].Click += delegate
            {
                DailyLifeInspect dailyInspect = new DailyLifeInspect();
                dailyInspect.ShowDialog();
            };

            #endregion

            #region 學生訓導紀錄表
            RibbonBarItem rbItem2 = Student.Instance.RibbonBarItems["資料統計"];
            //rbItem2["報表"]["學務相關報表"]["學生訓導紀錄表"].Enable = User.Acl["JHSchool.Student.Report0060"].Executable;
            rbItem2["報表"]["學務相關報表"]["學生訓導紀錄表"].Click += delegate
            {
                NewSRoutineForm StudentRW = new NewSRoutineForm();
                StudentRW.ShowDialog();
            };

            Student.Instance.SelectedListChanged += delegate
            {
                rbItem2["報表"]["學務相關報表"]["學生訓導紀錄表"].Enable =
                    Student.Instance.SelectedList.Count >= 1 & User.Acl["JHSchool.Student.Report0060"].Executable;

            };  
            #endregion

            #region 評等輸入
            //班級Ribbon
            Class.Instance.RibbonBarItems["學務"]["評等輸入"].Image = Resources.評等輸入;
            Class.Instance.RibbonBarItems["學務"]["評等輸入"].Enable = false;

            Class.Instance.RibbonBarItems["學務"]["評等輸入"].Click += delegate
            {
                new ClassScore().ShowDialog();
            };

            Class.Instance.SelectedListChanged += delegate
            {
                Class.Instance.RibbonBarItems["學務"]["評等輸入"].Enable = User.Acl["JHSchool.Class.Ribbon0080"].Executable;
            };
            #endregion

            #region 轉入補登
            Student.Instance.RibbonBarItems["學務"]["轉入補登"].Enable = false;
            Student.Instance.RibbonBarItems["學務"]["轉入補登"].Image = Properties.Resources.high_school_64;
            Student.Instance.RibbonBarItems["學務"]["轉入補登"].Click += delegate
            {
                if (Student.Instance.SelectedList.Count != 0)
                {
                    ChangeToRepairForm form = new ChangeToRepairForm(Student.Instance.SelectedList[0].ID);
                    form.ShowDialog();
                    //TransferStudentForm from = new TransferStudentForm(Student.Instance.SelectedList[0].ID);
                    //from.ShowDialog();
                }
            };
            Student.Instance.SelectedListChanged += delegate
            {
                Student.Instance.RibbonBarItems["學務"]["轉入補登"].Enable =
                     Student.Instance.SelectedList.Count == 1 & User.Acl["JHSchool.Student.Ribbon0101"].Executable;

            };
            #endregion


            #region 日常生活表現總表

            RibbonBarItem rbItem3 = Class.Instance.RibbonBarItems["資料統計"];

            rbItem3["報表"]["學務相關報表"]["日常生活表現總表"].Enable = Class.Instance.SelectedList.Count >= 1;

            rbItem3["報表"]["學務相關報表"]["日常生活表現總表"].Click += delegate
            {
                if (Class.Instance.SelectedList.Count >= 1)
                {
                    ClassDailyLifeReport StudentRW = new ClassDailyLifeReport();
                    StudentRW.ShowDialog();
                }
            };

            Class.Instance.SelectedListChanged += delegate
            {
                rbItem3["報表"]["學務相關報表"]["日常生活表現總表"].Enable =
                    Class.Instance.SelectedList.Count >= 1 & User.Acl["HsinChu.JHBehavior.Class.Report0010"].Executable;
            };

            #endregion

            #region 匯入及匯出

            RibbonBarButton rbItemImport = Student.Instance.RibbonBarItems["資料統計"]["匯入"];
            rbItemImport["學務相關匯入"]["匯入日常生活表現"].Enable = User.Acl["JHSchool.Student.Ribbon0163"].Executable;
            rbItemImport["學務相關匯入"]["匯入日常生活表現"].Click += delegate
            {
                SmartSchool.API.PlugIn.Import.Importer importer = new JHSchool.Behavior.ImportExport.ImportTextScore();
                JHSchool.Behavior.ImportExport.ImportStudentV2 wizard = new JHSchool.Behavior.ImportExport.ImportStudentV2(importer.Text, importer.Image);
                importer.InitializeImport(wizard);
                wizard.ShowDialog();
            };

            RibbonBarButton rbItemExport = Student.Instance.RibbonBarItems["資料統計"]["匯出"];
            rbItemExport["學務相關匯出"]["匯出日常生活表現"].Enable = User.Acl["JHSchool.Student.Ribbon0162"].Executable;
            rbItemExport["學務相關匯出"]["匯出日常生活表現"].Click += delegate
            {
                SmartSchool.API.PlugIn.Export.Exporter exporter = new JHSchool.Behavior.ImportExport.ExportTextScore();
                JHSchool.Behavior.ImportExport.ExportStudentV2 wizard = new JHSchool.Behavior.ImportExport.ExportStudentV2(exporter.Text, exporter.Image);
                exporter.InitializeExport(wizard);
                wizard.ShowDialog();
            };

            #endregion

            #region 註冊權限
            Catalog detail2 = RoleAclSource.Instance["學生"]["資料項目"]; //JHSchool.Student.Detail0060
            detail2.Add(new DetailItemFeature(typeof(HsinChuScoreItem)));

            Catalog ribbon2 = RoleAclSource.Instance["學生"]["功能按鈕"];
            ribbon2.Add(new RibbonFeature("JHSchool.Student.Ribbon0101", "轉入補登"));

            Catalog studentribbon1 = RoleAclSource.Instance["學生"]["功能按鈕"];
            studentribbon1.Add(new RibbonFeature("JHSchool.Student.Ribbon0162", "匯出日常生活表現"));

            Catalog studentribbon2 = RoleAclSource.Instance["學生"]["功能按鈕"];
            studentribbon2.Add(new RibbonFeature("JHSchool.Student.Ribbon0163", "匯入日常生活表現"));

            Catalog reportRibbon = RoleAclSource.Instance["學生"]["報表"];
            reportRibbon.Add(new ReportFeature("JHSchool.Student.Report0060", "學生訓導紀錄表"));

            Catalog ribbon1 = RoleAclSource.Instance["班級"]["報表"];
            ribbon1.Add(new RibbonFeature("HsinChu.JHBehavior.Class.Report0010", "日常生活表現總表"));

            Catalog ribbon3 = RoleAclSource.Instance["班級"]["功能按鈕"];
            ribbon3.Add(new RibbonFeature("JHSchool.Class.Ribbon0080", "評等輸入"));

            Catalog stuRibbon = RoleAclSource.Instance["學務作業"];
            stuRibbon.Add(new RibbonFeature("JHSchool.StuAdmin.Ribbon00110", "評等輸入狀況"));
            stuRibbon.Add(new RibbonFeature("JHSchool.StuAdmin.Ribbon0020", "日常生活表現評量設定"));

            #endregion
        }
    }
}
