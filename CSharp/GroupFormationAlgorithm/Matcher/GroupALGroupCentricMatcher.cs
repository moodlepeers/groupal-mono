using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroupAL
{
    class GroupALGroupCentricMatcher:IMatcher
    {
        public List<Group> MatchToGroups(List<Participant> notYetMatched, List<Group> groups) {
            float gpi = 0;
            float gpi_tmp = 0;
            float delta = 0;
            float delta_old = float.MinValue;
            Participant bestParticipant = notYetMatched.ElementAt(0);
            Participant p;

            // search the best participant for the group
            foreach (Group g in groups)
            {
                for (int j = 0; j < Group.GroupMembersMaxSize; j++)
                {
                    //if the group is full the go on with the next Group
                    if (g.Participants.Count >= Group.GroupMembersMaxSize) break;
                    if (notYetMatched.Count == 0) break;
                    for (int i = 0; i < notYetMatched.Count; i++)
                    {
                        p = notYetMatched.ElementAt(i);

                        if (g.Participants.Count == 0) 
                        {
                            bestParticipant = notYetMatched.ElementAt(i);
                            break; 
                        }
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
                        // transform to percentages
                        delta = gpi == 0 ? gpi_tmp : delta / gpi;

                        //if for this group the performance increas the most than safe the new candidate
                        if (delta > delta_old)
                        {
                            bestParticipant = notYetMatched.ElementAt(i);
                            delta_old = delta;
                        }
                    }

                    
                    //now bestparticipant is the participant with the best performance increas for the group
                    delta_old = float.MinValue;
                    g.Add(bestParticipant);
                    notYetMatched.Remove(bestParticipant);
                }
            }
            return groups;
        }
    }
}
