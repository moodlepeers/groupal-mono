using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace GroupAL.Generator
{
    /// <summary>
    ///Based on the heuristic described in 
    ///Automating the Process of Assigning Students to Cooperative-Learning Teams
    ///by
    ///Ryan Cavanaugh, Matt Ellis, Richard Layton, Mark Ardis 
    ///From 
    ///Rose-Hulman Institute of Technology
    /// </summary>
    class TeamMakerRule : IRule
    {
        

        //Here Matchmaker the two Criteria Types are simulated in which Matchmaker is 
        public Participant AdjustParticipant(Participant participant)
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            
            int countOfOnes=0;
            

            foreach (Criterion c in participant.Criteria)
            {                                
                if (c.IsHomogeneous) {
                    //Choose of any homogenity (checkbox like several possible answers to a Question)
                    // count of possible 1 Values
                    countOfOnes = rand.Next(1, c.Value.Length + 1);
                }
                else {
                    //MultipleChoice heterogenity (only one possible answer to a Question)
                    //itterate over each cell in the variable participant.criterion.Value and adjust the values
                    countOfOnes = 1;
                }

                //set all Values on the right position to 1 and the other to 0
                for (int i = 0; i < c.Value.Length; i++)
                {
                    c.Value[i] = 0;
                    if (i < countOfOnes ) c.Value[i] = 1;
                }
                c.Value = Shuffle(c.Value).ToArray();
                c.MinValue = 0;
                c.MaxValue = 1;                
            }

            return participant;
        }

        //returns a schuffled array
        public IList<T> Shuffle<T>(IList<T> list)
        {           
            byte[] randomNumber = new byte[1];
            RNGCryptoServiceProvider Gen = new RNGCryptoServiceProvider();
            Gen.GetBytes(randomNumber);
            Random rand = new Random((int)randomNumber[0]);

            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = rand.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }
    
    }
}
