using ConsoleApp.Communication;
using ConsoleApp.Devices;
using ConsoleApp.Model;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Teltonika.Avl;
using Teltonika.Avl.Communication.FMPro3;

namespace ConsoleApp
{
    public class DeviceManager
    {
        private readonly Logger logger;
        private readonly ModemManager ModemManager;
        private readonly object lock_object;
        private readonly CancellationTokenSource cancellationsource;


        private List<IoProperty> IoProperties;
        public Device Device { get; }
        private Dictionary<Scenario, Task> ScenarioTaskDictionary;

        public DeviceManager(Device device, RecordingTaskManager recordingTaskManager)
        {
            Device = device;
            logger = LogManager.GetCurrentClassLogger();
            ModemManager = new ModemManager(Device.Modem, this);
            lock_object = new object();
            cancellationsource = new CancellationTokenSource();

            AvlDataBufferes = new Queue<AvlData>();
            devicePacketFactory = new DevicePacketFactory(this);

            ScenarioTaskDictionary = recordingTaskManager.CreateRecordingTaskList(this, cancellationsource.Token);
        }

        internal Queue<AvlData> AvlDataBufferes { get; set; }

        private DevicePacketFactory devicePacketFactory;
        public void SetRecord(int scenarioId)
        {
            Debug.Assert(Device != null);
            lock (lock_object)
            {
                var avldata = new AvlData(
                    Teltonika.Avl.Data.AvlDataPriority.Low, 
                    DateTime.Now, 
                    new GpsElement(Device.InstalledOnVehicle.Position.Location.Y, Device.InstalledOnVehicle.Position.Location.X, 0, (short)Device.InstalledOnVehicle.Position.Speed, (short)Device.InstalledOnVehicle.Position.Angle, 2),
                    new IoElement(0, Device.InstalledOnVehicle.IoPropertiesList.Where(o=>Device.Scenarios[scenarioId].ObservedIoProperties.Any(a=>a.Id == o.Id))));
                AvlDataBufferes.Enqueue(avldata);
            }
        }

        public DevicePacket GetDevicePacket()
        {
            lock (lock_object)
            {
                return devicePacketFactory.Create();
            }
        }
    }
}
