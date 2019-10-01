using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using JHSchool.Data;
using SmartSchool.API.PlugIn;

namespace JHSchool.Behavior.ImportExport
{
    class ImportTextScore : SmartSchool.API.PlugIn.Import.Importer
    {
        //改為動態增加
        //private List<string> DailyBehaviors = new List<string>() { "愛整潔", "有禮貌", "守秩序", "責任心", "公德心", "友愛關懷", "團隊合作" };
        private List<string> DailyBehaviors = new List<string>();

        private List<string> Keys = new List<string>();
        private Dictionary<string, string> Indexes = new Dictionary<string, string>();

        public ImportTextScore()
        {
            this.Image = null;
            this.Text = "匯入日常生活表現";
        }

        public override void InitializeImport(SmartSchool.API.PlugIn.Import.ImportWizard wizard)
        {
            K12.Data.Configuration.ConfigData cd = K12.Data.School.Configuration["DLBehaviorConfig"];

            XmlDocument xmldoc = new XmlDocument();

            xmldoc.LoadXml(cd["DailyBehavior"]);

            foreach (XmlNode Node in xmldoc.DocumentElement.SelectNodes("Item"))
            {
                XmlElement Element = Node as XmlElement;

                if (Element != null)
                {
                    Indexes.Add(Element.GetAttribute("Name"), Element.GetAttribute("Index"));
                    DailyBehaviors.Add(Element.GetAttribute("Name"));
                }
            }

            Dictionary<string, JHMoralScoreRecord> CacheMoralScore = new Dictionary<string, JHMoralScoreRecord>();

            wizard.RequiredFields.AddRange("學年度", "學期", "具體建議");
            wizard.ImportableFields.AddRange("學年度", "學期", "具體建議", "其他表現");

            wizard.RequiredFields.AddRange(DailyBehaviors);
            wizard.ImportableFields.AddRange(DailyBehaviors);

            wizard.PackageLimit = 400;
            wizard.ValidateStart += (sender, e) =>
            {
                foreach (JHMoralScoreRecord record in JHMoralScore.SelectByStudentIDs(e.List))
                    if (!CacheMoralScore.ContainsKey(record.ID))
                        CacheMoralScore.Add(record.ID, record);

                Keys.Clear();
            };

            wizard.ValidateRow += (sender, e) =>
            {
                int schoolYear, semester;
                #region 驗共同必填欄位
                if (!int.TryParse(e.Data["學年度"], out schoolYear))
                {
                    e.ErrorFields.Add("學年度", "必需輸入數字");
                }
                if (!int.TryParse(e.Data["學期"], out semester))
                {
                    e.ErrorFields.Add("學期", "必需輸入數字");
                }
                else if (semester != 1 && semester != 2)
                {
                    e.ErrorFields.Add("學期", "必須填入1或2");
                }
                #endregion
                #region 驗證主鍵
                string Key = e.Data.ID + "-" + e.Data["學年度"] + "-" + e.Data["學期"];

                if (Keys.Contains(Key))
                    e.ErrorMessage = "學生編號、學年及學期的組合不能重覆!";
                else
                    Keys.Add(Key);
                #endregion
            };
            wizard.ImportComplete += (sender, e) => MessageBox.Show("匯入完成");
            wizard.ImportPackage += (sender, e) =>
            {
                //要更新的德行成績列表
                List<JHMoralScoreRecord> updateMoralScores = new List<JHMoralScoreRecord>();

                //要新增的德行成績列表
                List<JHMoralScoreRecord> insertMoralScores = new List<JHMoralScoreRecord>();

                //巡訪匯入資料
                foreach (RowData row in e.Items)
                {
                    int schoolYear = int.Parse(row["學年度"]);
                    int semester = int.Parse(row["學期"]);

                    //根據學生編號、學年度及學期尋找是否有對應的德行成績
                    List<JHMoralScoreRecord> records = CacheMoralScore.Values.Where(x => x.RefStudentID.Equals(row.ID) && (x.SchoolYear == schoolYear) && x.Semester == semester).ToList();

                    //該學生的學年度及學期德行成績已存在
                    if (records.Count > 0)
                    {
                        //根據學生編號、學年度、學期及日期取得的缺曠記錄應該只有一筆
                        JHMoralScoreRecord record = records[0];

                        MakeSureElement(record);

                        if (record.TextScore != null)
                        {
                            //處理日常行為表現
                            foreach (string DailyBehavior in DailyBehaviors)
                            {
                                if (row.ContainsKey(DailyBehavior))
                                {

                                    XmlElement Element = record.TextScore.SelectSingleNode("DailyBehavior/Item[@Name='" + DailyBehavior + "']") as XmlElement;

                                    if (Element != null)
                                        Element.SetAttribute("Degree", row[DailyBehavior]);
                                    else
                                    {
                                        XmlElement NewElement = record.TextScore.OwnerDocument.CreateElement("Item");
                                        NewElement.SetAttribute("Name", DailyBehavior);

                                        if (Indexes.ContainsKey(DailyBehavior))
                                            NewElement.SetAttribute("Index", Indexes[DailyBehavior]);

                                        if (!string.IsNullOrEmpty(row[DailyBehavior]))
                                            NewElement.SetAttribute("Degree", row[DailyBehavior]);
                                        record.TextScore.SelectSingleNode("DailyBehavior").AppendChild(NewElement);
                                    }
                                }
                            }


                            if (row.ContainsKey("具體建議"))
                            {

                                XmlElement DailyLifeRecommentElement = record.TextScore.SelectSingleNode("DailyLifeRecommend") as XmlElement;

                                if (DailyLifeRecommentElement != null)
                                    DailyLifeRecommentElement.SetAttribute("Description", row["具體建議"]);
                                else
                                {
                                    XmlElement Element = record.TextScore.OwnerDocument.CreateElement("DailyLifeRecommend");
                                    Element.SetAttribute("Description", row["具體建議"]);
                                    record.TextScore.AppendChild(Element);
                                }
                            }

                            if (row.ContainsKey("其他表現"))
                            {
                                XmlElement OtherRecommentElement = record.TextScore.SelectSingleNode("OtherRecommend") as XmlElement;

                                if (OtherRecommentElement != null)
                                    OtherRecommentElement.SetAttribute("Description", row["其他表現"]);
                                else
                                {
                                    XmlElement Element = record.TextScore.OwnerDocument.CreateElement("OtherRecommend");
                                    Element.SetAttribute("Description", row["其他表現"]);
                                    record.TextScore.AppendChild(Element);
                                }
                            }

                            updateMoralScores.Add(record);
                        }
                    }
                    else
                    {
                        JHMoralScoreRecord record = new JHMoralScoreRecord();

                        record.SchoolYear = schoolYear;
                        record.Semester = semester;
                        record.RefStudentID = row.ID;

                        MakeSureElement(record);

                        //日常生活表現
                        foreach (string DailyBehavior in DailyBehaviors)
                        {
                            XmlElement NewElement = record.TextScore.OwnerDocument.CreateElement("Item");
                            NewElement.SetAttribute("Name", DailyBehavior);

                            if (Indexes.ContainsKey(DailyBehavior))
                            {
                                NewElement.SetAttribute("Index", Indexes[DailyBehavior]);
                            }

                            NewElement.SetAttribute("Degree", "");
                            if (row.ContainsKey(DailyBehavior))
                                NewElement.SetAttribute("Degree", row[DailyBehavior]);
                            record.TextScore.SelectSingleNode("DailyBehavior").AppendChild(NewElement);
                        }

                        if (row.ContainsKey("具體建議"))
                        {
                            XmlElement Element = record.TextScore.SelectSingleNode("DailyLifeRecommend") as XmlElement;

                            if (Element != null)
                                Element.SetAttribute("Description", row["具體建議"]);
                        }

                        if (row.ContainsKey("其他表現"))
                        {
                            XmlElement OtherRecommentElement = record.TextScore.SelectSingleNode("OtherRecommend") as XmlElement;

                            if (OtherRecommentElement != null)
                                OtherRecommentElement.SetAttribute("Description", row["其他表現"]);
                            else
                            {
                                XmlElement Element = record.TextScore.OwnerDocument.CreateElement("OtherRecommend");
                                Element.SetAttribute("Description", row["其他表現"]);
                                record.TextScore.AppendChild(Element);
                            }
                        }

                        insertMoralScores.Add(record);
                    }
                }

                if (updateMoralScores.Count > 0)
                    JHMoralScore.Update(updateMoralScores);
                if (insertMoralScores.Count > 0)
                    JHMoralScore.Insert(insertMoralScores);
            };
        }

