// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.CodecRegistry
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Teltonika.Avl.Communication;

namespace Teltonika.Avl
{
  public sealed class CodecRegistry : IEnumerable<ICodec>, IEnumerable
  {
    public static readonly CodecRegistry Instance = new CodecRegistry();
    private readonly List<ICodec> _codecs = new List<ICodec>();

    public void Register(ICodec codec)
    {
      if (codec == null)
        throw new ArgumentNullException(nameof (codec));
      if (this[codec.Id] != null)
        throw new ApplicationException("Codec ID=" + codec.Id + " already registered");
      _codecs.Add(codec);
    }

    public void Unregister(ICodec codec)
    {
      if (codec == null)
        throw new ArgumentNullException(nameof (codec));
      if (!_codecs.Remove(codec))
        throw new ApplicationException("Codec not registered");
    }

    public ICodec this[byte[] data] => data.Length == 0 ? null : this[data[0]];

    public ICodec this[byte codecId] => _codecs.FirstOrDefault(c => c.Id == codecId);

    public ICodec this[UdpPacket packet]
    {
      get
      {
        UdpData udpData = !(packet == null) ? packet.Payload as UdpData : throw new ArgumentNullException(nameof (packet));
        if (udpData == null || udpData.Data == null || udpData.Data.Length == 0)
          throw new InvalidOperationException("No data");
        return this[udpData.Data[0]];
      }
    }

    public ICodec this[TcpPacket packet]
    {
      get
      {
        if (packet == null)
          throw new ArgumentNullException(nameof (packet));
        if (packet.Data == null || packet.Data.Length == 0)
          throw new InvalidOperationException("No data");
        return this[packet.Data[0]];
      }
    }

    public IEnumerator<ICodec> GetEnumerator() => _codecs.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _codecs.GetEnumerator();
  }
}
