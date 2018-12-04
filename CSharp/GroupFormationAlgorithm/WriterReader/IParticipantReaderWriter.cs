using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroupAL.Generator
{
    interface IParticipantReaderWriter
    {
        void WriteParticipantsToFile(List<Participant> entries, string file);
        List<Participant> ReadParticipantsFromFile(string file);
    }
}
