// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.AvlData
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using Teltonika.Avl.Data;

namespace Teltonika.Avl
{
  public sealed class AvlData : IEquatable<AvlData>
  {
    private readonly AvlDataPriority _priority;
    private DateTime _dateTime;
    private readonly GpsElement _gps;
    private IoElement _data;

    public AvlData(AvlDataPriority priority, DateTime dateTime, GpsElement gps, IoElement data = null)
    {
      _priority = priority;
      _dateTime = dateTime;
      _gps = gps;
      _data = data;
    }

    public AvlDataPriority Priority => _priority;

    public DateTime DateTime
    {
      get => _dateTime;
      set => _dateTime = value;
    }

    public GpsElement GPS => _gps;

    public IoElement Data
    {
      get
      {
        IoElement ioElement = _data;
        if ((object) ioElement == null)
          ioElement = _data = new IoElement();
        return ioElement;
      }
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj))
        return false;
      return ReferenceEquals(this, obj) || (object) (obj as AvlData) != null && Equals((AvlData) obj);
    }

    public bool Equals(AvlData other)
    {
      if (ReferenceEquals(null, other))
        return false;
      return ReferenceEquals(this, other) || _priority == other._priority && _dateTime.Equals(other._dateTime) && _gps.Equals(other._gps) && Equals(_data, other._data);
    }

    public override int GetHashCode() => (((int) _priority * 397 ^ _dateTime.GetHashCode()) * 397 ^ _gps.GetHashCode()) * 397 ^ (_data != null ? _data.GetHashCode() : 0);

    public static bool operator ==(AvlData left, AvlData right) => Equals(left, right);

    public static bool operator !=(AvlData left, AvlData right) => !Equals(left, right);

    public override string ToString() => string.Format("AvlData: Timestamp={0}, Priority={1}, [{2}], [{3}]", DateTime, Priority, GPS, Data);
  }
}
