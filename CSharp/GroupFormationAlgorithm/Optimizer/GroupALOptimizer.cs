using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroupAL.Optimizer
{
     class GroupALOptimizer : IOptimizer
    {

        IMatcher matcher;

        public GroupALOptimizer(IMatcher matcher) {
            this.matcher = matcher;
        }

       public void OptimizeCohort(Cohort cohort)
        {
            List<Group> groups = cohort.groups;
            //throw new NotImplementedException("optimize only if it realy optimizes not else");
            //sort cohort by Group Performance Index
            try
            {
                groups.Sort(delegate(Group x, Group y)
                {
                    double result= x.GroupPerformanceIndex - y.GroupPerformanceIndex;
                    return  result > 0.000000001? 1 : result==0? 0 : -1 ;
                });
            }
            catch (Exception e) {
                throw new Exception("DefaultOtptimizer.optimizeCohort: something seems wrong with sorting groups by their performance index value" + e.StackTrace);
            }
            
            //for each pair of good and bad group try to average them
            for (int i = 0; i < groups.Count / 2; i++)
            {
                Group goodGroup = groups.ElementAt(i);
                Group badGroup = groups.ElementAt((groups.Count - 1) - i);
                AverageTwoGroups(ref goodGroup,ref badGroup);
            }
            cohort.CalculateCohortPerformanceIndex();
        }
       
       public void AverageTwoGroups(ref Group _goodGroup, ref Group _badGroup)
       {
           if (Math.Abs(_goodGroup.GroupPerformanceIndex - _badGroup.GroupPerformanceIndex) < 0.02) return;
           // dissolve the groups and rondomize the position of participant
           List<Participant> localNGT = new List<Participant>();
           _goodGroup.Participants.ForEach(g => localNGT.Add(g));
           _badGroup.Participants.ForEach(g => localNGT.Add(g));

           //randomize position of entries
           Shuffle<Participant>(localNGT);
           //localNGT.Sort(delegate(Participant p1, Participant p2)
           //{
           //    float sum1 = 0;
           //    p1.Criteria.ForEach(x => sum1 += x.Value.Sum());
           //    float sum2 = 0;
           //    p2.Criteria.ForEach(x => sum2 += x.Value.Sum());
           //    return sum1 > sum2 ? 1 : (sum1 == sum2 ? 0 : -1);
           //});

           // match the groups new 
           Group g1 = new Group();
           Group g2 = new Group();
           List<Group> newGroups = new List<Group>() { g1, g2};
           matcher.MatchToGroups(localNGT, newGroups);
           //first condition for a better PerformanceIndex: the AVGGroupPerformanceIndex raises
           float oldAvg = (_goodGroup.GroupPerformanceIndex + _badGroup.GroupPerformanceIndex)/2;
           float newAvg = (g1.GroupPerformanceIndex + g2.GroupPerformanceIndex)/2;
           bool firstCondition = newAvg > oldAvg;
           //second condition for a better PerformanceIndex: the stdDiaviation gets smaller so the AVGGroupPerformanceIndex becomes more equal
           //on the other hand the average gets just 
           float oldStd = Math.Abs(_goodGroup.GroupPerformanceIndex - _badGroup.GroupPerformanceIndex);
           float newStd = Math.Abs(g1.GroupPerformanceIndex - g2.GroupPerformanceIndex);
           bool secondCondition = newStd < oldStd && Math.Abs(newAvg-oldAvg) < 0.01;
           if (firstCondition) {
               _goodGroup.Participants = g1.Participants;
               _goodGroup.GroupPerformanceIndex = g1.GroupPerformanceIndex;
               _badGroup.Participants = g2.Participants;
               _badGroup.GroupPerformanceIndex = g2.GroupPerformanceIndex;
           }
       }
         //old version
        //public void averageTwoGroups(ref Group _goodGroup,ref Group _badGroup)
        //{
        //    // dissolve the groups and rondomize the position of participant
        //    List<participant> localNGT = new List<participant>();
        //    _goodGroup.entries.ForEach(g => localNGT.Add(g));
        //    _goodGroup.entries.RemoveRange(0, _goodGroup.entries.Count);
        //    _goodGroup.groupPerformanceIndex = 0;

        //    _badGroup.entries.ForEach(g => localNGT.Add(g));
        //    _badGroup.entries.RemoveRange(0, _badGroup.entries.Count);
        //    _badGroup.groupPerformanceIndex = 0;

        //    //randomize position of entries
        //    Shuffle<participant>(localNGT);

        //    // match the groups new 
        //    List<Group> newGroups = new List<Group>();
        //    newGroups.Add(_goodGroup);
        //    newGroups.Add(_badGroup);
        //    matcher.matchToGroups(localNGT,newGroups);
        //}

        //Schuffle a List of Entries
        public void Shuffle<T>(IList<T> list)
        {
            var randomNumber = new Random(DateTime.Now.Millisecond);
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = randomNumber.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
