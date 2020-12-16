using ConsoleApp.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp.Devices
{
    public class RecordingTaskManager
    {
        public RecordingTaskManager()
        {
            registeredDictionary = new Dictionary<Type, Func<RecordCondition, RecordingPolicies>>();
        }
        private Dictionary<Type, Func<RecordCondition, RecordingPolicies>> registeredDictionary;
        public void RegisterRecordingPolicies(Func<RecordCondition, RecordingPolicies> recordingPoliciesConstructor, Type recordConditionType)
        {
            registeredDictionary.Add(recordConditionType, recordingPoliciesConstructor);
        }
        public Dictionary<Scenario, Task> CreateRecordingTaskList (DeviceManager deviceManager, CancellationToken cancellationToken)
        {
            Dictionary<Scenario, Task> tasksList = new Dictionary<Scenario, Task>();
            foreach (var scenario in deviceManager.Device.Scenarios)
            {
                RecordingPolicies recordingPolicies = new EmptyRecordingPolicies();
                scenario.RecordConditions.ForEach((o) => recordingPolicies.AddRecordingPolicies(registeredDictionary[o.GetType()](o)));
                tasksList.Add(scenario, Task.Factory.StartNew(async (o) => {
                    (RecordingPolicies, int) arg = ((RecordingPolicies, int))o;
                    while (true)
                    {
                        if (arg.Item1.CheckCondition())
                        {
                            deviceManager.SetRecord(arg.Item2);
                            await Task.Delay(1000);
                        };
                    }
                }, (recordingPolicies, deviceManager.Device.Scenarios.IndexOf(scenario)), cancellationToken));
            }
            return tasksList;
        }
    }
}
