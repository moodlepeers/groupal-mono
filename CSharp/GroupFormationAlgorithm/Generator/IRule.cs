using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroupAL.Generator
{
    interface IRule 
    {
        //use the rule to adjust Randomly generated criteria in an participant 
        Participant AdjustParticipant( Participant participant);
    }
}
