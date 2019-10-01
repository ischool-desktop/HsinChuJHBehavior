using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Framework;
using System.Xml;
using JHSchool;
using JHSchool.Data;
using FISCA.Presentation.Controls;
using FISCA.DSAUtil;
using FISCA.LogAgent;

namespace HsinChu.DailyLife
{
    public partial class HsinChuDailyLifeForm : BaseForm
    {
        //<DailyBehavior Name="日常行為表現">
        //    <Item Name="愛整潔" Index="....."/>
        //    <PerformanceDegree>
        //        <Mapping Degree="4" Desc="完全符合"/>
        //        <Mapping Degree="3" Desc="大部份符合"/>
        //        <Mapping Degree="2" Desc="部份符合"/>
        //    </PerformanceDegree>
        //</DailyBehavior>

        //<GroupActivity Name="團體活動表現">
        //    <Item Name="社團活動" SortOrder="1"/> For 高雄市
        //</GroupActivity>

        //<PublicService Name="公共服務表現">
        //    <Item Name="校內服務" SortOrder="1"/> For 高雄市
        //</PublicService>

        //<SchoolSpecial Name="校內外時特殊表現">
        //    <Item Name="校外特殊表現" SortOrder="1"/> For 高雄市
        //</SchoolSpecial>

        //<DailyLifeRecommend Name="日常生活表現具體建議"/> For 高雄市&新竹市

        //<OtherRecommend Name="其他表現建議"/>     For 新竹市

        XmlElement mapping;

        //2019/9/27 - Dylan add Log
        StringBuilder sb_log = new StringBuilder();

        public HsinChuDailyLifeForm()
        {
            InitializeComponent();

            #region 建構子
            K12.Data.Configuration.ConfigData cd = K12.Data.School.Configuration["DLBehaviorConfig"];

            sb_log.AppendLine("修改前資料：");
            #region 日常行為表現
            if (!string.IsNullOrEmpty(cd["DailyBehavior"]))
            {
                if (cd.Contains("DailyBehavior"))
                {

                    XmlElement dailyBehavior = XmlHelper.LoadXml(cd["DailyBehavior"]);

                    gpDailyBehavior.Text = dailyBehavior.GetAttribute("Name");

                    sb_log.AppendLine("");
                    sb_log.AppendLine(gpDailyBehavior.Text + "：");

                    txtDailyBehavior.Text = gpDailyBehavior.Text;

                    foreach (XmlElement item in dailyBehavior.SelectNodes("Item"))
                    {
                        dgvDailyBehavior.Rows.Add(item.GetAttribute("Name"), item.GetAttribute("Index"));

                        //2019/9/27 - Dylan add Log
                        sb_log.AppendLine(string.Format("欄位「{0}」說明「{1}」", item.GetAttribute("Name"), item.GetAttribute("Index")));
                    }

                    mapping = dailyBehavior.SelectSingleNode("PerformanceDegree") as XmlElement;
                }
            }
            #endregion

            #region 綜合評語
            if (!string.IsNullOrEmpty(cd["DailyLifeRecommend"]))
            {
                if (cd.Contains("DailyLifeRecommend"))
                {

                    XmlElement dailyLifeRecommend = XmlHelper.LoadXml(cd["DailyLifeRecommend"]);

                    gpDailyLifeRecommend.Text = dailyLifeRecommend.GetAttribute("Name");

                    txtDailyLifeRecommend.Text = gpDailyLifeRecommend.Text;
                    sb_log.AppendLine("");
                    sb_log.AppendLine("「" + gpDailyLifeRecommend.Text + "」");
                }
            }
            #endregion

            #region 其他表現
            if (!string.IsNullOrEmpty(cd["OtherRecommend"]))
            {
                if (cd.Contains("OtherRecommend"))
                {

                    XmlElement OtherRecommend = XmlHelper.LoadXml(cd["OtherRecommend"]);

                    gpOtherRecommend.Text = OtherRecommend.GetAttribute("Name");

                    txtOtherRecommend.Text = gpOtherRecommend.Text;
                    sb_log.AppendLine("");
                    sb_log.AppendLine("「" + gpOtherRecommend.Text + "」");
                }
            }
            #endregion

            sb_log.AppendLine("");

            #endregion
        }

