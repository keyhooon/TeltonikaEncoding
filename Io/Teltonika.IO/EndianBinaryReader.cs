// Decompiled with JetBrains decompiler
// Type: Teltonika.IO.EndianBinaryReader
// Assembly: Teltonika.IO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ACB30B4E-8DEF-4DCF-94D3-7CD8E800CE25
// Assembly location: F:\FLEET\Tavl\Teltonika.IO.dll

using System;
using System.IO;
using System.Text;

namespace Teltonika.IO
{
  public class EndianBinaryReader : IBinaryReader, IDisposable
  {
    private bool disposed;
    private Decoder decoder;
    private byte[] buffer = new byte[16];
    private char[] charBuffer = new char[1];
    private int minBytesPerChar;
    private EndianBitConverter bitConverter;
    private Encoding encoding;
    private Stream stream;

    public EndianBinaryReader(EndianBitConverter bitConverter, Stream stream)
      : this(bitConverter, stream, Encoding.UTF8)
    {
    }

    public EndianBinaryReader(EndianBitConverter bitConverter, Stream stream, Encoding encoding)
    {
      if (bitConverter == null)
        throw new ArgumentNullException(nameof (bitConverter));
      if (stream == null)
        throw new ArgumentNullException(nameof (stream));
      if (encoding == null)
        throw new ArgumentNullException(nameof (encoding));
      this.stream = stream.CanRead ? stream : throw new ArgumentException("Stream isn't writable", nameof (stream));
      this.bitConverter = bitConverter;
      this.encoding = encoding;
      decoder = encoding.GetDecoder();
      minBytesPerChar = 1;
      if (!(encoding is UnicodeEncoding))
        return;
      minBytesPerChar = 2;
    }

    public EndianBitConverter BitConverter => bitConverter;

    public Encoding Encoding => encoding;

    public Stream BaseStream => stream;

    public void Close() => Dispose();

    public void Seek(int offset, SeekOrigin origin)
    {
      CheckDisposed();
      stream.Seek(offset, origin);
    }

    public byte ReadByte()
    {
      ReadInternal(buffer, 1);
      return buffer[0];
    }

    public sbyte ReadSByte()
    {
      ReadInternal(buffer, 1);
      return (sbyte) buffer[0];
    }

    public bool ReadBoolean()
    {
      ReadInternal(buffer, 1);
      return bitConverter.ToBoolean(buffer, 0);
    }

    public short ReadInt16()
    {
      ReadInternal(buffer, 2);
      return bitConverter.ToInt16(buffer, 0);
    }

    public int ReadInt32()
    {
      ReadInternal(buffer, 4);
      return bitConverter.ToInt32(buffer, 0);
    }

    public long ReadInt64()
    {
      ReadInternal(buffer, 8);
      return bitConverter.ToInt64(buffer, 0);
    }

    public ushort ReadUInt16()
    {
      ReadInternal(buffer, 2);
      return bitConverter.ToUInt16(buffer, 0);
    }

    public uint ReadUInt32()
    {
      ReadInternal(buffer, 4);
      return bitConverter.ToUInt32(buffer, 0);
    }

    public ulong ReadUInt64()
    {
      ReadInternal(buffer, 8);
      return bitConverter.ToUInt64(buffer, 0);
    }

    public float ReadSingle()
    {
      ReadInternal(buffer, 4);
      return bitConverter.ToSingle(buffer, 0);
    }

    public double ReadDouble()
    {
      ReadInternal(buffer, 8);
      return bitConverter.ToDouble(buffer, 0);
    }

    public Decimal ReadDecimal()
    {
      ReadInternal(buffer, 16);
      return bitConverter.ToDecimal(buffer, 0);
    }

    public int Read() => Read(charBuffer, 0, 1) == 0 ? -1 : charBuffer[0];

