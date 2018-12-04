using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroupAL.Generator
{
    /// <summary>
    /// dont deals with any ajustment. It just returns the entries back that it gets
    /// </summary>
    class EmptyRule:IRule
    {
        public Participant AdjustParticipant(Participant participant)
        {
            return participant;
        }
    }
}
