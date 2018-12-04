using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroupAL.Criteria
{
    public class SpecificCriterion : Criterion
    {        
        public SpecificCriterion(String Name,int numberOfValues,float minVal,float maxVal, bool isHomogeneous, float weight) {
            base.Name = Name;
            base.Value = new float[numberOfValues];
            base.IsHomogeneous = isHomogeneous;
            base.MinValue = minVal;
            base.MaxValue = maxVal;
            CriterionWeight.addIfNotAllreadyExist(Name,weight);
            //base.FillValuesWithRandomValues();

        }
    }
}
