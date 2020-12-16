// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.At2000.At2000DataEncoding
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Teltonika.Avl.Cache;
using Teltonika.Avl.Extensions;
using Teltonika.IO;
using Teltonika.Logging;

namespace Teltonika.Avl.Data.At2000
{
  public sealed class At2000DataEncoding
  {
    private const int PacketSize = 63;
    private static readonly ILog Log = LogManager.GetLogger(typeof (At2000DataEncoding));
    private static readonly At2000Io[] At2000EventsToIgnore = new At2000Io[3]
    {
      At2000Io.CoordinateReliabilityEvent,
      At2000Io.DistanceThresholdExceededEvent,
      At2000Io.CourseThresholdExceededEvent
    };
    private static volatile At2000DataEncoding _instance;

    public static At2000DataEncoding Instance => _instance ?? (_instance = new At2000DataEncoding());

    public AvlData[] Decode(byte[] data, out int count) => Decode(new ArraySegment<byte>(data), out count);

    public AvlData[] Decode(ArraySegment<byte> data, out int count)
    {
      List<AvlData> avlDataList = new List<AvlData>();
      count = 0;
      foreach (ArraySegment<byte> segment in data.ToSegments(63))
      {
        if (segment.Count == 63)
        {
          AvlData avlData = DecodeInternal(segment);
          if (!(avlData == null))
          {
            avlDataList.Add(avlData);
            count += segment.Count;
          }
          else
            break;
        }
        else
          break;
      }
      return avlDataList.ToArray();
    }

    private static AvlData DecodeInternal(ArraySegment<byte> data)
    {
      if (data.Count != 63)
        throw new Exception("Invalid packet size!");
      using (MemoryStream memoryStream = new MemoryStream(data.Array, data.Offset, data.Count, false))
      {
        using (EndianBinaryReader endianBinaryReader = new EndianBinaryReader(EndianBitConverter.Little, memoryStream))
        {
          int num1 = endianBinaryReader.ReadInt16();
          int num2 = endianBinaryReader.ReadInt16();
          long num3 = endianBinaryReader.ReadInt64();
          DateTime dateTime;
          if (0L <= num3 && num3 <= uint.MaxValue)
          {
            dateTime = Common.UnixEpoch.AddSeconds(num3).ToLocalTime();
          }
          else
          {
            if (Log.IsDebugEnabled)
                            Log.DebugFormat("Invalid date/time: {0}", (object) data.ToHexString());
            dateTime = DateTime.MinValue;
          }
          float num4 = endianBinaryReader.ReadSingle();
          float num5 = endianBinaryReader.ReadSingle();
          short altitude = (short) endianBinaryReader.ReadSingle();
          short speed = (short) endianBinaryReader.ReadSingle();
          short angle = (short) endianBinaryReader.ReadSingle();
          uint geoEvents = endianBinaryReader.ReadUInt32();
          uint ioEvents = endianBinaryReader.ReadUInt32();
          uint values = endianBinaryReader.ReadUInt32();
          IEnumerable<IoProperty> second1 = ReadValues(IoType.Io, endianBinaryReader.ReadUInt32());
          IEnumerable<IoProperty> second2 = ReadEvents(IoType.IoEvent, ioEvents);
          IEnumerable<IoProperty> second3 = ReadValues(IoType.Geofencing, values);
          IEnumerable<IoProperty> second4 = ReadEvents(IoType.GeofencingEvent, geoEvents);
          IEnumerable<IoProperty> first = ReadIoProperties(endianBinaryReader);
          byte satellites = endianBinaryReader.ReadByte();
          GpsElement gps = new GpsElement(num4, num5, altitude, speed, angle, satellites);
          List<IoProperty> list = first.Union(second1).Union(second2).Union(second3).Union(second4).ToList();
          IoElement data1 = new IoElement(GetSettings(IoType.IoEvent).Where(x => !((IEnumerable<At2000Io>)At2000EventsToIgnore).Contains(x.Key)).Where(x =>
        {
            int num = 1 << x.Value.BitNumber;
            return (ioEvents & num) == num;
        }).Select(x => (int)(x.Key - 34)).Union(GetSettings(IoType.GeofencingEvent).Where(x => !((IEnumerable<At2000Io>)At2000EventsToIgnore).Contains(x.Key)).Where(x =>
      {
            int num = 1 << x.Value.BitNumber;
            return (geoEvents & num) == num;
        }).Select(x => (int)(x.Key - 34))).ToArray(), list);
          return new AvlData(AvlDataPriority.Low, dateTime, gps, data1);
        }
      }
    }

    private static IEnumerable<IoProperty> ReadIoProperties(
      IBinaryReader reader)
    {
      return new IoProperty[9]
      {
        IoProperty.Create(10, reader.ReadInt16()),
        IoProperty.Create(1, reader.ReadInt16()),
        IoProperty.Create(2, reader.ReadInt16()),
        IoProperty.Create(3, reader.ReadInt16()),
        IoProperty.Create(12, reader.ReadInt16()),
        IoProperty.Create(11, reader.ReadByte()),
        IoProperty.Create(5, reader.ReadByte()),
        IoProperty.Create(4, reader.ReadByte()),
        IoProperty.Create(6, reader.ReadByte())
      };
    }

    private static IEnumerable<IoProperty> ReadValues(IoType type, uint values)
    {
      Dictionary<At2000Io, BinaryIoAttribute> settings = GetSettings(type);
      return settings == null ? Enumerable.Empty<IoProperty>() : GetFlags(values, settings).Select(x => IoProperty.Create(x.Key, x.Value ? (byte)1 : (byte)0));
    }

    private static IEnumerable<IoProperty> ReadEvents(IoType type, uint values) => ReadValues(type, values).Where(x => x.ToBoolean());

    private static Dictionary<int, bool> GetFlags(
      uint value,
      IEnumerable<KeyValuePair<At2000Io, BinaryIoAttribute>> settings)
    {
      return settings.GroupBy(x => x.Key, x => IsSet(value, x.Value.BitNumber)).ToDictionary(x => (int)x.Key, x => x.Single());
    }

    public static Dictionary<At2000Io, BinaryIoAttribute> GetSettings(
      IoType type)
    {
      try
      {
        return Enum.GetValues(typeof (At2000Io)).Cast<At2000Io>().Select(x =>
        {
          var data = new
          {
            Type = x,
            Settings = EnumAttributeCache<At2000Io>.GetCustomAttribute<BinaryIoAttribute>(x)
          };
          return data;
        }).Where(x => x.Settings != null && x.Settings.IoIoType == type).GroupBy(x => x.Type, x => x.Settings).ToDictionary(x => x.Key, x => x.Single());
      }
      catch (InvalidOperationException ex)
      {
        throw new ApplicationException(string.Format("The AT 2000 I/O configuration is not valid. There are duplicate I/O values."), ex);
      }
    }

    private static bool IsSet(uint value, int index)
    {
      int num = 1 << index;
      return (value & num) == num;
    }
  }
}
