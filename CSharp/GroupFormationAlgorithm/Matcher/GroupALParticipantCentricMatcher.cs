using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroupAL.Matcher
{
    class GroupALParticipantCentricMatcher :IMatcher
    {

        public List<Group> MatchToGroups(List<Participant> notYetMatched, List<Group> groups)
        {
            float gpi = 0;
            float gpi_tmp = 0;
            float delta = 0;
            float delta_old= float.MinValue;
            bool hasProgress = true;
            Group bestGroup = new Group();  // why this and not groups[0] ?
            Participant p;

            //set one person as pivotelement for each Group            
            foreach (Group g in groups) {
                if (g.Participants.Count == 0 && notYetMatched.Count > 0)
                {
                    g.Participants.Add(notYetMatched.ElementAt(0));
                    notYetMatched.RemoveAt(0);
                }
            }
            // search for the best group for one participant
            while (notYetMatched.Count > 0 && hasProgress == true) 
            {
                p = notYetMatched.ElementAt(0);
                hasProgress = false; // indicate that at least one member found a new group
                foreach (Group g in groups) 
                {
                    //if the group is full the go on with the next Group
                    if (g.Participants.Count >= Group.GroupMembersMaxSize) continue;
                    //get the current gpi of the group
                    gpi = g.GroupPerformanceIndex;
                    //add an paticipant(participant) to the group
                    //calculate the new gpi
                    g.Add(p);
                    gpi_tmp = g.GroupPerformanceIndex;
                    //remove the participynt from group
                    g.Remove(p);

                    //calculate the delta between gpi of the group and the gpi of the group + the one participant
                    //delta = gpi == 0 ? gpi_tmp : gpi_tmp / gpi;
                    delta = gpi_tmp - gpi;
                    // convert to percentages
                    delta = gpi == 0 ? gpi_tmp : delta / gpi;
                    
                    //if for this group the performance increas the most than safe the group as an candidate
                    if (delta > delta_old) {
                        bestGroup = g;
                        delta_old = delta;
                        hasProgress = true;
                    }
                }
                //now bestGroup is the candidate with the best performance increas. with the current participant e as participant
                if (hasProgress)
                {
                    delta_old = float.MinValue;
                    bestGroup.Add(p);
                    notYetMatched.Remove(p);
                } // no hasProgress=false as it is set in while..                
            }
            return groups;
        }
    }
}
