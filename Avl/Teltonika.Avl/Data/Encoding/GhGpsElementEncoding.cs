// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.Encoding.GhGpsElementEncoding
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.Collections.Generic;
using Teltonika.IO;

namespace Teltonika.Avl.Data.Encoding
{
  public sealed class GhGpsElementEncoding : IEncoding<GpsElementExt>
  {
    private static GhGpsElementEncoding _instance;

    public static GhGpsElementEncoding Instance => _instance ?? (_instance = new GhGpsElementEncoding());

    public void Encode(GpsElementExt obj, IBitWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      GhGpsElementMask mask = obj.GetMask();
      writer.Write((byte) mask);
      if (mask.HasFlag(GhGpsElementMask.Coordinates))
      {
        writer.Write((float) obj.GPS.Latitude);
        writer.Write((float) obj.GPS.Longitude);
      }
      if (mask.HasFlag(GhGpsElementMask.Altitude))
        writer.Write(obj.GPS.Altitude);
      if (mask.HasFlag(GhGpsElementMask.Angle))
        writer.Write((byte) Math.Round(obj.GPS.Angle * 256.0 / 360.0));
      if (mask.HasFlag(GhGpsElementMask.Speed))
        writer.Write((byte) obj.GPS.Speed);
      if (mask.HasFlag(GhGpsElementMask.Satellites))
        writer.Write(obj.GPS.Satellites);
      if (mask.HasFlag(GhGpsElementMask.CellId))
        writer.Write((int) obj.IO[200]);
      if (mask.HasFlag(GhGpsElementMask.SignalQuality))
        writer.Write((byte) obj.IO[201]);
      if (mask.HasFlag(GhGpsElementMask.OperatorCode))
        writer.Write((int) obj.IO[202]);
      writer.Flush();
    }

    public GpsElementExt Decode(IBitReader reader)
    {
      GhGpsElementMask mask = reader != null ? (GhGpsElementMask) reader.ReadByte() : throw new ArgumentNullException(nameof (reader));
      int x = 0;
      int y = 0;
      if (mask.HasFlag(GhGpsElementMask.Coordinates))
      {
        float num1 = reader.ReadSingle();
        float num2 = reader.ReadSingle();
        if (!GpsElement.IsLatValid(num1))
          num1 = 0.0f;
        if (!GpsElement.IsLngValid(num2))
          num2 = 0.0f;
        y = (int) (num1 * 10000000.0);
        x = (int) (num2 * 10000000.0);
      }
      short altitude = 0;
      if (mask.HasFlag(GhGpsElementMask.Altitude))
        altitude = reader.ReadInt16();
      short angle = 0;
      if (mask.HasFlag(GhGpsElementMask.Angle))
        angle = (short) Math.Round(reader.ReadByte() * 360.0 / 256.0);
      short speed = 0;
      if (mask.HasFlag(GhGpsElementMask.Speed))
        speed = reader.ReadByte();
      byte satellites = mask.HasFlag(GhGpsElementMask.Satellites) ? reader.ReadByte() : (byte) 3;
      List<IoProperty> properties = new List<IoProperty>(3);
      if (mask.HasFlag(GhGpsElementMask.CellId))
      {
        int num = reader.ReadInt32();
        properties.Add(IoProperty.Create(200, num));
      }
      if (mask.HasFlag(GhGpsElementMask.SignalQuality))
      {
        byte num = reader.ReadByte();
        properties.Add(IoProperty.Create(201, num));
      }
      if (mask.HasFlag(GhGpsElementMask.OperatorCode))
      {
        int num = reader.ReadInt32();
        properties.Add(IoProperty.Create(202, num));
      }
      if (x == 0 && y == 0)
      {
        speed = byte.MaxValue;
        satellites = 0;
      }
      return new GpsElementExt(new GpsElement(x, y, altitude, speed, angle, satellites), new IoElement(0, properties));
    }
  }
}
