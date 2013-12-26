using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JHSchool.Data;
using System.Xml;

namespace HsinChu.DailyLife
{
    class DataObj
    {
        /// <summary>
        /// 班級名稱
        /// </summary>
        public string ClassID { get; set; }

        /// <summary>
        /// 班級名稱
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 班級導師姓名
        /// </summary>
        public string TeacherName { get; set; }

        /// <summary>
        /// 班級學生人數
        /// </summary>
        public int ClassCount { get; set; }

        /// <summary>
        /// 行為表現
        /// </summary>
        public Dictionary<string, int> DBList = new Dictionary<string, int>();

        /// <summary>
        /// 建構子,
        /// </summary>
        /// <param name="list"></param>
        public DataObj(Dictionary<string, List<string>> list)
        {
            foreach (string each1 in list.Keys)
            {
                foreach (string each2 in list[each1])
                {
                    DBList.Add(each2, 0);
                }
            }
        }
    }
}
