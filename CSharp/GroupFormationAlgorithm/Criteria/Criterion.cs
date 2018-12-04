using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GroupAL.Criteria;
using System.Diagnostics;
using System.Security.Cryptography;

namespace GroupAL
{
    public abstract class Criterion
    {
        public Criterion() { }

        /// <summary>
        /// The name of an Criterion e.g. "learner style after Silvermann & Felder"
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Value or values of an criterion
        /// </summary>
        public float[] Value { get; set; }

        /// <summary>
        /// Max valid value of an criterion
        /// </summary>
        public float MaxValue { get; set; }

        /// <summary>
        /// Min valid value of an criterion
        /// </summary>
        public float MinValue { get; set; }
        
        /// <summary>
        /// flag to mark Criterion as homogeneous or as not homogeneous (heterogeneous)
        /// </summary>
        public bool IsHomogeneous { get; set; }


        /// <summary>
        /// weight for the Criterion. The value is saved in the CLass CriterionWeight
        /// </summary>
        public float Weight {
            get { return CriterionWeight.GetWeight(this.Name); }    
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Criterion FillValuesWithRandomValues()
        {
            byte[] randomNumber = new byte[1];
            RNGCryptoServiceProvider Gen = new RNGCryptoServiceProvider();
            Gen.GetBytes(randomNumber);
            Random rand = new Random((int)randomNumber[0]);

            for (int i = 0; i < Value.Length; i++)
            {
                Value[i] = (float)rand.NextDouble(); // TODO maybe: ((float)rand.NextDouble() * (this.MaxValue - this.MinValue)) + this.MinValue;
                Debug.Assert(Value[i] >= this.MinValue && Value[i] <= this.MaxValue);
            }
            return this;
        }
    }
}
