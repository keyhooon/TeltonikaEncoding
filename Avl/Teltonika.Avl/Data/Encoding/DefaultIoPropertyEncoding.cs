// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.Encoding.DefaultIoPropertyEncoding
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using Teltonika.IO;

namespace Teltonika.Avl.Data.Encoding
{
  public class DefaultIoPropertyEncoding : IEncoding<IoProperty>, IEncoding<IoProperty[]>
  {
    public static readonly DefaultIoPropertyEncoding Int8 = new DefaultIoPropertyEncoding(FieldEncoding.Int8);
    public static readonly DefaultIoPropertyEncoding Int16 = new DefaultIoPropertyEncoding(FieldEncoding.Int16);
    public static readonly DefaultIoPropertyEncoding Int32 = new DefaultIoPropertyEncoding(FieldEncoding.Int32);
    public static readonly DefaultIoPropertyEncoding Int64 = new DefaultIoPropertyEncoding(FieldEncoding.Int64);
    public static readonly DefaultIoPropertyEncoding Int128 = new DefaultIoPropertyEncoding(FieldEncoding.Int128);
    protected readonly FieldEncoding Encoding;

    internal DefaultIoPropertyEncoding(FieldEncoding encoding) => Encoding = encoding;

    public void Encode(IoProperty property, IBitWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      writer.Write((byte) property.Id);
      switch (property.Size)
      {
        case 1:
          writer.Write((sbyte) property);
          break;
        case 2:
          writer.Write((short) property);
          break;
        case 4:
          writer.Write((int) property);
          break;
        case 8:
          writer.Write((long) property);
          break;
        case 16:
          writer.Write((byte[]) property);
          break;
      }
      writer.Flush();
    }

    public IoProperty Decode(IBitReader reader)
    {
      int id = reader != null ? reader.ReadByte() : throw new ArgumentNullException(nameof (reader));
      switch (Encoding)
      {
        case FieldEncoding.Int8:
          return IoProperty.Create(id, reader.ReadSByte());
        case FieldEncoding.Int16:
          return IoProperty.Create(id, reader.ReadInt16());
        case FieldEncoding.Int32:
          return IoProperty.Create(id, reader.ReadInt32());
        case FieldEncoding.Int64:
          return IoProperty.Create(id, reader.ReadInt64());
        case FieldEncoding.Int128:
          return IoProperty.Create(id, reader.ReadBytes(16));
        default:
          throw new NotSupportedException(string.Format("The field encoding \"{0}\" is not supported.", Encoding));
      }
    }

    public void Encode(IoProperty[] properties, IBitWriter writer)
    {
      if (properties == null)
        throw new ArgumentNullException(nameof (properties));
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      if (byte.MaxValue < properties.Length)
        throw new ArgumentException("The properties array cannot be larger then 255.", nameof (properties));
      writer.Write((byte) properties.Length);
      foreach (IoProperty property in properties)
        Encode(property, writer);
    }

    IoProperty[] IEncoding<IoProperty[]>.Decode(IBitReader reader)
    {
      byte num = reader != null ? reader.ReadByte() : throw new ArgumentNullException(nameof (reader));
      IoProperty[] ioPropertyArray = new IoProperty[num];
      for (int index = 0; index < num; ++index)
        ioPropertyArray[index] = Decode(reader);
      return ioPropertyArray;
    }
  }
}
