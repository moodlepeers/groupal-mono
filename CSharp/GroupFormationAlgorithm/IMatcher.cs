using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroupAL
{
    interface IMatcher
    {
        List<Group> MatchToGroups(List<Participant> entries, List<Group> groups);
    }
}
