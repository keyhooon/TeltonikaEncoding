using System;
using System.Threading;

namespace ConsoleApp
{
    public class TimeFixedRecordingPolicies : RecordingPolicies
    {
        readonly Timer timer;
        public TimeFixedRecordingPolicies(TimeSpan period)
        {
            Period = period;
            timer = new Timer((state) => TimerElapsed = true, null, 0, (uint) period.TotalMilliseconds);
        }
        public TimeSpan Period { get; }
        public bool TimerElapsed { get; private set; }
        protected override Func<bool> CoreCondition => ()=> {
            var ret = TimerElapsed;
            TimerElapsed = false;
            return ret;
        };
        protected override void Dispose()
        {
            base.Dispose();
            timer.Dispose();
        }
    }
}
