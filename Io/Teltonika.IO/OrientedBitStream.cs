// Decompiled with JetBrains decompiler
// Type: Teltonika.IO.OrientedBitStream
// Assembly: Teltonika.IO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ACB30B4E-8DEF-4DCF-94D3-7CD8E800CE25
// Assembly location: F:\FLEET\Tavl\Teltonika.IO.dll

using System;
using System.Collections;
using System.IO;

namespace Teltonika.IO
{
  public class OrientedBitStream : Stream
  {
    private BitOrder order;
    private Stream innerStream;
    private BitArray bits;
    private int bitIndex;
    private bool disposed;

    public OrientedBitStream(Stream stream, BitOrder order)
    {
      innerStream = stream != null ? stream : throw new ArgumentNullException(nameof (stream));
      this.order = order;
      ResetBitIndex(new byte?());
    }

    public int ReadBit(out bool bit)
    {
      CheckDisposed();
      bit = false;
      if (bits == null || order == BitOrder.LsbMsb && bitIndex >= 8 || order == BitOrder.MsbLsb && bitIndex <= -1)
      {
        int num = innerStream.ReadByte();
        if (num < 0)
          return -1;
        ResetBitIndex(new byte?((byte) num));
      }
      bit = GetBitAndAdvanceIndex();
      return 1;
    }

    public int ReadBits(out long bits, int count)
    {
      CheckDisposed();
      if (count < 0)
        throw new ArgumentException("count must be >= 0", nameof (count));
      bits = 0L;
      int num1 = 0;
      bool bit;
      for (int index = 0; index < count && ReadBit(out bit) != 0; ++index)
      {
        if (order == BitOrder.MsbLsb)
        {
          bits = bits << 1 | (bit ? 1L : 0L);
        }
        else
        {
          if (index != 0 && index % 8 == 0)
            bits <<= 8;
          long num2 = (bit ? 1L : 0L) << index % 8;
          bits |= num2;
        }
        ++num1;
      }
      return num1;
    }

    public void WriteBit(bool bit)
    {
      CheckDisposed();
      if (bits == null || order == BitOrder.LsbMsb && bitIndex > 7 || order == BitOrder.MsbLsb && bitIndex < 0)
      {
        if (bits != null)
          innerStream.WriteByte(BitsToByte());
        ResetBitIndex(new byte?(0));
      }
      SetBitAndAdvanceIndex(bit);
    }

    public override void WriteByte(byte value) => WriteBits(value, 8);

    public void WriteBits(byte value, int count)
    {
      CheckDisposed();
      if (count < 0)
        throw new ArgumentException("must be >= 0", nameof (count));
      BitArray bitArray = new BitArray(new byte[1]
      {
        value
      });
      if (order == BitOrder.MsbLsb)
      {
        for (int index = 7; index >= 7 - count + 1; --index)
          WriteBit(bitArray[index]);
      }
      else
      {
        for (int index = 0; index < count; ++index)
          WriteBit(bitArray[index]);
      }
    }

    public void Write(int value)
    {
      CheckDisposed();
      byte[] bytes = BitConverter.GetBytes(value);
      Array.Reverse(bytes);
      foreach (byte num in bytes)
        WriteByte(num);
    }

    public override int ReadByte()
    {
      CheckDisposed();
      byte num1 = 0;
      for (int index = 0; index <= 7; ++index)
      {
        bool bit;
        if (ReadBit(out bit) == 0)
          return -1;
        if (order == BitOrder.MsbLsb)
        {
          num1 = (byte) (num1 << 1 | (bit ? 1 : 0));
        }
        else
        {
          byte num2 = (byte) ((bit ? 1U : 0U) << index);
          num1 |= num2;
        }
      }
      return num1;
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
      CheckDisposed();
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      int num1 = 0;
      byte[] numArray = new byte[count];
      for (int index = 0; index < count; ++index)
      {
        int num2 = ReadByte();
        if (num2 != -1)
        {
          numArray[index] = (byte) num2;
          ++num1;
        }
        else
          break;
      }
      Buffer.BlockCopy(numArray, 0, buffer, offset, count);
      return num1;
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
      CheckDisposed();
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      for (int index = offset; index < offset + count; ++index)
        WriteByte(buffer[index]);
    }

    public override void Close() => innerStream.Close();

    public override bool CanRead => innerStream.CanRead;

    public override bool CanSeek => false;

    public override bool CanWrite => innerStream.CanWrite;

    public override void Flush()
    {
      if (bits != null)
      {
        while (order == BitOrder.LsbMsb && bitIndex < 8 || order == BitOrder.MsbLsb && bitIndex > -1)
          SetBitAndAdvanceIndex(false);
        innerStream.WriteByte(BitsToByte());
        ResetBitIndex(new byte?());
      }
      innerStream.Flush();
    }

    public override long Length => innerStream.Length * 8L;

    public override long Position
    {
      get => (innerStream.Position > 0L ? innerStream.Position - 1L : 0L) * 8L + bitIndex;
      set
      {
        if (value < 0L)
          throw new ArgumentOutOfRangeException(nameof (value), "must be >= 0");
        if (value != 0L)
          throw new NotImplementedException();
        innerStream.Position = 0L;
        ResetBitIndex(new byte?());
      }
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      if (disposed || !disposing)
        return;
      innerStream.Dispose();
      disposed = true;
    }

    private void CheckDisposed()
    {
      if (disposed)
        throw new ObjectDisposedException("OrderedBitStream");
    }

    private void ResetBitIndex(byte? value)
    {
      if (value.HasValue)
      {
        bits = new BitArray(new byte[1]{ value.Value });
        bitIndex = order == BitOrder.LsbMsb ? 0 : 7;
      }
      else
      {
        bits = null;
        bitIndex = order == BitOrder.LsbMsb ? -1 : 8;
      }
    }

    private void SetBitAndAdvanceIndex(bool bit)
    {
      bits[bitIndex] = bit;
      AdvanceBitIndex();
    }

    private bool GetBitAndAdvanceIndex()
    {
      bool bit = bits[bitIndex];
      AdvanceBitIndex();
      return bit;
    }

    private void AdvanceBitIndex() => bitIndex = order == BitOrder.LsbMsb ? bitIndex + 1 : bitIndex - 1;

    private byte BitsToByte()
    {
      byte num1 = 0;
      if (order == BitOrder.MsbLsb)
      {
        for (int index = 7; index >= 0; --index)
          num1 = (byte) (num1 << 1 | (bits[index] ? 1 : 0));
      }
      else
      {
        for (int index = 0; index < 8; ++index)
        {
          byte num2 = (byte) ((bits[index] ? 1U : 0U) << index);
          num1 |= num2;
        }
      }
      return num1;
    }

    public override void EndWrite(IAsyncResult asyncResult) => throw new NotImplementedException();

    public override int EndRead(IAsyncResult asyncResult) => throw new NotImplementedException();

    public override long Seek(long offset, SeekOrigin origin) => throw new NotImplementedException();

    public override void SetLength(long value) => throw new NotImplementedException();

    public override bool CanTimeout => false;

    public override int ReadTimeout
    {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    public override int WriteTimeout
    {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    public enum BitOrder
    {
      LsbMsb,
      MsbLsb,
    }
  }
}
