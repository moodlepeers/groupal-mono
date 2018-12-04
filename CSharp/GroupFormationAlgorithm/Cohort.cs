using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GroupAL.Evaluator;

namespace GroupAL
{

    class Cohort
    {
       
        public static IEvaluator _evaluator;
        public List<Group> groups;
        public Statistics results;
        public string whichMatcherUsed = "";
        public int countOfGroups=0;

        public Cohort(int CountOfGroups)
        {
            this.groups = new List<Group>(CountOfGroups);
            //create CountOfGroups empty groups
            for (int i = 0; i < CountOfGroups; i++) AddEmptyGroup();
        }
        
        //[Obsolete]
        //public Cohort(List<Group> groups, string whichMatcherUsed)
        //{
        //    this.groups = groups;
        //    CountOfGroups = groups.Count;
        //    this.whichMatcherUsed = whichMatcherUsed;
        //    evaluateKohortPerformanceIndex();
        //}

        /// <summary>
        /// Adds a Group to this Cohort
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public bool Add(Group g){
            if (groups.Contains(g)) 
                return false;
            groups.Add(g);
            countOfGroups++;
            CalculateCohortPerformanceIndex();
            return true;
        }

        /// <summary>
        /// remove one group from this Cohort if present
        /// </summary>
        /// <param name="g"></param>
        /// <returns>true if group was found and removed</returns>
        public bool Remove(Group g) {           
            Boolean result =  groups.Remove(g);
            if (result) {
                countOfGroups--;
            }
            return result;
        }

        /// <summary>
        /// removes an participant from an group of this cohort
        /// </summary>
        /// <param name="e"></param>
        internal void Remove(Participant e)
        {
            foreach (Group g in groups)
            {
                if (g.Participants.Contains(e))
                {
                    g.Remove(e);
                }
            }
            RemoveEmptyGroups();
            CalculateCohortPerformanceIndex();
        }

        //removes empty groups if
        void RemoveEmptyGroups(){
            for (int i = 0; i < groups.Count; i++)
            {
                if (groups[i].Participants.Count == 0) 
                {
                    Remove(groups[i]);
                }
            }
        }
        
        /// <summary>
        /// Adds an empty Group if mor participants are provided than fits in all groups
        /// </summary>
        public void AddEmptyGroup(){
            Add(new Group());
        }

        /// <summary>
        /// evaluates the Performance of this Cohort using the evaluator
        /// </summary>
        /// <returns></returns>
        public float CalculateCohortPerformanceIndex()
        {
            if (_evaluator == null) throw new Exception("Cohort.evaluateCohortPerformanceIndex(): set Evaluator before execute evaluateCohortPerformanceIndex()");
            return _evaluator.EvaluateCohortPerformanceIndex(this);
            #region old stuff
            //List<float> GPIs = groups.Select(x => x.groupPerformanceIndex).ToList();
            //results = MainWindow.getPerformanceIndex(GPIs);

            //n = results.n;
            //avgGroupPerformanceIndex = results.avg;
            //normStDev = results.normStDev;
            //performanceIndex = results.performanceIndex;
            //stDev = results.stDev;

            //return performanceIndex;
            #endregion
        }
    }
}
