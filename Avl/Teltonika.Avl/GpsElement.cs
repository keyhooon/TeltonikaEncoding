// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.GpsElement
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using Teltonika.Logging;

namespace Teltonika.Avl
{
  public struct GpsElement : IEquatable<GpsElement>
  {
    public const int WgsPrecision = 10000000;
    public const int MinX = -1800000000;
    public const int MinY = -900000000;
    public const int MaxX = 1800000000;
    public const int MaxY = 900000000;
    public const int MinAngle = 0;
    public const int MaxAngle = 360;
    public const short InvalidGpsSpeed = 255;
    private static ILog Log = LogManager.GetLogger(typeof (GpsElement));
    public static readonly GpsElement Default = new GpsElement();
    private readonly int _x;
    private readonly int _y;
    private readonly short _altitude;
    private readonly short _speed;
    private readonly short _angle;
    private readonly byte _satellites;

    public static bool IsLatValid(double latitude) => -90.0 <= latitude && latitude <= 90.0;

    public static bool IsLngValid(double longitude) => -180.0 <= longitude && longitude <= 180.0;

    public GpsElement(int x, int y, short altitude, short speed, short angle, byte satellites)
    {
      bool flag1 = -1800000000 <= x && x < 1800000000;
      bool flag2 = -900000000 <= y && y < 900000000;
      bool flag3 = 0 <= angle && angle <= 360;
      if (!flag1 || !flag2 || !flag3)
                Log.WarnFormat("GPS Element not valid [X: {0}, Y: {1}, Angle: {2}]", x, y, angle);
      _x = x;
      _y = y;
      _altitude = altitude;
      _speed = speed;
      _angle = angle;
      _satellites = satellites;
    }

    public GpsElement(
      double latitude,
      double longitude,
      short altitude,
      short speed,
      short angle,
      byte satellites)
      : this((int) Math.Round(longitude * 10000000.0), (int) Math.Round(latitude * 10000000.0), altitude, speed, angle, satellites)
    {
    }

    public bool IsDefault => _x == 0 && _y == 0 && (_altitude == 0 && _speed == 0) && _angle == 0 && _satellites == 0;

    public bool IsValid => _speed != byte.MaxValue && (_speed != 0 || _angle != 0 || _altitude != 0 || _satellites != 0);

    public double Latitude => _y / 10000000.0;

    public double Longitude => _x / 10000000.0;

    public int X => _x;

    public int Y => _y;

    public short Altitude => _altitude;

    public short Speed => _speed;

    public short Angle => _angle;

    public byte Satellites => _satellites;

    public override bool Equals(object obj) => !ReferenceEquals(null, obj) && (obj is GpsElement other && Equals(other));

    public bool Equals(GpsElement other) => _x == other._x && _y == other._y && (_altitude == other._altitude && _speed == other._speed) && _angle == other._angle && _satellites == other._satellites;

    public override int GetHashCode() => ((((_x * 397 ^ _y) * 397 ^ _altitude.GetHashCode()) * 397 ^ _speed.GetHashCode()) * 397 ^ _angle.GetHashCode()) * 397 ^ _satellites.GetHashCode();

    public static bool operator ==(GpsElement left, GpsElement right) => left.Equals(right);

    public static bool operator !=(GpsElement left, GpsElement right) => !left.Equals(right);

    public override string ToString() => string.Format("GPS:X={0}, Y={1}, Alt={2}, Angle={3}, Sat={4}, Speed={5}]", X, Y, Altitude, Angle, Satellites, Speed);
  }
}
