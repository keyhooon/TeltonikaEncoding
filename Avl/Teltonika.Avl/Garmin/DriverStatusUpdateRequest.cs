// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Garmin.DriverStatusUpdateRequest
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.IO;
using Teltonika.IO;

namespace Teltonika.Avl.Garmin
{
  public class DriverStatusUpdateRequest : GarminFleetManagementPacket
  {
    private const short FLEET_MANAGEMENT_PACKET_ID = 2081;

    public DriverStatusUpdateRequest()
      : base(2081)
    {
    }

    public DriverStatusUpdateRequest(DateTime originationTime, uint uniqueId, uint driverStatus)
      : base(2081)
    {
      using (BitStream bitStream = new BitStream())
      {
        EndianBinaryWriter endianBinaryWriter = new EndianBinaryWriter(EndianBitConverter.Little, bitStream);
        endianBinaryWriter.Write((short) 2081);
        endianBinaryWriter.Write(uniqueId);
        endianBinaryWriter.Write(formatOriginationTime(new DateTime?(originationTime)));
        endianBinaryWriter.Write(driverStatus);
        endianBinaryWriter.Write(0);
        Payload = bitStream.ToByteArray();
      }
    }

    public DateTime? OriginationTime { private set; get; }

    public uint UniqueId { private set; get; }

    public uint DriverStatus { private set; get; }

    public override byte[] Payload
    {
      get => base.Payload;
      set
      {
        base.Payload = value;
        parsePayload();
      }
    }

    protected override void parsePayload()
    {
      using (MemoryStream memoryStream = new MemoryStream(FleetManagementPayload))
      {
        EndianBinaryReader endianBinaryReader = new EndianBinaryReader(EndianBitConverter.Little, memoryStream);
        UniqueId = endianBinaryReader.ReadUInt32();
        OriginationTime = parseOriginationTime(endianBinaryReader.ReadUInt32());
        DriverStatus = endianBinaryReader.ReadUInt32();
      }
    }

    public override string ToString() => string.Format("DriverStatusUpdateRequest: UniqueId = {0}, OriginationTime = {1}, DriverStatus = {2}, {3}", UniqueId, OriginationTime.ToString(), DriverStatus, base.ToString());
  }
}