        private void butExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void butSave_Click(object sender, EventArgs e)
        {
            K12.Data.Configuration.ConfigData cd = K12.Data.School.Configuration["DLBehaviorConfig"];

            #region 日常行為表現
            //<DailyBehavior Name=\"日常行為表現\">
            //    <Item Name=\"愛整潔\" Index=\"抽屜乾淨\"></Item>
            //    <Item Name=\"有禮貌\" Index=\"懂得向老師,學長敬禮\"></Item>
            //    <Item Name=\"守秩序\" Index=\"自習時間能夠安靜自習\"></Item>
            //    <Item Name=\"責任心\" Index=\"打掃時間,徹底整理自己打掃範圍\"></Item>
            //    <Item Name=\"公德心\" Index=\"不亂丟垃圾\"></Item>
            //    <Item Name=\"友愛關懷\" Index=\"懂得關心同學朋友\"></Item>
            //    <Item Name=\"團隊合作\" Index=\"團體活動能夠遵守相關規定\"></Item>
            //    <PerformanceDegree>
            //        <Mapping Degree=\"8\" Desc=\"完全符合\"></Mapping>
            //        <Mapping Degree=\"7\" Desc=\"大部份符合\"></Mapping>
            //        <Mapping Degree=\"6\" Desc=\"部份符合\"></Mapping>
            //        <Mapping Degree=\"5\" Desc=\"尚再努力\"></Mapping>
            //        <Mapping Degree=\"4\" Desc=\"努力又努力\"></Mapping>
            //        <Mapping Degree=\"3\" Desc=\"不努力也不努力\"></Mapping>
            //        <Mapping Degree=\"2\" Desc=\"有點努力\"></Mapping>
            //        <Mapping Degree=\"1\" Desc=\"很不努力\"></Mapping>
            //    </PerformanceDegree>
            //</DailyBehavior>"
            DSXmlHelper dailyBehavior = new DSXmlHelper("DailyBehavior");
            //dailyBehavior.SetAttribute(".", "Name", gpDailyBehavior.Text);
            dailyBehavior.SetAttribute(".", "Name", txtDailyBehavior.Text);

            sb_log.AppendLine("修改後資料：");

            sb_log.AppendLine("");
            sb_log.AppendLine(string.Format("「{0}」修改為「{1}」", gpDailyBehavior.Text, txtDailyBehavior.Text));


            foreach (DataGridViewRow row in dgvDailyBehavior.Rows)
            {
                if (row.IsNewRow) continue;

                string rowValue = "" + row.Cells[0].Value + row.Cells[1].Value;
                if (string.IsNullOrEmpty(rowValue.Trim()))
                    continue;

                XmlElement node = dailyBehavior.AddElement("Item");
                node.SetAttribute("Name", "" + row.Cells[0].Value);
                node.SetAttribute("Index", "" + row.Cells[1].Value);

                sb_log.AppendLine(string.Format("欄位「{0}」說明「{1}」", "" + row.Cells[0].Value, "" + row.Cells[1].Value));

            }

            dailyBehavior.AddElement("PerformanceDegree");

            if (mapping != null)
            {
                foreach (XmlElement each in mapping.SelectNodes("Mapping"))
                {
                    XmlElement node = dailyBehavior.AddElement("PerformanceDegree", "Mapping");
                    node.SetAttribute("Degree", "" + each.Attributes[0].Value);
                    node.SetAttribute("Desc", "" + each.Attributes[1].Value);
                }
            }
            cd["DailyBehavior"] = dailyBehavior.ToString();

            #endregion

            #region 綜合評語
            //    <DailyLifeRecommend Name="綜合評語"/>
            DSXmlHelper dailyLifeRecommend = new DSXmlHelper("DailyLifeRecommend");
            dailyLifeRecommend.SetAttribute(".", "Name", txtDailyLifeRecommend.Text);
            cd["DailyLifeRecommend"] = dailyLifeRecommend.ToString();

            sb_log.AppendLine("");
            sb_log.AppendLine(string.Format("「{0}」修改為「{1}」", gpDailyLifeRecommend.Text, txtDailyLifeRecommend.Text));


            #endregion

            #region 其他表現
            //    <OtherRecommend Name="其他表現"/>
            DSXmlHelper OtherRecommend = new DSXmlHelper("OtherRecommend");
            OtherRecommend.SetAttribute(".", "Name", txtOtherRecommend.Text);
            cd["OtherRecommend"] = OtherRecommend.ToString();
            sb_log.AppendLine("");
            sb_log.AppendLine(string.Format("「{0}」修改為「{1}」", gpOtherRecommend.Text, txtOtherRecommend.Text));

            #endregion

            try
            {
                cd.Save();
                K12.Data.School.Configuration.Sync("DLBehaviorConfig");
            }
            catch
            {
                FISCA.Presentation.Controls.MsgBox.Show("儲存失敗");
                return;
            }

            ApplicationLog.Log("日常生活表現設定", "修改", sb_log.ToString());
            FISCA.Presentation.Controls.MsgBox.Show("儲存成功");

            this.Close();
        }

        #region TextBox相關事件
        private void txtDailyBehavior_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode & Keys.Escape) == Keys.Escape)
            {
                gpDailyBehavior.Text = txtDailyBehavior.Text;
                txtDailyBehavior.SendToBack();
                return;
            }

            if ((e.KeyCode & Keys.Enter) == Keys.Enter)
            {
                gpDailyBehavior.Text = txtDailyBehavior.Text;
                txtDailyBehavior.SendToBack();
            }
        }

        private void txtDailyLifeRecommend_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode & Keys.Escape) == Keys.Escape)
            {
                gpDailyLifeRecommend.Text = txtDailyLifeRecommend.Text;
                txtDailyLifeRecommend.SendToBack();
                return;
            }

            if ((e.KeyCode & Keys.Enter) == Keys.Enter)
            {
                gpDailyLifeRecommend.Text = txtDailyLifeRecommend.Text;
                txtDailyLifeRecommend.SendToBack();
            }
        }

        private void txtOtherRecommend_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode & Keys.Escape) == Keys.Escape)
            {
                gpOtherRecommend.Text = txtOtherRecommend.Text;
                txtOtherRecommend.SendToBack();
                return;
            }

            if ((e.KeyCode & Keys.Enter) == Keys.Enter)
            {
                gpOtherRecommend.Text = txtOtherRecommend.Text;
                txtOtherRecommend.SendToBack();
            }
        }

        #endregion

        #region 修改名稱
        private void lnkDailyBehavior_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            txtDailyBehavior.BringToFront();
            txtDailyBehavior.Focus();
        }

        private void lnkDailyLifeRecommend_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            txtDailyLifeRecommend.BringToFront();
            txtDailyLifeRecommend.Focus();
        }

        #endregion

        private void lnkOtherRecommend_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            txtOtherRecommend.BringToFront();
            txtOtherRecommend.Focus();
        }

    }
}
