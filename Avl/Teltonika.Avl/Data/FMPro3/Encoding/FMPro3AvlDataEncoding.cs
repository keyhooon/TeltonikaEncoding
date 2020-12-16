// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.FMPro3.Encoding.FMPro3AvlDataEncoding
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using Teltonika.Avl.Tools;
using Teltonika.IO;

namespace Teltonika.Avl.Data.FMPro3.Encoding
{
  public sealed class FMPro3AvlDataEncoding : IEncoding<AvlData>
  {
    public static readonly IEncoding<AvlData> Instance = new FMPro3AvlDataEncoding();

    public void Encode(AvlData data, IBitWriter writer)
    {
      if (data == null)
        throw new ArgumentNullException(nameof (data));
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      long avl = DateTimeExt.ToAvl(data.DateTime);
      writer.Write((uint) ((ulong) avl / 1000UL));
      writer.Write((byte) ((ulong) (avl % 1000L) / 10UL));
      writer.Write((byte) data.Priority);
      data.Data.Remove(500001);
      FMPro3GpsElementEncoding.Instance.Encode(GpsElementExt.Create(data, 500001), writer);
      FMPro3IOElementEncoding.Instance.Encode(data.Data, writer);
    }

    public AvlData Decode(IBitReader reader)
    {
      DateTime dateTime = reader != null ? DateTimeExt.FromAvl(reader.ReadUInt32() * 1000L + reader.ReadByte() * 10) : throw new ArgumentNullException(nameof (reader));
      AvlDataPriority priority = (AvlDataPriority) reader.ReadByte();
      GpsElementExt gpsElementExt = FMPro3GpsElementEncoding.Instance.Decode(reader);
      IoElement data = FMPro3IOElementEncoding.Instance.Decode(reader);
      data.Add(gpsElementExt.IO[500001]);
      return new AvlData(priority, dateTime, gpsElementExt.GPS, data);
    }
  }
}
