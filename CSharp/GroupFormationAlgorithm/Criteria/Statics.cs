using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroupAL.Criteria
{
    /// <summary>
    /// Class providing convenient access to popular criterion "sets" with correct dimension names.
    /// (not really used yet)
    /// </summary>
    class Statics
    {
        public enum AvailibleCriteria {

            SpellingErrorHomogeniousCriterion,
            AgeHomogeniousCriterion,
            
            BartleHeterogeneousCriterion,
            BigFiveHeterogeneousCriterion,
            KolbHeterogeneousCriterion,
            SilvermanFelderHeterogeneousCriterion,

            GroupSizeMetaCriterion,
            TimeAfterGroupRefreshingMetaCriterion,

            LocationLearnerContextCriterion,
            TemperaturLearnerContextCriterion,
            ActivityLearnerContextCriterion,
            AvailabilityLearnerContextCriterion
        }

        public static string SpellingErrorCriterionName = "SpellingError";
        public static string AgeCriterionName = "Age";

        public static string BartleCriterionName = "Bartle Player Model";
        public static string BigFiveCriterion = "BigFive";
        public static string KolbCriterion = "Kolb Learningtype Model";
        public static string SilvermanFelderCriterion = "SilvermanFelder Learningtype Model";

        public static string GroupSizeCriterion = "Group Size";
        public static string TimeAfterGroupRefreshingCriterion = "Time After Group Refreshing";

        public static string LocationCriterion = "Location";
        public static string TemperaturCriterion = "Temperatur";
        public static string ActivityCriterion = "Activity";
        public static string AvailabilityCriterion = "Availability";

        internal static Criterion getCriterionObject(AvailibleCriteria c)
        {
            switch (c) { 
                case AvailibleCriteria.SpellingErrorHomogeniousCriterion:
                    SpecificCriterion SpellingErrorRate = new SpecificCriterion("SpellingError",1,0,1,true,1);    
                    return SpellingErrorRate;
                    break;
                case AvailibleCriteria.AgeHomogeniousCriterion:
                    SpecificCriterion age = new SpecificCriterion("Age",1,0,100,true,1);
                    return age;
                    break;

                case AvailibleCriteria.BartleHeterogeneousCriterion:
                    SpecificCriterion bartle = new SpecificCriterion("Bartle",4,0,1,false,1);
                    return bartle;
                    break;
                case AvailibleCriteria.BigFiveHeterogeneousCriterion:
                    SpecificCriterion bigFive = new SpecificCriterion("Big5",5,0,1,false,1);
                    return bigFive;
                    break;
                case AvailibleCriteria.KolbHeterogeneousCriterion:
                    throw new NotSupportedException("the given case is not supported yet");
                    break;
                case AvailibleCriteria.SilvermanFelderHeterogeneousCriterion:
                    throw new NotSupportedException("the given case is not supported yet");
                    break;

                case AvailibleCriteria.GroupSizeMetaCriterion:
                    throw new NotSupportedException("the given case is not supported yet");
                    break;
                case AvailibleCriteria.TimeAfterGroupRefreshingMetaCriterion:
                    throw new NotSupportedException("the given case is not supported yet");
                    break;

                case AvailibleCriteria.LocationLearnerContextCriterion:
                    throw new NotSupportedException("the given case is not supported yet");
                    break;

                case AvailibleCriteria.ActivityLearnerContextCriterion:
                    throw new NotSupportedException("the given case is not supported yet");
                    break;
                case AvailibleCriteria.AvailabilityLearnerContextCriterion:
                    throw new NotSupportedException("the given case is not supported yet");
                case AvailibleCriteria.TemperaturLearnerContextCriterion:
                    throw new NotSupportedException("the given case is not supported yet");

                default:
                    throw new NotSupportedException("the given criterion is not supported yet");
                    break;
            }

            throw new NotSupportedException("the given criterion is not supported yet");
            return null;
        }

        internal static Criterion getCriterionObject(string c) {
            return getCriterionObject(getCriterionEnumFromName(c));
        }
        
        static AvailibleCriteria getCriterionEnumFromName(string s){
            if(AvailibleCriteria.SpellingErrorHomogeniousCriterion.ToString("g") == s){return AvailibleCriteria.SpellingErrorHomogeniousCriterion ;}
            if(AvailibleCriteria.AgeHomogeniousCriterion.ToString("g") == s){return AvailibleCriteria.AgeHomogeniousCriterion ;}
            
            if(AvailibleCriteria.BartleHeterogeneousCriterion.ToString("g") == s){return AvailibleCriteria.BartleHeterogeneousCriterion ;}
            if(AvailibleCriteria.BigFiveHeterogeneousCriterion.ToString("g") == s){return AvailibleCriteria.BigFiveHeterogeneousCriterion ;}
            if(AvailibleCriteria.KolbHeterogeneousCriterion.ToString("g") == s){return AvailibleCriteria.KolbHeterogeneousCriterion ;}
            if(AvailibleCriteria.SilvermanFelderHeterogeneousCriterion.ToString("g") == s){return AvailibleCriteria.SilvermanFelderHeterogeneousCriterion ;}

            if(AvailibleCriteria.GroupSizeMetaCriterion.ToString("g") == s){return AvailibleCriteria.GroupSizeMetaCriterion ;}
            if(AvailibleCriteria.TimeAfterGroupRefreshingMetaCriterion.ToString("g") == s){return AvailibleCriteria.TimeAfterGroupRefreshingMetaCriterion ;}

            if(AvailibleCriteria.LocationLearnerContextCriterion.ToString("g") == s){return AvailibleCriteria.LocationLearnerContextCriterion ;}
            if(AvailibleCriteria.TemperaturLearnerContextCriterion.ToString("g") == s){return AvailibleCriteria.TemperaturLearnerContextCriterion ;}
            if(AvailibleCriteria.ActivityLearnerContextCriterion.ToString("g") == s){return AvailibleCriteria.ActivityLearnerContextCriterion ;}
            if(AvailibleCriteria.AvailabilityLearnerContextCriterion.ToString("g") == s){return AvailibleCriteria.AvailabilityLearnerContextCriterion ;}

            throw new NotImplementedException("Statics.getCriterionObject Error: ther is no criterion called \""+ s+"\"");
        }

    }
}
