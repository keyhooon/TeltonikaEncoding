// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Garmin.DLEStream
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.IO;
using Teltonika.IO;

namespace Teltonika.Avl.Garmin
{
  public class DLEStream : Stream
  {
    private const byte DLE = 16;

    public DLEStream(Stream stream) => InnerStream = stream;

    public Stream InnerStream { private set; get; }

    public override bool CanRead => InnerStream.CanRead;

    public override bool CanSeek => InnerStream.CanSeek;

    public override bool CanWrite => InnerStream.CanWrite;

    public override void Flush() => InnerStream.Flush();

    public override long Length => InnerStream.Length;

    public override long Position
    {
      get => InnerStream.Position;
      set => InnerStream.Position = value;
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
      if (count == 0)
        return 0;
      int num1 = 0;
      for (int index = offset; index < offset + count; ++index)
      {
        int num2 = ReadByte();
        if (num2 < 0)
          return num1 == 0 ? -1 : num1;
        ++num1;
        buffer[index] = (byte) num2;
      }
      return num1;
    }

    public override long Seek(long offset, SeekOrigin origin) => InnerStream.Seek(offset, origin);

    public override void SetLength(long value) => InnerStream.SetLength(value);

    public override void Write(byte[] buffer, int offset, int count)
    {
      if (offset < 0 || count < 0)
        throw new IndexOutOfRangeException();
      for (int index = offset; index < (buffer.Length - offset < count ? buffer.Length - offset : count); ++index)
        WriteByte(buffer[index]);
    }

    public void Write(short value)
    {
      byte[] bytes = EndianBitConverter.Little.GetBytes(value);
      Write(bytes, 0, bytes.Length);
    }

    public void Write(byte value)
    {
      byte[] bytes = EndianBitConverter.Little.GetBytes(value);
      Write(bytes, 0, bytes.Length);
    }

    public void Write(byte[] buffer) => Write(buffer, 0, buffer.Length);

    public override void WriteByte(byte value)
    {
      if (value == 16)
        InnerStream.WriteByte(16);
      InnerStream.WriteByte(value);
    }

    public override int ReadByte()
    {
      int num = InnerStream.ReadByte();
      return num == 16 ? InnerStream.ReadByte() : num;
    }

    public byte[] ReadBytes(int count)
    {
      byte[] buffer = new byte[count];
      Read(buffer, 0, count);
      return buffer;
    }
  }
}
