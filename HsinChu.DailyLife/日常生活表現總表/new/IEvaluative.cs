using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JHSchool.Evaluation.Calculation
{
    internal interface IEvaluative
    {
        Dictionary<string, bool> Evaluate(IEnumerable<StudentRecord> list);
        EvaluationResult Result { get; }
    }
}
