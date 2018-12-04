using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroupAL.Generator
{
    class RelationBartleBigFiveRule : IRule 
    {

        public Participant AdjustParticipant( Participant participant)
        {
            Criterion bartleCriterion = participant.Criteria.First(c => c.Name == "Bartle");
            Criterion bigFiveCriterion = participant.Criteria.First(c => c.Name == "BigFive");
                //0)if the value for sozialiser is high than make the value for the 

                //0) exstraversion and 
                //1)Agreeableness should be high ass well
                //2) and Openness to experience as well 
                //3)Neuroticism  should be low
                //4)Conscientiousness  does not matter and
                
                if (bartleCriterion.Value[0] > 0.7) 
                {
                    //make killerValue lower
                    bartleCriterion.Value[1]  *= 0.25f;

                    bigFiveCriterion.Value[0] += (1 - bigFiveCriterion.Value[0]) * 0.6f;
                    bigFiveCriterion.Value[1] += (1 - bigFiveCriterion.Value[0]) * 0.6f;
                    bigFiveCriterion.Value[2] += (1 - bigFiveCriterion.Value[0]) * 0.6f;
                    bigFiveCriterion.Value[3] *= 0.4f;

                    //take kare that there is no Vlueas over 1 and under 0
                    for (int i = 0; i < bigFiveCriterion.Value.Length; i++)
                    {
                        if (bigFiveCriterion.Value[i] < 0)
                            bigFiveCriterion.Value[i] = 0;
                        if (bigFiveCriterion.Value[i] > 1)
                            bigFiveCriterion.Value[i] = 1;
                    }
                }
                
            return participant;
        }
    }
}
