using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroupAL.Matcher
{

    class RandomMatcher: IMatcher
    {
        public List<Group> MatchToGroups(List<Participant> NotYetMatched,List<Group> groups)
        {
            //List<Group> groups= new List<Group>();
            ///// Initial add of a group
            //Group g=new Group();
            //groups.Add(g);
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
                        ////create new group
                        //g = new Group();
                        //groups.Add(g);
                    }
                }
            }
            groups.ForEach(group => group.CalculateGroupPerformanceIndex());
            return groups;
            #region old area
            ////simple matcher just grouping people by their position in List
            //foreach (participant e in entries) {
            //    //if the group capacity is reached save the group to the groups list and create a new group to fill with entries
            //    if (g.entries.Count >= groupSize)
            //    {
            //        g = new Group();
            //        continue;
            //    }
            //    g.entries.Add(e);
            //    entries.Remove(e);
            //}

            //return groups;
            #endregion

        }
    }
}
