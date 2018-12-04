using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Diagnostics;

namespace GroupAL.Generator
{
    public class GaussianValueGenerator: IValueGenerator
    {
        List<float> IValueGenerator.GenerateValues(float min, float max, long amount)
        {
            byte[] randomNumber = new byte[1];
            RNGCryptoServiceProvider Gen = new RNGCryptoServiceProvider();
            Gen.GetBytes(randomNumber);
            Random rand = new Random((int)randomNumber[0]);

            List<float> result = new List<float>();
            // x=mu + sigma*randn(m,n)   for gaussian normal distribution

            // we use http://en.wikipedia.org/wiki/Box_Muller_transform  and only calculate z1
            
            for (long i=0; i<amount; i++)
            {        
                double u1 = rand.NextDouble();
                double u2 = rand.NextDouble();
                //while ((u1 * u1) + (u2 * u2) >= 1)
                //{
                //    u1 = rand.NextDouble();
                //    u2 = rand.NextDouble();
                //}
                //double R = Math.Sqrt((-2) * Math.Log(u1)); // radius
                //double z1 = R*Math.Sin(2*Math.PI*u2);
                //double z2 = (z1 + R) / (2*R);
                
                double z2 = (u1 + u2) / 2;  // as the mean is as well gausian distributed...
                Debug.Assert(z2 >= 0 && z2 <= 1);
                result.Add((float)(z2*(max-min))+min);
            }
            return result;
        }
    }
}
