// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.IoProperty
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Teltonika.Avl
{
  [StructLayout(LayoutKind.Explicit)]
  public struct IoProperty : IEquatable<IoProperty>
  {
    [FieldOffset(0)]
    private readonly int _id;
    [FieldOffset(4)]
    private readonly byte _size;
    [FieldOffset(5)]
    private readonly byte _status;
    [FieldOffset(6)]
    private readonly IoPropertyType _type;
    [FieldOffset(8)]
    private unsafe fixed byte _buffer[16];
    [FieldOffset(8)]
    private readonly long _valueLow;
    [FieldOffset(16)]
    private readonly long _valueHigh;
    public static readonly IoProperty Default;

    private IoProperty(int id, byte size, byte status, long value)
    {
      _id = id;
      _size = size;
      _status = status;
      _type = IoPropertyType.Number;
      _valueHigh = 0L;
      _valueLow = value;
    }

    private unsafe IoProperty(int id, byte status, byte[] buffer)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      if (16 < buffer.Length)
        throw new ArgumentException("The buffer size exceedes maximum buffer size of 16.");
      _id = id;
      _status = status;
      _valueHigh = 0L;
      _valueLow = 0L;
      _type = IoPropertyType.Buffer;
      _size = (byte) buffer.Length;
      fixed (byte* numPtr = _buffer)
        Marshal.Copy(buffer, 0, new IntPtr((void*) numPtr), buffer.Length);
    }

    public static IoProperty Create(int id, bool value) => new IoProperty(id, 1, 0, value ? 1L : 0L);

    public static IoProperty Create(int id, sbyte value) => new IoProperty(id, 1, 0, value);

    public static IoProperty Create(int id, byte value) => new IoProperty(id, 1, 0, value);

    public static IoProperty Create(int id, short value) => new IoProperty(id, 2, 0, value);

    public static IoProperty Create(int id, ushort value) => new IoProperty(id, 2, 0, value);

    public static IoProperty Create(int id, int value) => new IoProperty(id, 4, 0, value);

    public static IoProperty Create(int id, uint value) => new IoProperty(id, 4, 0, value);

    public static IoProperty Create(int id, long value) => new IoProperty(id, 8, 0, value);

    public static IoProperty Create(int id, ulong value) => new IoProperty(id, 8, 0, (long) value);

    public static IoProperty Create(int id, ulong value, byte status) => new IoProperty(id, 8, status, (long) value);

    public static IoProperty Create(int id, ulong value, bool status) => new IoProperty(id, 8, status ? (byte) 1 : (byte) 0, (long) value);

    public static IoProperty Create(int id, byte[] value) => new IoProperty(id, 0, value);

    public bool IsDefault => _id == 0 && _size == 0 && _type == IoPropertyType.Number && _valueLow == 0L;

    public int Id => _id;

    public int Size => _size;

    public IoPropertyType Type => _type;

    public byte Status => _status;

    public unsafe string ToHexString()
    {
      fixed (byte* numPtr = _buffer)
      {
        if ((IntPtr) numPtr == IntPtr.Zero)
          return string.Empty;
        byte size = _size;
        int capacity = size << 1;
        StringBuilder stringBuilder = new StringBuilder(capacity)
        {
          Length = capacity
        };
        int index1 = 0;
        int index2 = capacity - 2;
        while (index1 < size)
        {
          byte num = numPtr[index1];
          char ch1 = EncodingHelper.Alphabet[num >> 4];
          char ch2 = EncodingHelper.Alphabet[num & 15];
          stringBuilder[index2] = ch1;
          stringBuilder[index2 + 1] = ch2;
          ++index1;
          index2 -= 2;
        }
        return stringBuilder.ToString();
      }
    }

    public static explicit operator bool(IoProperty data) => data._valueLow != 0L;

    public static explicit operator sbyte(IoProperty data) => (sbyte) data._valueLow;

    public static explicit operator byte(IoProperty data) => (byte) data._valueLow;

    public static explicit operator short(IoProperty data) => (short) data._valueLow;

    public static explicit operator ushort(IoProperty data) => (ushort) data._valueLow;

    public static explicit operator int(IoProperty data) => (int) data._valueLow;

    public static explicit operator uint(IoProperty data) => (uint) data._valueLow;

    public static explicit operator long(IoProperty data) => data._valueLow;

    public static explicit operator ulong(IoProperty data) => (ulong) data._valueLow;

    public static unsafe explicit operator byte[](IoProperty data)
    {
      byte[] destination = new byte[data._size];
      Marshal.Copy(new IntPtr((void*) data._buffer), destination, 0, destination.Length);
      return destination;
    }

    public bool ToBoolean() => _valueLow != 0L;

    public sbyte ToSByte() => (sbyte) _valueLow;

    public byte ToByte() => (byte) _valueLow;

    public short ToInt16() => (short) _valueLow;

    public ushort ToUInt16() => (ushort) _valueLow;

    public int ToInt32() => (int) _valueLow;

    public uint ToUInt32() => (uint) _valueLow;

    public long ToInt64() => _valueLow;

    public ulong ToUInt64() => (ulong) _valueLow;

    public unsafe byte[] ToBuffer()
    {
      byte[] destination = new byte[_size];
      fixed (byte* numPtr = _buffer)
        Marshal.Copy(new IntPtr((void*) numPtr), destination, 0, destination.Length);
      return destination;
    }

    public override bool Equals(object obj) => !ReferenceEquals(null, obj) && (obj is IoProperty other && Equals(other));

    public bool Equals(IoProperty other) => _id == other._id && _size == other._size && (_status == other._status && _type == other._type) && _valueLow == other._valueLow && _valueHigh == other._valueHigh;

    public override int GetHashCode() => ((int) ((IoPropertyType) (((_id * 397 ^ _size.GetHashCode()) * 397 ^ _status.GetHashCode()) * 397) ^ _type) * 397 ^ _valueLow.GetHashCode()) * 397 ^ _valueHigh.GetHashCode();

    public static bool operator ==(IoProperty left, IoProperty right) => left.Equals(right);

    public static bool operator !=(IoProperty left, IoProperty right) => !left.Equals(right);

    public override string ToString() => string.Format("{0}={1}", Id, ToHexString());
  }
}
