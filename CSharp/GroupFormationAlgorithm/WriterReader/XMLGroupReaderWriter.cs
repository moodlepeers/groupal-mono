using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace GroupAL.WriterReader
{
    [Obsolete]
    class XMLGroupReaderWriter : ICohortReaderWriter
    {
        public XDocument _document = new XDocument();
        public float version { 
            get {return 1;} 
        }
         public List<Group> ReadCohort(string filename)
        {
            List<Group> groups = new List<Group>();
            _document = XDocument.Load(filename);
            XElement GroupsElement = _document.Element("Groups");

            foreach (XElement groupElement in GroupsElement.Elements("group"))
            {
                Group g = getGroup(groupElement);
                groups.Add(g);
            }
            //set Cohortperformanceindex as static in to Groups
            return groups;           
        }

        private  Group getGroup(XElement groupElement)
        {
            throw new NotImplementedException();
        }

        //writes the given Groups in a XML file 
         public void WriteCohort(Cohort cohort, string filename, bool isNewFile)
        {
             List<Group> groups=cohort.groups;
            //sort groups by Avg
            //groups.Sort(delegate(Group a, Group b) { return a.results.avg < b.results.avg ? -1 : 1; });
            _document.Declaration = new XDeclaration("1.0", "utf-8", null);

            XElement Groups = new XElement("Groups",
                new XAttribute("usedMatcher", cohort.whichMatcherUsed),
                new XAttribute("CohortPerformanceIndex", cohort.results.performanceIndex),
                new XAttribute("CohortAveragePerformanceIndex", cohort.results.avg),
                new XAttribute("CohortNormStDev", cohort.results.normStDev)
            );
            _document.Add(Groups);
            foreach (Group g in groups) {
                XElement groupElement = new XElement("Group",
                    new XAttribute("id",g.groupID),
                    new XAttribute("groupPerformanceIndex",g.results.performanceIndex),
                    new XAttribute("groupAverage", g.results.avg),
                    new XAttribute("normalizedStDev", g.results.normStDev)
                    );
                Groups.Add(groupElement);
                foreach (Participant e in g.Participants) 
                {
                    XElement participantElement = new XElement("participant",
                        new XAttribute("id", e.ID)
                        );
                    groupElement.Add(participantElement);
                    foreach (Criterion c in e.Criteria) 
                    { 
                        XElement criterionElement = new XElement("Criterion",
                            new XAttribute("name",c.Name),
                            new XAttribute("isHomogeneous",c.IsHomogeneous),
                            new XAttribute("minValue",c.MinValue),
                            new XAttribute("maxValue",c.MaxValue)
                            );
                        for (int i = 0; i < c.Value.Length; i++)
                        {
                            criterionElement.Add(new XAttribute("value"+i, c.Value[i]));
                        }
                        participantElement.Add(criterionElement);
                    }
                }
            }

            // Write the document
            _document.Save(filename);
        }


         
    }
}
