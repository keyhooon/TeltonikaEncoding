// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Communication.Iridium.InformationElementEncoding
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Teltonika.IO;
using Teltonika.Logging;

namespace Teltonika.Avl.Communication.Iridium
{
  public class InformationElementEncoding
  {
    private static readonly ILog Log = LogManager.GetLogger(typeof (InformationElementEncoding));
    private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

    public static IEnumerable<InformationElement> Decode(byte[] content)
    {
      ushort length;
      for (int offset = 0; offset < content.Length; offset += 3 + length)
      {
        byte id = content[offset];
        length = EndianBitConverter.Big.ToUInt16(content, offset + 1);
        yield return DecodeInformationElement(id, new ArraySegment<byte>(content, offset + 3, length));
      }
    }

    private static InformationElement DecodeInformationElement(
      byte id,
      ArraySegment<byte> content)
    {
      using (MemoryStream memoryStream = new MemoryStream(content.Array, content.Offset, content.Count, false))
      {
        using (EndianBinaryReader endianBinaryReader = new EndianBinaryReader(EndianBitConverter.Big, memoryStream))
        {
          switch (id)
          {
            case 1:
              return DecodeMoHeader(endianBinaryReader);
            case 2:
              return new MobileOriginatedPayload(endianBinaryReader.ReadBytes(content.Count));
            case 3:
              return DecodeLocation(endianBinaryReader);
            default:
              if (Log.IsWarnEnabled)
                                Log.WarnFormat("Unknown MOIE received ID:[{0}] content:[{1}], skipping.", id, endianBinaryReader.ReadBytes(content.Count).ToHexString());
              return null;
          }
        }
      }
    }

    private static MobileOriginatedHeader DecodeMoHeader(IBinaryReader reader) => new MobileOriginatedHeader(reader.ReadUInt32(), long.Parse(new string(((IEnumerable<byte>) reader.ReadBytes(15)).Select(x => (char)x).ToArray())), reader.ReadByte(), reader.ReadUInt16(), reader.ReadUInt16(), UnixEpoch.AddSeconds(reader.ReadUInt32()));

    private static MobileOriginatedLocation DecodeLocation(
      IBinaryReader reader)
    {
      byte num1 = reader.ReadByte();
      int num2 = (num1 & 2) >> 1;
      byte num3 = reader.ReadByte();
      ushort num4 = reader.ReadUInt16();
      double latitude = (num2 == 0 ? 1 : -1) * num3 + num4 / 1000.0 / 60.0;
      int num5 = num1 & 1;
      byte num6 = reader.ReadByte();
      ushort num7 = reader.ReadUInt16();
      double longitude = (num5 == 0 ? 1 : -1) * num6 + num7 / 1000.0 / 60.0;
      uint cepRadius = reader.ReadUInt32();
      return new MobileOriginatedLocation(latitude, longitude, cepRadius);
    }
  }
}
