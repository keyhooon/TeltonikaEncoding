// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Communication.Extensions
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;

namespace Teltonika.Avl.Communication
{
  public static class Extensions
  {
    public static object Decode(this ICodec codec, UdpPacket packet)
    {
      UdpData udpData = !(packet == null) ? packet.Payload as UdpData : throw new ArgumentNullException(nameof (packet));
      if (udpData == null)
        throw new InvalidOperationException("No data");
      return codec.Decode(udpData.Data);
    }

    public static object Decode(this ICodec codec, TcpPacket packet)
    {
      if (packet == null)
        throw new ArgumentNullException(nameof (packet));
      return codec.Decode(packet.Data);
    }
  }
}
