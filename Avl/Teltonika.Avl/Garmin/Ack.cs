// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Garmin.Ack
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

namespace Teltonika.Avl.Garmin
{
  public sealed class Ack : GarminPacket
  {
    public Ack()
      : base(6)
    {
    }

    public Ack(byte acknowledgedPacketId)
      : this()
    {
      AckPacketId = acknowledgedPacketId;
      Payload = new byte[2]
      {
        acknowledgedPacketId,
         0
      };
    }

    public byte AckPacketId { get; private set; }

    protected override void parsePayload()
    {
      base.parsePayload();
      if (Payload == null || Payload.Length <= 0)
        return;
      AckPacketId = Payload[0];
    }

    public override string ToString() => base.ToString() + ", AckId=" + AckPacketId;
  }
}
