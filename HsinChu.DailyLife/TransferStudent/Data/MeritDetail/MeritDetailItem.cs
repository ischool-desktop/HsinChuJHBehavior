using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HsinChu.DailyLife.TransferStudent.Data.MeritDetail
{
    public class MeritDetailItem
    {
        public DateTime OccurDate { get; set; }
        public string MeritType { get; set; }
        public int Count { get; set; }
        public string Reason { get; set; }
    }
}
