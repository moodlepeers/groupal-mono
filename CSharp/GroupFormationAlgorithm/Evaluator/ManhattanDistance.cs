using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroupAL.Evaluator
{
    public class ManhattanDistance : IDistance
    {
        // normes distance for each dimension
        // return max value is number of dimensions
        private float getDistance(Criterion c1, Criterion c2)
        {
            float distance = 0;
            for (int i = 0; i < c1.Value.Length; i++)
            {
                distance += Math.Abs((c1.Value[i] - c2.Value[i])/c1.MaxValue);
            }
            return distance;
        }

        public float normalizedDistanze(Criterion c1, Criterion c2)
        {
            return (getDistance(c1, c2) / c1.Value.Length);
        }
    }
}
