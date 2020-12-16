// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Tools.DateTimeExt
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;

namespace Teltonika.Avl.Tools
{
  public static class DateTimeExt
  {
    private static readonly DateTime AvlEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
    private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

    public static bool IsAvlValid(DateTime dateTime) => dateTime > AvlEpoch;

    public static int ToUnix(DateTime dateTime) => (int) ((dateTime - UnixEpoch).TotalSeconds + 0.5);

    public static DateTime FromUnix(int timestamp) => UnixEpoch.AddSeconds(timestamp);

    public static long ToAvl(DateTime dateTime) => (long) ((dateTime - AvlEpoch).TotalMilliseconds + 0.5);

    public static DateTime FromAvl(long timestamp) => AvlEpoch.AddMilliseconds(timestamp);
  }
}
