// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Garmin.ClientToServerTextMessage
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.IO;
using System.Text;
using Teltonika.IO;

namespace Teltonika.Avl.Garmin
{
  public class ClientToServerTextMessage : GarminFleetManagementPacket
  {
    private const short FLEET_MANAGEMENT_PACKET_ID = 36;

    public ClientToServerTextMessage()
      : base(36)
    {
    }

    public ClientToServerTextMessage(DateTime originationTime, uint uniqueId, string text)
      : base(36)
    {
      OriginationTime = new DateTime?(originationTime);
      UniqueId = uniqueId;
      Text = text;
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

    public uint UniqueId { get; private set; }

    public string Text { get; private set; }

    protected override void parsePayload()
    {
      using (MemoryStream memoryStream = new MemoryStream(FleetManagementPayload))
      {
        EndianBinaryReader endianBinaryReader = new EndianBinaryReader(EndianBitConverter.Little, memoryStream);
        OriginationTime = parseOriginationTime(endianBinaryReader.ReadUInt32());
        UniqueId = endianBinaryReader.ReadUInt32();
        byte[] numArray = new byte[endianBinaryReader.BaseStream.Length - endianBinaryReader.BaseStream.Position - 1L];
        endianBinaryReader.Read(numArray, 0, numArray.Length);
        Text = Encoding.UTF8.GetString(numArray);
      }
    }

    public override string ToString() => base.ToString() + string.Format(" Text={0}, UID={1}", Text, UniqueId);
  }
}
