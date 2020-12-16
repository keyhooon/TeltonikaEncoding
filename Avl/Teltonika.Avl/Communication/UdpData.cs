// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Communication.UdpData
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Teltonika.Avl.Communication
{
  public sealed class UdpData : UdpPacketPayload, IEquatable<UdpData>
  {
    public string Imei { get; set; }

    public byte[] Data { get; set; }

    public override string ToString() => string.Format("Data:AvlPacketId={0}, Imei={1}, Data={2}", AvlPacketId, Imei, Data.ToHexString());

    public override int GetHashCode() => base.GetHashCode() ^ (Imei != null ? Imei.GetHashCode() : 1955087533) ^ (Data != null ? Data.GetHashCode() : 1955087533);

    public override bool Equals(object obj)
    {
      UdpData other = obj as UdpData;
      return (object) other != null && Equals(other);
    }

    public bool Equals(UdpData other) => (object) other != null && (Equals((UdpPacketPayload) other) && Imei == other.Imei && (Data == null && other.Data == null || ((IEnumerable<byte>) Data).SequenceEqual(other.Data)));

    public static bool operator ==(UdpData first, UdpData second)
    {
      if (ReferenceEquals(first, second))
        return true;
      return (object) first != null && (object) second != null && first.Equals(second);
    }

    public static bool operator !=(UdpData first, UdpData second) => !(first == second);
  }
}
