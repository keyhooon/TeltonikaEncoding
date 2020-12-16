// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.Encoding.GhMaskExtensions
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

namespace Teltonika.Avl.Data.Encoding
{
  public static class GhMaskExtensions
  {
    public static bool HasFlag(this GhGpsElementMask mask, GhGpsElementMask flag) => (mask & flag) == flag;

    public static GhGpsElementMask GetMask(this GpsElementExt obj)
    {
      GhGpsElementMask ghGpsElementMask = ~(GhGpsElementMask.Coordinates | GhGpsElementMask.Altitude | GhGpsElementMask.Angle | GhGpsElementMask.Speed | GhGpsElementMask.Satellites | GhGpsElementMask.CellId | GhGpsElementMask.SignalQuality | GhGpsElementMask.OperatorCode);
      if (obj.GPS.X != 0 || obj.GPS.Y != 0)
        ghGpsElementMask |= GhGpsElementMask.Coordinates;
      if (obj.GPS.Altitude != 0)
        ghGpsElementMask |= GhGpsElementMask.Altitude;
      if (obj.GPS.Angle != 0)
        ghGpsElementMask |= GhGpsElementMask.Angle;
      if (obj.GPS.Speed != 0)
        ghGpsElementMask |= GhGpsElementMask.Speed;
      if (obj.GPS.Satellites != 3)
        ghGpsElementMask |= GhGpsElementMask.Satellites;
      IoProperty ioProperty = obj.IO[200];
      if (!ioProperty.IsDefault)
        ghGpsElementMask |= GhGpsElementMask.CellId;
      ioProperty = obj.IO[201];
      if (!ioProperty.IsDefault)
        ghGpsElementMask |= GhGpsElementMask.SignalQuality;
      ioProperty = obj.IO[202];
      if (!ioProperty.IsDefault)
        ghGpsElementMask |= GhGpsElementMask.OperatorCode;
      return ghGpsElementMask;
    }
  }
}
