// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.Encoding.LegacyAvlDataEncoding
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.Collections.Generic;
using Teltonika.Avl.Tools;
using Teltonika.IO;

namespace Teltonika.Avl.Data.Encoding
{
  public sealed class LegacyAvlDataEncoding : IEncoding<AvlData>
  {
    private static LegacyAvlDataEncoding _instance;

    public static LegacyAvlDataEncoding Instance => _instance ?? (_instance = new LegacyAvlDataEncoding());

    public void Encode(AvlData avlData, IBitWriter writer)
    {
      if (avlData == null)
        throw new ArgumentNullException(nameof (avlData));
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      writer.Write(DateTimeExt.ToAvl(avlData.DateTime));
      writer.Write(avlData.Priority == AvlDataPriority.High ? (byte) 20 : (byte) 10);
      DefaultGpsElementEncoding.Instance.Encode(avlData.GPS, writer);
      LegacyIoElementEncoding.Instance.Encode(avlData.Data, writer);
    }

    public AvlData Decode(IBitReader reader)
    {
      DateTime dateTime = reader != null ? DateTimeExt.FromAvl(reader.ReadInt64()) : throw new ArgumentNullException(nameof (reader));
      AvlDataPriority priority = AvlDataPriority.Low;
      byte num = reader.ReadByte();
      if (num > 10 && num <= 20)
        priority = AvlDataPriority.High;
      GpsElement gps = DefaultGpsElementEncoding.Instance.Decode(reader);
      IoElement data = LegacyIoElementEncoding.Instance.Decode(reader);
      if (priority == AvlDataPriority.High)
        data = new IoElement(-1, data.Properties);
      return new AvlData(priority, dateTime, gps, data);
    }
  }
}
