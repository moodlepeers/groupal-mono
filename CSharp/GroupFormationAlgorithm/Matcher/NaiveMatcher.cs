using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace GroupAL.Matcher
{
    /// <summary>
    /// Brute force matcher trying to calculate all possible combinations and find the optimal solution for the given set of participants.
    /// FIXME: Code not finished yet.
    /// </summary>
    class NaiveMatcher: IMatcher
    {
        static Func<int, int> Factorial = x => x < 0 ? -1 : x == 1 || x == 0 ? 1 : x * Factorial(x - 1);

        public List<Group> MatchToGroups(List<Participant> NotYetMatched,List<Group> groups)
        {
            throw new NotImplementedException();
            // not finished yet...tried to code brute force combinatory...too much computation.

            ArrayList combination = new ArrayList(NotYetMatched);
            ArrayList combinations = new ArrayList();
            int M = Group.GroupMembersMaxSize;
            int N = NotYetMatched.Count();
            // Naive Matcher has to make (M over M/X) combinations and GPI/CPI calculations.
            // M Over NumGroups  (how many compinations are there to put M participants to the X groups)
            System.Console.Out.WriteLine("NaiveMatcher has to calc "+getRuns(M, groups.Count()));
            //starting permutation:
            //a[0...N-1] Objects  -->NotYetMatched
            //c[0..M-1] first combination --> init with c[a[N-M]...a[N-1]]
            int j = 0;
            for (int i=M; i>0; i--)
            {
                combination[j] = NotYetMatched[N - i];
                j++;
            }
            combinations.Add(combination.Clone());
                
            
            int[] permuts = new int[NotYetMatched.Count()+2];
            int x = 0,y= 0,z = 0; // twittle variables
            TwittleCombinatoryCalculator.inittwiddle(Group.GroupMembersMaxSize,NotYetMatched.Count(),ref permuts);
            while (TwittleCombinatoryCalculator.twiddle(ref x, ref y, ref z, ref permuts) == 0)
            {
                combination[z] = NotYetMatched[x];
                combinations.Add(combination.Clone());
            }
            System.Console.Out.WriteLine("Calculated combinations: " + combinations.Count);

            // now combinations contains all Lists of possible draws of X (2,3,6) participants from all participants
            // yet do recursive combine all these draws and calculate the GPIs..
            ArrayList bestGPIcombinations = new ArrayList(groups.Count());
            Stack<ArrayList> currentGroupCombi = new Stack<ArrayList>(groups.Count());
            recursiveCombine(ref combinations, ref groups, ref currentGroupCombi, ref bestGPIcombinations, 0);

            for (int i=0; i < groups.Count(); i++)
            {
                ArrayList c = (ArrayList)bestGPIcombinations[i];
                foreach (Participant p in c)
                {
                    NotYetMatched.Remove(p);
                    groups[i].Participants.Add(p);
                }
            }
            groups.ForEach(group => group.CalculateGroupPerformanceIndex());
            return groups;
        }

        private void recursiveCombine(ref ArrayList combinations, ref List<Group> groups, ref Stack<ArrayList> currentGroupCombi, ref ArrayList bestGPIcombinations, int startIndex)
        {
            for (int i = startIndex; i < combinations.Count; i++)
            {
                currentGroupCombi.Push((ArrayList)combinations[i]); // add the next possible partner to stack
                // FIXME: continue coding once here...recursiveCombine(
                throw new InvalidOperationException("Coding not yet finished ...");
            }
        }

        private long getRuns(int M, int N)
        {
            //calculate M over N
            long pM = M, pN = N;
            for (int i = 1; i < N; i++)
            {
                pM *= (M - i);
            }
            pN = Factorial(N);

            return pN > 0 ? pM / pN : 0;
        }       
    }
}
