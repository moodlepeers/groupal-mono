using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroupAL.Matcher
{
    class FarawaySoCloseMatcher : IMatcher
    {
        //        //######################################################################################################
        //        //
        //        // the algorithmical part of the Faraway so close algorithm
        //        //
        //        //######################################################################################################


        float pairTreshold = 0.7f;
        float groupTreshold = 0.7f;
        public List<Group> MatchToGroups(List<Participant> notYetMatchedParticipants, List<Group> groups)
        {
            Group g;
            groups.Clear();
            //G set of completed groups, initially empty (groups).
            //S set of not grouped students, initially containing all the students
            #region Step1) At the end of this step, G will contain M groups, some of them incomplete, while S will contain the students without group.
            List<Participant> ListOfPivotParticipants = new List<Participant>();
            try
            {

                MakeAsManyGroupsAsPossible(ref notYetMatchedParticipants, ref groups, ref ListOfPivotParticipants);
            }
            catch (Exception e) { }
            #endregion

            #region Step2) the system tries to form new groups using as pivots students who have not been used yet.
            try
            {
                //phase 1) 
                MakeAsManyGroupsAsPossible(ref notYetMatchedParticipants, ref groups, ref ListOfPivotParticipants);
            }
            catch (Exception e) { }
            try
            {
                foreach (Group notFinishedGroup in groups.Where(x => x.Participants.Count < Group.GroupMembersMaxSize).ToList())
                {
                    for (int i = 0; i < notYetMatchedParticipants.Count; i++)
                    {
                        Participant p = notYetMatchedParticipants[0];
                        if (AddToGroupIfFits(p, notFinishedGroup))
                        {
                            notYetMatchedParticipants.Remove(p);
                        }
                    }
                }
                //phase 2)
                //phase 3)
            }
            catch (Exception e) { }
            #endregion

            #region Step3) At the end, G has a number of complete groups and S has the students without a group.
            try
            {
                foreach (Group notFinishedGroup in groups.Where(x => x.Participants.Count < Group.GroupMembersMaxSize).ToList())
                {
                    List<Participant> copy = new List<Participant>();
                    copy.AddRange(notYetMatchedParticipants);
                    foreach (Participant p in copy)
                    {
                        if (AddToGroupIfFits(p, notFinishedGroup))
                        {
                            notYetMatchedParticipants.Remove(p);
                        }
                    }
                }
            }
            catch (Exception e) { }
            #endregion

            #region Step4) The fourth step sequentially assigns students to incomplete and new groups without checking the join-constraints, until S contains no more students.
            try
            {
                //sort all groups over the number of participants
                groups.Sort((Group g1, Group g2) => g1.Participants.Count.CompareTo(g2.Participants.Count));
                //try to assign remain participants randomly to incomplete groups.
                foreach (Group notFinishedGroup in groups)
                {
                    if (notYetMatchedParticipants.Count == 0) return groups;
                    while (notFinishedGroup.Participants.Count < Group.GroupMembersMaxSize)
                    {
                        if (notYetMatchedParticipants.Count == 0) return groups;
                        notFinishedGroup.Add(notYetMatchedParticipants.ElementAt(0));
                        notYetMatchedParticipants.RemoveAt(0);
                    }
                }
                g = new Group();
                //create new groups and assign participants randomly
                while (notYetMatchedParticipants.Count > 0)
                {
                    Participant p = notYetMatchedParticipants.ElementAt(0);
                    notYetMatchedParticipants.Remove(p);

                    if (g.Participants.Count >= Group.GroupMembersMaxSize)
                    {
                        groups.Add(g);
                        g = new Group();
                    }
                    g.Add(p);

                }
                if (g.Participants.Count != 0)
                    groups.Add(g);

            }
            catch (Exception e) { }
            #endregion

            return groups;
        }

        private bool AddToGroupIfFits(Participant newParticipant, Group notFinishedGroup)
        {
            Group backup = new Group();
            backup.Participants.AddRange(notFinishedGroup.Participants);
            foreach (Participant GroupParticipant in backup.Participants)
                if (EuklidianDistance(GroupParticipant.Criteria.ElementAt(0).Value, newParticipant.Criteria.ElementAt(0).Value) > pairTreshold)
                {
                    notFinishedGroup.Add(newParticipant);
                    if (GroupAvergeDistance(notFinishedGroup) > groupTreshold)
                    {
                        return true;
                    }
                    else
                    {
                        notFinishedGroup.Remove(newParticipant);
                    }
                }

            return false;
        }

        private void MakeAsManyGroupsAsPossible(ref List<Participant> notYetMatchedParticipants, ref List<Group> groups, ref List<Participant> ListOfPivotParticipants)
        {
            for (int i = 0; i < notYetMatchedParticipants.Count; i++)
            {
                if (notYetMatchedParticipants.Count == 0) break;
                Participant pivotParticipant = notYetMatchedParticipants.ElementAt(0);
                //if p was not yet a pivot element then try to make a Group with p as pivot
                if (ListOfPivotParticipants.Contains(pivotParticipant)) continue;

                ListOfPivotParticipants.Add(pivotParticipant);

                Group g = MakeAGroup(ref notYetMatchedParticipants, pivotParticipant);
                if (g != null)
                {
                    groups.Add(g);
                }
            }
        }
        /// <summary>
        /// create a group
        /// set the pivot element
        /// set other group members  
        /// </summary>
        /// <param name="notYetMatchedParticipants"></param>
        /// <param name="pivotParticipant"></param>
        /// <returns>if no other groupmembers fullfills the criteria then return null else the group</returns>
        private Group MakeAGroup(ref List<Participant> notYetMatchedParticipants, Participant pivotParticipant)
        {
            Group g = new Group();
            g.Add(pivotParticipant);
            foreach (Participant p in notYetMatchedParticipants)
            {
                if (pivotParticipant.Equals(p)) continue;
                if (g.Participants.Count >= Group.GroupMembersMaxSize) break;
                //we are assuming that there is only one working crtierion within the FarawaySoCloseImplementation
                if (EuklidianDistance(p.Criteria.ElementAt(0).Value, pivotParticipant.Criteria.ElementAt(0).Value) > pairTreshold)
                {
                    g.Add(p);
                    if (!(GroupAvergeDistance(g) > groupTreshold))
                    {
                        g.Remove(p);
                    }
                }
            }

            //remove the assigned participants from participants list
            if (g.Participants.Count > 1)
            {
                foreach (Participant p in g.Participants)
                {
                    notYetMatchedParticipants.Remove(p);
                }
            }
            else
            {
                return null;
            }
            return g;
        }

        //calculates the euklidiean distance
        float EuklidianDistance(float[] p1, float[] p2)
        {
            if (p1.Length != p2.Length) return 0;

            float result = 0;
            for (int i = 0; i < p1.Length; i++)
            {
                result += (float)Math.Pow((p1[i] - p2[i]), 2);
            }
            return (float)Math.Sqrt(result);
        }

        //arithmetical mean computed over all student distances in a group
        float GroupAvergeDistance(Group g)
        {
            if (g.Participants.Count < 2) return float.MaxValue;
            float result = InternEuclidianDistance(g);
            return result / ((g.Participants.Count * (g.Participants.Count - 1)) / 2);
        }

        private float InternEuclidianDistance(Group g)
        {
            List<Criterion> criteria = g.Participants[0].Criteria;

            float result = 0;
            foreach (Criterion c in criteria)
            {
                for (int i = 0; i < g.Participants.Count - 1; i++)
                {
                    for (int j = 1; j < g.Participants.Count; j++)
                    {
                        result += EuklidianDistance(
                         g.Participants[i].Criteria.First(c_tmp => c_tmp.Name == c.Name).Value,
                         g.Participants[j].Criteria.First(c_tmp => c_tmp.Name == c.Name).Value);
                    }
                }
            }
            return result;
        }
    }
}

