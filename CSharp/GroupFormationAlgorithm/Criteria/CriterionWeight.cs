using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroupAL.Criteria
{
    public class CriterionWeight
    {
        private static Dictionary<string, float> _criterionWeights = new Dictionary<string, float>();

        /// <summary>
        /// sets for each used criterion the given weight
        /// </summary>
        /// <param name="criterionWeights"></param>
        public static void init(Dictionary<string, float> criterionWeights){ 
            _criterionWeights=criterionWeights;
        }

        /// <summary>
        /// Checks that all given keys already exist and that sum of values is 1 (normalized)
        /// </summary>
        /// <param name="newWeights"></param>
        /// <returns>true on success, false of not</returns>
        public static bool ChangeWeights(Dictionary<string, float> newWeights) {
            bool isSameKeySet = true;
            foreach (string s in newWeights.Keys) {
                isSameKeySet &= _criterionWeights.ContainsKey(s);
            }

            if (newWeights.Values.Sum() == 1 && isSameKeySet) {
                _criterionWeights = newWeights;
                return true;
            }
            return false;
        }

        public static float GetWeight(string CriterionName)
        {
            if (_criterionWeights.ContainsKey(CriterionName))
                return _criterionWeights[CriterionName];
            throw new Exception("CriterionWeight does not contains the CriterionName you are looking for!");   
        }

        /// <summary>
        /// Allows a multiple setting of same criterion weight as long as it is the same value
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="weight"></param>
        internal static void addIfNotAllreadyExist(string Name, float weight)
        {
            if (_criterionWeights.ContainsKey(Name)) {
                if (_criterionWeights[Name] != weight)
                {
                    throw new Exception("CriterionWeight: the given CriterionName has allready an other weight");
                }
            }
            else
                _criterionWeights.Add(Name, weight);
        }
    }
}
