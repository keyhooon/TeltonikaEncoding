using System;

namespace ConsoleApp
{
    public abstract class RecordingPolicies: IDisposable
    {
        public RecordingPolicies() 
        {
            InnerRecordingPolicies = null;
        }
        protected abstract Func<bool> CoreCondition { get; }
        public bool  CheckCondition()
        {
            return CoreCondition.Invoke() || InnerRecordingPolicies.CheckCondition();
        }
        private RecordingPolicies InnerRecordingPolicies { get; set; }

        public RecordingPolicies AddRecordingPolicies (RecordingPolicies recordingPolicies)
        {
            InnerRecordingPolicies = recordingPolicies;
            return recordingPolicies;
        }

        protected virtual void Dispose()
        {
            InnerRecordingPolicies.Dispose();
        }

        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
