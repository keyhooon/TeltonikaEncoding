using ConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp.Model
{
    public class Scenario
    {
        public Scenario()
        {
            ObservedIoProperties = new List<IoPropertyType>();
            RecordConditions = new List<RecordCondition>();
        }
        public int Id { get; }

        public List<IoPropertyType> ObservedIoProperties { get; set; }

        public List<RecordCondition> RecordConditions {get; set;}
    }
}
