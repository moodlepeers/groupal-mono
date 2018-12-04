using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroupAL.Evaluator
{

    /// <summary>
    ///Based on the heuristic described in 
    ///Automating the Process of Assigning Students to Cooperative-Learning Teams
    ///by
    ///Ryan Cavanaugh, Matt Ellis, Richard Layton, Mark Ardis 
    ///From 
    ///Rose-Hulman Institute of Technology
    /// </summary>
    class TeamMakerEvaluator : IEvaluator
    {
        //The score for each individual team
        //is the sum of the weight of each question multiplied by the score for that question
        public float EvaluateGroupPerformanceIndex(Group g)
        {
            if (g.Participants.Count == 0) return 0;
            List<Criterion> ListOfCriteria = (List<Criterion>)g.Participants.ElementAt(0).Criteria;
            //A weight of zero indicates the question should be disregarded. 
            //A negative weight indicates that students with different responses 
            float heterogenWeight = -1;  // the multiplyer that normaly is on a scale from -1 to 1 in Teammmaker is made here to extremes!
            // as the calculations always return 1 if a maxmimun homogeneiety is in the answers, we set the weight for het = -1 as we dont want hom here.
            // for hom-criteria it is set to 1 as we do want hom.


            //should be placed in the same team, where a positive weight indicates that students with similar
            //responses should be placed in the same team.
            float homogenWeight = 1;
            float GPI = 0;
            foreach (Criterion c in ListOfCriteria)
            {
                if (c.IsHomogeneous)
                  GPI +=  CalculateChooseAnyOfHeuristic(g, c) * homogenWeight;  // we assume that every(!) homogeneous question is a any-of
                else
                  GPI += CalculateMultipleChoiceHeuristic(g, c) * heterogenWeight; // we assume that every(!) heterogeneous question is a multiplechoice!
            }
            g.GroupPerformanceIndex = GPI;
            return GPI;
        }

        //Cohort is going to be evaluated by my Evaluator for comparing reasons
        public float EvaluateCohortPerformanceIndex(Cohort cohort)
        {
            cohort.groups.ForEach(g => g.CalculateGroupPerformanceIndex());
            List<float> GPIs = cohort.groups.Select(x => x.GroupPerformanceIndex).ToList();            
            Statistics results = GetPerformanceIndex(GPIs);
            
            cohort.results = results;
            // Teammaker gives as performance the lowest group value:
            cohort.results.performanceIndex = GPIs.Min(X => X);
            return results.performanceIndex;
        }

        public static Statistics GetPerformanceIndex(List<float> listOfPerformanceIndices)
        {
            if (listOfPerformanceIndices.Count < 1) return new Statistics();
            //calc avergae of NPIs
            float avg = listOfPerformanceIndices.Average();

            //calc standad deaviation
            double sumOfQuadErrors = listOfPerformanceIndices.Sum(d => Math.Pow(d - avg, 2));

            //standard deaviation of all npi values (NPIs) in one Groups 
            float stdDev = (float)Math.Sqrt((sumOfQuadErrors) / (listOfPerformanceIndices.Count - 1));

            //normalize stdNPIs
            float nStd = 1 / (1 + stdDev);
            float performanceIndex = listOfPerformanceIndices.Count < 2 ? avg : avg * nStd;
            Statistics s = new Statistics();

            s.n = listOfPerformanceIndices.Count;
            s.avg = avg;
            s.stDev = stdDev;
            s.normStDev = nStd;
            s.performanceIndex = performanceIndex;

            return s;
        }

        //Heterogenity : MultipleChois (only one possible answer)
        //Itterate over all Participants for each position in Criterion.Value
        float CalculateMultipleChoiceHeuristic(Group g, Criterion c)
        {
            //List<Criterion> ListOfHeterogenCriteria = (List<Criterion>)g.Participants.ElementAt(0).Criteria.Where(x => !x.IsHomogeneous);
            int answerTMP = 0;
            float result = 0;

            for (int i = 0; i < c.Value.Length; i++)
            {
                answerTMP = 0;
                foreach (Participant p in g.Participants)
                {
                    // set 1 if any of the participants has selected this answer (OR of all participants)
                    if (answerTMP == 0)
                    {
                        //get the criterion of each Participant
                        answerTMP = p.Criteria.First(x => x.Name == c.Name).Value[i] == 1 ? 1 : 0;
                    }
                    else 
                    {
                        break;
                    }
                }
                result += answerTMP;
            }

            result /= g.Participants.Count();

            return 1- result;  // return 1 if it is homogeneous (all selected the same makes a lower score. This was first a BUG in the paper at it returned result directly!)
        }

        //Homogenity   : Choose Any of Evaluation (checkbox like several answers to a question )
        float CalculateChooseAnyOfHeuristic(Group g, Criterion c)
        {
            //List<Criterion> ListOfHeterogenCriteria = (List<Criterion>)g.Participants.ElementAt(0).Criteria.Where(x => x.IsHomogeneous);
            int R_count = c.Value.Length;
            int answerTMP = 0;
            float result = 0;


            for (int i = 0; i < c.Value.Length; i++)
            {
                answerTMP = 0;
                //calculation of G(r,t) = sumup one options for every participant
                foreach (Participant p in g.Participants)
                {
                    //sumup the value for this dimension over all partiticpants
                    answerTMP += p.Criteria.First(x => x.Name == c.Name).Value[i] == 1 ? 1 : 0;

                }
                //implementation of d(r)= if x<1 than 0 else x²
                answerTMP = answerTMP <= 1 ? 0 : answerTMP * answerTMP;               
                result += answerTMP;
                
            }

            // NO!!  result = result * result;
            result /= (g.Participants.Count() * R_count);  // this is sum(i=1 to R) / |t|*|R|

            return (1-result > 0 ? 1-result : 0);  // return max ( 1-sumR;0)
        }
    }
}
