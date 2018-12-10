using System;
using System.Collections.Generic;

using GroupAL.Criteria;
using System.Text.RegularExpressions;
using System.Globalization;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using System.Linq;

namespace GroupAL.Generator
{
    class XMLParticipantReaderWriter : IParticipantReaderWriter
    {

        private XDocument _document = new XDocument();
        private float version=1.0f;
        static CultureInfo ci = CultureInfo.CreateSpecificCulture("en-en");

        public void WriteParticipantsToFile(List<Participant> entries, string file)
        {
            _document.Declaration = new XDeclaration("1.0", "utf-8", null);

            XElement UsedCriteriaElement = GetUsedCriteria(entries);

            XElement EntriesElement = new XElement("Participants",
                new XAttribute("version", version)
            );
            
            EntriesElement.Add(UsedCriteriaElement);

            foreach (Participant participant in entries)
            {
                XElement participantElement = ParticipantToXMLString(participant);
                EntriesElement.Add(participantElement);
            }

            
            _document.Add(EntriesElement);
             
            // Write the document
            _document.Save(file);
        }

        private System.Xml.Linq.XElement GetUsedCriteria(List<Participant> entries)
        {
            if(entries.Count < 1) throw new NullReferenceException("XMLparticipantReaderWriter.getUsedCriteria needs at least one participant to write out.");
            XElement UsedCriteriaElement = new XElement("UsedCriteria");
            foreach (Criterion c in entries.ElementAt(0).Criteria) { 
            XElement CriterionElement = new XElement("Criterion",
                new XAttribute ("name",c.Name),
                new XAttribute ("minValue",c.MinValue),
                new XAttribute ("maxValue",c.MaxValue),
                new XAttribute ("isHomogeneous",c.IsHomogeneous),
                new XAttribute("weight", c.Weight),
                new XAttribute("valueCount", c.Value.Length)
                );
            UsedCriteriaElement.Add(CriterionElement);
            }
            return UsedCriteriaElement;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="participant">an participant</param>
        /// <returns>the content of the given participant in XML format</returns>
        private XElement ParticipantToXMLString(Participant participant)
        {
            XElement participantElement = new XElement("participant",
                new XAttribute("id", participant.ID)                
            );
            
            foreach (Criterion c in participant.Criteria)
            {
                XElement CriterionElement = new XElement("Criterion",
                    new XAttribute("name", c.Name),
                    new XAttribute("minValue", c.MinValue),
                    new XAttribute("maxValue", c.MaxValue),
                    new XAttribute("isHomogeneous", c.IsHomogeneous),
                    new XAttribute("weight", c.Weight));
                for (int i = 0; i < c.Value.Length; i++)
                {
                    XElement val = new XElement("Value", 
                        new XAttribute("name","value"+i),
                        new XAttribute("value",c.Value[i]));
                    CriterionElement.Add(val);
                }
                participantElement.Add(CriterionElement);
            }
           

            return participantElement;
        }

        public List<Participant> ReadParticipantsFromFile(string file)
        {
            List<Participant> loEntries = new List<Participant>();
            try
            {
               _document = XDocument.Load(file);            
            }
            catch (Exception e) {
                throw new Exception("XMLparticipantReaderWriter.readEntriesFromFile: seems not posible to open file!!!:");
            }
            //readin all Used Criteria
            // save the weiths for Each criteria in an indipendant class
            //read in all creteria foreach participant
            XElement EntriesElement = _document.Element("Participants");
            foreach (XElement participantElement in EntriesElement.Elements("participant")) {
                Participant p = GetParticipantOutOfXML(participantElement);
                loEntries.Add(p);
            }
            return loEntries;
        }

        public List<Participant> ReadParticipantsFromString(string xmlFileString)
        {
            List<Participant> loEntries = new List<Participant>();
          
            //_document = XDocument.Load(file);
            XmlReader xmlReader = XmlReader.Create(new StringReader(xmlFileString));                
            _document = XDocument.Load(xmlReader);
         
            //readin all Used Criteria
            // save the weiths for Each criteria in an indipendant class
            //read in all creteria foreach participant
            XElement EntriesElement = _document.Element("Participants");
            foreach (XElement participantElement in EntriesElement.Elements("participant"))
            {
                Participant p = GetParticipantOutOfXML(participantElement);
                loEntries.Add(p);
            }
            return loEntries;
        }


        private Participant GetParticipantOutOfXML(XElement participantElement)
        {
            List<Criterion> loCriteria = new List<Criterion>();
            foreach (XElement CriterionElement in participantElement.Elements("Criterion")) 
            {
                String name = CriterionElement.Attribute("name").Value;
                float minVal = float.Parse(CriterionElement.Attribute("minValue").Value);
                float maxVal = float.Parse(CriterionElement.Attribute("maxValue").Value);
                float weight = float.Parse(CriterionElement.Attribute("weight").Value);
                bool isHomogeneous= bool.Parse(CriterionElement.Attribute("isHomogeneous").Value);
                
                int valueCount = CriterionElement.Elements("Value").Count();
                float [] values = new float[valueCount];
                // may I ask WTF
                foreach(XElement val in CriterionElement.Elements("Value")){
                    int pos = int.Parse(Regex.Match(val.Attribute("name").Value, @"\d+").Value);
                    values[pos] = float.Parse(val.Attribute("value").Value, ci);
                }

                Criterion c = new SpecificCriterion(name, valueCount,minVal,maxVal, isHomogeneous, weight);
                c.Value = values;
                
                loCriteria.Add(c);
            }
            return new Participant(loCriteria);
        }
    }
}
