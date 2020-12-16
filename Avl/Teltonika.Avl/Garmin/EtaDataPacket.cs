// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Garmin.EtaDataPacket
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.IO;
using Teltonika.IO;

namespace Teltonika.Avl.Garmin
{
  public class EtaDataPacket : GarminFleetManagementPacket
  {
    private const short FLEET_MANAGEMENT_PACKET_ID = 513;

    public EtaDataPacket()
      : base(513)
    {
    }

    public EtaDataPacket(
      DateTime eta,
      uint uniqueId,
      uint distanceToDestination,
      double x,
      double y)
      : base(513)
    {
      ETA = new DateTime?(eta);
      UniqueId = uniqueId;
      DistanceToDestination = distanceToDestination;
      X = x;
      Y = y;
      formatPayload();
    }

    public override byte[] Payload
    {
      get => base.Payload;
      set
      {
        base.Payload = value;
        parsePayload();
      }
    }

    public DateTime? ETA { private set; get; }

    public uint UniqueId { get; private set; }

    public uint DistanceToDestination { get; private set; }

    public double X { get; private set; }

    public double Y { get; private set; }

    protected override void formatPayload() => throw new NotImplementedException();

    protected override void parsePayload()
    {
      using (MemoryStream memoryStream = new MemoryStream(FleetManagementPayload))
      {
        EndianBinaryReader endianBinaryReader = new EndianBinaryReader(EndianBitConverter.Little, memoryStream);
        UniqueId = endianBinaryReader.ReadUInt32();
        ETA = parseOriginationTime(endianBinaryReader.ReadUInt32());
        DistanceToDestination = endianBinaryReader.ReadUInt32();
        Y = convertSemicirclesToDegrees(endianBinaryReader.ReadInt32());
        X = convertSemicirclesToDegrees(endianBinaryReader.ReadInt32());
      }
    }

    public override string ToString() => base.ToString() + string.Format(", ETA={0}, UniqueId={1}, Distance={2}, X={3}, Y={4}, ", ETA, UniqueId, DistanceToDestination.ToString("X"), X, Y);
  }
}
