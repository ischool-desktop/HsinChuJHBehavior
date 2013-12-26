using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HsinChu.DailyLife.TransferStudent
{
    public interface IStep
    {
        bool Valid();
        string ErrorMessage { get; set; }
        void OnChangeStep();
    }
}
