// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.RawValueTransform
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;

namespace Teltonika.Avl.Data
{
  public static class RawValueTransform
  {
    public static double PolynomialTransform(
      double value,
      double a0,
      double a1,
      double a2,
      double a3)
    {
      double num1 = value * value;
      double num2 = num1 * value;
      return a0 + value * a1 + num1 * a2 + num2 * a3;
    }

    public static double Encode(double value, RawValueEncoding encoding)
    {
      switch (encoding)
      {
        case RawValueEncoding.Unsigned1Byte:
          return (long)value & byte.MaxValue;
        case RawValueEncoding.Unsigned2Byte:
          return (long)value & ushort.MaxValue;
        case RawValueEncoding.Unsigned4Byte:
          return (long)value & uint.MaxValue;
        case RawValueEncoding.Unsigned8Byte:
          return Math.Abs((long)value);
        default:
          return value;
      }
    }
  }
}
