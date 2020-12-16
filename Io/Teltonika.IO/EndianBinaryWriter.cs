// Decompiled with JetBrains decompiler
// Type: Teltonika.IO.EndianBinaryWriter
// Assembly: Teltonika.IO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ACB30B4E-8DEF-4DCF-94D3-7CD8E800CE25
// Assembly location: F:\FLEET\Tavl\Teltonika.IO.dll

using System;
using System.IO;
using System.Text;

namespace Teltonika.IO
{
  public class EndianBinaryWriter : IBinaryWriter, IDisposable
  {
    private bool disposed;
    private byte[] buffer = new byte[16];
    private char[] charBuffer = new char[1];
    private EndianBitConverter bitConverter;
    private Encoding encoding;
    private Stream stream;

    public EndianBinaryWriter(EndianBitConverter bitConverter, Stream stream)
      : this(bitConverter, stream, Encoding.UTF8)
    {
    }

    public EndianBinaryWriter(EndianBitConverter bitConverter, Stream stream, Encoding encoding)
    {
      if (bitConverter == null)
        throw new ArgumentNullException(nameof (bitConverter));
      if (stream == null)
        throw new ArgumentNullException(nameof (stream));
      if (encoding == null)
        throw new ArgumentNullException(nameof (encoding));
      this.stream = stream.CanWrite ? stream : throw new ArgumentException("Stream isn't writable", nameof (stream));
      this.bitConverter = bitConverter;
      this.encoding = encoding;
    }

    public EndianBitConverter BitConverter => bitConverter;

    public Encoding Encoding => encoding;

    public Stream BaseStream => stream;

    public void Close() => Dispose();

    public void Flush()
    {
      CheckDisposed();
      stream.Flush();
    }

    public void Seek(int offset, SeekOrigin origin)
    {
      CheckDisposed();
      stream.Seek(offset, origin);
    }

    public void Write(bool value)
    {
      bitConverter.CopyBytes(value, buffer, 0);
      WriteInternal(buffer, 1);
    }

    public void Write(short value)
    {
      bitConverter.CopyBytes(value, buffer, 0);
      WriteInternal(buffer, 2);
    }

    public void Write(int value)
    {
      bitConverter.CopyBytes(value, buffer, 0);
      WriteInternal(buffer, 4);
    }

    public void Write(long value)
    {
      bitConverter.CopyBytes(value, buffer, 0);
      WriteInternal(buffer, 8);
    }

    public void Write(ushort value)
    {
      bitConverter.CopyBytes(value, buffer, 0);
      WriteInternal(buffer, 2);
    }

    public void Write(uint value)
    {
      bitConverter.CopyBytes(value, buffer, 0);
      WriteInternal(buffer, 4);
    }

    public void Write(ulong value)
    {
      bitConverter.CopyBytes(value, buffer, 0);
      WriteInternal(buffer, 8);
    }

    public void Write(float value)
    {
      bitConverter.CopyBytes(value, buffer, 0);
      WriteInternal(buffer, 4);
    }

    public void Write(double value)
    {
      bitConverter.CopyBytes(value, buffer, 0);
      WriteInternal(buffer, 8);
    }

    public void Write(Decimal value)
    {
      bitConverter.CopyBytes(value, buffer, 0);
      WriteInternal(buffer, 16);
    }

    public void Write(byte value)
    {
      buffer[0] = value;
      WriteInternal(buffer, 1);
    }

    public void Write(sbyte value)
    {
      buffer[0] = (byte) value;
      WriteInternal(buffer, 1);
    }

    public void Write(byte[] value)
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      WriteInternal(value, value.Length);
    }

    public void Write(byte[] value, int offset, int count)
    {
      CheckDisposed();
      stream.Write(value, offset, count);
    }

    public void Write(char value)
    {
      charBuffer[0] = value;
      Write(charBuffer);
    }

    public void Write(char[] value)
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      CheckDisposed();
      byte[] bytes = Encoding.GetBytes(value, 0, value.Length);
      WriteInternal(bytes, bytes.Length);
    }

    public void Write(string value)
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      CheckDisposed();
      byte[] bytes = Encoding.GetBytes(value);
      Write7BitEncodedInt(bytes.Length);
      WriteInternal(bytes, bytes.Length);
    }

    public void Write7BitEncodedInt(int value)
    {
      CheckDisposed();
      if (value < 0)
        throw new ArgumentOutOfRangeException(nameof (value), "Value must be greater than or equal to 0.");
      int num1 = 0;
      while (value >= 128)
      {
        byte[] buffer = this.buffer;
        int index = num1;
        int num2 = index + 1;
        int num3 = (byte)(value & sbyte.MaxValue | 128);
        buffer[index] = (byte) num3;
        value >>= 7;
        num1 = num2 + 1;
      }
      byte[] buffer1 = buffer;
      int index1 = num1;
      int count = index1 + 1;
      int num4 = (byte)value;
      buffer1[index1] = (byte) num4;
      stream.Write(buffer, 0, count);
    }

    private void CheckDisposed()
    {
      if (disposed)
        throw new ObjectDisposedException(nameof (EndianBinaryWriter));
    }

    private void WriteInternal(byte[] bytes, int length)
    {
      CheckDisposed();
      stream.Write(bytes, 0, length);
    }

    public void Dispose()
    {
      if (disposed)
        return;
      Flush();
      disposed = true;
      stream.Dispose();
    }
  }
}
