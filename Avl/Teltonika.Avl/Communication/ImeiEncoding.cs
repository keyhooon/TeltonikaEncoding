// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Communication.ImeiEncoding
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.IO;
using System.Text;
using Teltonika.IO;

namespace Teltonika.Avl.Communication
{
  public static class ImeiEncoding
  {
    public static readonly Encoding Encoding = Encoding.UTF8;

    public static void Encode(string imei, Stream stream)
    {
      switch (imei)
      {
        case "":
          throw new ArgumentException(nameof (imei));
        case null:
          throw new ArgumentNullException(nameof (imei));
        default:
          EndianBinaryWriter endianBinaryWriter = stream != null ? stream.CreateSuitableBinaryWriter() : throw new ArgumentNullException(nameof (stream));
          endianBinaryWriter.Write((short)Encoding.GetByteCount(imei));
          byte[] bytes = Encoding.GetBytes(imei);
          endianBinaryWriter.Write(bytes);
          break;
      }
    }

    public static byte[] Encode(string imei)
    {
      switch (imei)
      {
        case "":
          throw new ArgumentException(nameof (imei));
        case null:
          throw new ArgumentNullException(nameof (imei));
        default:
          using (MemoryStream memoryStream = new MemoryStream(2 + Encoding.GetByteCount(imei)))
          {
                        Encode(imei, memoryStream);
            memoryStream.Position = 0L;
            using (BinaryReader binaryReader = new BinaryReader(memoryStream))
              return binaryReader.ReadBytes((int) memoryStream.Length);
          }
      }
    }

    public static string Decode(Stream stream)
    {
      EndianBinaryReader endianBinaryReader = stream != null ? stream.CreateSuitableBinaryReader() : throw new ArgumentNullException(nameof (stream));
      short num = endianBinaryReader.ReadInt16();
      byte[] bytes = endianBinaryReader.ReadBytes(num);
      return Encoding.GetString(bytes);
    }

    public static string Decode(byte[] bytes)
    {
      if (bytes == null)
        throw new ArgumentNullException(nameof (bytes));
      if (bytes.Length < 3)
        throw new ArgumentException(nameof (bytes));
      using (MemoryStream memoryStream = new MemoryStream(bytes))
        return Decode(memoryStream);
    }
  }
}
