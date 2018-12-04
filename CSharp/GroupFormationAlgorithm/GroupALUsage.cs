using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using GroupAL.Generator;
using GroupAL.Matcher;
using GroupAL.Evaluator;
using GroupAL.Optimizer;
using System.IO;
using GroupAL.WriterReader;

namespace GroupAL
{
    /// <summary>
    /// A demo / application specific class implementation using the components of GroupAL for matching
    /// </summary>
    class GroupALUsage
    {
        Stopwatch sw = new Stopwatch();  // to measure and write out computation time needed
        public List<Participant> participants;
        ICohortReaderWriter CohortWriter = new TextFileCohortReaderWriter();
        public int GroupSize;
        int CountofEntries;
         
        // specific characters to be used for excel CSV reading/writing...could be encapusalted more nicely in future (TODO)
        string separator = "\t";
        string newLine = "\n";
        string replacementForNumberComma = ","; // comma here leaves comma basically as seperator in numbers


        public GroupALUsage(int _GroupSize, int _CountofEntries)
        {
            GroupSize = _GroupSize;
            CountofEntries = _CountofEntries;
        }

        public List<Participant> generateNewDummyEntries()
        {            
            participants = ParticipantGenerator.DummyEntries(CountofEntries);
            return participants;
        }

        public void ReadEntriesFromFile(string fileName)
        {
            participants = new XMLParticipantReaderWriter().ReadParticipantsFromFile(fileName);
            CountofEntries = participants.Count;

        }

        /// <summary>
        /// Example method evaluating optimization cycles of different matchers with 100 generated dummy participants
        /// </summary>
        public void TaskRunCompareMatchersOptimization()
        {
            GroupFormationAlgorithm gf;
            TextFileCohortReaderWriter CohortWriter = new TextFileCohortReaderWriter();
            string filename = "ExampleOptimizationProcessForeachMatcher.csv";
            // columns header of CSV file
            string output = "MatcherName" + separator + "Cycle" + separator + "CohortPerformanceIndex" + separator + "CohortAverage" + separator + "OptimizationCount" + newLine;
            CohortWriter.WriteTemporaryResultsToFile(output, filename, true);

            List<IMatcher> matchers = new List<IMatcher> { new GroupALGroupCentricMatcher(), new GroupALParticipantCentricMatcher(), new RandomMatcher() };
            
            int GroupSize = 6;
            int cycles = 10;
            int CountOfOptimizations = 5;            
            
            for (int i = 0; i < cycles; i++)
            {
                participants = ParticipantGenerator.DummyEntries(100);
                foreach (IMatcher matcher in matchers)
                {
                    gf = new GroupFormationAlgorithm(participants, matcher, new GroupALEvaluator(), new GroupALOptimizer(matcher), GroupSize);
                    gf.DoOneFormation();

                    for (int j = 0; j < CountOfOptimizations; j++)
                    {
                        if (j == 0)
                        {
                            output = gf.Cohort.whichMatcherUsed + separator + i + separator + gf.Cohort.results.performanceIndex + separator + gf.Cohort.results.avg + separator + j + newLine;
                            CohortWriter.WriteTemporaryResultsToFile(output.Replace(",", replacementForNumberComma), filename, false);
                            continue;
                        }
                        gf.OptimizeCohort();
                        output = gf.Cohort.whichMatcherUsed + separator + i + separator + gf.Cohort.results.performanceIndex + separator + gf.Cohort.results.avg + separator + j + newLine;
                        CohortWriter.WriteTemporaryResultsToFile(output.Replace(",", replacementForNumberComma), filename, false);
                    }
                }
            }
        }

