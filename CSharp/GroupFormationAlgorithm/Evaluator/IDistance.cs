using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroupAL.Evaluator
{
    interface IDistance
    {        
        float normalizedDistanze(Criterion c1, Criterion c2);
    }
}
