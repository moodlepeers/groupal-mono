using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroupAL.Generator
{
    class NoExtremaRule:IRule
    {
        // limits the valid value arrea by 10% e.g. befor min 0 max 1 after min 0.1 max 0.9 
        float limitByPercent = 0.4f;
        
        /// <summary>
        /// adjust the criteria of the given participant by limeting the range of valid Values using the private variable limitByPercent
        /// </summary>
        /// <param name="participant"></param>
        /// <returns></returns>
        public Participant AdjustParticipant(Participant participant)
        {
            foreach (Criterion c in participant.Criteria)
            {
                float newMinValue = c.MinValue + (c.MaxValue - c.MinValue) * (limitByPercent);
                float newMaxValue = c.MaxValue - (c.MaxValue - c.MinValue) * (limitByPercent);

                float range = newMaxValue - newMinValue;

                //itterate over each cell in the variable participant.criterion.Value and adjust the values
                for (int i = 0; i < c.Value.Length; i++)
                {
                    if (c.Value[i] > c.MaxValue) c.Value[i] = c.MaxValue;
                    if (c.Value[i] < c.MinValue) c.Value[i] = c.MinValue;

                    /// Maping to the range given by newMaxValue - newMinValue
                    c.Value[i] = newMinValue + range * c.Value[i];
                }
            }

            return participant;
        }
    }
}
