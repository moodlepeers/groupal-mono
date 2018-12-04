using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroupAL
{
    interface  IEvaluator
    {
        float EvaluateGroupPerformanceIndex(Group g);
        float EvaluateCohortPerformanceIndex(Cohort cohort);
    }
}
