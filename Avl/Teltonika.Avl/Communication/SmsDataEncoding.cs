// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Communication.SmsDataEncoding
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.IO;
using Teltonika.IO;

namespace Teltonika.Avl.Communication
{
  public static class SmsDataEncoding
  {
    public static void Encode(SmsData smsData, Stream stream)
    {
      if (smsData == null)
        throw new ArgumentNullException(nameof (smsData));
      if (stream == null)
        throw new ArgumentNullException(nameof (stream));
      if (smsData.Data == null)
        throw new ArgumentNullException("smsData.Data");
      EndianBinaryWriter suitableBinaryWriter = stream.CreateSuitableBinaryWriter();
      suitableBinaryWriter.Write(smsData.Data);
      suitableBinaryWriter.Write(smsData.Imei);
    }

    public static byte[] Encode(SmsData smsData)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
                Encode(smsData, memoryStream);
        memoryStream.Position = 0L;
        using (BinaryReader binaryReader = new BinaryReader(memoryStream))
          return binaryReader.ReadBytes((int) memoryStream.Length);
      }
    }

    public static SmsData Decode(Stream stream, int size)
    {
      EndianBinaryReader endianBinaryReader = stream != null ? stream.CreateSuitableBinaryReader() : throw new ArgumentNullException(nameof (stream));
      byte[] numArray = endianBinaryReader.ReadBytes(size - 8);
      long num = endianBinaryReader.ReadInt64();
      return new SmsData() { Imei = num, Data = numArray };
    }

    public static SmsData Decode(byte[] bytes, int size)
    {
      if (bytes == null)
        throw new ArgumentNullException(nameof (bytes));
      using (MemoryStream memoryStream = new MemoryStream(bytes))
        return Decode(memoryStream, size);
    }
  }
}
