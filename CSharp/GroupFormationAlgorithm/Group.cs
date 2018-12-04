using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GroupAL.Evaluator;

namespace GroupAL
{
    class Group
    {
        static int groupCount = 0;
        public static IEvaluator _evaluator;
        public int groupID=0;
        //saves the Statistics for this Group (Groupperformanceindex...)
        public Statistics results;
        
        /// <summary> Group members </summary>
        List<Participant> _participants = new List<Participant>();

        //Default Constructor
        public Group() {
            groupCount++;
            groupID = groupCount;
        }
       

        public List<Participant> Participants { 
            get { return _participants; }
            set { _participants = value;}
        }
        
        /// <summary>
        /// Performance index of this group calculated by an evaluator
        /// </summary>
        public float GroupPerformanceIndex { get; set; }

        /// <summary>
        /// max Count of groupmembers
        /// </summary>
        public static int GroupMembersMaxSize { get; set; }

        /// <summary>
        ///hours of time after which the group is gone be new formated 
        /// </summary>
        public static int TimeBeforeRefreshGroup { get; set; }


        //clears this group -> remooves alle entries and sets the GPI to 0
        //public void Clear(){
        //    this.Participants.RemoveAll(e => e.ID > 0);
        //    this.GroupPerformanceIndex = 0;
        //}

        /// <summary>
        /// Removes an Participant from this Group and calculates the new GroupPerformanceIndex
        /// </summary>
        /// <param name="p"></param>
        /// <returns>true if was successfull, otherwise false</returns>
        public bool Remove(Participant p) 
        {
            bool participantRemoved;
            if (Participants.Count == 0) return false;
            participantRemoved=Participants.Remove(p);
            if (participantRemoved) CalculateGroupPerformanceIndex();
            return participantRemoved;
        }

        /// <summary>
        /// Adds an Participant to this Group and calculates the new GroupPerformanceIndex
        /// </summary>
        /// <param name="p"></param>
        /// <returns>true if was successfull, otherwise false</returns>
        public bool Add(Participant p) {
            if (Participants.Count >= Group.GroupMembersMaxSize) return false;
            if (Participants.Contains(p)) { 
                return false;
            }
            Participants.Add(p);
            CalculateGroupPerformanceIndex();
            return true;
        }


        /// <summary>
        /// Calculates the GroupPerformanceIndex using the _evaluator
        /// </summary>
        public void CalculateGroupPerformanceIndex()
        {
            if (_evaluator == null) throw new Exception("Group.calculateGroupPerformanceIndex(): set Evaluator befor execute calculateGroupPerformanceIndex()");
            _evaluator.EvaluateGroupPerformanceIndex(this);            
            
        }
 
    }
}
