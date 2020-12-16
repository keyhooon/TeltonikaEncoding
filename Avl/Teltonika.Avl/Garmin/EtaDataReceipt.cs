﻿// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Garmin.EtaDataReceipt
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System.IO;
using Teltonika.IO;

namespace Teltonika.Avl.Garmin
{
  public class EtaDataReceipt : GarminFleetManagementPacket
  {
    private const short FLEET_MANAGEMENT_PACKET_ID = 514;

    public EtaDataReceipt()
      : base(514)
    {
    }

    public EtaDataReceipt(uint uniqueId)
      : base(514)
    {
      UniqueId = uniqueId;
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

    public uint UniqueId { get; private set; }

    protected override void formatPayload()
    {
      using (BitStream bitStream = new BitStream())
      {
        EndianBinaryWriter endianBinaryWriter = new EndianBinaryWriter(EndianBitConverter.Little, bitStream);
        endianBinaryWriter.Write((short) 514);
        endianBinaryWriter.Write(UniqueId);
        Payload = bitStream.ToByteArray();
      }
    }

    protected override void parsePayload()
    {
      using (MemoryStream memoryStream = new MemoryStream(FleetManagementPayload))
        UniqueId = new EndianBinaryReader(EndianBitConverter.Little, memoryStream).ReadUInt32();
    }

    public override string ToString() => "EtaDatapacket: UniqueId=" + UniqueId + ", " + base.ToString();
  }
}
