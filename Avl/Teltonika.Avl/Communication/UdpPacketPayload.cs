// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Communication.UdpPacketPayload
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;

namespace Teltonika.Avl.Communication
{
  public abstract class UdpPacketPayload : IEquatable<UdpPacketPayload>
  {
    public byte AvlPacketId { get; set; }

    public virtual int Size => 1;

    public override int GetHashCode() => AvlPacketId.GetHashCode();

    public override bool Equals(object obj)
    {
      UdpPacketPayload other = obj as UdpPacketPayload;
      return (object) other != null && Equals(other);
    }

    public bool Equals(UdpPacketPayload other) => (object) other != null && AvlPacketId == other.AvlPacketId;

    public static bool operator ==(UdpPacketPayload first, UdpPacketPayload second)
    {
      if (ReferenceEquals(first, second))
        return true;
      return (object) first != null && (object) second != null && first.Equals(second);
    }

    public static bool operator !=(UdpPacketPayload first, UdpPacketPayload second) => !(first == second);
  }
}
