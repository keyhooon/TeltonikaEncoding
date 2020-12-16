// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Communication.UdpPacketEncoding
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.IO;
using Teltonika.IO;
using Teltonika.Logging;

namespace Teltonika.Avl.Communication
{
  public static class UdpPacketEncoding
  {
    private static readonly ILog Log = LogManager.GetLogger(typeof (UdpPacketEncoding));

    public static void Encode(UdpPacket packet, Stream stream)
    {
      if (packet == null)
        throw new ArgumentNullException(nameof (packet));
      EndianBinaryWriter endianBinaryWriter = stream != null ? stream.CreateSuitableBinaryWriter() : throw new ArgumentNullException(nameof (stream));
      switch (packet.Type)
      {
        case UdpPacketType.DataReqAck:
        case UdpPacketType.DataNotReqAck:
          UdpData payload1 = packet.Payload as UdpData;
          if (payload1 != null)
          {
            int num = 6 + ImeiEncoding.Encoding.GetByteCount(payload1.Imei) + payload1.Data.Length;
            endianBinaryWriter.Write((short) num);
            endianBinaryWriter.Write(packet.Id);
            endianBinaryWriter.Write((byte) packet.Type);
            endianBinaryWriter.Write(payload1.AvlPacketId);
            endianBinaryWriter.Write(ImeiEncoding.Encode(payload1.Imei));
            endianBinaryWriter.Write(payload1.Data);
            break;
          }
          UdpResponse payload2 = packet.Payload as UdpResponse;
          if (payload2 == null)
            throw new InvalidOperationException("Payload");
          endianBinaryWriter.Write((short) 5);
          endianBinaryWriter.Write(packet.Id);
          endianBinaryWriter.Write((byte) packet.Type);
          endianBinaryWriter.Write(payload2.AvlPacketId);
          endianBinaryWriter.Write(payload2.AcceptedNum);
          break;
        case UdpPacketType.Ack:
          if (packet.Payload as UdpResponse == null)
            throw new InvalidOperationException("UdpAck");
          endianBinaryWriter.Write((short) 3);
          endianBinaryWriter.Write(packet.Id);
          endianBinaryWriter.Write((byte) packet.Type);
          break;
        default:
          throw new ApplicationException("Not supported packet type");
      }
      endianBinaryWriter.Flush();
    }

    public static byte[] Encode(UdpPacket packet)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
                Encode(packet, memoryStream);
        memoryStream.Position = 0L;
        using (BinaryReader binaryReader = new BinaryReader(memoryStream))
          return binaryReader.ReadBytes((int) memoryStream.Length);
      }
    }

    public static UdpPacket Decode(Stream stream)
    {
      EndianBinaryReader endianBinaryReader = stream != null ? stream.CreateSuitableBinaryReader() : throw new ArgumentNullException(nameof (stream));
      short num1 = endianBinaryReader.ReadInt16();
      short num2 = endianBinaryReader.ReadInt16();
      UdpPacketType udpPacketType = (UdpPacketType) endianBinaryReader.ReadByte();
      byte num3 = endianBinaryReader.ReadByte();
      switch (udpPacketType)
      {
        case UdpPacketType.DataReqAck:
        case UdpPacketType.DataNotReqAck:
          if (num1 == 5)
          {
            byte num4 = endianBinaryReader.ReadByte();
            UdpPacket udpPacket1 = new UdpPacket();
            udpPacket1.Id = num2;
            udpPacket1.Type = udpPacketType;
            UdpPacket udpPacket2 = udpPacket1;
            UdpResponse udpResponse1 = new UdpResponse();
            udpResponse1.AvlPacketId = num3;
            udpResponse1.AcceptedNum = num4;
            UdpResponse udpResponse2 = udpResponse1;
            udpPacket2.Payload = udpResponse2;
            return udpPacket1;
          }
          string s = ImeiEncoding.Decode(stream);
          int count = num1 - 2 - 1 - 1 - (2 + ImeiEncoding.Encoding.GetByteCount(s));
          byte[] numArray = endianBinaryReader.ReadBytes(count);
          UdpPacket udpPacket3 = new UdpPacket();
          udpPacket3.Id = num2;
          udpPacket3.Type = udpPacketType;
          UdpPacket udpPacket4 = udpPacket3;
          UdpData udpData1 = new UdpData();
          udpData1.AvlPacketId = num3;
          udpData1.Imei = s;
          udpData1.Data = numArray;
          UdpData udpData2 = udpData1;
          udpPacket4.Payload = udpData2;
          return udpPacket3;
        case UdpPacketType.Ack:
          endianBinaryReader.ReadByte();
          return new UdpPacket()
          {
            Id = num2,
            Type = udpPacketType,
            Payload = null
          };
        default:
          throw new ApplicationException("Not supported packet type");
      }
    }

    public static UdpPacket Decode(byte[] bytes)
    {
      if (bytes == null)
        throw new ArgumentNullException(nameof (bytes));
      if (Log.IsDebugEnabled)
                Log.DebugFormat("Decode(0x{0})", (object) bytes.ToHexString());
      using (MemoryStream memoryStream = new MemoryStream(bytes))
        return Decode(memoryStream);
    }
  }
}
