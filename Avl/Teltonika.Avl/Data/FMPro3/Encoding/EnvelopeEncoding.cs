// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.FMPro3.Encoding.EnvelopeEncoding
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using Teltonika.Avl.Tools;
using Teltonika.IO;

namespace Teltonika.Avl.Data.FMPro3.Encoding
{
  public sealed class EnvelopeEncoding : IEncoding<byte[]>
  {
    private const int MaxDataSize = 1020;
    public static readonly IEncoding<byte[]> Instance = new EnvelopeEncoding();

    public void Encode(byte[] data, IBitWriter writer)
    {
      if (data == null)
        throw new ArgumentNullException(nameof (data));
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      if (data.Length > 1020)
        throw new ArgumentOutOfRangeException(nameof (data), "The data buffer must not be larger than 1020 bytes.");
      writer.Write((ushort) data.Length);
      writer.Write(data);
      writer.Write((ushort) CRC.FMPro3.CalcCrc16(data));
    }

    public byte[] Decode(IBitReader reader)
    {
      ushort num1 = reader != null ? reader.ReadUInt16() : throw new ArgumentNullException(nameof (reader));
      byte[] buffer = num1 <= 1020 ? reader.ReadBytes(num1) : throw new InvalidOperationException(string.Format("The packet length must not be larger than 1020 bytes (it is: " + num1 + ")."));
      ushort num2 = reader.ReadUInt16();
      int num3 = CRC.FMPro3.CalcCrc16(buffer);
      if (num2 != num3)
        throw new InvalidOperationException(string.Format("The CRC check failed expected: 0x{0:X4}, actual 0x{1:X4}", num2, num3));
      return buffer;
    }
  }
}