        /// <summary>
        /// Example method comparing comutation time for different matchers
        /// </summary>
        public void TaskRunCompareMatchersRuntime()
        {
            TextFileCohortReaderWriter CohortWriter = new TextFileCohortReaderWriter();
            
            string filename="ExampleTimeComparisonBetweenMatcher.csv";
            // columns header of CSV file
            string output = "MatcherName" + separator + "countOfParticipants" + separator + "TimeNeeded"+newLine;

            CohortWriter.WriteTemporaryResultsToFile(output, filename, true);
            for (int i = 0; i < 3; i++)
            {
                participants = ParticipantGenerator.DummyEntries((i == 0 ? 100 : i * 500));
                
                GroupFormationAlgorithm gf = new GroupFormationAlgorithm(participants, new GroupALParticipantCentricMatcher(), new GroupALEvaluator(), new GroupALOptimizer(new GroupALGroupCentricMatcher()), GroupSize);
                sw.Start();
                gf.DoOneFormation();
                sw.Stop();
                output = gf.Cohort.whichMatcherUsed + separator + participants.Count + separator + sw.ElapsedMilliseconds + newLine;
                
                
                GroupFormationAlgorithm gfGbG = new GroupFormationAlgorithm(participants, new GroupALGroupCentricMatcher(), new GroupALEvaluator(), new GroupALOptimizer(new GroupALGroupCentricMatcher()), GroupSize);
                sw.Restart();
                gfGbG.DoOneFormation();
                sw.Stop();
                output += gfGbG.Cohort.whichMatcherUsed + separator + participants.Count + separator + sw.ElapsedMilliseconds + newLine;

                
                GroupFormationAlgorithm gfRandom = new GroupFormationAlgorithm(participants, new RandomMatcher(), new GroupALEvaluator(), new GroupALOptimizer(new GroupALGroupCentricMatcher()), GroupSize);
                sw.Restart();
                gfRandom.DoOneFormation();
                sw.Stop();
                output += gfRandom.Cohort.whichMatcherUsed + separator + participants.Count + separator + sw.ElapsedMilliseconds + newLine;

                CohortWriter.WriteTemporaryResultsToFile(output.Replace(",", replacementForNumberComma), filename, false);
            }

        }


        /// <summary>
        ///  Example: compare Group-centric Matcher with matcher implementations of TeamMaker and Omado Genesis with one heterogeneous criterion as e.g. in a similar way used for publication
        ///  Konert, J.; Burlak, D.; Göbel, S.; Steinmetz, R.: GroupAL: ein Algorithmus zur Formation und Qualitätsbewertung von Lerngruppen in E-Learning-Szenarien mittels n-dimensionaler Gütekriterien. In: Proceedings of the DeLFI 2013: Die 11. e-Learning Fachtagung Informatik der Gesellschaft für Informatik e.V. (Hrsg. Breitner, A.; C. Rensing) (pp. 71–82). Bremen, Germany: Köllen, 2013. 
        ///  or more detailed in
        ///  Konert, J.; Burlak, D.; Göbel, S.; Steinmetz, R.: GroupAL: ein Algorithmus zur Formation und Qualitätsbewertung von Lerngruppen in E-Learning-Szenarien. i-com , 13(1) (2014), 70–81 doi:10.1515/icom-2014-0010
        /// </summary>
        /// <param name="runs">number of runs to repeat (each with 500 new participants generated)</param>

