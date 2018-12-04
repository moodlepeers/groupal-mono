using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

using System.Xml.Linq;


namespace GroupAL
{
    class Configurator
    {
        //which evaluator is used in this project
        public IEvaluator Evaluator {get;set;}
        //which matcher is used in this project
        public IMatcher Matcher{get;set;}
        //which Optimizer is used in this project
        public IOptimizer Optimizer { get; set; }


        private static  Configurator configurator;
        
        private Configurator() { 
        }


        public static Configurator getInstance(){
            if (configurator == null)
            {
                configurator = new Configurator();
            }
            return configurator;
        }

        
    }
}
