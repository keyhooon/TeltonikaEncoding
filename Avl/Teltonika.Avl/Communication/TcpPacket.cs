// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Communication.TcpPacket
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

namespace Teltonika.Avl.Communication
{
  public sealed class TcpPacket
  {
    public byte[] Data { get; set; }

    public int Crc { get; set; }
  }
}
