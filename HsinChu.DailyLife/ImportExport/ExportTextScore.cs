using System;
using System.Collections.Generic;
using System.Xml;
using FISCA.Presentation;
using JHSchool.Data;
using SmartSchool.API.PlugIn;

namespace JHSchool.Behavior.ImportExport
{
    class ExportTextScore : SmartSchool.API.PlugIn.Export.Exporter
    {
        //private List<string> DailyBehaviors = new List<string>() { "愛整潔", "有禮貌", "守秩序", "責任心", "公德心", "友愛關懷", "團隊合作" };
        private List<string> DailyBehaviors = new List<string>();

        //建構子
        public ExportTextScore()
        {
            this.Image = null;
            this.Text = "匯出日常生活表現";
        }

        //覆寫
        public override void InitializeExport(SmartSchool.API.PlugIn.Export.ExportWizard wizard)
        {
            K12.Data.Configuration.ConfigData cd = K12.Data.School.Configuration["DLBehaviorConfig"];

            XmlDocument xmldoc = new XmlDocument();

            xmldoc.LoadXml(cd["DailyBehavior"]);

            foreach (XmlNode Node in xmldoc.DocumentElement.SelectNodes("Item"))
            {
                XmlElement Element = Node as XmlElement;

                if (Element != null)
                {
                    DailyBehaviors.Add(Element.GetAttribute("Name"));
                }
            }

            wizard.ExportableFields.AddRange("學年度", "學期", "具體建議","其他表現");
            wizard.ExportableFields.AddRange(DailyBehaviors);

            wizard.ExportPackage += (sender,e)=>
            {
                //取得選取學生的缺曠記錄
                List<JHMoralScoreRecord> records = JHMoralScore.SelectByStudentIDs(e.List);

                try
                {

                    //尋訪每個缺曠記錄
                    foreach (JHMoralScoreRecord record in records)
                    {
                        if (record.TextScore != null && !string.IsNullOrEmpty(record.TextScore.InnerXml))
                        {

                            //新增匯出列
                            RowData row = new RowData();

                            //指定匯出列的學生編號
                            row.ID = record.RefStudentID;

                            //判斷匯出欄位
                            foreach (string field in e.ExportFields)
                            {
                                if (wizard.ExportableFields.Contains(field))
                                {
                                    //匯出學年度及學期欄位
                                    switch (field)
                                    {
                                        case "學年度":
                                            row.Add(field, "" + record.SchoolYear);
                                            break;
                                        case "學期":
                                            row.Add(field, "" + record.Semester);
                                            break;
                                        case "具體建議":
                                            if (record.TextScore != null)
                                            {
                                                XmlElement Element = record.TextScore.SelectSingleNode("DailyLifeRecommend") as XmlElement;

                                                if (Element != null)
                                                    row.Add(field, "" + Element.GetAttribute("Description"));
                                            }
                                            break;
                                        case "其他表現":
                                            if (record.TextScore != null)
                                            {
                                                XmlElement Element = record.TextScore.SelectSingleNode("OtherRecommend") as XmlElement;

                                                if (Element != null)
                                                    row.Add(field, "" + Element.GetAttribute("Description"));
                                            }
                                            break;
                                    }

                                    if (record.TextScore != null)
                                    {
                                        //匯出日常生活表現
                                        if (DailyBehaviors.Contains(field))
                                        {
                                            XmlElement Element = record.TextScore.SelectSingleNode("DailyBehavior/Item[@Name=\"" + field + "\"]") as XmlElement;

                                            if (Element != null)
                                                row.Add(field, "" + Element.GetAttribute("Degree"));
                                        }
                                    }
                                }
                            }
                            e.Items.Add(row);
                        }
                    }
                }
                catch (Exception ve)
                {
                    MotherForm.SetStatusBarMessage(ve.Message);
                }
            };
        }
    }
}