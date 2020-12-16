// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Tools.CRC
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Teltonika.Avl.Tools
{
  public sealed class CRC
  {
    private readonly int _polynom;
    public static readonly CRC Default = new CRC(40961);
    public static readonly CRC FMPro3 = new CRC(33800);

    public CRC(int polynom) => _polynom = polynom;

    public int CalcCrc16(byte[] buffer) => CalcCrc16(buffer, 0, buffer.Length, _polynom, 0);

    public int CalcCrc16(byte[] buffer, int offset, int length) => CalcCrc16(buffer, offset, length, _polynom, 0);

    public static int CalcCrc16(byte[] buffer, int offset, int bufLen, int polynom, int preset)
    {
      preset &= ushort.MaxValue;
      polynom &= ushort.MaxValue;
      int num1 = preset;
      for (int index1 = 0; index1 < bufLen; ++index1)
      {
        int num2 = buffer[(index1 + offset) % buffer.Length] & byte.MaxValue;
        num1 ^= num2;
        for (int index2 = 0; index2 < 8; ++index2)
        {
          if ((num1 & 1) != 0)
            num1 = num1 >> 1 ^ polynom;
          else
            num1 >>= 1;
        }
      }
      return num1 & ushort.MaxValue;
    }

    public static byte CalculateXor(byte[] buffer) => ((IEnumerable<byte>) buffer).Aggregate<byte, byte>(0, (current, dataByte) => (byte)(current ^ (uint)dataByte));
  }
}
