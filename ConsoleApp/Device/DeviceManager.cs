using ConsoleApp.Model;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Teltonika.Avl;
using Teltonika.Avl.Communication.FMPro3;

namespace ConsoleApp
{
    public class DeviceManager
    {
        readonly object lock_object;
        public Device Device { get; }

        public DeviceManager(Device device)
        {
            lock_object = new object();
             AvlDataBufferes = new Queue<AvlData>();
            devicePacketFactory = new DevicePacketFactory(this);
            RecordingPolicies = new EmptyRecordingPolicies();
            Task.Factory.StartNew(async() => {
                if (RecordingPolicies.CheckCondition())
                {
                    SetRecord();
                    await Task.Delay(1000);
                };
            });
            Device = device;
        }

        public AvlData CurrentAvlData { get; set; }

        internal Queue<AvlData> AvlDataBufferes { get; set; }
        private DevicePacketFactory devicePacketFactory;
        public void SetRecord()
        {
            lock (lock_object)
            {
                AvlDataBufferes.Enqueue(CurrentAvlData);
            }

        }
        RecordingPolicies RecordingPolicies { get; }
        public DevicePacket GetDevicePacket()
        {
            lock (lock_object)
            {
                return devicePacketFactory.Create();
            }
        }
    }
}
