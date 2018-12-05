using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;
using System.Diagnostics;
using GroupAL.Optimizer;
using GroupAL.Evaluator;
using GroupAL.Matcher;
using GroupAL.WriterReader;
using GroupAL.Generator;
using System.IO;
using System.Collections;
using GroupAL.Criteria;
using Nancy.ModelBinding;
using Nancy.Extensions;

namespace GroupAL.RestService
{

    public class GroupFormationService : Nancy.NancyModule
    {
        public GroupFormationService()
        {
            // you can find some example data generated and readable/written with ParticipantReaderWriter.cs in folder /bin/Debug/
            // i.e. "DemoEntries_100Pers_3Hom_3Het_RandomValues.xml"
            Get["/"] = x => { return "This is the GroupAlService. Send the participant data to / users / preferences /{ groupSize}"; };

            Post["/users/preferences/{GroupSize}"] = parameters =>
            {
                var xmlString  = this.Request.Body.AsString();                             
                List<Participant> particpants = new XMLParticipantReaderWriter().ReadParticipantsFromString(xmlString);
                GroupFormationAlgorithm gfGbG = new GroupFormationAlgorithm(particpants, new GroupALGroupCentricMatcher(), new GroupALEvaluator(), new GroupALOptimizer(new GroupALGroupCentricMatcher()), parameters.GroupSize);
                Cohort result = gfGbG.DoOneFormation();          
                return result;
            };

        }
    }            
}
