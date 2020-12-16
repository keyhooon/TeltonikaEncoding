// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.Iridium.AvlDataEncoding
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Teltonika.Avl.Communication.Iridium;
using Teltonika.IO;
using Teltonika.Logging;

namespace Teltonika.Avl.Data.Iridium
{
  public class AvlDataEncoding
  {
    private const byte IridiumVisibleSatelliteConst = 255;
    private static readonly ILog Log = LogManager.GetLogger(typeof (AvlDataEncoding));
    public static AvlDataEncoding Instance = new AvlDataEncoding();
    private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

    public AvlData[] Decode(MobileOriginatedPayload moPayload)
    {
      List<AvlData> avlDataList = new List<AvlData>();
      using (MemoryStream memoryStream = new MemoryStream(moPayload.Payload, 0, moPayload.Payload.Length, false))
      {
        using (EndianBinaryReader endianBinaryReader = new EndianBinaryReader(EndianBitConverter.Big, memoryStream))
        {
          while (memoryStream.Position < memoryStream.Length)
          {
            AvlData avlData = DecodeAvlData(endianBinaryReader);
            if (avlData == null)
            {
              if (Log.IsWarnEnabled)
              {
                                Log.WarnFormat("Failed to decode avl data [{0}] at position [{1}]", moPayload.Payload.ToHexString(), memoryStream.Position);
                break;
              }
              break;
            }
            avlDataList.Add(avlData);
          }
        }
      }
      return avlDataList.ToArray();
    }

    private static AvlData DecodeAvlData(IBinaryReader reader)
    {
      uint num1 = reader.ReadUInt32();
      double latitude = reader.ReadInt32() / 10000000.0;
      double longitude = reader.ReadInt32() / 10000000.0;
      ushort num2 = reader.ReadUInt16();
      short angle = (short) (((num2 & 64512) >> 10) * 5.625);
      short speed = (short) (((num2 & 1008) >> 4) * 5);
      bool flag = (num2 & 128) == 128;
      byte num3 = (byte) (num2 & 7U);
      byte num4 = reader.ReadByte();
      KeyValuePair<bool, IoProperty>[] keyValuePairArray = null;
      if (num4 > 0)
        keyValuePairArray = DecodeOptionalIoProperties(reader.ReadBytes(num4)).ToArray();
      List<IoProperty> properties = new List<IoProperty>()
      {
        IoProperty.Create(232, num3)
      };
      List<int> intList = new List<int>();
      if (flag)
        intList.Add(232);
      if (keyValuePairArray != null)
      {
        properties.AddRange(((IEnumerable<KeyValuePair<bool, IoProperty>>) keyValuePairArray).Select(x => x.Value));
        intList.AddRange(((IEnumerable<KeyValuePair<bool, IoProperty>>) keyValuePairArray).Where(x => x.Key).Select(x => x.Value.Id));
      }
      return new AvlData(AvlDataPriority.Low, UnixEpoch.AddSeconds(num1), new GpsElement(latitude, longitude, 0, speed, angle, byte.MaxValue), new IoElement(intList.ToArray(), properties));
    }

    private static IEnumerable<KeyValuePair<bool, IoProperty>> DecodeOptionalIoProperties(
      byte[] data)
    {
      if (data.Length != 0)
      {
        if (data.Length == 1 && Log.IsWarnEnabled)
                    Log.WarnFormat("Can't decode optional I/O properties, data [{0}] contains only 1 byte.", (object) data.ToHexString());
        using (MemoryStream stream = new MemoryStream(data, false))
        {
          using (EndianBinaryReader reader = new EndianBinaryReader(EndianBitConverter.Big, stream))
          {
            byte id = reader.ReadByte();
            byte value = reader.ReadByte();
            if (id == 0)
            {
              yield return new KeyValuePair<bool, IoProperty>((value & 8) == 8, IoProperty.Create(227, (value & 128) == 128));
              yield return new KeyValuePair<bool, IoProperty>((value & 4) == 4, IoProperty.Create(230, (value & 64) == 64));
              yield return new KeyValuePair<bool, IoProperty>((value & 2) == 2, IoProperty.Create(231, (value & 32) == 32));
              yield return new KeyValuePair<bool, IoProperty>((value & 1) == 1, IoProperty.Create(233, (value & 16) == 16));
            }
            else if (Log.IsWarnEnabled)
                            Log.WarnFormat("Unknow ID:[{0}] encountered, skipping optional I/O properties decoding.");
          }
        }
      }
    }
  }
}
