// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Extensions.ExtensionMethods
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.Collections.Generic;
using System.IO;
using Teltonika.Avl.Communication.FMPro3;
using Teltonika.Avl.Data.FMPro3.Encoding;
using Teltonika.IO;

namespace Teltonika.Avl.Extensions
{
  public static class ExtensionMethods
  {
    public static T[] SubArray<T>(this T[] data, int index, int length)
    {
      T[] objArray = new T[length];
      Array.Copy(data, index, objArray, 0, length);
      return objArray;
    }

    public static byte[] Encode<T>(this IEncoding<T> encoding, T value)
    {
      using (MemoryStream stream = new MemoryStream())
      {
        using (IBitWriter suitableBitWriter = stream.CreateSuitableBitWriter())
          encoding.Encode(value, suitableBitWriter);
        return stream.ToArray();
      }
    }

    public static T Decode<T>(this IEncoding<T> encoding, byte[] buffer)
    {
      using (MemoryStream memoryStream = new MemoryStream(buffer, false))
        return encoding.Decode(memoryStream);
    }

    public static T Decode<T>(this IEncoding<T> encoding, Stream stream)
    {
      using (IBitReader suitableBitReader = stream.CreateSuitableBitReader())
        return encoding.Decode(suitableBitReader);
    }

    public static byte[] Encode(this ServerPacket packet) => ServerPacketEncoding.Instance.Encode(packet);

    public static ServerPacket DecodeServicePacket(this byte[] buffer) => ServerPacketEncoding.Instance.Decode(buffer);

    public static byte[] Encode(this DevicePacket packet) => DevicePacketEncoding.Instance.Encode(packet);

    public static DevicePacket DecodeDevicePacket(this byte[] buffer) => DevicePacketEncoding.Instance.Decode(buffer);

    public static IEnumerable<ArraySegment<T>> ToSegments<T>(
      this T[] data,
      int offset,
      int size)
    {
      return new ArraySegment<T>(data, offset, data.Length - offset).ToSegments(size);
    }

    public static IEnumerable<ArraySegment<T>> ToSegments<T>(
      this ArraySegment<T> data,
      int size)
    {
      int offset = data.Offset;
      int offsetEnd;
      for (offsetEnd = offset + data.Count; offset + size <= offsetEnd; offset += size)
        yield return new ArraySegment<T>(data.Array, offset, size);
      if (offset < offsetEnd)
        yield return new ArraySegment<T>(data.Array, offset, offsetEnd - offset);
    }

    public static T[] ToArray<T>(this ArraySegment<T> segment)
    {
      T[] objArray = new T[segment.Count];
      Array.Copy(segment.Array, segment.Offset, objArray, 0, segment.Count);
      return objArray;
    }

    public static T[] ToArray<T>(this T[] array, int offset, int count) => new ArraySegment<T>(array, offset, count).ToArray();
  }
}
