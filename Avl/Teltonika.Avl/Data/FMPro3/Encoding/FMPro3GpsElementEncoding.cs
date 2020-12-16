// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.FMPro3.Encoding.FMPro3GpsElementEncoding
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.Collections.Generic;
using Teltonika.IO;

namespace Teltonika.Avl.Data.FMPro3.Encoding
{
  public sealed class FMPro3GpsElementEncoding : IEncoding<GpsElementExt>
  {
    private static FMPro3GpsElementEncoding _instance;

    public static FMPro3GpsElementEncoding Instance => _instance ?? (_instance = new FMPro3GpsElementEncoding());

    public void Encode(GpsElementExt element, IBitWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      writer.Write(element.GPS.X);
      IBitWriter bitWriter1 = writer;
      GpsElement gps = element.GPS;
      int y = gps.Y;
      bitWriter1.Write(y);
      IBitWriter bitWriter2 = writer;
      gps = element.GPS;
      int num1 = (short)(gps.Altitude * 10);
      bitWriter2.Write((short) num1);
      IBitWriter bitWriter3 = writer;
      gps = element.GPS;
      int num2 = (short)(gps.Angle * 100);
      bitWriter3.Write((short) num2);
      IBitWriter bitWriter4 = writer;
      gps = element.GPS;
      int satellites = gps.Satellites;
      bitWriter4.Write((byte) satellites);
      IBitWriter bitWriter5 = writer;
      gps = element.GPS;
      int speed = gps.Speed;
      bitWriter5.Write((short) speed);
      writer.Write((byte) element.IO[500001]);
    }

    public GpsElementExt Decode(IBitReader reader)
    {
      int x = reader != null ? reader.ReadInt32() : throw new ArgumentNullException(nameof (reader));
      int y = reader.ReadInt32();
      short altitude = (short) Math.Round(reader.ReadInt16() / 10.0);
      short angle = (short) Math.Round(reader.ReadUInt16() / 100.0);
      byte satellites = reader.ReadByte();
      short speed = reader.ReadInt16();
      IoProperty ioProperty = IoProperty.Create(500001, reader.ReadByte());
      return new GpsElementExt(new GpsElement(x, y, altitude, speed, angle, satellites), new IoElement(0, new List<IoProperty>(1)
      {
        ioProperty
      }));
    }
  }
}
