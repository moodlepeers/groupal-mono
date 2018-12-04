using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GroupAL.WriterReader
{
    class TextFileCohortReaderWriter : ICohortReaderWriter
    {
        public List<Group> ReadCohort(string filename)
        {
            throw new NotImplementedException();
        }

        string separator = "\t";
        string newLine = "\n";
        public void WriteCohort(Cohort cohort, string filename, bool isNewFile)
        {
            List<Group> groups = cohort.groups;
            string output = "";
            //first header row
            string row="";
            if (isNewFile)
            {
                row = "whichMatcherUsed" + separator + "CohortAVGPerformanceIndex" + separator + "CohortPerformanceIndex" + separator + "CohortNormStDev" + separator;
                row += "groupID" + separator + "groupPerformanceIndex" + separator + "groupAverage" + separator + "normalizedStDev" + separator;
                row += "participantID" + separator + "criterionName" + separator + "isHomogeneous" + separator + "minValue" + separator + "maxValue" + separator + "value" + separator + "ValName";
                output += row + newLine;
            }
            //Groups
            foreach (Group g in groups) {
                
                //write each group
                foreach (Participant p in g.Participants) 
                {
                    
                    //write each participant of the group
                    foreach (Criterion c in p.Criteria) 
                    { //write each criterion
                        
                        for (int i = 0; i < c.Value.Length; i++)
                        {
                            row = cohort.whichMatcherUsed + separator + cohort.results.avg + separator + cohort.results.performanceIndex + separator + cohort.results.normStDev + separator;
                            row += g.groupID + separator + g.results.performanceIndex + separator + g.results.avg + separator + g.results.normStDev + separator;
                            row += p.ID + separator;
                            row += c.Name + separator + c.IsHomogeneous + separator + c.MinValue + separator + c.MaxValue + separator;
                        //write each value to each criterion
                            output += row + c.Value[i]+separator+i+newLine;
                            row = "";
                        }
                    }
                }
            }
            //better for visualization software
            output = output.Replace(",", ".");

            // Write the string to a file.
            if (!File.Exists(filename) || isNewFile)
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter(filename);
                file.WriteLine(output);
                file.Close();
            }
            else
            {
                StreamWriter file = new StreamWriter(filename, true);
                file.WriteLine(output);
            }
        }
        
        public void WriteTemporaryResultsToFile(string output, string filename, bool isNewFile_Header)
        {
            if (!File.Exists(filename) || isNewFile_Header)
            {
                // Write the string to a file.
                System.IO.StreamWriter file = new System.IO.StreamWriter(filename);
                file.Close();
            }
            // 2: Append line to the file
            using (StreamWriter writer = new StreamWriter(filename, true))
            {
                writer.Write(output);
                writer.Flush();
                writer.Close();
            }

        }


        
    }
}
