// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.Encoding.GhAvlDataEncoding
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.Collections.Generic;
using System.Linq;
using Teltonika.IO;

namespace Teltonika.Avl.Data.Encoding
{
  public sealed class GhAvlDataEncoding : IEncoding<AvlData>
  {
    private static readonly DateTime GHepoch = new DateTime(2007, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
    private static GhAvlDataEncoding _instance;
    private static readonly AvlDataPriority[] Priorities = new AvlDataPriority[4]
    {
      AvlDataPriority.Low,
      AvlDataPriority.High,
      AvlDataPriority.Panic,
      AvlDataPriority.Security
    };

    public static GhAvlDataEncoding Instance => _instance ?? (_instance = new GhAvlDataEncoding());

    public void Encode(AvlData avlData, IBitWriter writer)
    {
      if (avlData == null)
        throw new ArgumentNullException(nameof (avlData));
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      int num1 = Array.IndexOf(Priorities, avlData.Priority);
      int num2 = (int) ((avlData.DateTime - GHepoch).TotalSeconds + 0.5) | (num1 & 3) << 30;
      writer.Write(num2);
      GhGlobalMask mask = avlData.GetMask();
      writer.Write((byte) mask);
      if (mask.HasFlag(GhGlobalMask.GpsElement))
        GhGpsElementEncoding.Instance.Encode(GpsElementExt.Create(avlData, 200, 202, 201), writer);
      bool flag1 = mask.HasFlag(GhGlobalMask.IoInt8);
      bool flag2 = mask.HasFlag(GhGlobalMask.IoInt16);
      bool flag3 = mask.HasFlag(GhGlobalMask.IoInt32);
      if (!flag1 && !flag2 && !flag3)
        return;
      List<IoProperty> properties1 = flag1 ? avlData.Data.Where(x => x.Size == 1).ToList() : null;
      List<IoProperty> properties2 = flag2 ? avlData.Data.Where(x => x.Size == 2).ToList() : null;
      List<IoProperty> properties3 = flag3 ? avlData.Data.Where(x => x.Size == 4).ToList() : null;
      if (properties1 != null)
      {
        properties1.RemoveAll(x => x.Id == 200);
        properties1.RemoveAll(x => x.Id == 201);
        properties1.RemoveAll(x => x.Id == 202);
        properties1.RemoveAll(x => x.Id == 204);
        GhIoElementEncoding.Int8.Encode(new IoElement(0, properties1), writer);
      }
      if (properties2 != null)
      {
        properties2.RemoveAll(x => x.Id == 200);
        properties2.RemoveAll(x => x.Id == 201);
        properties2.RemoveAll(x => x.Id == 202);
        properties2.RemoveAll(x => x.Id == 204);
        GhIoElementEncoding.Int16.Encode(new IoElement(0, properties2), writer);
      }
      if (properties3 != null)
      {
        properties3.RemoveAll(x => x.Id == 200);
        properties3.RemoveAll(x => x.Id == 201);
        properties3.RemoveAll(x => x.Id == 202);
        properties3.RemoveAll(x => x.Id == 204);
        GhIoElementEncoding.Int32.Encode(new IoElement(0, properties3), writer);
      }
      writer.Flush();
    }

    public AvlData Decode(IBitReader reader)
    {
      int num1 = reader != null ? reader.ReadInt32() : throw new ArgumentNullException(nameof (reader));
      int index = 3 & num1 >> 30;
      AvlDataPriority priority = Priorities[index];
      long num2 = num1 & 1073741823;
      DateTime dateTime = GHepoch.AddSeconds(num2);
      GhGlobalMask mask = (GhGlobalMask) reader.ReadByte();
      GpsElement gps = GpsElement.Default;
      IoElement ioElement = new IoElement();
      if (mask.HasFlag(GhGlobalMask.GpsElement))
      {
        GpsElementExt gpsElementExt = GhGpsElementEncoding.Instance.Decode(reader);
        gps = gpsElementExt.GPS;
        ioElement = gpsElementExt.IO;
      }
      IList<IoProperty> properties1 = GetProperties(reader, mask, GhGlobalMask.IoInt8, GhIoElementEncoding.Int8);
      IList<IoProperty> properties2 = GetProperties(reader, mask, GhGlobalMask.IoInt16, GhIoElementEncoding.Int16);
      IList<IoProperty> properties3 = GetProperties(reader, mask, GhGlobalMask.IoInt32, GhIoElementEncoding.Int32);
      List<IoProperty> ioPropertyList = new List<IoProperty>();
      ioPropertyList.AddRange(ioElement);
      ioPropertyList.AddRange(properties1 ?? Enumerable.Empty<IoProperty>());
      ioPropertyList.AddRange(properties2 ?? Enumerable.Empty<IoProperty>());
      ioPropertyList.AddRange(properties3 ?? Enumerable.Empty<IoProperty>());
      int eventId = 0;
      if (priority == AvlDataPriority.Panic)
      {
        IoProperty ioProperty = IoProperty.Create(204, (byte) 1);
        ioPropertyList.Add(ioProperty);
        eventId = ioPropertyList.SingleOrDefault(x => x.Id == 222) == IoProperty.Default ? 204 : 222;
      }
      IoElement data = new IoElement(eventId, ioPropertyList);
      return new AvlData(priority, dateTime, gps, data);
    }

    private static IList<IoProperty> GetProperties(
      IBitReader reader,
      GhGlobalMask mask,
      GhGlobalMask flag,
      GhIoElementEncoding encoding)
    {
      return mask.HasFlag(flag) ? encoding.Decode(reader).Properties : null;
    }

    public static class Constants
    {
      public const int CellIDPropertyId = 200;
      public const int SignalQualityPropertyId = 201;
      public const int OperatorCodePropertyId = 202;
      public const int AlarmPropertyId = 204;
      public const int AlarmCausePropertyId = 222;
    }
  }
}
