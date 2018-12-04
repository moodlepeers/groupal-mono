using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace GroupAL.Evaluator
{
    class GroupALEvaluator : IEvaluator
    {
        IDistance distanceFunktion;

        public GroupALEvaluator() {
            distanceFunktion = new ManhattanDistance();
        }
        

        /// <summary>
        ///homogeneous criteria->subtract values-> the smaller the better
        ///heterogeneous criteria->subtract values->the bigger the better
        ///the difference: heterogeneous value - homogeneous is the return value the biger the better
        /// normalize by the best possible GroupPerformanceIndex
        ///            (the difference of a perfect homogeneous pair of values is 0)
        ///            (the difference of a perfect heterogeneous pair of value is 1)
        ///            (the besst possible (non realistic) GroupPerformanceIndex, with resct to these rules, is the sum (of 1 to nummber of groupmembers count) * (the count of heterogen criterion) * (the sum of the count of each criterions values)
        ///            e.g. for a Group of 3 persons with 2 heterogen Criterion each with 4 values the best posible GroupPerformanceIndex would be (3+2+1)*2*4
        /// </summary>
        /// <param name="g"></param>
        public float EvaluateGroupPerformanceIndex(Group group)
        {
            ////All Normalized paar performance indices of a Group
            List<float> NPIs = new List<float>();

            //One Normalized paar performance index of a minimal Group (two entries) 
            float npi = 0;
            float gpi = 0;

            //calculate npi for every pair of entries in the  group g.
            for (int i = 0; i < group.Participants.Count - 1; i++)
            {
                for (int j = i + 1; j < group.Participants.Count; j++)
                {
                    //calulate normlizedPaarperformance index
                    npi = CalcNormlizedPairPerformance(group.Participants.ElementAt(i), group.Participants.ElementAt(j));
                    NPIs.Add(npi);
                }
            }
            group.results = GetPerformanceIndex(NPIs);
            gpi = group.results.performanceIndex;
            group.GroupPerformanceIndex = gpi;
            return gpi;
        }

        public float EvaluateCohortPerformanceIndex(Cohort cohort)
        {
            cohort.groups.ForEach(g => g.CalculateGroupPerformanceIndex());
            List<float> GPIs = cohort.groups.Select(x => x.GroupPerformanceIndex).ToList();
            Statistics results = GetPerformanceIndex(GPIs);
            cohort.results = results;
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
            // TODO add if clasue for case count = 1;

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

        /// <summary>
        ///homogeneous criteria->subtract values-> the smaller the better
        ///heterogeneous criteria->subtract values->the biger the better
        ///the difference: heterogeneous value - homogeneous is the return value the biger the better
        /// normalize by the best possible GroupPerformanceIndex
        ///            (the difference of a perfect homogeneous pair of values is 0)
        ///            (the difference of a perfect heterogeneous pair of value is 1)
        ///            (the besst possible (non realistic) GroupPerformanceIndex, with resct to these rules, is the sum (of 1 to nummber of groupmembers count) * (the count of heterogen criterion) * (the sum of the count of each criterions values)
        ///            e.g. for a Group of 3 persons with 2 heterogen Criterion each with 4 values the best posible GroupPerformanceIndex would be (3+2+1)*2*4
        /// </summary>
        /// <param name="g"></param>
        private float CalcNormlizedPairPerformance(Participant participant, Participant participant_2)
        {
            //the summed distances of all hommogeneous values
            float homVal = 0;
            //the summed distances of all heterogeneous values
            float hetVal = 0;
            //not normalized pairperformance index (hetVal - homVal) 
            float pairPerformanceIndex = 0;
            Criterion c_2;
            //distance between two Criteria
            float d = 0;
            // weighted distance
            float wd = 0;
            //normlized pair performance index
            float npi = 0;

            if (participant.Criteria.Count != participant_2.Criteria.Count) throw new Exception("calcPairPerformance: the entries have different count of criteria!!!");

            foreach (Criterion c in participant.Criteria)
            {
                //get the same kriterion of the other participant
                c_2 = participant_2.Criteria.First(x => x.Name == c.Name);
                //calculate Manhatan distanze for both Criteria
                //and normalize the distanze over the maximal amount of dimensions so evry criterion gets a value between 0 and 1 
                //(otherwise the criterion will be unthought weighted )
                //d = ManhattanDistance(c, c_2);
                d = distanceFunktion.normalizedDistanze(c, c_2);
                wd = d * c.Weight;
                if (c.IsHomogeneous)
                    homVal += wd;
                else
                    hetVal += wd;
            }
            pairPerformanceIndex = hetVal - homVal;
            float MaxDist = 0;
            //worst case Heterogen Kriteria is 0 and hom is 1 than the value for pairPerformanceIndex < 0. therfore the worst possible value for
            // hom Kriteria is added to the pairPerformanceIndex: and the target set lies between 0 and 1
            float homMaxDist = 0;
            //beacuse i normalize each distance of two criterions over their highest possible value 
            //here i neede to normalize pairPerformanceIndex by the count of the Criterions multiplied by its weight
            foreach (Criterion c in participant.Criteria)
            {
                if (c.IsHomogeneous) homMaxDist += 1 * c.Weight;
                MaxDist += 1 * c.Weight;
                //if (c.IsHomogeneous) homMaxDist += c.Value.Length * c.Weight;
                //MaxDist += c.Value.Length * c.Weight;
            }
            //Debug.Assert(MaxDist == 1);  only work sif weights in sum = 1!

            npi = (pairPerformanceIndex + homMaxDist) / MaxDist;

            return npi;
        }
    }
}
