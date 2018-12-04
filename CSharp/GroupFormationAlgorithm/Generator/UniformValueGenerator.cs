using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Security.Cryptography;

namespace GroupAL.Generator
{
    public class UniformValueGenerator: IValueGenerator
    {
         List<float> IValueGenerator.GenerateValues(float min, float max, long amount)
        {
            byte[] randomNumber = new byte[1];
            RNGCryptoServiceProvider Gen = new RNGCryptoServiceProvider();
            Gen.GetBytes(randomNumber);
            Random rand = new Random((int)randomNumber[0]);

            List<float> result = new List<float>();
            for (long i=0; i<amount; i++)
            {                   
                result.Add((float)(rand.NextDouble()*(max-min))+min);
                Debug.Assert(result.Last() >= 0 && result.Last() <= 1);
            }
            return result;
        }
    }
}
