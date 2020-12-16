// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.At2000.DataPacket
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

namespace Teltonika.Avl.Data.At2000
{
  public static class DataPacket
  {
    public const int MaxSegmentSize = 496;

    public static int CalculateExpectedByteCount(int count) => (count & 15) == 0 ? count : (count >> 4) + 1 << 4;
  }
}
