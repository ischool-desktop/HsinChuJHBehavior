using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HsinChu.DailyLife.TransferStudent.Data.SemesterHistory
{
    public class SemesterHistoryData
    {
        public string SchoolYear { get; set; }
        public string Semester { get; set; }
        public string GradeYear { get; set; }
        public string SemesterDesc { get { return SchoolYear + Semester; } }
        public string GradeYearDesc { get { return GradeYear + "年級"; } }
    }
}