        public void MakeSureElement(JHMoralScoreRecord record)
        {
            //<TextScore>
            //<DailyBehavior Name="日常行為表現"/>
            //<DailyLifeRecommend Description="" Name="日常生活表現具體建議"/>
            //<OtherRecommend Description=\"\" Name=\"其他表現\"/>
            //</TextScore>

            if (record.TextScore == null)
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml("<TextScore/>");
                record.TextScore = xmldoc.DocumentElement;
            }

            //<DailyBehavior Name=\"日常行為表現\"/>
            if (record.TextScore.SelectSingleNode("DailyBehavior") == null)
            {
                XmlDocumentFragment Fragment = record.TextScore.OwnerDocument.CreateDocumentFragment();
                Fragment.InnerXml = "<DailyBehavior Name=\"日常行為表現\"/>";
                record.TextScore.AppendChild(Fragment);
            }

            //<DailyLifeRecommend Description=\"\" Name=\"具體建議\"/>
            if (record.TextScore.SelectSingleNode("DailyLifeRecommend") == null)
            {
                XmlDocumentFragment Fragment = record.TextScore.OwnerDocument.CreateDocumentFragment();
                Fragment.InnerXml = "<DailyLifeRecommend Description=\"\" Name=\"具體建議\"/>";
                record.TextScore.AppendChild(Fragment);
            }

            //<OtherRecommend Name="其他表現"/>
            if (record.TextScore.SelectSingleNode("OtherRecommend") == null)
            {
                XmlDocumentFragment Fragment = record.TextScore.OwnerDocument.CreateDocumentFragment();
                Fragment.InnerXml = "<OtherRecommend Description=\"\" Name=\"其他表現\"/>";
                record.TextScore.AppendChild(Fragment);
            }
        }
    }
}