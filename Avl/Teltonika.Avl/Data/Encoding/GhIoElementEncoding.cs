// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.Encoding.GhIoElementEncoding
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.Collections.Generic;
using Teltonika.IO;

namespace Teltonika.Avl.Data.Encoding
{
  public sealed class GhIoElementEncoding : IEncoding<IoElement>
  {
    private static GhIoElementEncoding _int8 = new GhIoElementEncoding(FieldEncoding.Int8);
    private static GhIoElementEncoding _int16 = new GhIoElementEncoding(FieldEncoding.Int16);
    private static GhIoElementEncoding _int32 = new GhIoElementEncoding(FieldEncoding.Int32);
    private readonly FieldEncoding _fieldEncoding;

    public static GhIoElementEncoding Int8 => _int8 ?? (_int8 = new GhIoElementEncoding(FieldEncoding.Int8));

    public static GhIoElementEncoding Int16 => _int16 ?? (_int16 = new GhIoElementEncoding(FieldEncoding.Int16));

    public static GhIoElementEncoding Int32 => _int32 ?? (_int32 = new GhIoElementEncoding(FieldEncoding.Int32));

    private GhIoElementEncoding(FieldEncoding encoding) => _fieldEncoding = encoding;

    public void Encode(IoElement obj, IBitWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      DefaultIoPropertyEncoding propertyEncoding = new DefaultIoPropertyEncoding(_fieldEncoding);
      writer.Write((byte) obj.Count);
      foreach (IoProperty property in obj)
        propertyEncoding.Encode(property, writer);
      writer.Flush();
    }

    public IoElement Decode(IBitReader reader)
    {
      if (reader == null)
        throw new ArgumentNullException(nameof (reader));
      DefaultIoPropertyEncoding propertyEncoding = new DefaultIoPropertyEncoding(_fieldEncoding);
      byte num = reader.ReadByte();
      List<IoProperty> properties = new List<IoProperty>(num);
      for (int index = 0; index < num; ++index)
        properties.Add(propertyEncoding.Decode(reader));
      return new IoElement(0, properties);
    }
  }
}