    public int Read(char[] data, int index, int count)
    {
      CheckDisposed();
      if (buffer == null)
        throw new ArgumentNullException("buffer");
      if (index < 0)
        throw new ArgumentOutOfRangeException(nameof (index));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (index));
      if (count + index > data.Length)
        throw new ArgumentException("Not enough space in buffer for specified number of characters starting at specified index");
      int num = 0;
      bool flag = true;
      byte[] numArray = buffer;
      if (numArray.Length < count * minBytesPerChar)
        numArray = new byte[4096];
      while (num < count)
      {
        int size;
        if (flag)
        {
          size = count * minBytesPerChar;
          flag = false;
        }
        else
          size = (count - num - 1) * minBytesPerChar + 1;
        if (size > numArray.Length)
          size = numArray.Length;
        int byteCount = TryReadInternal(numArray, size);
        if (byteCount == 0)
          return num;
        int chars = decoder.GetChars(numArray, 0, byteCount, data, index);
        num += chars;
        index += chars;
      }
      return num;
    }

    public int Read(byte[] buffer, int index, int count)
    {
      CheckDisposed();
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      if (index < 0)
        throw new ArgumentOutOfRangeException(nameof (index));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (index));
      if (count + index > buffer.Length)
        throw new ArgumentException("Not enough space in buffer for specified number of bytes starting at specified index");
      int num1 = 0;
      int num2;
      for (; count > 0; count -= num2)
      {
        num2 = stream.Read(buffer, index, count);
        if (num2 == 0)
          return num1;
        index += num2;
        num1 += num2;
      }
      return num1;
    }

    public byte[] ReadBytes(int count)
    {
      CheckDisposed();
      byte[] buffer = count >= 0 ? new byte[count] : throw new ArgumentOutOfRangeException(nameof (count));
      int num;
      for (int length = 0; length < count; length += num)
      {
        num = stream.Read(buffer, length, count - length);
        if (num == 0)
        {
          byte[] numArray = new byte[length];
          Buffer.BlockCopy(buffer, 0, numArray, 0, length);
          return numArray;
        }
      }
      return buffer;
    }

    public byte[] ReadBytesOrThrow(int count)
    {
      byte[] data = new byte[count];
      ReadInternal(data, count);
      return data;
    }

    public int Read7BitEncodedInt()
    {
      CheckDisposed();
      int num1 = 0;
      for (int index = 0; index < 35; index += 7)
      {
        int num2 = stream.ReadByte();
        if (num2 == -1)
          throw new EndOfStreamException();
        num1 |= (num2 & sbyte.MaxValue) << index;
        if ((num2 & 128) == 0)
          return num1;
      }
      throw new IOException("Invalid 7-bit encoded integer in stream.");
    }

    public int ReadBigEndian7BitEncodedInt()
    {
      CheckDisposed();
      int num1 = 0;
      for (int index = 0; index < 5; ++index)
      {
        int num2 = stream.ReadByte();
        if (num2 == -1)
          throw new EndOfStreamException();
        num1 = num1 << 7 | num2 & sbyte.MaxValue;
        if ((num2 & 128) == 0)
          return num1;
      }
      throw new IOException("Invalid 7-bit encoded integer in stream.");
    }

    public string ReadString()
    {
      int size = Read7BitEncodedInt();
      byte[] numArray = new byte[size];
      ReadInternal(numArray, size);
      return encoding.GetString(numArray, 0, numArray.Length);
    }

    private void CheckDisposed()
    {
      if (disposed)
        throw new ObjectDisposedException(nameof (EndianBinaryReader));
    }

    private void ReadInternal(byte[] data, int size)
    {
      CheckDisposed();
      int num;
      for (int offset = 0; offset < size; offset += num)
      {
        num = stream.Read(data, offset, size - offset);
        if (num == 0)
          throw new EndOfStreamException(string.Format("End of stream reached with {0} byte{1} left to read.", size - offset, size - offset == 1 ? "s" : ""));
      }
    }

    private int TryReadInternal(byte[] data, int size)
    {
      CheckDisposed();
      int offset;
      int num;
      for (offset = 0; offset < size; offset += num)
      {
        num = stream.Read(data, offset, size - offset);
        if (num == 0)
          return offset;
      }
      return offset;
    }

    public void Dispose()
    {
      if (disposed)
        return;
      disposed = true;
      stream.Dispose();
    }
  }
}
