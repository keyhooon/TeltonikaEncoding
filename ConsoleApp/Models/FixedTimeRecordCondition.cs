using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp.Model
{
    public class FixedTimeRecordCondition : RecordCondition
    {
        public TimeSpan Period { get; set; }
    }
}
