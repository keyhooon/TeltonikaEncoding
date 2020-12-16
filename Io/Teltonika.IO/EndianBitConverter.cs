// Decompiled with JetBrains decompiler
// Type: Teltonika.IO.EndianBitConverter
// Assembly: Teltonika.IO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ACB30B4E-8DEF-4DCF-94D3-7CD8E800CE25
// Assembly location: F:\FLEET\Tavl\Teltonika.IO.dll

using System;
using System.Runtime.InteropServices;

namespace Teltonika.IO
{
  public abstract class EndianBitConverter
  {
    private static readonly LittleEndianBitConverter little = new LittleEndianBitConverter();
    private static readonly BigEndianBitConverter big = new BigEndianBitConverter();

    public static LittleEndianBitConverter Little => little;

    public static BigEndianBitConverter Big => big;

    public long DoubleToInt64Bits(double value) => BitConverter.DoubleToInt64Bits(value);

    public double Int64BitsToDouble(long value) => BitConverter.Int64BitsToDouble(value);

    public int SingleToInt32Bits(float value) => new Int32SingleUnion(value).AsInt32;

    public float Int32BitsToSingle(int value) => new Int32SingleUnion(value).AsSingle;

    public bool ToBoolean(byte[] value, int startIndex)
    {
            CheckByteArgument(value, startIndex, 1);
      return BitConverter.ToBoolean(value, startIndex);
    }

    public char ToChar(byte[] value, int startIndex) => (char) CheckedFromBytes(value, startIndex, 2);

    public double ToDouble(byte[] value, int startIndex) => Int64BitsToDouble(ToInt64(value, startIndex));

    public float ToSingle(byte[] value, int startIndex) => Int32BitsToSingle(ToInt32(value, startIndex));

    public short ToInt16(byte[] value, int startIndex) => (short) CheckedFromBytes(value, startIndex, 2);

    public int ToInt32(byte[] value, int startIndex) => (int) CheckedFromBytes(value, startIndex, 4);

    public long ToInt64(byte[] value, int startIndex) => CheckedFromBytes(value, startIndex, 8);

    public ushort ToUInt16(byte[] value, int startIndex) => (ushort) CheckedFromBytes(value, startIndex, 2);

    public uint ToUInt32(byte[] value, int startIndex) => (uint) CheckedFromBytes(value, startIndex, 4);

    public ulong ToUInt64(byte[] value, int startIndex) => (ulong) CheckedFromBytes(value, startIndex, 8);

    private static void CheckByteArgument(byte[] value, int startIndex, int bytesRequired)
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      if (startIndex < 0 || startIndex > value.Length - bytesRequired)
        throw new ArgumentOutOfRangeException(nameof (startIndex));
    }

    private long CheckedFromBytes(byte[] value, int startIndex, int bytesToConvert)
    {
            CheckByteArgument(value, startIndex, bytesToConvert);
      return FromBytes(value, startIndex, bytesToConvert);
    }

    protected abstract long FromBytes(byte[] value, int startIndex, int bytesToConvert);

    public static string ToString(byte[] value) => BitConverter.ToString(value);

    public static string ToString(byte[] value, int startIndex) => BitConverter.ToString(value, startIndex);

    public static string ToString(byte[] value, int startIndex, int length) => BitConverter.ToString(value, startIndex, length);

    public Decimal ToDecimal(byte[] value, int startIndex)
    {
      int[] bits = new int[4];
      for (int index = 0; index < 4; ++index)
        bits[index] = ToInt32(value, startIndex + index * 4);
      return new Decimal(bits);
    }

    public byte[] GetBytes(Decimal value)
    {
      byte[] buffer = new byte[16];
      int[] bits = Decimal.GetBits(value);
      for (int index = 0; index < 4; ++index)
        CopyBytesImpl(bits[index], 4, buffer, index * 4);
      return buffer;
    }

    public void CopyBytes(Decimal value, byte[] buffer, int index)
    {
      int[] bits = Decimal.GetBits(value);
      for (int index1 = 0; index1 < 4; ++index1)
        CopyBytesImpl(bits[index1], 4, buffer, index1 * 4 + index);
    }

    private byte[] GetBytes(long value, int bytes)
    {
      byte[] buffer = new byte[bytes];
      CopyBytes(value, bytes, buffer, 0);
      return buffer;
    }

    public byte[] GetBytes(bool value) => BitConverter.GetBytes(value);

    public byte[] GetBytes(char value) => GetBytes(value, 2);

    public byte[] GetBytes(double value) => GetBytes(DoubleToInt64Bits(value), 8);

    public byte[] GetBytes(short value) => GetBytes(value, 2);

    public byte[] GetBytes(int value) => GetBytes(value, 4);

    public byte[] GetBytes(long value) => GetBytes(value, 8);

    public byte[] GetBytes(float value) => GetBytes(SingleToInt32Bits(value), 4);

    public byte[] GetBytes(ushort value) => GetBytes(value, 2);

    public byte[] GetBytes(uint value) => GetBytes(value, 4);

    public byte[] GetBytes(ulong value) => GetBytes((long) value, 8);

    private void CopyBytes(long value, int bytes, byte[] buffer, int index)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer), "Byte array must not be null");
      if (buffer.Length < index + bytes)
        throw new ArgumentOutOfRangeException("Buffer not big enough for value");
      CopyBytesImpl(value, bytes, buffer, index);
    }

    protected abstract void CopyBytesImpl(long value, int bytes, byte[] buffer, int index);

    public void CopyBytes(bool value, byte[] buffer, int index) => CopyBytes(value ? 1L : 0L, 1, buffer, index);

    public void CopyBytes(char value, byte[] buffer, int index) => CopyBytes(value, 2, buffer, index);

    public void CopyBytes(double value, byte[] buffer, int index) => CopyBytes(DoubleToInt64Bits(value), 8, buffer, index);

    public void CopyBytes(short value, byte[] buffer, int index) => CopyBytes(value, 2, buffer, index);

    public void CopyBytes(int value, byte[] buffer, int index) => CopyBytes(value, 4, buffer, index);

    public void CopyBytes(long value, byte[] buffer, int index) => CopyBytes(value, 8, buffer, index);

    public void CopyBytes(float value, byte[] buffer, int index) => CopyBytes(SingleToInt32Bits(value), 4, buffer, index);

    public void CopyBytes(ushort value, byte[] buffer, int index) => CopyBytes(value, 2, buffer, index);

    public void CopyBytes(uint value, byte[] buffer, int index) => CopyBytes(value, 4, buffer, index);

    public void CopyBytes(ulong value, byte[] buffer, int index) => CopyBytes((long) value, 8, buffer, index);

    [StructLayout(LayoutKind.Explicit)]
    private struct Int32SingleUnion
    {
      [FieldOffset(0)]
      private int i;
      [FieldOffset(0)]
      private float f;

      internal Int32SingleUnion(int i)
      {
        f = 0.0f;
        this.i = i;
      }

      internal Int32SingleUnion(float f)
      {
        i = 0;
        this.f = f;
      }

      internal int AsInt32 => i;

      internal float AsSingle => f;
    }
  }
}
