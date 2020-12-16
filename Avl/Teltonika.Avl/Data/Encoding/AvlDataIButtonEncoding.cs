// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.Encoding.AvlDataIButtonEncoding
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using Teltonika.IO;

namespace Teltonika.Avl.Data.Encoding
{
  public sealed class AvlDataIButtonEncoding : IEncoding<AvlData>
  {
    private static AvlDataIButtonEncoding _instance;

    public static AvlDataIButtonEncoding Instance => _instance ?? (_instance = new AvlDataIButtonEncoding());

    public void Encode(AvlData data, IBitWriter writer)
    {
      if (data == null)
        throw new ArgumentNullException(nameof (data));
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      IoProperty property = data.Data[78];
      if (property.IsDefault)
        throw new InvalidOperationException("No IButton data");
      data.Data.Remove(property.Id);
      DefaultAvlDataEncoding.Instance.Encode(data, writer);
      writer.Write((ulong) property);
      writer.Write(property.Status);
      writer.Flush();
      data.Data.Add(property);
    }

    public AvlData Decode(IBitReader reader)
    {
      AvlData avlData = reader != null ? DefaultAvlDataEncoding.Instance.Decode(reader) : throw new ArgumentNullException(nameof (reader));
      ulong num = reader.ReadUInt64();
      byte status = reader.ReadByte();
      avlData.Data.Add(IoProperty.Create(78, num, status));
      return avlData;
    }
  }
}
