// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.Encoding.DefaultIOElementEncoding
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.Collections.Generic;
using System.Linq;
using Teltonika.IO;

namespace Teltonika.Avl.Data.Encoding
{
  public sealed class DefaultIOElementEncoding : IEncoding<IoElement>
  {
    private static DefaultIOElementEncoding _instance = new DefaultIOElementEncoding(false);
    private static DefaultIOElementEncoding _instance16 = new DefaultIOElementEncoding(true);
    private readonly bool _isIoInt128Supported;

    private DefaultIOElementEncoding(bool isIoInt128Supported) => _isIoInt128Supported = isIoInt128Supported;

    public static DefaultIOElementEncoding Instance => _instance ?? (_instance = new DefaultIOElementEncoding(false));

    public static DefaultIOElementEncoding Instance16 => _instance16 ?? (_instance16 = new DefaultIOElementEncoding(true));

    public void Encode(IoElement obj, IBitWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      writer.Write((byte) obj.EventId);
      writer.Write((byte) obj.Count);
      IoProperty[] array1 = obj.Where(i => i.Size == 1).ToArray();
      writer.Write((byte) array1.Length);
      foreach (IoProperty property in array1)
        DefaultIoPropertyEncoding.Int8.Encode(property, writer);
      IoProperty[] array2 = obj.Where(i => i.Size == 2).ToArray();
      writer.Write((byte) array2.Length);
      foreach (IoProperty property in array2)
        DefaultIoPropertyEncoding.Int16.Encode(property, writer);
      IoProperty[] array3 = obj.Where(i => i.Size == 4).ToArray();
      writer.Write((byte) array3.Length);
      foreach (IoProperty property in array3)
        DefaultIoPropertyEncoding.Int32.Encode(property, writer);
      IoProperty[] array4 = obj.Where(i => i.Size == 8).ToArray();
      writer.Write((byte) array4.Length);
      foreach (IoProperty property in array4)
        DefaultIoPropertyEncoding.Int64.Encode(property, writer);
      if (_isIoInt128Supported)
      {
        IoProperty[] array5 = obj.Where(i => i.Size == 16).ToArray();
        writer.Write((byte) array5.Length);
        foreach (IoProperty property in array5)
          DefaultIoPropertyEncoding.Int128.Encode(property, writer);
      }
      writer.Flush();
    }

    public IoElement Decode(IBitReader reader)
    {
      byte num1 = reader != null ? reader.ReadByte() : throw new ArgumentNullException(nameof (reader));
      int capacity = reader.ReadByte();
      List<IoProperty> properties = new List<IoProperty>(capacity);
      int num2 = reader.ReadByte();
      for (int index = 0; index < num2; ++index)
        properties.Add(DefaultIoPropertyEncoding.Int8.Decode(reader));
      int num3 = reader.ReadByte();
      for (int index = 0; index < num3; ++index)
        properties.Add(DefaultIoPropertyEncoding.Int16.Decode(reader));
      int num4 = reader.ReadByte();
      for (int index = 0; index < num4; ++index)
        properties.Add(DefaultIoPropertyEncoding.Int32.Decode(reader));
      int num5 = reader.ReadByte();
      for (int index = 0; index < num5; ++index)
        properties.Add(DefaultIoPropertyEncoding.Int64.Decode(reader));
      if (_isIoInt128Supported)
      {
        int num6 = reader.ReadByte();
        for (int index = 0; index < num6; ++index)
          properties.Add(DefaultIoPropertyEncoding.Int128.Decode(reader));
      }
      if (capacity != properties.Count)
        throw new ApplicationException("Wrong total io count field");
      return new IoElement(num1, properties);
    }
  }
}
