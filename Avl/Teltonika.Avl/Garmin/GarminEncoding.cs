// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Garmin.GarminEncoding
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Teltonika.IO;

namespace Teltonika.Avl.Garmin
{
  public sealed class GarminEncoding
  {
    private const byte DLE = 16;
    private const byte ETX = 3;
    public static readonly GarminEncoding Instance = new GarminEncoding();
    private static readonly Dictionary<byte, Type> garminPacketIdToType = new Dictionary<byte, Type>();
    private static readonly Dictionary<short, Type> garminFleetmanagementPacketIdToType = new Dictionary<short, Type>();

    static GarminEncoding()
    {
      foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
      {
        if (type.IsSubclassOf(typeof (GarminPacket)))
        {
          try
          {
            GarminPacket garminPacket = (GarminPacket) type.GetConstructor(new Type[0]).Invoke(new object[0]);
            if (garminPacket.Id == 161)
            {
              if (garminPacket is GarminFleetManagementPacket managementPacket)
                                garminFleetmanagementPacketIdToType[managementPacket.FleetManagementPacketId] = type;
            }
            else
                            garminPacketIdToType[garminPacket.Id] = type;
          }
          catch
          {
          }
        }
      }
    }

    public byte[] Encode(GarminPacket packet)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        Encode(packet, memoryStream);
        memoryStream.Position = 0L;
        return new BinaryReader(memoryStream).ReadBytes((int) memoryStream.Length);
      }
    }

    public void Encode(GarminPacket packet, Stream stream)
    {
      if (packet == null)
        throw new ArgumentNullException(nameof (packet));
      EndianBinaryWriter endianBinaryWriter = stream != null ? new EndianBinaryWriter(EndianBitConverter.Little, stream) : throw new ArgumentNullException(nameof (stream));
      endianBinaryWriter.Write((byte) 16);
      endianBinaryWriter.Write(packet.Id);
      GarminFleetManagementPacket managementPacket = packet as GarminFleetManagementPacket;
      DLEStream dleStream = new DLEStream(stream);
      if (managementPacket != null)
      {
        dleStream.WriteByte((byte) packet.Payload.Length);
        dleStream.Write(packet.Payload);
      }
      else
      {
        dleStream.WriteByte((byte) packet.Payload.Length);
        dleStream.Write(packet.Payload);
      }
      byte num = CalcChecksum(packet);
      dleStream.WriteByte(num);
      endianBinaryWriter.Write((byte) 16);
      endianBinaryWriter.Write((byte) 3);
    }

    public GarminPacket Decode(Stream stream)
    {
      EndianBinaryReader endianBinaryReader = stream != null ? new EndianBinaryReader(EndianBitConverter.Little, stream) : throw new ArgumentNullException(nameof (stream));
      byte num = endianBinaryReader.ReadByte() == 16 ? endianBinaryReader.ReadByte() : throw new ApplicationException("DLE expected");
      DLEStream dleStream = new DLEStream(stream);
      int count = dleStream.ReadByte();
      GarminPacket packet;
      if (num == 161)
      {
        byte[] numArray = dleStream.ReadBytes(count);
        short int16 = BitConverter.ToInt16(((IEnumerable<byte>) numArray).Take(2).ToArray(), 0);
        packet = !garminFleetmanagementPacketIdToType.ContainsKey(int16) ? new GarminFleetManagementPacket(int16) : garminFleetmanagementPacketIdToType[int16].GetConstructor(new Type[0]).Invoke(new object[0]) as GarminPacket;
        packet.Payload = numArray;
      }
      else
      {
        byte[] numArray = dleStream.ReadBytes(count);
        packet = !garminPacketIdToType.ContainsKey(num) ? new GarminPacket(num) : garminPacketIdToType[num].GetConstructor(new Type[0]).Invoke(new object[0]) as GarminPacket;
        packet.Payload = numArray;
      }
      if (dleStream.ReadByte() != CalcChecksum(packet))
        throw new ApplicationException("Invalid checksum");
      if (endianBinaryReader.ReadByte() != 16)
        throw new ApplicationException("DLE expected");
      if (3 != endianBinaryReader.ReadByte())
        throw new ApplicationException("ETX expected");
      return packet;
    }

    public GarminPacket Decode(byte[] bytes)
    {
      using (MemoryStream memoryStream = new MemoryStream(bytes))
        return Decode(memoryStream);
    }

    private static byte CalcChecksum(GarminPacket packet)
    {
      byte num = (byte) (packet.Id + (uint) packet.Payload.Length);
      int index = 0;
      for (int length = packet.Payload.Length; index < length; ++index)
        num += packet.Payload[index];
      return (byte) ((num ^ byte.MaxValue) + 1);
    }
  }
}