        internal void TaskRunCompareMatchersGcmOmadoTeammaker1HetCrit(int runs) {
            TextFileCohortReaderWriter CohortWriter = new TextFileCohortReaderWriter();
            IRule rule = new TeamMakerRule();
            int CountOfPraticipants = 500;
            int GroupSize = 3;
            string fileName = "TaskRunCompareMatchers_" + (rule == null ? "nR" : "R") + "_" + GroupSize + "G_1K.csv";
            string output = "";
            CohortWriter.WriteTemporaryResultsToFile("CohortAveragePerformance" + separator + "CohortPerformanceIndex" + separator + "Matcher" + newLine, fileName, true);
            for (int i = 0; i < runs; i++)
            {

                participants = ParticipantGenerator.DummyEntries(CountOfPraticipants, rule);
                //participants = ParticipantGenerator.PerfectDummyEntries();


                GroupFormationAlgorithm gfTM = new GroupFormationAlgorithm(participants, new TeamMakerMatcher(), new TeamMakerEvaluator(), new GroupALOptimizer(new GroupALGroupCentricMatcher()), GroupSize);
                gfTM.DoOneFormation();

                GroupFormationAlgorithm gfGcM = new GroupFormationAlgorithm(participants, new GroupALGroupCentricMatcher(), new GroupALEvaluator(), new GroupALOptimizer(new GroupALGroupCentricMatcher()), GroupSize);
                gfGcM.DoOneFormation();

                //omadogenesis is Random
                GroupFormationAlgorithm gfRandom = new GroupFormationAlgorithm(participants, new RandomMatcher(), new GroupALEvaluator(), new GroupALOptimizer(new GroupALGroupCentricMatcher()), GroupSize);
                gfRandom.DoOneFormation();

                output = (gfTM.Cohort.results.avg + separator + gfTM.Cohort.results.performanceIndex + separator + gfTM.Cohort.whichMatcherUsed).Replace(",", ".") + newLine;
                output += (gfGcM.Cohort.results.avg + separator + gfGcM.Cohort.results.performanceIndex + separator + gfGcM.Cohort.whichMatcherUsed).Replace(",", ".") + newLine;
                output += (gfRandom.Cohort.results.avg + separator + gfRandom.Cohort.results.performanceIndex + separator + gfRandom.Cohort.whichMatcherUsed).Replace(",", ".") + newLine;

                CohortWriter.WriteTemporaryResultsToFile(output, fileName, false);
            }
        
        }


        /// <summary>
        /// Helper method writing all given group formations ("runs") to one CSV statistical string (for file output most likely)
        /// Items will occur in following order: "CohortAveragePerformance (av. GPI)"    "CohortVarianz"    "CohortPerformanceIndex (CPI)"    "AverageOfAverages" "AverageVarianz" "VarianzeOfVarianzes"  "Matcher"
        /// </summary>
        /// <param name="gfOutputList"></param>
        /// <returns></returns>
        private string printAllGfsToSTring(List<GroupFormationAlgorithm> gfOutputList)
        {

            string output = "";
            var groups = gfOutputList.GroupBy(x => x.Cohort.whichMatcherUsed).Select(g => new Statistics { Matcher = g.Key, avg = g.Average(x => x.Cohort.results.avg), stDev = g.Average(x => x.Cohort.results.stDev), averageVarianz = g.Average(x => x.Cohort.results.varianz) });
            if (gfOutputList.Count < 1) return "";

            for (int i = 0; i < gfOutputList.Count; i++)
            {

                GroupFormationAlgorithm gf = gfOutputList[i];
                float averageOfAverage = groups.Single(x => x.Matcher == gf.Cohort.whichMatcherUsed).avg;
                float varianzeOfVarianz = groups.Single(x => x.Matcher == gf.Cohort.whichMatcherUsed).varianz;
                float averageVarianze = groups.Single(x => x.Matcher == gf.Cohort.whichMatcherUsed).averageVarianz;
                // Items will occur in following order: "CohortAveragePerformance (av. GPI)"    "CohortVarianz"    "CohortPerformanceIndex (CPI)"    "AverageOfAverages" "AverageVarianz" "VarianzeOfVarianzes"  "Matcher"
                //special treatment for the last gfs
                output += (gf.Cohort.results.avg + separator + gf.Cohort.results.varianz + separator + gf.Cohort.results.performanceIndex + separator + averageOfAverage + separator + averageVarianze + separator + varianzeOfVarianz + separator + gf.Cohort.whichMatcherUsed).Replace(",", replacementForNumberComma) + newLine;
            }
            return output;
        }

        /// <summary>
        /// Helper method which can be used to shuffle a list by randomizer. 
        /// Implemented as: n-times element swaps (n = size of list).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Shuffle<T>(IList<T> list)
        {
            Random rng = new Random(System.DateTime.Now.Millisecond);
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

                
    } // class
} // NS
