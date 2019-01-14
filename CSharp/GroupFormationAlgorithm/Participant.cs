using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroupAL
{
    public class Participant
    {
        //List of all Criteria one Participant (its the virtual image of the person siting in forn of the Device)
        public List<Criterion> Criteria { get; private set; }
        //static Count of all Participants in System
        static int id=0;
        // the unique ID of an Participant
        public int ID { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteria"> will directly use this List (not copied)</param>
        public Participant(List<Criterion> criteria) {
            this.Criteria = criteria;
            id++;
            ID = id;
        }

        public Participant(int extId, List<Criterion> criteria) : this(criteria)
        {
            ID = extId;
        }

        public Participant Clone(){
            List<Criterion> emptyList = new List<Criterion>();
            Participant participant = new Participant(emptyList);
            foreach (Criterion c in Criteria)
            {
                participant.Criteria.Add(c);
            }
            participant.ID = ID; 
            return participant;
        }
    }
}
