using System.Collections.Generic;
using Teltonika.Avl;
using Teltonika.Avl.Communication.FMPro3;
using Teltonika.Avl.Data.FMPro3.Commands.Device;

namespace ConsoleApp
{
    public class DevicePacketFactory
    {
        internal DevicePacketFactory(DeviceManager deviceManager)
        {
            DeviceManager = deviceManager;
        }
        public DeviceManager DeviceManager { get; }

        public DevicePacket Create()
        {
            var record = new RecordCommand(new List<AvlData>());
            while (DeviceManager.AvlDataBufferes.TryDequeue( out var avlData))
            {
                record.Records.Add(avlData);
            }
            return new DevicePacket(DeviceManager.Device.Imei, record);
        }
    }
}
