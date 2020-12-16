// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Garmin.DestinationStatusData
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System.IO;
using Teltonika.IO;

namespace Teltonika.Avl.Garmin
{
  public class DestinationStatusData : GarminFleetManagementPacket
  {
    private const short FLEET_MANAGEMENT_PACKET_ID = 529;

    public DestinationStatusData()
      : base(529)
    {
    }

    public DestinationStatusData(uint uniqueId, ushort status, ushort indexInList)
      : base(529)
    {
      UniqueId = uniqueId;
      StopStatus = status;
      IndexInList = indexInList;
    }

    public uint UniqueId { private set; get; }

    public ushort StopStatus { private set; get; }

    public ushort IndexInList { private set; get; }

    public override byte[] Payload
    {
      get => base.Payload;
      set => base.Payload = value;
    }

    protected override void parsePayload()
    {
      using (MemoryStream memoryStream = new MemoryStream(FleetManagementPayload))
      {
        EndianBinaryReader endianBinaryReader = new EndianBinaryReader(EndianBitConverter.Little, memoryStream);
        UniqueId = endianBinaryReader.ReadUInt32();
        StopStatus = endianBinaryReader.ReadUInt16();
        IndexInList = endianBinaryReader.ReadUInt16();
      }
    }

    public override string ToString() => base.ToString() + string.Format(", UID={0}, StopStatus={1}({2}), IndexInList={3}", UniqueId, StopStatus, ((StopStatusEnum)StopStatus).ToString(), IndexInList);

    public enum StopStatusEnum
    {
      REQUESTING_STOP_STATUS = 0,
      MARK_STOP_AS_DONE = 1,
      ACTIVATE_STOP = 2,
      DELETE_STOP = 3,
      MOVE_STOP = 4,
      ACTIVE = 100, // 0x00000064
      DONE = 101, // 0x00000065
      UNREAD_INACTIVE = 102, // 0x00000066
      READ_INACTIVE = 103, // 0x00000067
      DELETED = 104, // 0x00000068
    }
  }
}
