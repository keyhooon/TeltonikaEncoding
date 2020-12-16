// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Codec`2
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.IO;
using Teltonika.IO;

namespace Teltonika.Avl
{
  public abstract class Codec<TData, TEncoding> : ICodec
    where TData : class
    where TEncoding : IEncoding<TData>, new()
  {
    public abstract byte Id { get; }

    public Type DataType => typeof (TData);

    public virtual void Encode(object data, Stream stream)
    {
      if (data == null)
        throw new ArgumentNullException(nameof (data));
      IBitWriter writer = stream != null ? stream.CreateSuitableBitWriter() : throw new ArgumentNullException(nameof (stream));
      writer.Write(Id);
      if (!(data is TData data1))
        throw new CodecException("Expected data type " + DataType);
      new TEncoding().Encode(data1, writer);
      writer.Flush();
    }

    public virtual byte[] Encode(object data)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        Encode(data, memoryStream);
        return memoryStream.ToArray();
      }
    }

    public virtual object Decode(Stream stream)
    {
      IBitReader reader = stream != null ? stream.CreateSuitableBitReader() : throw new ArgumentNullException(nameof (stream));
      byte num = reader.ReadByte();
      if (Id != num)
        throw new CodecException("Incorrect data codec ID " + num + "; expected " + Id);
      try
      {
        return new TEncoding().Decode(reader);
      }
      catch (Exception ex)
      {
        throw new CodecException("Unable to decode", ex);
      }
    }

    public virtual object Decode(byte[] bytes)
    {
      if (bytes == null)
        throw new ArgumentNullException(nameof (bytes));
      if (bytes.Length == 0)
        throw new InvalidOperationException("bytes.Length = 0");
      using (MemoryStream memoryStream = new MemoryStream(bytes))
        return Decode(memoryStream);
    }
  }
}
