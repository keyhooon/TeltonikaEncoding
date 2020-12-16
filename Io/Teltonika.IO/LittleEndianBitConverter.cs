// Decompiled with JetBrains decompiler
// Type: Teltonika.IO.LittleEndianBitConverter
// Assembly: Teltonika.IO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ACB30B4E-8DEF-4DCF-94D3-7CD8E800CE25
// Assembly location: F:\FLEET\Tavl\Teltonika.IO.dll

namespace Teltonika.IO
{
  public sealed class LittleEndianBitConverter : EndianBitConverter
  {
    protected override void CopyBytesImpl(long value, int bytes, byte[] buffer, int index)
    {
      for (int index1 = 0; index1 < bytes; ++index1)
      {
        buffer[index1 + index] = (byte) ((ulong) value & byte.MaxValue);
        value >>= 8;
      }
    }

    protected override long FromBytes(byte[] buffer, int startIndex, int bytesToConvert)
    {
      long num = 0;
      for (int index = 0; index < bytesToConvert; ++index)
        num = num << 8 | buffer[startIndex + bytesToConvert - 1 - index];
      return num;
    }
  }
}
