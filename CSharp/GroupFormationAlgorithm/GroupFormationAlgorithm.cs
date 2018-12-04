using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroupAL
{
    class GroupFormationAlgorithm
    {
        List<Participant> participants=new List<Participant>();
        //non matched participants
        public List<Participant> NMP { get; set; }
        public Cohort Cohort {get; set;}
        
        public IEvaluator Evaluator {get; set;}
        public IMatcher Matcher {get; set;}
        public IOptimizer Optimizer {get; set;}
        

        //count of entries(participants) in the system
        int ParticipantsCount=0;
        // group size(how many people has a Group at most)
        int GroupSize=0;
        //count of groups
        int X=0;


        public GroupFormationAlgorithm(List<Participant> _participants,IMatcher matcher, IEvaluator evaluator, IOptimizer optimizer,int groupSize)
        {
            _participants.ForEach(p => participants.Add(p.Clone()));
            this.Evaluator = evaluator;
            this.Matcher = matcher;
            this.Optimizer = optimizer;

            this.GroupSize=groupSize;
            
            Init();
        }

        private void Init()
        {
            ParticipantsCount = participants.Count;
            Group.GroupMembersMaxSize = GroupSize;
            
            Group._evaluator = Evaluator;
            Cohort._evaluator = Evaluator;

            //set cohort: generate empty groups in cohort to fill with participants
            Cohort = new Cohort((int)Math.Ceiling((double)ParticipantsCount / GroupSize));
            
            // set the list of not yet matched participants by cloning the list (to have indipendent indices)
            NMP = participants.ConvertAll<Participant>(x =>x);
        }

        public bool AddNewParticipant(Participant e){
            if (participants == null || participants.Contains(e)) return false;
            
            //increase count of Participants
            ParticipantsCount++;
            int tmpX = (int)Math.Ceiling((double)ParticipantsCount / GroupSize);
            //if count of groups changed then add new empty Group
            // TODO improve by ensuring that the number of groups is correct afterwards
            if (tmpX != X) 
            {
                X = tmpX;
                Cohort.AddEmptyGroup();
            }

            //add the new participant to entries
            participants.Add(e);
            //add the new participant to the set of not yet matched entries
            NMP.Add(e);
            return true;
         }

        public bool RemoveParticipant(Participant e) {
            if (participants == null || !participants.Contains(e)) return false;

            //increase count of Participants
            ParticipantsCount--;
            int tmpX = (int)Math.Ceiling((double)ParticipantsCount / GroupSize);            
            Cohort.Remove(e);

            //remove a participant to all Entries
            participants.Remove(e);
            //remove a participant to the set of not yet Matched
            if (NMP.Contains(e)) NMP.Remove(e);
            return true;
        }

        void MatchToGroups(List<Participant> NMP, List<Group> groups) 
        {
            //match the new added participant to one of the groups
            Matcher.MatchToGroups(NMP, groups);
        }

        public void OptimizeCohort() {
            Optimizer.OptimizeCohort(Cohort);
        }

        /// <summary>
        /// Uses the global set matcher to assign evry not yet matched participant to a group
        /// </summary>
        /// <returns>returns a Cohort, this includes filled groups, the cohortperformanceindex an other statistics</returns>
        public Cohort DoOneFormation() {
            MatchToGroups(NMP, Cohort.groups);
             Cohort.countOfGroups = Cohort.groups.Count;
             Cohort.whichMatcherUsed = Matcher.GetType().Name;
             Cohort.CalculateCohortPerformanceIndex();
             return Cohort;
        }
            
    }

}
