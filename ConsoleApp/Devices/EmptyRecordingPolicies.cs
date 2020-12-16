using System;

namespace ConsoleApp
{
    public class EmptyRecordingPolicies : RecordingPolicies
    {
        protected override Func<bool> CoreCondition => () => false;
    }
}
