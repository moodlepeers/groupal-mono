using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroupAL.Matcher
{
    class OmadoGenesisMatcher :IMatcher
    {
        
        //Matrix from Paper "Forming Homogeneous, Heterogeneous and Mixed Groups of Learners"
        public List<Group> MatchToGroups(List<Participant> entries, List<Group> groups)
        {
            // destribute participant into the matrix
            OmadoMatrix om = new OmadoMatrix(entries,5);
            // create q empty groups of max k parricipants
            foreach (Group g in groups) {
                om.freeMatrix();
                // fill the group with Participants from Matrix
                while (g.Participants.Count < Group.GroupMembersMaxSize)
                {
                    try
                    {
                        Participant p = om.NextParticipant();
                        //if there is no further participant
                        if (p == null)
                            return groups;
                        g.Add(p);
                    }
                    catch (Exception e) {
                        Console.WriteLine(e);
                    }
                }
            }

            return groups;
        }
    }
}
