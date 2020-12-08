using System;
using System.Threading;

namespace ConsoleApp
{
    public class TimerRecordingPolicies : RecordingPolicies
    {
        readonly Timer timer;
        public TimerRecordingPolicies(TimeSpan period)
        {
            Period = period;
            timer = new Timer((state) => TimerElapsed = true, null, 0, (int) period.TotalMilliseconds);
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
