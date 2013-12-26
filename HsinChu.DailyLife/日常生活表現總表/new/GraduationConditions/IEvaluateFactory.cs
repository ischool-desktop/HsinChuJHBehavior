using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JHSchool.Evaluation.Calculation.GraduationConditions
{
    internal interface IEvaluateFactory
    {
        Dictionary<string, IEvaluative> CreateEvaluativeEntities();
    }
}
