// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.Encoding.SmsAvlDataArrayEncoding
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.Collections.Generic;
using Teltonika.Avl.Tools;
using Teltonika.IO;

namespace Teltonika.Avl.Data.Encoding
{
  public sealed class SmsAvlDataArrayEncoding : IEncoding<AvlData[]>
  {
    private const int DateBits = 35;
    private const int ElementCountBits = 5;
    private const int LongitudeOffsetBits = 14;
    private const long LongitudeOffsets = 8191;
    private const int LatitudeOffsetBits = 14;
    private const long LatitudeOffsets = 8191;
    private const int LongitudeStepBits = 21;
    private const long LongitudeSteps = 2097151;
    private const int LatitudeStepBits = 20;
    private const long LatitudeSteps = 1048575;
    private const long LongitudeMin = -1800000000;
    private const long LongitudeMax = 1800000000;
    private const long Longitudes = 3600000000;
    private const long LatitudeMin = -900000000;
    private const long LatitudeMax = 900000000;
    private const long Latitudes = 1800000000;
    public const long MaxXError = 1717;
    public const long MaxYError = 1717;
    private const int MaxElementCount = 24;
    private const long Y2K = 946677600000;
    private static SmsAvlDataArrayEncoding _instance;

    public static SmsAvlDataArrayEncoding Instance => _instance ?? (_instance = new SmsAvlDataArrayEncoding());

    public void Encode(AvlData[] obj, IBitWriter writer)
    {
      if (obj == null)
        throw new ArgumentNullException(nameof (obj));
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      int length = obj.Length;
      if (length == 0)
        throw new ArgumentException("0 AvlData to encode");
      long num1 = Timestamp2Y2K(DateTimeExt.ToAvl(obj[0].DateTime));
            WriteBitSequence(writer, num1, 35);
            WriteBitSequence(writer, length, 5);
      long num2 = 0;
      long num3 = 0;
      for (int index = 0; index < length; ++index)
      {
        AvlData avlData = obj[index];
        GpsElement gps = avlData.GPS;
        bool isValid = gps.IsValid;
        writer.WriteBit(isValid);
        if (isValid)
        {
          long num4 = num2;
          long num5 = num3;
          gps = avlData.GPS;
          num2 = CompressGws(gps.X, -1800000000L, 1800000000L, 3600000000L, 2097151L);
          gps = avlData.GPS;
          num3 = CompressGws(gps.Y, -900000000L, 900000000L, 1800000000L, 1048575L);
          long num6 = num4 - num2;
          long num7 = num5 - num3;
          if (Math.Abs(num6) < 8191L && Math.Abs(num7) < 8191L)
          {
            writer.WriteBit(true);
                        WriteBitSequence(writer, num6 + 8191L, 14);
                        WriteBitSequence(writer, num7 + 8191L, 14);
          }
          else
          {
            writer.WriteBit(false);
                        WriteBitSequence(writer, num2, 21);
                        WriteBitSequence(writer, num3, 20);
          }
          IBitWriter writer1 = writer;
          gps = avlData.GPS;
          long speed = gps.Speed;
                    WriteBitSequence(writer1, speed, 8);
        }
      }
    }

    public AvlData[] Decode(IBitReader reader)
    {
      if (reader == null)
        throw new ArgumentNullException(nameof (reader));
      long y2Ktime;
      if (ReadBitSequence(reader, out y2Ktime, 35) != 35)
        throw new Exception("Wrong timestamp bits count");
      long timestamp = Y2K2Timestamp(y2Ktime);
      long bits;
      if (reader.ReadBits(out bits, 5) != 5)
        throw new Exception("Wrong element count bits count");
      long num1 = Math.Min(bits, 24L);
      List<AvlData> avlDataList = new List<AvlData>();
      long compressedGws1 = 0;
      long compressedGws2 = 0;
      for (int index = 0; index < num1; ++index)
      {
        bool bit1;
        if (reader.ReadBit(out bit1) != 1)
          throw new Exception("Wrong data bits count");
        if (bit1)
        {
          bool bit2;
          if (reader.ReadBit(out bit2) != 1)
            throw new Exception("Wrong data bits count");
          if (bit2)
          {
            long num2;
            if (ReadBitSequence(reader, out num2, 14) != 14)
              throw new Exception("Wrong data bits count");
            long num3;
            if (ReadBitSequence(reader, out num3, 14) != 14)
              throw new Exception("Wrong data bits count");
            compressedGws1 = compressedGws1 - num2 + 8191L;
            compressedGws2 = compressedGws2 - num3 + 8191L;
          }
          else
          {
            if (ReadBitSequence(reader, out compressedGws1, 21) != 21)
              throw new Exception("Wrong data bits count");
            if (ReadBitSequence(reader, out compressedGws2, 20) != 20)
              throw new Exception("Wrong data bits count");
          }
          long num4 = DecompressGws(compressedGws1, -1800000000L, 1800000000L, 3600000000L, 2097151L);
          long num5 = DecompressGws(compressedGws2, -900000000L, 900000000L, 1800000000L, 1048575L);
          short speed = reader.ReadByte();
          AvlData avlData = new AvlData(AvlDataPriority.Low, DateTimeExt.FromAvl(timestamp), new GpsElement((int) num4, (int) num5, 0, speed, 0, 3));
          avlDataList.Add(avlData);
        }
        timestamp += 3600000L;
      }
      return avlDataList.ToArray();
    }

    private static long Y2K2Timestamp(long y2Ktime) => y2Ktime * 1000L + 946677600000L;

    private static long Timestamp2Y2K(long timestamp) => (timestamp - 946677600000L) / 1000L;

    private static long DecompressGws(
      long compressedGws,
      long gwsMinimum,
      long gwsMaximum,
      long gwsSteps,
      long compressedSteps)
    {
      return (long) Math.Round(compressedGws * (double) gwsSteps / compressedSteps + gwsMinimum);
    }

    private static long CompressGws(
      long gws,
      long gwsMinimum,
      long gwsMaximum,
      long gwsSteps,
      long compressedSteps)
    {
      return (long) Math.Round((gws - (double) gwsMinimum) * compressedSteps / gwsSteps);
    }

    public static int ReadBitSequence(IBitReader reader, out long value, int length)
    {
      value = 0L;
      for (int index = 0; index < length; ++index)
      {
        bool bit;
        if (reader.ReadBit(out bit) != 1)
          return index;
        long num = (bit ? 1L : 0L) << index;
        value |= num;
      }
      return length;
    }

    private static void WriteBitSequence(IBitWriter writer, long value, int length)
    {
      for (int index = 0; index < length; ++index)
      {
        bool bit = ((byte)value & 1) == 1;
        writer.WriteBit(bit);
        value >>= 1;
      }
    }
  }
}
