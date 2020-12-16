// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Garmin.UnicodeSupportResponse
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System.IO;
using Teltonika.IO;

namespace Teltonika.Avl.Garmin
{
  public class UnicodeSupportResponse : GarminFleetManagementPacket
  {
    private const short FLEET_MANAGEMENT_PACKET_ID = 5;

    public UnicodeSupportResponse()
      : base(5)
    {
      using (BitStream bitStream = new BitStream())
      {
        EndianBinaryWriter endianBinaryWriter = new EndianBinaryWriter(EndianBitConverter.Little, bitStream);
        endianBinaryWriter.Write((short) 5);
        endianBinaryWriter.Write(0);
        Payload = bitStream.ToByteArray();
      }
    }
  }
}
