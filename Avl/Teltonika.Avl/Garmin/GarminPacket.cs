// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Garmin.GarminPacket
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

namespace Teltonika.Avl.Garmin
{
  public class GarminPacket
  {
    protected byte[] payload;

    public GarminPacket(byte id) => Id = id;

    public byte Id { get; private set; }

    public virtual byte[] Payload
    {
      get => payload;
      set
      {
        payload = value;
        parsePayload();
      }
    }

    public override string ToString() => string.Format(GetType().Name + ": Id = {0}", Id);

    protected virtual void formatPayload()
    {
    }

    protected virtual void parsePayload()
    {
    }
  }
}
