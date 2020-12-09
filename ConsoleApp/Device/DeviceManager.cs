using ConsoleApp.Communication;
using ConsoleApp.Model;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Teltonika.Avl;
using Teltonika.Avl.Communication.FMPro3;

namespace ConsoleApp
{
    public class DeviceManager
    {
        private readonly Logger logger;
        readonly object lock_object;
        private VehicleManager VehicleManager { get; }
        public Device Device { get; }
        public Modem Modem { get; }


        public DeviceManager(VehicleManager vehicleManager, Device device)
        {
            logger = LogManager.GetCurrentClassLogger();
            lock_object = new object();
            VehicleManager = vehicleManager;
            Device = device;
            Modem = new Modem(this);
            AvlDataBufferes = new Queue<AvlData>();
            devicePacketFactory = new DevicePacketFactory(this);
            RecordingPolicies = new EmptyRecordingPolicies();
            Task.Factory.StartNew(async() => {
                while (true)
                {
                    if (RecordingPolicies.CheckCondition())
                    {
                        SetRecord();
                        await Task.Delay(1000);
                    };
                }
            });
        }

        internal Queue<AvlData> AvlDataBufferes { get; set; }

        private DevicePacketFactory devicePacketFactory;
        public void SetRecord()
        {
            lock (lock_object)
            {
                var avldata = new AvlData(
                    Teltonika.Avl.Data.AvlDataPriority.Low, 
                    DateTime.Now, 
                    new GpsElement(VehicleManager.Position.Location.Y, VehicleManager.Position.Location.X, 0, (short)VehicleManager.Position.Speed, (short)VehicleManager.Position.Angle, 2));
                AvlDataBufferes.Enqueue(avldata);
                logger.Info($"Device Record : {avldata}");
            }
        }

        public RecordingPolicies RecordingPolicies { get; }
        public DevicePacket GetDevicePacket()
        {
            lock (lock_object)
            {
                return devicePacketFactory.Create();
            }
        }
    }
}
