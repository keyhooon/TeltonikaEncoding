// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Garmin.DriverIdUpdateRequest
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.IO;
using System.Text;
using Teltonika.IO;

namespace Teltonika.Avl.Garmin
{
  public class DriverIdUpdateRequest : GarminFleetManagementPacket
  {
    private const short FLEET_MANAGEMENT_PACKET_ID = 2065;

    public DriverIdUpdateRequest()
      : base(2065)
    {
    }

    public DriverIdUpdateRequest(DateTime originationTime, int uniqueId, string driverId)
      : base(2065)
    {
      byte[] bytes;
      try
      {
        bytes = new UTF8Encoding().GetBytes(driverId);
        if (bytes.Length > 49)
          throw new ArgumentException("Max text size is 49 bytes: " + bytes.Length);
      }
      catch (EncoderFallbackException ex)
      {
        throw new SystemException("Server doesn't support UTF-8", ex);
      }
      using (BitStream bitStream = new BitStream())
      {
        EndianBinaryWriter endianBinaryWriter = new EndianBinaryWriter(EndianBitConverter.Little, bitStream);
        endianBinaryWriter.Write((short) 2065);
        endianBinaryWriter.Write(uniqueId);
        endianBinaryWriter.Write(formatOriginationTime(new DateTime?(originationTime)));
        endianBinaryWriter.Write(bytes);
        endianBinaryWriter.Write(0);
        Payload = bitStream.ToByteArray();
      }
    }

    public DateTime? OriginationTime { private set; get; }

    public int UniqueId { private set; get; }

    public string DriverId { private set; get; }

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
        UniqueId = endianBinaryReader.ReadInt32();
        OriginationTime = parseOriginationTime(endianBinaryReader.ReadUInt32());
        byte[] numArray = new byte[endianBinaryReader.BaseStream.Length - endianBinaryReader.BaseStream.Position - 1L];
        endianBinaryReader.Read(numArray, 0, numArray.Length);
        DriverId = Encoding.UTF8.GetString(numArray);
      }
    }

    public override string ToString() => "DriverIdUpdateRequest: " + base.ToString();
  }
}
