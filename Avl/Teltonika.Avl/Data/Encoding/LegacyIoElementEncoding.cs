// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.Encoding.LegacyIoElementEncoding
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.Collections.Generic;
using System.IO;
using Teltonika.IO;

namespace Teltonika.Avl.Data.Encoding
{
  public sealed class LegacyIoElementEncoding : IEncoding<IoElement>
  {
    private const byte BooleanType = 0;
    private const byte IntegerType = 1;
    private const byte TypeLength = 3;
    private static LegacyIoElementEncoding _instance;

    public static LegacyIoElementEncoding Instance => _instance ?? (_instance = new LegacyIoElementEncoding());

    public void Encode(IoElement obj, IBitWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      int count = obj.Count;
      writer.Write((byte) count);
      int id = 0;
      while (count > 0)
      {
        IoProperty ioProperty = obj[id];
        if (!ioProperty.IsDefault)
        {
          writer.WriteBit(true);
          switch (ioProperty.Size)
          {
            case 1:
              writer.WriteBits(0, 3);
              writer.WriteBit((bool) ioProperty);
              break;
            case 4:
              writer.WriteBits(1, 3);
              writer.Write((int) ioProperty);
              break;
            default:
              throw new NotSupportedException("Not supported type.");
          }
          --count;
        }
        else
          writer.WriteBit(false);
        ++id;
      }
    }

    public IoElement Decode(IBitReader reader)
    {
      byte num1 = reader != null ? reader.ReadByte() : throw new ArgumentNullException(nameof (reader));
      int num2 = 0;
      int id = 0;
      List<IoProperty> properties = new List<IoProperty>(num1);
      while (num2 < num1)
      {
        bool bit;
        if (reader.ReadBit(out bit) != 1)
          throw new EndOfStreamException();
        if (bit)
        {
          long bits1;
          if (reader.ReadBits(out bits1, 3) < 3)
            throw new EndOfStreamException();
          if (bits1 == 0L)
          {
            long bits2;
            if (reader.ReadBits(out bits2, 1) < 1)
              throw new EndOfStreamException();
            properties.Add(IoProperty.Create(id, (byte) bits2));
          }
          else
          {
            if (bits1 != 1L)
              throw new Exception("Unknown type");
            long bits2;
            if (reader.ReadBits(out bits2, 32) < 32)
              throw new EndOfStreamException();
            properties.Add(IoProperty.Create(id, (int) bits2));
          }
          ++num2;
        }
        ++id;
      }
      return new IoElement(0, properties);
    }
  }
}
