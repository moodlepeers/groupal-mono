using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroupAL.Matcher
{
    class TeamMakerMatcher : IMatcher
    {
        /// <summary>
        ///Based on the heuristic described in 
        ///Automating the Process of Assigning Students to Cooperative-Learning Teams
        ///by
        ///Ryan Cavanaugh, Matt Ellis, Richard Layton, Mark Ardis 
        ///From 
        ///Rose-Hulman Institute of Technology
        /// </summary>
        public List<Group> MatchToGroups(List<Participant> NotYetMatched, List<Group> groups)
        {
            #region 2) Place students randomly into a non-full team until all are placed
            foreach (Group g in groups)
            {
                while (NotYetMatched.Count > 0)
                {
                    //get and remoove(pop) the first ellement from NotYetMatched entries
                    Participant participant = NotYetMatched.ElementAt(0);
                    NotYetMatched.Remove(participant);
                    //add the participant to the group
                    g.Participants.Add(participant);
                    //if the group is full and there are mor Entries to group than create a new group
                    if (g.Participants.Count >= Group.GroupMembersMaxSize && NotYetMatched.Count > 0)
                    {
                        break;
                    }
                }
            }
            #endregion

            #region Evaluate the heuristics and compute an overall score for the section
            for (int i = 0; i < 50; i++)
            {
                groups.ForEach(group => group.CalculateGroupPerformanceIndex());
                //compute an over all score
                groups.Sort(delegate(Group x, Group y) { return x.GroupPerformanceIndex - y.GroupPerformanceIndex >= 0 ? 1 : -1; });
                //swap students between two teams
                //compute an over all score. undo the swap if score became slower
                Group worstGroup = groups.First();
                Group bestGroup = groups.Last();
                correctSwap(ref worstGroup, ref bestGroup,0);

                //If the new set of teams is better than the old set, replace the old set with the new set
                //Repeat steps 1 through 4 a fixed number of times (usually 50)
            }
            #endregion

            return groups;
        }

        private void correctSwap(ref Group _badGroup, ref Group _goodGroup, int countNotSuccessfullSwaps)
        {
            if (countNotSuccessfullSwaps > 5) return;

            float GPI_Overall = _badGroup.GroupPerformanceIndex;
            
            Group g1 = new Group();
            Group g2 = new Group();
            _badGroup.Participants.ForEach(p => g1.Add(p.Clone()));
            _goodGroup.Participants.ForEach(p => g2.Add(p.Clone()));

            swapTwoParticipants(ref g1, ref g2);
            //new groups are better than the worst group before
            bool firstCondition = g1.GroupPerformanceIndex > GPI_Overall && g2.GroupPerformanceIndex > GPI_Overall;
            //bool secondCondition = newStd < oldStd && Math.Abs(newAvg - oldAvg) < 0.01;
            if (firstCondition)
            {
                _goodGroup.Participants = g1.Participants;
                _goodGroup.GroupPerformanceIndex = g1.GroupPerformanceIndex;
                _badGroup.Participants = g2.Participants;
                _badGroup.GroupPerformanceIndex = g2.GroupPerformanceIndex;
            }
            else {
                countNotSuccessfullSwaps++;
            }

            correctSwap(ref _badGroup,ref _goodGroup, countNotSuccessfullSwaps);
        }

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

        Random rand = new Random(DateTime.Now.Millisecond);
        void swapTwoParticipants(ref Group g1, ref Group g2) {
            int pos1 = rand.Next(0, g1.Participants.Count);
            int pos2 = rand.Next(0, g2.Participants.Count);

            Participant p1 = g1.Participants[pos1];
            Participant p2 = g2.Participants[pos2];
            
            g1.Remove(p1);
            g1.Add(p2);

            g2.Remove(p2);
            g2.Add(p1);
        }
    }
}
