// Decompiled with JetBrains decompiler
// Type: Teltonika.IO.BitStream
// Assembly: Teltonika.IO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ACB30B4E-8DEF-4DCF-94D3-7CD8E800CE25
// Assembly location: F:\FLEET\Tavl\Teltonika.IO.dll

using System;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace Teltonika.IO
{
  public class BitStream : Stream
  {
    private const int SizeOfByte = 8;
    private const int SizeOfChar = 128;
    private const int SizeOfUInt16 = 16;
    private const int SizeOfUInt32 = 32;
    private const int SizeOfSingle = 32;
    private const int SizeOfUInt64 = 64;
    private const int SizeOfDouble = 64;
    private const uint BitBuffer_SizeOfElement = 32;
    private const int BitBuffer_SizeOfElement_Shift = 5;
    private const uint BitBuffer_SizeOfElement_Mod = 31;
    private static uint[] BitMaskHelperLUT = new uint[33]
    {
      0U,
      1U,
      3U,
      7U,
      15U,
      31U,
      63U,
      (uint) sbyte.MaxValue,
       byte.MaxValue,
      511U,
      1023U,
      2047U,
      4095U,
      8191U,
      16383U,
      (uint) short.MaxValue,
       ushort.MaxValue,
      131071U,
      262143U,
      524287U,
      1048575U,
      2097151U,
      4194303U,
      8388607U,
      16777215U,
      33554431U,
      67108863U,
      134217727U,
      268435455U,
      536870911U,
      1073741823U,
       int.MaxValue,
      uint.MaxValue
    };
    private bool _blnIsOpen = true;
    private uint[] _auiBitBuffer;
    private uint _uiBitBuffer_Length;
    private uint _uiBitBuffer_Index;
    private uint _uiBitBuffer_BitIndex;
    private static IFormatProvider _ifp = CultureInfo.InvariantCulture;

    public override long Length
    {
      get
      {
        if (!_blnIsOpen)
          throw new ObjectDisposedException(nameof (BitStream));
        return _uiBitBuffer_Length;
      }
    }

    public virtual long Length8
    {
      get
      {
        if (!_blnIsOpen)
          throw new ObjectDisposedException(nameof (BitStream));
        return (_uiBitBuffer_Length >> 3) + ((_uiBitBuffer_Length & 7U) > 0U ? 1L : 0L);
      }
    }

    public virtual long Length16
    {
      get
      {
        if (!_blnIsOpen)
          throw new ObjectDisposedException(nameof (BitStream));
        return (_uiBitBuffer_Length >> 4) + ((_uiBitBuffer_Length & 15U) > 0U ? 1L : 0L);
      }
    }

    public virtual long Length32
    {
      get
      {
        if (!_blnIsOpen)
          throw new ObjectDisposedException(nameof (BitStream));
        return (_uiBitBuffer_Length >> 5) + ((_uiBitBuffer_Length & 31U) > 0U ? 1L : 0L);
      }
    }

    public virtual long Length64
    {
      get
      {
        if (!_blnIsOpen)
          throw new ObjectDisposedException(nameof (BitStream));
        return (_uiBitBuffer_Length >> 6) + ((_uiBitBuffer_Length & 63U) > 0U ? 1L : 0L);
      }
    }

    public virtual long Capacity
    {
      get
      {
        if (!_blnIsOpen)
          throw new ObjectDisposedException(nameof (BitStream));
        return (long) _auiBitBuffer.Length << 5;
      }
    }

    public override long Position
    {
      get
      {
        if (!_blnIsOpen)
          throw new ObjectDisposedException(nameof (BitStream));
        return (_uiBitBuffer_Index << 5) + _uiBitBuffer_BitIndex;
      }
      set
      {
        if (!_blnIsOpen)
          throw new ObjectDisposedException(nameof (BitStream));
        uint num = value >= 0L ? (uint) value : throw new ArgumentOutOfRangeException(nameof (value));
        if (_uiBitBuffer_Length < num + 1U)
          throw new ArgumentOutOfRangeException(nameof (value));
        _uiBitBuffer_Index = num >> 5;
        if ((num & 31U) > 0U)
          _uiBitBuffer_BitIndex = num & 31U;
        else
          _uiBitBuffer_BitIndex = 0U;
      }
    }

    public override bool CanRead => _blnIsOpen;

    public override bool CanSeek => false;

    public override bool CanWrite => _blnIsOpen;

    public static bool CanSetLength => false;

    public static bool CanFlush => false;

    public BitStream() => _auiBitBuffer = new uint[1];

    public BitStream(long capacity) => _auiBitBuffer = capacity > 0L ? new uint[(capacity >> 5) + ((capacity & 31L) > 0L ? 1L : 0L)] : throw new ArgumentOutOfRangeException("Negative or zero capacity");

    public BitStream(Stream bits)
      : this()
    {
      byte[] buffer = bits != null ? new byte[bits.Length] : throw new ArgumentNullException(nameof (bits));
      long position = bits.Position;
      bits.Position = 0L;
      bits.Read(buffer, 0, (int) bits.Length);
      bits.Position = position;
      Write(buffer, 0, (int) bits.Length);
    }

    private void Write(ref uint bits, ref uint bitIndex, ref uint count)
    {
      uint num1 = (_uiBitBuffer_Index << 5) + _uiBitBuffer_BitIndex;
      uint num2 = _uiBitBuffer_Length >> 5;
      uint num3 = bitIndex + count;
      int num4 = (int) bitIndex;
      uint num5 = BitMaskHelperLUT[count] << num4;
      bits &= num5;
      uint bits1 = 32U - _uiBitBuffer_BitIndex;
      int num6 = (int) bits1 - (int) num3;
      uint num7 = num6 >= 0 ? bits << num6 : bits >> Math.Abs(num6);
      if (_uiBitBuffer_Length >= num1 + 1U)
      {
        int num8 = (int) bits1 - (int) count;
        _auiBitBuffer[_uiBitBuffer_Index] &= num8 >= 0 ? (uint) (-1 ^ (int)BitMaskHelperLUT[count] << num8) : uint.MaxValue ^ BitMaskHelperLUT[count] >> Math.Abs(num8);
        if ((int) num2 == (int) _uiBitBuffer_Index)
        {
          uint num9 = bits1 < count ? num1 + bits1 : num1 + count;
          if (num9 > _uiBitBuffer_Length)
            UpdateLengthForWrite(num9 - _uiBitBuffer_Length);
        }
      }
      else if (bits1 >= count)
        UpdateLengthForWrite(count);
      else
        UpdateLengthForWrite(bits1);
      _auiBitBuffer[_uiBitBuffer_Index] |= num7;
      if (bits1 >= count)
      {
        UpdateIndicesForWrite(count);
      }
      else
      {
        UpdateIndicesForWrite(bits1);
        uint count1 = count - bits1;
        uint bitIndex1 = bitIndex;
        Write(ref bits, ref bitIndex1, ref count1);
      }
    }

    public virtual void Write(bool bit)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      uint bits = bit ? 1U : 0U;
      uint bitIndex = 0;
      uint count = 1;
      Write(ref bits, ref bitIndex, ref count);
    }

    public virtual void Write(bool[] bits)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bits == null)
        throw new ArgumentNullException(nameof (bits));
      Write(bits, 0, bits.Length);
    }

    public virtual void Write(bool[] bits, int offset, int count)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bits == null)
        throw new ArgumentNullException(nameof (bits));
      if (offset < 0)
        throw new ArgumentOutOfRangeException(nameof (offset));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (count > bits.Length - offset)
        throw new ArgumentException("Invalid count or offset");
      int num = offset + count;
      for (int index = offset; index < num; ++index)
        Write(bits[index]);
    }

    public virtual void Write(byte bits) => Write(bits, 0, 8);

    public virtual void Write(byte bits, int bitIndex, int count)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bitIndex < 0)
        throw new ArgumentOutOfRangeException(nameof (bitIndex));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (count > 8 - bitIndex)
        throw new ArgumentException("Invalid count or bit index byte");
      uint bits1 = bits;
      uint bitIndex1 = (uint) bitIndex;
      uint count1 = (uint) count;
      Write(ref bits1, ref bitIndex1, ref count1);
    }

    public virtual void Write(byte[] bits)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bits == null)
        throw new ArgumentNullException(nameof (bits));
      Write(bits, 0, bits.Length);
    }

    public override void Write(byte[] bits, int offset, int count)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bits == null)
        throw new ArgumentNullException(nameof (bits));
      if (offset < 0)
        throw new ArgumentOutOfRangeException(nameof (offset));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (count > bits.Length - offset)
        throw new ArgumentException("Invalid count or offset");
      int num = offset + count;
      for (int index = offset; index < num; ++index)
        Write(bits[index]);
    }

    public virtual void Write(sbyte bits) => Write(bits, 0, 8);

    public virtual void Write(sbyte bits, int bitIndex, int count) => Write((byte) bits, bitIndex, count);

    public virtual void Write(sbyte[] bits)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bits == null)
        throw new ArgumentNullException(nameof (bits));
      Write(bits, 0, bits.Length);
    }

    public virtual void Write(sbyte[] bits, int offset, int count)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bits == null)
        throw new ArgumentNullException(nameof (bits));
      if (offset < 0)
        throw new ArgumentOutOfRangeException(nameof (offset));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (count > bits.Length - offset)
        throw new ArgumentException("Invalid count or offset");
      byte[] buffer = new byte[count];
      Buffer.BlockCopy(bits, offset, buffer, 0, count);
      Write(buffer, 0, count);
    }

    public override void WriteByte(byte value) => Write(value);

    public virtual void Write(char bits) => Write(bits, 0, 128);

    public virtual void Write(char bits, int bitIndex, int count)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bitIndex < 0)
        throw new ArgumentOutOfRangeException(nameof (bitIndex));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (count > 128 - bitIndex)
        throw new ArgumentException("Invalid count or bit index char");
      uint bits1 = bits;
      uint bitIndex1 = (uint) bitIndex;
      uint count1 = (uint) count;
      Write(ref bits1, ref bitIndex1, ref count1);
    }

    public virtual void Write(char[] bits)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bits == null)
        throw new ArgumentNullException(nameof (bits));
      Write(bits, 0, bits.Length);
    }

    public virtual void Write(char[] bits, int offset, int count)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bits == null)
        throw new ArgumentNullException(nameof (bits));
      if (offset < 0)
        throw new ArgumentOutOfRangeException(nameof (offset));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (count > bits.Length - offset)
        throw new ArgumentException("Invalid count or offset");
      int num = offset + count;
      for (int index = offset; index < num; ++index)
        Write(bits[index]);
    }

    public virtual void Write(ushort bits) => Write(bits, 0, 16);

    public virtual void Write(ushort bits, int bitIndex, int count)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bitIndex < 0)
        throw new ArgumentOutOfRangeException(nameof (bitIndex));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (count > 16 - bitIndex)
        throw new ArgumentException("Invalid count or bit index UInt16");
      uint bits1 = bits;
      uint bitIndex1 = (uint) bitIndex;
      uint count1 = (uint) count;
      Write(ref bits1, ref bitIndex1, ref count1);
    }

    public virtual void Write(ushort[] bits)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bits == null)
        throw new ArgumentNullException(nameof (bits));
      Write(bits, 0, bits.Length);
    }

    public virtual void Write(ushort[] bits, int offset, int count)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bits == null)
        throw new ArgumentNullException(nameof (bits));
      if (offset < 0)
        throw new ArgumentOutOfRangeException(nameof (offset));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (count > bits.Length - offset)
        throw new ArgumentException("Invalid count or offset");
      int num = offset + count;
      for (int index = offset; index < num; ++index)
        Write(bits[index]);
    }

    public virtual void Write(short bits) => Write(bits, 0, 16);

    public virtual void Write(short bits, int bitIndex, int count) => Write((ushort) bits, bitIndex, count);

    public virtual void Write(short[] bits)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bits == null)
        throw new ArgumentNullException(nameof (bits));
      Write(bits, 0, bits.Length);
    }

    public virtual void Write(short[] bits, int offset, int count)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bits == null)
        throw new ArgumentNullException(nameof (bits));
      if (offset < 0)
        throw new ArgumentOutOfRangeException(nameof (offset));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (count > bits.Length - offset)
        throw new ArgumentException("Invalid count or offset");
      ushort[] bits1 = new ushort[count];
      Buffer.BlockCopy(bits, offset << 1, bits1, 0, count << 1);
      Write(bits1, 0, count);
    }

    public virtual void Write(uint bits) => Write(bits, 0, 32);

    public virtual void Write(uint bits, int bitIndex, int count)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bitIndex < 0)
        throw new ArgumentOutOfRangeException(nameof (bitIndex));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      uint bitIndex1 = count <= 32 - bitIndex ? (uint) bitIndex : throw new ArgumentException("Invalid count or bit index UInt32");
      uint count1 = (uint) count;
      Write(ref bits, ref bitIndex1, ref count1);
    }

    public virtual void Write(uint[] bits)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bits == null)
        throw new ArgumentNullException(nameof (bits));
      Write(bits, 0, bits.Length);
    }

    public virtual void Write(uint[] bits, int offset, int count)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bits == null)
        throw new ArgumentNullException(nameof (bits));
      if (offset < 0)
        throw new ArgumentOutOfRangeException(nameof (offset));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (count > bits.Length - offset)
        throw new ArgumentException("Invalid count or offset");
      int num = offset + count;
      for (int index = offset; index < num; ++index)
        Write(bits[index]);
    }

    public virtual void Write(int bits) => Write(bits, 0, 32);

    public virtual void Write(int bits, int bitIndex, int count) => Write((uint) bits, bitIndex, count);

    public virtual void Write(int[] bits)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bits == null)
        throw new ArgumentNullException(nameof (bits));
      Write(bits, 0, bits.Length);
    }

    public virtual void Write(int[] bits, int offset, int count)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bits == null)
        throw new ArgumentNullException(nameof (bits));
      if (offset < 0)
        throw new ArgumentOutOfRangeException(nameof (offset));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (count > bits.Length - offset)
        throw new ArgumentException("Invalid count or offset");
      uint[] bits1 = new uint[count];
      Buffer.BlockCopy(bits, offset << 2, bits1, 0, count << 2);
      Write(bits1, 0, count);
    }

    public virtual void Write(float bits) => Write(bits, 0, 32);

    public virtual void Write(float bits, int bitIndex, int count)
    {
      byte[] bytes = BitConverter.GetBytes(bits);
      Write((uint) (bytes[0] | bytes[1] << 8 | bytes[2] << 16 | bytes[3] << 24), bitIndex, count);
    }

    public virtual void Write(float[] bits)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bits == null)
        throw new ArgumentNullException(nameof (bits));
      Write(bits, 0, bits.Length);
    }

    public virtual void Write(float[] bits, int offset, int count)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bits == null)
        throw new ArgumentNullException(nameof (bits));
      if (offset < 0)
        throw new ArgumentOutOfRangeException(nameof (offset));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (count > bits.Length - offset)
        throw new ArgumentException("Invalid count or offset");
      int num = offset + count;
      for (int index = offset; index < num; ++index)
        Write(bits[index]);
    }

    public virtual void Write(ulong bits) => Write(bits, 0, 64);

    public virtual void Write(ulong bits, int bitIndex, int count)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bitIndex < 0)
        throw new ArgumentOutOfRangeException(nameof (bitIndex));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (count > 64 - bitIndex)
        throw new ArgumentException("Invalid count or bit index UInt64");
      int num1 = bitIndex >> 5 < 1 ? bitIndex : -1;
      int num2 = bitIndex + count > 32 ? (num1 < 0 ? bitIndex - 32 : 0) : -1;
      int num3 = num1 > -1 ? (num1 + count > 32 ? 32 - num1 : count) : 0;
      int num4 = num2 > -1 ? (num3 == 0 ? count : count - num3) : 0;
      if (num3 > 0)
      {
        uint bits1 = (uint) bits;
        uint bitIndex1 = (uint) num1;
        uint count1 = (uint) num3;
        Write(ref bits1, ref bitIndex1, ref count1);
      }
      if (num4 <= 0)
        return;
      uint bits2 = (uint) (bits >> 32);
      uint bitIndex2 = (uint) num2;
      uint count2 = (uint) num4;
      Write(ref bits2, ref bitIndex2, ref count2);
    }

    public virtual void Write(ulong[] bits)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bits == null)
        throw new ArgumentNullException(nameof (bits));
      Write(bits, 0, bits.Length);
    }

    public virtual void Write(ulong[] bits, int offset, int count)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bits == null)
        throw new ArgumentNullException(nameof (bits));
      if (offset < 0)
        throw new ArgumentOutOfRangeException(nameof (offset));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (count > bits.Length - offset)
        throw new ArgumentException("Invalid count or offset");
      int num = offset + count;
      for (int index = offset; index < num; ++index)
        Write(bits[index]);
    }

    public virtual void Write(long bits) => Write(bits, 0, 64);

    public virtual void Write(long bits, int bitIndex, int count) => Write((ulong) bits, bitIndex, count);

    public virtual void Write(long[] bits)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bits == null)
        throw new ArgumentNullException(nameof (bits));
      Write(bits, 0, bits.Length);
    }

    public virtual void Write(long[] bits, int offset, int count)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bits == null)
        throw new ArgumentNullException(nameof (bits));
      if (offset < 0)
        throw new ArgumentOutOfRangeException(nameof (offset));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (count > bits.Length - offset)
        throw new ArgumentException("Invalid count or offset");
      ulong[] bits1 = new ulong[count];
      Buffer.BlockCopy(bits, offset << 4, bits1, 0, count << 4);
      Write(bits1, 0, count);
    }

    public virtual void Write(double bits) => Write(bits, 0, 64);

    public virtual void Write(double bits, int bitIndex, int count)
    {
      byte[] bytes = BitConverter.GetBytes(bits);
      Write((ulong) (bytes[0] | (long) bytes[1] << 8 | (long) bytes[2] << 16 | (long) bytes[3] << 24 | (long) bytes[4] << 32 | (long) bytes[5] << 40 | (long) bytes[6] << 48 | (long) bytes[7] << 56), bitIndex, count);
    }

    public virtual void Write(double[] bits)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bits == null)
        throw new ArgumentNullException(nameof (bits));
      Write(bits, 0, bits.Length);
    }

    public virtual void Write(double[] bits, int offset, int count)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bits == null)
        throw new ArgumentNullException(nameof (bits));
      if (offset < 0)
        throw new ArgumentOutOfRangeException(nameof (offset));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (count > bits.Length - offset)
        throw new ArgumentException("Invalid count or offset");
      int num = offset + count;
      for (int index = offset; index < num; ++index)
        Write(bits[index]);
    }

    public virtual void WriteTo(Stream bits)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bits == null)
        throw new ArgumentNullException(nameof (bits));
      byte[] byteArray = ToByteArray();
      bits.Write(byteArray, 0, byteArray.Length);
    }

    private uint Read(ref uint bits, ref uint bitIndex, ref uint count)
    {
      uint num1 = (_uiBitBuffer_Index << 5) + _uiBitBuffer_BitIndex;
      uint bits1 = count;
      if (_uiBitBuffer_Length < num1 + bits1)
        bits1 = _uiBitBuffer_Length - num1;
      uint num2 = _auiBitBuffer[_uiBitBuffer_Index];
      int num3 = 32 - ((int) _uiBitBuffer_BitIndex + (int) bits1);
      uint num4;
      if (num3 < 0)
      {
        int num5 = Math.Abs(num3);
        uint num6 = BitMaskHelperLUT[bits1] >> num5;
        uint num7 = (num2 & num6) << num5;
        uint count1 = (uint) num5;
        uint bitIndex1 = 0;
        uint bits2 = 0;
        UpdateIndicesForRead(bits1 - count1);
        int num8 = (int) Read(ref bits2, ref bitIndex1, ref count1);
        num4 = num7 | bits2;
      }
      else
      {
        uint num5 = BitMaskHelperLUT[bits1] << num3;
        num4 = (num2 & num5) >> num3;
        UpdateIndicesForRead(bits1);
      }
      bits = num4 << (int) bitIndex;
      return bits1;
    }

    public virtual int Read(out bool bit)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      uint bitIndex = 0;
      uint count = 1;
      uint bits = 0;
      uint num = Read(ref bits, ref bitIndex, ref count);
      bit = Convert.ToBoolean(bits);
      return (int) num;
    }

    public virtual int Read(bool[] bits)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      return bits != null ? Read(bits, 0, bits.Length) : throw new ArgumentNullException(nameof (bits));
    }

    public virtual int Read(bool[] bits, int offset, int count)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bits == null)
        throw new ArgumentNullException(nameof (bits));
      if (offset < 0)
        throw new ArgumentOutOfRangeException(nameof (offset));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (count > bits.Length - offset)
        throw new ArgumentException("Invalid count or offset");
      int num1 = offset + count;
      int num2 = 0;
      for (int index = offset; index < num1; ++index)
        num2 += Read(out bits[index]);
      return num2;
    }

    public virtual int Read(out byte bits) => Read(out bits, 0, 8);

    public virtual int Read(out byte bits, int bitIndex, int count)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bitIndex < 0)
        throw new ArgumentOutOfRangeException(nameof (bitIndex));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      uint bitIndex1 = count <= 8 - bitIndex ? (uint) bitIndex : throw new ArgumentException("Invalid count or bit index Byte");
      uint count1 = (uint) count;
      uint bits1 = 0;
      uint num = Read(ref bits1, ref bitIndex1, ref count1);
      bits = (byte) bits1;
      return (int) num;
    }

    public virtual int Read(byte[] bits)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      return bits != null ? Read(bits, 0, bits.Length) : throw new ArgumentNullException(nameof (bits));
    }

    public override int Read(byte[] bits, int offset, int count)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bits == null)
        throw new ArgumentNullException(nameof (bits));
      if (offset < 0)
        throw new ArgumentOutOfRangeException(nameof (offset));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (count > bits.Length - offset)
        throw new ArgumentException("Invalid count or offset");
      int num1 = offset + count;
      int num2 = 0;
      for (int index = offset; index < num1; ++index)
        num2 += Read(out bits[index]);
      return num2;
    }

    public virtual int Read(out sbyte bits) => Read(out bits, 0, 8);

    public virtual int Read(out sbyte bits, int bitIndex, int count)
    {
      byte bits1 = 0;
      int num = Read(out bits1, bitIndex, count);
      bits = (sbyte) bits1;
      return num;
    }

    public virtual int Read(sbyte[] bits)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      return bits != null ? Read(bits, 0, bits.Length) : throw new ArgumentNullException(nameof (bits));
    }

    public virtual int Read(sbyte[] bits, int offset, int count)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bits == null)
        throw new ArgumentNullException(nameof (bits));
      if (offset < 0)
        throw new ArgumentOutOfRangeException(nameof (offset));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (count > bits.Length - offset)
        throw new ArgumentException("Invalid count or offset");
      int num1 = offset + count;
      int num2 = 0;
      for (int index = offset; index < num1; ++index)
        num2 += Read(out bits[index]);
      return num2;
    }

    public override int ReadByte()
    {
      byte bits;
      return Read(out bits) == 0 ? -1 : bits;
    }

    public virtual byte[] ToByteArray()
    {
      long position = Position;
      Position = 0L;
      byte[] buffer = new byte[Length8];
      Read(buffer, 0, (int) Length8);
      if (Position != position)
        Position = position;
      return buffer;
    }

    public virtual int Read(out char bits) => Read(out bits, 0, 128);

    public virtual int Read(out char bits, int bitIndex, int count)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bitIndex < 0)
        throw new ArgumentOutOfRangeException(nameof (bitIndex));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      uint bitIndex1 = count <= 128 - bitIndex ? (uint) bitIndex : throw new ArgumentException("Invalid count or bit index char");
      uint count1 = (uint) count;
      uint bits1 = 0;
      uint num = Read(ref bits1, ref bitIndex1, ref count1);
      bits = (char) bits1;
      return (int) num;
    }

    public virtual int Read(char[] bits)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      return bits != null ? Read(bits, 0, bits.Length) : throw new ArgumentNullException(nameof (bits));
    }

    public virtual int Read(char[] bits, int offset, int count)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bits == null)
        throw new ArgumentNullException(nameof (bits));
      if (offset < 0)
        throw new ArgumentOutOfRangeException(nameof (offset));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (count > bits.Length - offset)
        throw new ArgumentException("Invalid count or offset");
      int num1 = offset + count;
      int num2 = 0;
      for (int index = offset; index < num1; ++index)
        num2 += Read(out bits[index]);
      return num2;
    }

    public virtual int Read(out ushort bits) => Read(out bits, 0, 16);

    public virtual int Read(out ushort bits, int bitIndex, int count)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bitIndex < 0)
        throw new ArgumentOutOfRangeException(nameof (bitIndex));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      uint bitIndex1 = count <= 16 - bitIndex ? (uint) bitIndex : throw new ArgumentException("Invalid count or bit index UInt16");
      uint count1 = (uint) count;
      uint bits1 = 0;
      uint num = Read(ref bits1, ref bitIndex1, ref count1);
      bits = (ushort) bits1;
      return (int) num;
    }

    public virtual int Read(ushort[] bits)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      return bits != null ? Read(bits, 0, bits.Length) : throw new ArgumentNullException(nameof (bits));
    }

    public virtual int Read(ushort[] bits, int offset, int count)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bits == null)
        throw new ArgumentNullException(nameof (bits));
      if (offset < 0)
        throw new ArgumentOutOfRangeException(nameof (offset));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (count > bits.Length - offset)
        throw new ArgumentException("Invalid count or offset");
      int num1 = offset + count;
      int num2 = 0;
      for (int index = offset; index < num1; ++index)
        num2 += Read(out bits[index]);
      return num2;
    }

    public virtual int Read(out short bits) => Read(out bits, 0, 16);

    public virtual int Read(out short bits, int bitIndex, int count)
    {
      ushort bits1 = 0;
      int num = Read(out bits1, bitIndex, count);
      bits = (short) bits1;
      return num;
    }

    public virtual int Read(short[] bits)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      return bits != null ? Read(bits, 0, bits.Length) : throw new ArgumentNullException(nameof (bits));
    }

    public virtual int Read(short[] bits, int offset, int count)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bits == null)
        throw new ArgumentNullException(nameof (bits));
      if (offset < 0)
        throw new ArgumentOutOfRangeException(nameof (offset));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (count > bits.Length - offset)
        throw new ArgumentException("Invalid count or offset");
      int num1 = offset + count;
      int num2 = 0;
      for (int index = offset; index < num1; ++index)
        num2 += Read(out bits[index]);
      return num2;
    }

    public virtual int Read(out uint bits) => Read(out bits, 0, 32);

    public virtual int Read(out uint bits, int bitIndex, int count)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bitIndex < 0)
        throw new ArgumentOutOfRangeException(nameof (bitIndex));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      uint bitIndex1 = count <= 32 - bitIndex ? (uint) bitIndex : throw new ArgumentException("Invalid count or bit index UInt32");
      uint count1 = (uint) count;
      uint bits1 = 0;
      uint num = Read(ref bits1, ref bitIndex1, ref count1);
      bits = bits1;
      return (int) num;
    }

    public virtual int Read(uint[] bits)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      return bits != null ? Read(bits, 0, bits.Length) : throw new ArgumentNullException(nameof (bits));
    }

    public virtual int Read(uint[] bits, int offset, int count)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bits == null)
        throw new ArgumentNullException(nameof (bits));
      if (offset < 0)
        throw new ArgumentOutOfRangeException(nameof (offset));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (count > bits.Length - offset)
        throw new ArgumentException("Invalid count or offset");
      int num1 = offset + count;
      int num2 = 0;
      for (int index = offset; index < num1; ++index)
        num2 += Read(out bits[index]);
      return num2;
    }

    public virtual int Read(out int bits) => Read(out bits, 0, 32);

    public virtual int Read(out int bits, int bitIndex, int count)
    {
      uint bits1 = 0;
      int num = Read(out bits1, bitIndex, count);
      bits = (int) bits1;
      return num;
    }

    public virtual int Read(int[] bits)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      return bits != null ? Read(bits, 0, bits.Length) : throw new ArgumentNullException(nameof (bits));
    }

    public virtual int Read(int[] bits, int offset, int count)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bits == null)
        throw new ArgumentNullException(nameof (bits));
      if (offset < 0)
        throw new ArgumentOutOfRangeException(nameof (offset));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (count > bits.Length - offset)
        throw new ArgumentException("Invalid count or offset");
      int num1 = offset + count;
      int num2 = 0;
      for (int index = offset; index < num1; ++index)
        num2 += Read(out bits[index]);
      return num2;
    }

    public virtual int Read(out float bits) => Read(out bits, 0, 32);

    public virtual int Read(out float bits, int bitIndex, int count)
    {
      int bits1 = 0;
      int num = Read(out bits1, bitIndex, count);
      bits = BitConverter.ToSingle(BitConverter.GetBytes(bits1), 0);
      return num;
    }

    public virtual int Read(float[] bits)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      return bits != null ? Read(bits, 0, bits.Length) : throw new ArgumentNullException(nameof (bits));
    }

    public virtual int Read(float[] bits, int offset, int count)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bits == null)
        throw new ArgumentNullException(nameof (bits));
      if (offset < 0)
        throw new ArgumentOutOfRangeException(nameof (offset));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (count > bits.Length - offset)
        throw new ArgumentException("Invalid count or offset");
      int num1 = offset + count;
      int num2 = 0;
      for (int index = offset; index < num1; ++index)
        num2 += Read(out bits[index]);
      return num2;
    }

    public virtual int Read(out ulong bits) => Read(out bits, 0, 64);

    public virtual int Read(out ulong bits, int bitIndex, int count)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bitIndex < 0)
        throw new ArgumentOutOfRangeException(nameof (bitIndex));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (count > 64 - bitIndex)
        throw new ArgumentException("Argument_InvalidCountOrBitIndex_UInt64");
      int num1 = bitIndex >> 5 < 1 ? bitIndex : -1;
      int num2 = bitIndex + count > 32 ? (num1 < 0 ? bitIndex - 32 : 0) : -1;
      int num3 = num1 > -1 ? (num1 + count > 32 ? 32 - num1 : count) : 0;
      int num4 = num2 > -1 ? (num3 == 0 ? count : count - num3) : 0;
      uint num5 = 0;
      uint bits1 = 0;
      uint bits2 = 0;
      if (num3 > 0)
      {
        uint bitIndex1 = (uint) num1;
        uint count1 = (uint) num3;
        num5 = Read(ref bits1, ref bitIndex1, ref count1);
      }
      if (num4 > 0)
      {
        uint bitIndex1 = (uint) num2;
        uint count1 = (uint) num4;
        num5 += Read(ref bits2, ref bitIndex1, ref count1);
      }
      bits = (ulong) bits2 << 32 | bits1;
      return (int) num5;
    }

    public virtual int Read(ulong[] bits)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      return bits != null ? Read(bits, 0, bits.Length) : throw new ArgumentNullException(nameof (bits));
    }

    public virtual int Read(ulong[] bits, int offset, int count)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bits == null)
        throw new ArgumentNullException(nameof (bits));
      if (offset < 0)
        throw new ArgumentOutOfRangeException(nameof (offset));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (count > bits.Length - offset)
        throw new ArgumentException("Invalid count or offset");
      int num1 = offset + count;
      int num2 = 0;
      for (int index = offset; index < num1; ++index)
        num2 += Read(out bits[index]);
      return num2;
    }

    public virtual int Read(out long bits) => Read(out bits, 0, 64);

    public virtual int Read(out long bits, int bitIndex, int count)
    {
      ulong bits1 = 0;
      int num = Read(out bits1, bitIndex, count);
      bits = (long) bits1;
      return num;
    }

    public virtual int Read(long[] bits)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      return bits != null ? Read(bits, 0, bits.Length) : throw new ArgumentNullException(nameof (bits));
    }

    public virtual int Read(long[] bits, int offset, int count)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bits == null)
        throw new ArgumentNullException(nameof (bits));
      if (offset < 0)
        throw new ArgumentOutOfRangeException(nameof (offset));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (count > bits.Length - offset)
        throw new ArgumentException("Invalid count or offset");
      int num1 = offset + count;
      int num2 = 0;
      for (int index = offset; index < num1; ++index)
        num2 += Read(out bits[index]);
      return num2;
    }

    public virtual int Read(out double bits) => Read(out bits, 0, 64);

    public virtual int Read(out double bits, int bitIndex, int count)
    {
      ulong bits1 = 0;
      int num = Read(out bits1, bitIndex, count);
      bits = BitConverter.ToDouble(BitConverter.GetBytes(bits1), 0);
      return num;
    }

    public virtual int Read(double[] bits)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      return bits != null ? Read(bits, 0, bits.Length) : throw new ArgumentNullException(nameof (bits));
    }

    public virtual int Read(double[] bits, int offset, int count)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bits == null)
        throw new ArgumentNullException(nameof (bits));
      if (offset < 0)
        throw new ArgumentOutOfRangeException(nameof (offset));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (count > bits.Length - offset)
        throw new ArgumentException("Invalid count or offset");
      int num1 = offset + count;
      int num2 = 0;
      for (int index = offset; index < num1; ++index)
        num2 += Read(out bits[index]);
      return num2;
    }

    public virtual BitStream And(BitStream bits)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bits == null)
        throw new ArgumentNullException(nameof (bits));
      if (bits.Length != _uiBitBuffer_Length)
        throw new ArgumentException("Argument_DifferentBitStreamLengths");
      BitStream bitStream = new BitStream(_uiBitBuffer_Length);
      uint num1 = _uiBitBuffer_Length >> 5;
      uint num2;
      for (num2 = 0U; num2 < num1; ++num2)
        bitStream._auiBitBuffer[num2] = _auiBitBuffer[num2] & bits._auiBitBuffer[num2];
      if ((_uiBitBuffer_Length & 31U) > 0U)
      {
        uint num3 = (uint) (-1 << 32 - ((int) _uiBitBuffer_Length & 31));
        bitStream._auiBitBuffer[num2] = _auiBitBuffer[num2] & bits._auiBitBuffer[num2] & num3;
      }
      return bitStream;
    }

    public virtual BitStream Or(BitStream bits)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bits == null)
        throw new ArgumentNullException(nameof (bits));
      if (bits.Length != _uiBitBuffer_Length)
        throw new ArgumentException("Argument_DifferentBitStreamLengths");
      BitStream bitStream = new BitStream(_uiBitBuffer_Length);
      uint num1 = _uiBitBuffer_Length >> 5;
      uint num2;
      for (num2 = 0U; num2 < num1; ++num2)
        bitStream._auiBitBuffer[num2] = _auiBitBuffer[num2] | bits._auiBitBuffer[num2];
      if ((_uiBitBuffer_Length & 31U) > 0U)
      {
        uint num3 = (uint) (-1 << 32 - ((int) _uiBitBuffer_Length & 31));
        bitStream._auiBitBuffer[num2] = _auiBitBuffer[num2] | bits._auiBitBuffer[num2] & num3;
      }
      return bitStream;
    }

    public virtual BitStream Xor(BitStream bits)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      if (bits == null)
        throw new ArgumentNullException(nameof (bits));
      if (bits.Length != _uiBitBuffer_Length)
        throw new ArgumentException("Argument_DifferentBitStreamLengths");
      BitStream bitStream = new BitStream(_uiBitBuffer_Length);
      uint num1 = _uiBitBuffer_Length >> 5;
      uint num2;
      for (num2 = 0U; num2 < num1; ++num2)
        bitStream._auiBitBuffer[num2] = _auiBitBuffer[num2] ^ bits._auiBitBuffer[num2];
      if ((_uiBitBuffer_Length & 31U) > 0U)
      {
        uint num3 = (uint) (-1 << 32 - ((int) _uiBitBuffer_Length & 31));
        bitStream._auiBitBuffer[num2] = _auiBitBuffer[num2] ^ bits._auiBitBuffer[num2] & num3;
      }
      return bitStream;
    }

    public virtual BitStream Not()
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      BitStream bitStream = new BitStream(_uiBitBuffer_Length);
      uint num1 = _uiBitBuffer_Length >> 5;
      uint num2;
      for (num2 = 0U; num2 < num1; ++num2)
        bitStream._auiBitBuffer[num2] = ~_auiBitBuffer[num2];
      if ((_uiBitBuffer_Length & 31U) > 0U)
      {
        uint num3 = (uint) (-1 << 32 - ((int) _uiBitBuffer_Length & 31));
        bitStream._auiBitBuffer[num2] = ~_auiBitBuffer[num2] & num3;
      }
      return bitStream;
    }

    public virtual BitStream ShiftLeft(long count)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      BitStream bitStream = Copy();
      uint num = (uint) count;
      uint length = (uint) bitStream.Length;
      if (num >= length)
      {
        bitStream.Position = 0L;
        for (uint index = 0; index < length; ++index)
          bitStream.Write(false);
      }
      else
      {
        bool bit = false;
        for (uint index = 0; index < length - num; ++index)
        {
          bitStream.Position = num + index;
          bitStream.Read(out bit);
          bitStream.Position = index;
          bitStream.Write(bit);
        }
        for (uint index = length - num; index < length; ++index)
          bitStream.Write(false);
      }
      bitStream.Position = 0L;
      return bitStream;
    }

    public virtual BitStream ShiftRight(long count)
    {
      if (!_blnIsOpen)
        throw new ObjectDisposedException(nameof (BitStream));
      BitStream bitStream = Copy();
      uint num = (uint) count;
      uint length = (uint) bitStream.Length;
      if (num >= length)
      {
        bitStream.Position = 0L;
        for (uint index = 0; index < length; ++index)
          bitStream.Write(false);
      }
      else
      {
        bool bit = false;
        for (uint index = 0; index < length - num; ++index)
        {
          bitStream.Position = index;
          bitStream.Read(out bit);
          bitStream.Position = index + num;
          bitStream.Write(bit);
        }
        bitStream.Position = 0L;
        for (uint index = 0; index < num; ++index)
          bitStream.Write(false);
      }
      bitStream.Position = 0L;
      return bitStream;
    }

    public override string ToString()
    {
      uint num1 = _uiBitBuffer_Length >> 5;
      uint num2 = 1;
      StringBuilder stringBuilder = new StringBuilder((int) _uiBitBuffer_Length);
      uint num3;
      for (num3 = 0U; num3 < num1; ++num3)
      {
        stringBuilder.Append("[" + num3.ToString(_ifp) + "]:{");
        for (int index = 31; index >= 0; --index)
        {
          uint num4 = num2 << index;
          if (((int) _auiBitBuffer[num3] & (int) num4) == (int) num4)
            stringBuilder.Append('1');
          else
            stringBuilder.Append('0');
        }
        stringBuilder.Append("}\r\n");
      }
      if ((_uiBitBuffer_Length & 31U) > 0U)
      {
        stringBuilder.Append("[" + num3.ToString(_ifp) + "]:{");
        int num4 = 32 - ((int) _uiBitBuffer_Length & 31);
        for (int index = 31; index >= num4; --index)
        {
          uint num5 = num2 << index;
          if (((int) _auiBitBuffer[num3] & (int) num5) == (int) num5)
            stringBuilder.Append('1');
          else
            stringBuilder.Append('0');
        }
        for (int index = num4 - 1; index >= 0; --index)
          stringBuilder.Append('.');
        stringBuilder.Append("}\r\n");
      }
      return stringBuilder.ToString();
    }

    public static string ToString(bool bit) => "Boolean{" + (bit ? 1 : 0) + "}";

    public static string ToString(byte bits)
    {
      StringBuilder stringBuilder = new StringBuilder(8);
      uint num1 = 1;
      stringBuilder.Append("Byte{");
      for (int index = 7; index >= 0; --index)
      {
        uint num2 = num1 << index;
        if ((bits & (int) num2) == (int) num2)
          stringBuilder.Append('1');
        else
          stringBuilder.Append('0');
      }
      stringBuilder.Append("}");
      return stringBuilder.ToString();
    }

    public static string ToString(sbyte bits)
    {
      byte num1 = (byte) bits;
      StringBuilder stringBuilder = new StringBuilder(8);
      uint num2 = 1;
      stringBuilder.Append("SByte{");
      for (int index = 7; index >= 0; --index)
      {
        uint num3 = num2 << index;
        if ((num1 & (int) num3) == (int) num3)
          stringBuilder.Append('1');
        else
          stringBuilder.Append('0');
      }
      stringBuilder.Append("}");
      return stringBuilder.ToString();
    }

    public static string ToString(char bits)
    {
      StringBuilder stringBuilder = new StringBuilder(16);
      uint num1 = 1;
      stringBuilder.Append("Char{");
      for (int index = 15; index >= 0; --index)
      {
        uint num2 = num1 << index;
        if ((bits & (int) num2) == (int) num2)
          stringBuilder.Append('1');
        else
          stringBuilder.Append('0');
      }
      stringBuilder.Append("}");
      return stringBuilder.ToString();
    }

    public static string ToString(ushort bits)
    {
      short num1 = (short) bits;
      StringBuilder stringBuilder = new StringBuilder(16);
      uint num2 = 1;
      stringBuilder.Append("UInt16{");
      for (int index = 15; index >= 0; --index)
      {
        uint num3 = num2 << index;
        if ((num1 & num3) == num3)
          stringBuilder.Append('1');
        else
          stringBuilder.Append('0');
      }
      stringBuilder.Append("}");
      return stringBuilder.ToString();
    }

    public static string ToString(short bits)
    {
      StringBuilder stringBuilder = new StringBuilder(16);
      uint num1 = 1;
      stringBuilder.Append("Int16{");
      for (int index = 15; index >= 0; --index)
      {
        uint num2 = num1 << index;
        if ((bits & num2) == num2)
          stringBuilder.Append('1');
        else
          stringBuilder.Append('0');
      }
      stringBuilder.Append("}");
      return stringBuilder.ToString();
    }

    public static string ToString(uint bits)
    {
      StringBuilder stringBuilder = new StringBuilder(32);
      uint num1 = 1;
      stringBuilder.Append("UInt32{");
      for (int index = 31; index >= 0; --index)
      {
        uint num2 = num1 << index;
        if (((int) bits & (int) num2) == (int) num2)
          stringBuilder.Append('1');
        else
          stringBuilder.Append('0');
      }
      stringBuilder.Append("}");
      return stringBuilder.ToString();
    }

    public static string ToString(int bits)
    {
      uint num1 = (uint) bits;
      StringBuilder stringBuilder = new StringBuilder(32);
      uint num2 = 1;
      stringBuilder.Append("Int32{");
      for (int index = 31; index >= 0; --index)
      {
        uint num3 = num2 << index;
        if (((int) num1 & (int) num3) == (int) num3)
          stringBuilder.Append('1');
        else
          stringBuilder.Append('0');
      }
      stringBuilder.Append("}");
      return stringBuilder.ToString();
    }

    public static string ToString(ulong bits)
    {
      StringBuilder stringBuilder = new StringBuilder(64);
      ulong num1 = 1;
      stringBuilder.Append("UInt64{");
      for (int index = 63; index >= 0; --index)
      {
        ulong num2 = num1 << index;
        if (((long) bits & (long) num2) == (long) num2)
          stringBuilder.Append('1');
        else
          stringBuilder.Append('0');
      }
      stringBuilder.Append("}");
      return stringBuilder.ToString();
    }

    public static string ToString(long bits)
    {
      ulong num1 = (ulong) bits;
      StringBuilder stringBuilder = new StringBuilder(64);
      ulong num2 = 1;
      stringBuilder.Append("Int64{");
      for (int index = 63; index >= 0; --index)
      {
        ulong num3 = num2 << index;
        if (((long) num1 & (long) num3) == (long) num3)
          stringBuilder.Append('1');
        else
          stringBuilder.Append('0');
      }
      stringBuilder.Append("}");
      return stringBuilder.ToString();
    }

    public static string ToString(float bits)
    {
      byte[] bytes = BitConverter.GetBytes(bits);
      uint num1 = (uint) (bytes[0] | bytes[1] << 8 | bytes[2] << 16 | bytes[3] << 24);
      StringBuilder stringBuilder = new StringBuilder(32);
      uint num2 = 1;
      stringBuilder.Append("Single{");
      for (int index = 31; index >= 0; --index)
      {
        uint num3 = num2 << index;
        if (((int) num1 & (int) num3) == (int) num3)
          stringBuilder.Append('1');
        else
          stringBuilder.Append('0');
      }
      stringBuilder.Append("}");
      return stringBuilder.ToString();
    }

    public static string ToString(double bits)
    {
      byte[] bytes = BitConverter.GetBytes(bits);
      ulong num1 = (ulong) (bytes[0] | (long) bytes[1] << 8 | (long) bytes[2] << 16 | (long) bytes[3] << 24 | (long) bytes[4] << 32 | (long) bytes[5] << 40 | (long) bytes[6] << 48 | (long) bytes[7] << 56);
      StringBuilder stringBuilder = new StringBuilder(64);
      ulong num2 = 1;
      stringBuilder.Append("Double{");
      for (int index = 63; index >= 0; --index)
      {
        ulong num3 = num2 << index;
        if (((long) num1 & (long) num3) == (long) num3)
          stringBuilder.Append('1');
        else
          stringBuilder.Append('0');
      }
      stringBuilder.Append("}");
      return stringBuilder.ToString();
    }

    private void UpdateLengthForWrite(uint bits) => _uiBitBuffer_Length += bits;

    private void UpdateIndicesForWrite(uint bits)
    {
      _uiBitBuffer_BitIndex += bits;
      if (_uiBitBuffer_BitIndex == 32U)
      {
        ++_uiBitBuffer_Index;
        _uiBitBuffer_BitIndex = 0U;
        if (_auiBitBuffer.Length != _uiBitBuffer_Length >> 5)
          return;
        _auiBitBuffer = ReDimPreserve(_auiBitBuffer, (uint) (_auiBitBuffer.Length << 1));
      }
      else if (_uiBitBuffer_BitIndex > 32U)
        throw new InvalidOperationException("InvalidOperation_BitIndexGreaterThan32");
    }

    private void UpdateIndicesForRead(uint bits)
    {
      _uiBitBuffer_BitIndex += bits;
      if (_uiBitBuffer_BitIndex == 32U)
      {
        ++_uiBitBuffer_Index;
        _uiBitBuffer_BitIndex = 0U;
      }
      else if (_uiBitBuffer_BitIndex > 32U)
        throw new InvalidOperationException("InvalidOperation_BitIndexGreaterThan32");
    }

    private static uint[] ReDimPreserve(uint[] buffer, uint newLength)
    {
      uint[] numArray = new uint[newLength];
      uint length = (uint) buffer.Length;
      if (length < newLength)
        Buffer.BlockCopy(buffer, 0, numArray, 0, (int) length << 2);
      else
        Buffer.BlockCopy(buffer, 0, numArray, 0, (int) newLength << 2);
      buffer = null;
      return numArray;
    }

    public override void Close()
    {
      _blnIsOpen = false;
      _uiBitBuffer_Index = 0U;
      _uiBitBuffer_BitIndex = 0U;
    }

    public virtual uint[] GetBuffer() => _auiBitBuffer;

    public virtual BitStream Copy()
    {
      BitStream bitStream = new BitStream(Length);
      Buffer.BlockCopy(_auiBitBuffer, 0, bitStream._auiBitBuffer, 0, bitStream._auiBitBuffer.Length << 2);
      bitStream._uiBitBuffer_Length = _uiBitBuffer_Length;
      return bitStream;
    }

    public override IAsyncResult BeginRead(
      byte[] buffer,
      int offset,
      int count,
      AsyncCallback callback,
      object state)
    {
      throw new NotSupportedException();
    }

    public override IAsyncResult BeginWrite(
      byte[] buffer,
      int offset,
      int count,
      AsyncCallback callback,
      object state)
    {
      throw new NotSupportedException();
    }

    public override int EndRead(IAsyncResult asyncResult) => throw new NotSupportedException();

    public override void EndWrite(IAsyncResult asyncResult) => throw new NotSupportedException();

    public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

    public override void SetLength(long value) => throw new NotSupportedException();

    public override void Flush() => throw new NotSupportedException();

    public static implicit operator BitStream(MemoryStream bits) => bits != null ? new BitStream(bits) : throw new ArgumentNullException(nameof (bits));

    public static implicit operator MemoryStream(BitStream bits) => bits != null ? new MemoryStream(bits.ToByteArray()) : throw new ArgumentNullException(nameof (bits));

    public static implicit operator BitStream(FileStream bits) => bits != null ? new BitStream(bits) : throw new ArgumentNullException(nameof (bits));

    public static implicit operator BitStream(BufferedStream bits) => bits != null ? new BitStream(bits) : throw new ArgumentNullException(nameof (bits));

    public static implicit operator BufferedStream(BitStream bits) => bits != null ? new BufferedStream((MemoryStream)bits) : throw new ArgumentNullException(nameof (bits));

    public static implicit operator BitStream(NetworkStream bits) => bits != null ? new BitStream(bits) : throw new ArgumentNullException(nameof (bits));

    public static implicit operator BitStream(CryptoStream bits) => bits != null ? new BitStream(bits) : throw new ArgumentNullException(nameof (bits));
  }
}
