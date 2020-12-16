// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Communication.UdpResponse
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;

namespace Teltonika.Avl.Communication
{
  public sealed class UdpResponse : UdpPacketPayload, IEquatable<UdpResponse>
  {
    public byte AcceptedNum { get; set; }

    public override string ToString() => string.Format("Resp:AvlPacketId={0}, AccpNum={1}", AvlPacketId, AcceptedNum);

    public override int GetHashCode() => base.GetHashCode() ^ AcceptedNum.GetHashCode();

    public override bool Equals(object obj)
    {
      UdpResponse other = obj as UdpResponse;
      return (object) other != null && Equals(other);
    }

    public bool Equals(UdpResponse other) => (object) other != null && (Equals((UdpPacketPayload) other) && AcceptedNum == other.AcceptedNum);

    public static bool operator ==(UdpResponse first, UdpResponse second)
    {
      if (ReferenceEquals(first, second))
        return true;
      return (object) first != null && (object) second != null && first.Equals(second);
    }

    public static bool operator !=(UdpResponse first, UdpResponse second) => !(first == second);
  }
}
