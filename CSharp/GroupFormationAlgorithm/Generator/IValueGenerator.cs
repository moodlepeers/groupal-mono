using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroupAL.Generator
{
    public interface IValueGenerator
    {
        //use the generator to perform specific distributions
        List<float> GenerateValues(float min, float max, long amount);
    }
}
