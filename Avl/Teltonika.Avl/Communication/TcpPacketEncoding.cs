// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Communication.TcpPacketEncoding
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.IO;
using Teltonika.Avl.Tools;
using Teltonika.IO;

namespace Teltonika.Avl.Communication
{
  public static class TcpPacketEncoding
  {
    private static readonly byte[] Preamble = new byte[4];

    public static void Encode(TcpPacket packet, Stream stream)
    {
      if (packet == null)
        throw new ArgumentNullException(nameof (packet));
      if (stream == null)
        throw new ArgumentNullException(nameof (stream));
      if (packet.Data == null)
        throw new ArgumentNullException("packet.Data");
      EndianBinaryWriter suitableBinaryWriter = stream.CreateSuitableBinaryWriter();
            Encode(packet.Data, suitableBinaryWriter);
    }

    public static byte[] Encode(byte[] data)
    {
      if (data == null)
        throw new ArgumentNullException(nameof (data));
      using (MemoryStream stream = new MemoryStream())
      {
        EndianBinaryWriter suitableBinaryWriter = stream.CreateSuitableBinaryWriter();
                Encode(data, suitableBinaryWriter);
        stream.Position = 0L;
        return new BinaryReader(stream).ReadBytes((int) stream.Length);
      }
    }

    public static byte[] Encode(TcpPacket packet)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
                Encode(packet, memoryStream);
        memoryStream.Position = 0L;
        using (BinaryReader binaryReader = new BinaryReader(memoryStream))
          return binaryReader.ReadBytes((int) memoryStream.Length);
      }
    }

    private static void Encode(byte[] data, EndianBinaryWriter w)
    {
      w.Write(Preamble);
      w.Write(data.Length);
      w.Write(data);
      w.Write(CRC.Default.CalcCrc16(data));
      w.Flush();
    }

    public static TcpPacket Decode(Stream stream)
    {
      EndianBinaryReader endianBinaryReader = stream != null ? stream.CreateSuitableBinaryReader() : throw new ArgumentNullException(nameof (stream));
      int count = endianBinaryReader.ReadInt32() == 0 ? endianBinaryReader.ReadInt32() : throw new ApplicationException("Unable to decode. Missing package prefix.");
      byte[] buffer = endianBinaryReader.ReadBytes(count);
      int num = endianBinaryReader.ReadInt32();
      if (CRC.Default.CalcCrc16(buffer) != num)
        throw new ApplicationException("CRC check fail");
      return new TcpPacket() { Data = buffer, Crc = num };
    }

    public static TcpPacket Decode(byte[] bytes)
    {
      if (bytes == null)
        throw new ArgumentNullException(nameof (bytes));
      using (MemoryStream memoryStream = new MemoryStream(bytes))
        return Decode(memoryStream);
    }

    public static byte[] CreateTcpPacket(this byte[] data) => data != null ? Encode(data) : throw new NullReferenceException("bytes");
  }
}
