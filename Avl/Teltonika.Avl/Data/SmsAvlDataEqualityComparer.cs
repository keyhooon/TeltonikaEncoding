// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.SmsAvlDataEqualityComparer
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.Collections.Generic;

namespace Teltonika.Avl.Data
{
  public sealed class SmsAvlDataEqualityComparer : IEqualityComparer<AvlData>
  {
    private static SmsAvlDataEqualityComparer _instance;

    public static SmsAvlDataEqualityComparer Instance => _instance ?? (_instance = new SmsAvlDataEqualityComparer());

    public bool Equals(AvlData x, AvlData y)
    {
      if (ReferenceEquals(x, y))
        return true;
      if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
        return false;
      GpsElement gps = x.GPS;
      int num1;
      if (!gps.IsValid)
      {
        gps = y.GPS;
        num1 = gps.IsValid ? 1 : 0;
      }
      else
        num1 = 1;
      if (num1 == 0)
        return true;
      if (x.DateTime != y.DateTime)
        return false;
      gps = x.GPS;
      int speed1 = gps.Speed;
      gps = y.GPS;
      int speed2 = gps.Speed;
      int num2;
      if (speed1 == speed2)
      {
        gps = x.GPS;
        int x1 = gps.X;
        gps = y.GPS;
        int x2 = gps.X;
        if (Math.Abs(x1 - x2) <= 1717)
        {
          gps = x.GPS;
          int y1 = gps.Y;
          gps = y.GPS;
          int y2 = gps.Y;
          num2 = Math.Abs(y1 - y2) <= 1717 ? 1 : 0;
          goto label_15;
        }
      }
      num2 = 0;
label_15:
      return num2 != 0;
    }

    public int GetHashCode(AvlData obj) => ReferenceEquals(obj, null) ? 0 : obj.GetHashCode();
  }
}
