// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.Encoding.Codec25AvlDataEncoding
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.IO;
using Teltonika.Avl.Tools;
using Teltonika.IO;

namespace Teltonika.Avl.Data.Encoding
{
  public sealed class Codec25AvlDataEncoding : IEncoding<AvlData>
  {
    private static Codec25AvlDataEncoding _instance;

    public static Codec25AvlDataEncoding Instance => _instance ?? (_instance = new Codec25AvlDataEncoding());

    public void Encode(AvlData obj, IBitWriter writer)
    {
      if (obj == null)
        throw new ArgumentNullException(nameof (obj));
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      byte num;
      byte[] buffer;
      using (MemoryStream stream = new MemoryStream())
      {
        using (IBitWriter suitableBitWriter = stream.CreateSuitableBitWriter())
        {
          long avl = DateTimeExt.ToAvl(obj.DateTime);
          suitableBitWriter.Write(Convert.ToUInt32(avl / 1000L));
          suitableBitWriter.Write((byte) obj.Priority);
          DefaultGpsElementEncoding.Instance.Encode(obj.GPS, suitableBitWriter);
          DefaultIOElementEncoding.Instance.Encode(obj.Data, suitableBitWriter);
          suitableBitWriter.Flush();
          num = Convert.ToByte(stream.Length);
          buffer = new byte[num];
          stream.Position = 0L;
          stream.Read(buffer, 0, num);
        }
      }
      byte xor = CRC.CalculateXor(buffer);
      writer.Write(num);
      writer.BaseStream.Write(buffer, 0, num);
      writer.Write(xor);
      writer.Flush();
    }

    public AvlData Decode(IBitReader reader)
    {
      byte num = reader != null ? reader.ReadByte() : throw new ArgumentNullException(nameof (reader));
      byte[] buffer = reader.ReadBytes(num);
      if (reader.ReadByte() != CRC.CalculateXor(buffer))
        throw new Exception("AvlData Record CRC failure. Received CRC - {0}, calculated CRC - {1}.");
      using (MemoryStream stream = new MemoryStream())
      {
        using (IBitReader suitableBitReader = stream.CreateSuitableBitReader())
        {
          stream.Write(buffer, 0, num);
          stream.Position = 0L;
          long timestamp = suitableBitReader.ReadUInt32() * 1000L;
          AvlDataPriority priority = (AvlDataPriority) suitableBitReader.ReadByte();
          GpsElement gps = DefaultGpsElementEncoding.Instance.Decode(suitableBitReader);
          IoElement data = DefaultIOElementEncoding.Instance.Decode(suitableBitReader);
          DateTime dateTime = DateTimeExt.FromAvl(timestamp);
          return new AvlData(priority, dateTime, gps, data);
        }
      }
    }
  }
}
