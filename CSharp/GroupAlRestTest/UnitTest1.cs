using System;
using System.Collections.Generic;
using System.Diagnostics;
using GroupAL;
using GroupAL.Criteria;
using NUnit.Framework;


namespace GroupAlRestTest
{
    [TestFixture]
    public class UnitTest1
    {

        private TestContext testContextInstance;

        /// <summary>
        ///  Gets or sets the test context which provides
        ///  information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }


        [Test]
        public void TestMethod1()
        {
            //Console.WriteLine("My bonnie is over the ozean 2");
            
            Assert.IsFalse(false);

            List<Participant> participants = new List<Participant>();
            List<Criterion> criteria = new List<Criterion>();
            SpecificCriterion criterion = new SpecificCriterion("Unsicherheitstoleranz", 5, 1, 5, true, 3.4f);
            SpecificCriterion criterion2 = new SpecificCriterion("Unsicherheitstoleranz 2", 5, 1, 5, true, 1.4f);
            SpecificCriterion criterion3 = new SpecificCriterion("Unsicherheitstoleranz 3", 5, 1, 5, true, 2.4f);
            SpecificCriterion criterion4 = new SpecificCriterion("Unsicherheitstoleranz 4", 5, 1, 5, true, 5.4f);
            SpecificCriterion criterion5 = new SpecificCriterion("Unsicherheitstoleranz 5", 5, 1, 5, true, 6.4f);
            criteria.Add(criterion);
            criteria.Add(criterion2);
            criteria.Add(criterion3);
            criteria.Add(criterion4);
            criteria.Add(criterion5);       
            participants.Add(new Participant(criteria));
            //GroupFormationAlgorithm gfGcM = new GroupFormationAlgorithm(participants, new GroupALGroupCentricMatcher(), new GroupALEvaluator(), new GroupALOptimizer(new GroupALGroupCentricMatcher()), GroupSize);
            //gfGcM.DoOneFormation();

            
        }


    }
}
