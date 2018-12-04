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

namespace GroupAL.RestService
{

    public struct Statistics
    {
        public int n;
        public float avg;
        public float stDev;
        public float normStDev;
        public float performanceIndex;
    }

    class StartService
    {
        static public void Main()
        {
            Console.WriteLine("Hello Mono World 200 AABBCCDDEEFF");


            //generateDummyParticipants();                        

            // "GroupALUsage" is a scenario specific class (you might write your own) which uses all the components of GroupAL nicely.
            // Basically most things could have been implemented in MainWindow as well, but have been encapsulated into GroupALUsage class as the main point of behaviour coding.
            GroupALUsage app = new GroupALUsage(3, 100);  // Build Groups of 3 from 100 participants

            //app.ReadEntriesFromFile("DemoEntries_100Pers_3Hom_3Het_RandomValues.xml");  // you can find some example data generated and readable/written with ParticipantReaderWriter.cs in folder /bin/Debug/
            // or 
            app.generateNewDummyEntries();

            app.TaskRunCompareMatchersGcmOmadoTeammaker1HetCrit(1);

            //Application.Current.Shutdown();

        }

        /// <summary>
        /// This is a specific evaluation data generation method used to create
        /// - 2 sets of data (one set with only one homogeneous criterion with 4 dimensions, one set with 2 homogeneous and 2 heterogeous matching criteria, each with 4 dimensions)
        /// - each set in three variations (data distribution uniformly (all values 0..1 have same possibility, gausian values between 0..1, and only extreme values (0,1))
        /// - the 6 combinations of sets (S) and variances (V) are all generated 100 times for 500 participants
        /// - automatically named and stored
        /// 
        /// Data created with this method has been used to compare the different matchers implemented in namespace GroupAL.Matcher, e.g. to be published in
        /// Konert, J.: Interactive Multimedia Learning: Using Social Media for Peer Education in Single-Player Educational Games (accepted for publication). Technische Universität Darmstadt, Germany 2014. 
        /// </summary>
        private void generateDummyParticipants()
        {
            IParticipantReaderWriter tw = new TextFileParticipantReaderWriter();
            for (int i = 0; i < 2; i++)
            {// 2 sets
                int numhet = 1;
                int dimhet = 4;
                int numhom = 0;
                int dimhom = 4;
                if (i == 1)
                {
                    numhet = 2;
                    numhom = 2;
                }
                for (int j = 0; j < 3; j++)
                {// 3 variations
                    IRule rule = new EmptyRule();
                    IValueGenerator valgen = new UniformValueGenerator();
                    if (j == 1) valgen = new GaussianValueGenerator();
                    if (j == 2) rule = new TeamMakerRule(); // builds 1 and 0 (extreme) values. Only ~one~ 1 for heterogeneous criteria, several 1s for homogeneous criteria
                    for (int k = 1; k <= 100; k++) // number of rounds to do it. (100 evaluation runs)
                    {
                        String filename = ".\\data\\GroupALParticipantsSet" + (i + 1) + "V" + (j + 1) + "_" + k + ".csv";
                        //String filename = "GroupALParticipantsN"+amount+"Het"+numHet+"Dim"+hetDim+"Hom"+numHom+"Dim"+homDim+".csv";
                        // set here number of participants (500)
                        ParticipantGenerator gen = new ParticipantGenerator(500, numhet, dimhet, numhom, dimhom, valgen, rule, tw, filename);
                    }
                }
            }
        }

    }
}
