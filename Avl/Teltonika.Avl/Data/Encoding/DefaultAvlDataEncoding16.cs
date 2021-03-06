﻿// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.Encoding.DefaultAvlDataEncoding16
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using Teltonika.Avl.Tools;
using Teltonika.IO;

namespace Teltonika.Avl.Data.Encoding
{
  public sealed class DefaultAvlDataEncoding16 : IEncoding<AvlData>
  {
    private static DefaultAvlDataEncoding16 _instance;

    public static DefaultAvlDataEncoding16 Instance => _instance ?? (_instance = new DefaultAvlDataEncoding16());

    public void Encode(AvlData obj, IBitWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      long avl = DateTimeExt.ToAvl(obj.DateTime);
      writer.Write(avl);
      writer.Write((byte) obj.Priority);
      DefaultGpsElementEncoding.Instance.Encode(obj.GPS, writer);
      DefaultIOElementEncoding.Instance16.Encode(obj.Data, writer);
      writer.Flush();
    }

    public AvlData Decode(IBitReader reader)
    {
      DateTime dateTime = reader != null ? DateTimeExt.FromAvl(reader.ReadInt64()) : throw new ArgumentNullException(nameof (reader));
      AvlDataPriority priority = (AvlDataPriority) reader.ReadByte();
      GpsElement gps = DefaultGpsElementEncoding.Instance.Decode(reader);
      IoElement data = DefaultIOElementEncoding.Instance16.Decode(reader);
      return new AvlData(priority, dateTime, gps, data);
    }
  }
}
