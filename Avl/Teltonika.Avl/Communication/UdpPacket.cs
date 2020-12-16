// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Communication.UdpPacket
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.Text;

namespace Teltonika.Avl.Communication
{
  public sealed class UdpPacket : IEquatable<UdpPacket>
  {
    public short Id { get; set; }

    public UdpPacketType Type { get; set; }

    public UdpPacketPayload Payload { get; set; }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("UDP:").AppendFormat("Id={0}, Type={1}", Id, Type);
      if (Payload != null)
        stringBuilder.AppendFormat(",[{0}]", Payload);
      return stringBuilder.ToString();
    }

    public override int GetHashCode()
    {
      int num = Id.GetHashCode() ^ Type.GetHashCode();
      if (Payload != null)
        num ^= Payload.GetHashCode();
      return num;
    }

    public override bool Equals(object obj)
    {
      UdpPacket other = obj as UdpPacket;
      return (object) other != null && Equals(other);
    }

    public bool Equals(UdpPacket other) => (object) other != null && (Id == other.Id && Type == other.Type && Payload == other.Payload);

    public static bool operator ==(UdpPacket first, UdpPacket second)
    {
      if (ReferenceEquals(first, second))
        return true;
      return (object) first != null && (object) second != null && first.Equals(second);
    }

    public static bool operator !=(UdpPacket first, UdpPacket second) => !(first == second);
  }
}
