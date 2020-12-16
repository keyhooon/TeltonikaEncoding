// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Garmin.Nak
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

namespace Teltonika.Avl.Garmin
{
  public sealed class Nak : GarminPacket
  {
    public Nak()
      : base(21)
    {
    }

    public Nak(byte acknowgedgedPacketId)
      : this()
    {
      NakPacketId = acknowgedgedPacketId;
      Payload = new byte[2]
      {
        acknowgedgedPacketId,
         0
      };
    }

    public byte NakPacketId { get; private set; }

    protected override void parsePayload()
    {
      base.parsePayload();
      if (Payload == null || Payload.Length <= 0)
        return;
      NakPacketId = Payload[0];
    }

    public override string ToString() => base.ToString() + ", NakId=" + NakPacketId;
  }
}
