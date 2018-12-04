using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroupAL.WriterReader
{
    interface ICohortReaderWriter
    {
        List<Group> ReadCohort(string filename);
        void WriteCohort(Cohort cohort, string filename,bool writeToNewFile);
    }
}
