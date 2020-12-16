// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.Encoding.DefaultGpsElementEncoding
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using Teltonika.IO;

namespace Teltonika.Avl.Data.Encoding
{
  public sealed class DefaultGpsElementEncoding : IEncoding<GpsElement>
  {
    private static DefaultGpsElementEncoding _instance;

    public static DefaultGpsElementEncoding Instance => _instance ?? (_instance = new DefaultGpsElementEncoding());

    private DefaultGpsElementEncoding()
    {
    }

    public void Encode(GpsElement obj, IBitWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      writer.Write(obj.X);
      writer.Write(obj.Y);
      writer.Write(obj.Altitude);
      writer.Write(obj.Angle);
      writer.Write(obj.Satellites);
      writer.Write(obj.Speed);
      writer.Flush();
    }

    public GpsElement Decode(IBitReader reader)
    {
      int x = reader != null ? reader.ReadInt32() : throw new ArgumentNullException(nameof (reader));
      int y = reader.ReadInt32();
      short altitude = reader.ReadInt16();
      short angle = reader.ReadInt16();
      byte satellites = reader.ReadByte();
      short speed = reader.ReadInt16();
      return new GpsElement(x, y, altitude, speed, angle, satellites);
    }
  }
}
