using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Globalization;
using GroupAL.Criteria;
using GroupAL.Evaluator;
using GroupAL.Matcher;

namespace GroupAL
{
    [Obsolete]
    class XMLIO
    {
        /// <summary>
        /// Cultural info for parsing float values from xml
        /// </summary>
        static CultureInfo ci = CultureInfo.CreateSpecificCulture("en-en");
        
        /// <summary>
        /// The document from which the Criteria is imported.
        /// </summary>
        static private XDocument _document;

        /// <summary>
        /// For Configuration import 
        /// </summary>
        /// <param name="uri"></param>
        static public Configurator readConfigurationFromXML(string filePath)
        {
            _document = XDocument.Load(filePath);
            Configurator config = Configurator.getInstance();
            XElement configurationElement = _document.Element("configuration");
            XElement evaluatorsElement = configurationElement.Element("evaluators");
            XElement matchersElement = configurationElement.Element("matchers");

            // in future it should load a list of possible matchers 
            foreach(XElement match in matchersElement.Elements("matcher")){
                string matcher=match.Attribute("name").Value;
                    switch(matcher){
                        case "FarawaySoCloseMatcher":
                            config.Matcher= new FarawaySoCloseMatcher();
                            break;
                        default:
                            config.Matcher= new RandomMatcher();
                            break;
                    }
                }
                
            // in future it should load a list of possible evluators 
            foreach(XElement eval in evaluatorsElement.Elements("evaluator")){
                    switch(eval.Attribute("name").Value){
                        case "FarawaySoCloseEvaluator":
                            config.Evaluator = new FarawaySoCloseEvaluator();
                            break;
                        default:
                            config.Evaluator = new GroupALEvaluator();
                            break;
                    }
                }
                
            return config;
            }
        

        /// <summary>
        /// Imports the  
        /// </summary>
        /// <param name="uri"></param>
        static public List<Participant> readEntriesFromXML(string filePath)
        {
            

            List<Participant> listOfEntriesFromXML = new List<Participant>();
            List<Criterion> loCriteria = new List<Criterion>();
            _document = XDocument.Load(filePath);
            XElement EntriesElement = _document.Element("Entries");
            readMetaCriteria();

            //go through evry participant
            foreach (XElement participantElement in EntriesElement.Elements("participant")) {
                int id = Int32.Parse(participantElement.Attribute("id").Value);
                //each participant gets a new list of criteria
                loCriteria = new List<Criterion>();

                // go through ~every~ (loop is missing) criterion of participant
                // FIXME Loop
                XElement LearnerContextSpecificCriterionElement = participantElement.Element("LearnerContextSpecificCriteria");
                getAttributesFromCriterion(loCriteria, LearnerContextSpecificCriterionElement , new SpecificCriterion("test", 1,1,2,true, 1));                
                
                Participant participantFromXML = new Participant(loCriteria);
                listOfEntriesFromXML.Add(participantFromXML);
            }

            return listOfEntriesFromXML;
            throw new NotImplementedException("reading entries out of a file is not implemented yet");
        }

        /// <summary>
        /// reads the meta criteria which is valid for each group in .xml file
        /// and stores the criteria values in the static field of the Group class
        /// </summary>
        static public void readMetaCriteria() {
            XElement EntriesElement = _document.Element("Entries");

            XElement MetaCriteriaElement = EntriesElement.Element("MetaCriteria");
            XElement GroupSizeElement = MetaCriteriaElement.Elements("Criterion").Where(x => x.Attribute("name").Value == Statics.AvailibleCriteria.GroupSizeMetaCriterion.ToString("g")).ElementAt(0);
            XElement TimeRefreshingElement = MetaCriteriaElement.Elements("Criterion").Where(x => x.Attribute("name").Value == Statics.AvailibleCriteria.TimeAfterGroupRefreshingMetaCriterion.ToString("g")).ElementAt(0);

            Group.GroupMembersMaxSize = (int) GroupSizeElement.Attribute("value0");
            Group.TimeBeforeRefreshGroup = (int)TimeRefreshingElement.Attribute("value0");
            

        }
       
        /// <summary>
        /// gets all atributes from an Criterion
        /// </summary>
        /// <param name="loCriteria"></param>
        /// <param name="criteriaElement"></param>
        /// <param name="hmc"></param>
        private static void getAttributesFromCriterion(List<Criterion> loCriteria, XElement criteriaElement, Criterion hmcObj)
        {

            Type t = hmcObj.GetType();
            

            foreach (XElement creterionElement in criteriaElement.Elements("Criterion"))
            {
                Criterion hmc = Activator.CreateInstance(t) as Criterion;
                // get all values in the criterion save theme in the array "value" by position number which is part of the attribute name  
                int valuesCount = creterionElement.Attributes().Count();
                float[] values = new float[valuesCount - 1];

                for (int i = 0; i < valuesCount; i++)
                {
                    if (creterionElement.Attribute("value" + i) != null)
                    {
                        values[i] = float.Parse(creterionElement.Attribute("value" + i).Value, ci);
                    }
                }

                // get the name of the criterion 
                string criterionName = creterionElement.Attribute("name").Value;

                // create new HomogeniousCriterionobject
                //HeterogeneousCriterion  hmc = new HeterogeneousCriterion();
                hmc.Name = criterionName;
                hmc.Value = values;
                if (creterionElement.Attribute("MinValue") !=null ) hmc.MinValue = float.Parse(creterionElement.Attribute("MinValue").Value);
                if (creterionElement.Attribute("MaxValue") != null) hmc.MaxValue = float.Parse(creterionElement.Attribute("MaxValue").Value);
                loCriteria.Add(hmc);
            }
        }
    }
}
