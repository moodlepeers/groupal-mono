using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using GroupAL.Criteria;
using System.Text.RegularExpressions;
using System.Globalization;
using System.IO;

namespace GroupAL.Generator
{
    class TextFileParticipantReaderWriter : IParticipantReaderWriter
    {

        private float version=1.0f;
        private static char seperator = '\t';
        static CultureInfo ci = CultureInfo.CreateSpecificCulture("en-en");

        public void WriteParticipantsToFile(List<Participant> entries, string file)
        {
            using(StreamWriter _writer = new StreamWriter(file))
            {
                _writer.WriteLine(GetHeader(entries));
                foreach (Participant participant in entries)
                {
                   _writer.Write(ParticipantToTextString(participant));                
                }                                     
                // Write the document
                _writer.Close();
            }
        }

        private string ParticipantToTextString(Participant p)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Criterion c in p.Criteria)
            {
                int i=0;
                foreach (float v in c.Value)
                {                           
                    sb.Append(p.ID).Append(seperator);
                    sb.Append(c.Name).Append(seperator);
                    sb.Append(c.MinValue).Append(seperator);
                    sb.Append(c.MaxValue).Append(seperator);
                    sb.Append(c.IsHomogeneous).Append(seperator);
                    sb.Append(c.Weight).Append(seperator);
                    sb.Append(c.Value.Length).Append(seperator);
                    sb.Append(i).Append(seperator);
                    sb.Append(v);
                    sb.Append("\n");
                    i++;
                }
            }
            return sb.ToString();
        }

        private String GetHeader(List<Participant> entries)
        {
            if (entries.Count < 1) throw new NullReferenceException("TextFileParticipantReaderWriter.getHeader needs at least one participant to write out.");
            StringBuilder sb = new StringBuilder();
            // first the fields for participants
            sb.Append("id").Append(seperator);
            sb.Append("criterion").Append(seperator);
            sb.Append("minValue").Append(seperator);
            sb.Append("maxValue").Append(seperator);
            sb.Append("isHomogeneous").Append(seperator);
            sb.Append("weight").Append(seperator);
            sb.Append("valueCount").Append(seperator);
            sb.Append("valueNumber").Append(seperator);
            sb.Append("value");
            return sb.ToString();
        }

        public List<Participant> ReadParticipantsFromFile(string file)
        {
            List<Participant> loEntries = new List<Participant>();
            try
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    String critBefore = "";
                    int pIDbefore = -1;
                    Criterion c = null;
                    List<Criterion> criteria = new List<Criterion>();
                    if (!reader.EndOfStream) reader.ReadLine();  // throw away the header
                    while (!reader.EndOfStream)
                    {                        
                        String line = reader.ReadLine();
                        if (line.Length == 0) continue;
                        String[] l = line.Split(seperator);
                        int id = int.Parse(l[0]);
                        string crit = l[1];
                        float minVal = float.Parse(l[2]);
                        float maxVal = float.Parse(l[3]);
                        bool isHom = bool.Parse(l[4]);
                        float weight = float.Parse(l[5]);
                        int valCount = int.Parse(l[6]);
                        int valNo = int.Parse(l[7]);
                        float val = float.Parse(l[8]);

                        if (!pIDbefore.Equals(id))
                        {
                            criteria = new List<Criterion>();  // create new reference
                            critBefore = "";
                            Participant p = new Participant(criteria);                 // use the referenceing for all coming criteria            
                            loEntries.Add(p);
                            pIDbefore = id;
                        }

                        if (!critBefore.Equals(crit))
                        {
                            c = new SpecificCriterion(crit, valCount, minVal, maxVal, isHom, weight);
                            criteria.Add(c);
                            critBefore = crit;
                        }
                        c.Value[valNo] = val;                                                
                    }
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw new Exception("TextFileparticipantReaderWriter.readEntriesFromFile: seems not posible to open file!!!:");
            }
            return loEntries;            
        }        
    }
}
