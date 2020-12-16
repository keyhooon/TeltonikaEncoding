// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Garmin.DriverIdUpdateResponse
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System.IO;
using Teltonika.IO;

namespace Teltonika.Avl.Garmin
{
  public class DriverIdUpdateResponse : GarminFleetManagementPacket
  {
    private const short FLEET_MANAGEMENT_PACKET_ID = 2066;

    public DriverIdUpdateResponse()
      : base(2066)
    {
    }

    public DriverIdUpdateResponse(int uniqueId, bool? result = null)
      : base(2066)
    {
      using (BitStream bitStream = new BitStream())
      {
        EndianBinaryWriter endianBinaryWriter = new EndianBinaryWriter(EndianBitConverter.Little, bitStream);
        endianBinaryWriter.Write((short) 2066);
        endianBinaryWriter.Write(uniqueId);
        if (result.HasValue)
          endianBinaryWriter.Write(result.Value);
        endianBinaryWriter.Write(new byte[3]);
        endianBinaryWriter.Write(0);
        Payload = bitStream.ToByteArray();
      }
    }

    public int UniqueId { private set; get; }

    public bool? Result { private set; get; }

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
      using (MemoryStream memoryStream = new MemoryStream(Payload))
      {
        EndianBinaryReader endianBinaryReader = new EndianBinaryReader(EndianBitConverter.Little, memoryStream);
        endianBinaryReader.ReadBytes(2);
        UniqueId = endianBinaryReader.ReadInt32();
        Result = new bool?(endianBinaryReader.ReadBoolean());
      }
    }

    public override string ToString() => string.Format("DriverIdUpdateResponse: UniqueId = {0}, Result = {1}, {2} ", UniqueId, Result.ToString(), base.ToString());
  }
}
