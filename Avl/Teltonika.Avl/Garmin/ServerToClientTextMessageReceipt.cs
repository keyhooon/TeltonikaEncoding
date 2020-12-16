// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Garmin.ServerToClientTextMessageReceipt
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.IO;
using Teltonika.IO;

namespace Teltonika.Avl.Garmin
{
  public class ServerToClientTextMessageReceipt : GarminFleetManagementPacket
  {
    private const short FLEET_MANAGEMENT_PACKET_ID = 43;

    public ServerToClientTextMessageReceipt()
      : base(43)
    {
    }

    public ServerToClientTextMessageReceipt(DateTime date, bool result, byte[] messageId)
      : base(43)
    {
      OriginationTime = new DateTime?(date);
      Result = result;
      MessageId = messageId;
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

    public DateTime? OriginationTime { get; private set; }

    public bool Result { get; private set; }

    public byte[] MessageId { get; private set; }

    protected override void parsePayload()
    {
      using (MemoryStream memoryStream = new MemoryStream(FleetManagementPayload))
      {
        EndianBinaryReader endianBinaryReader = new EndianBinaryReader(EndianBitConverter.Little, memoryStream);
        OriginationTime = parseOriginationTime(endianBinaryReader.ReadUInt32());
        byte num1 = endianBinaryReader.ReadByte();
        Result = endianBinaryReader.ReadBoolean();
        int num2 = endianBinaryReader.ReadUInt16();
        MessageId = endianBinaryReader.ReadBytes(num1);
      }
    }
  }
}
