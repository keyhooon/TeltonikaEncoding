// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.GpsElementExt
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Teltonika.Avl
{
  public struct GpsElementExt
  {
    private readonly GpsElement _gps;
    private readonly IoElement _io;

    public GpsElementExt(GpsElement gps, IoElement io)
    {
      _gps = gps;
      _io = io;
    }

    public static GpsElementExt Create(AvlData data, params int[] propertyIds) => new GpsElementExt(data.GPS, new IoElement(0, data.Data.Where(x => ((IEnumerable<int>)propertyIds).Contains(x.Id))));

    public GpsElement GPS => _gps;

    public IoElement IO => _io;
  }
}
