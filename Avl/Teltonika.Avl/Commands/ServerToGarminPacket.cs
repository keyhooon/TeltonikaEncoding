// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Commands.ServerToGarminPacket
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using Teltonika.Avl.Garmin;

namespace Teltonika.Avl.Commands
{
  public sealed class ServerToGarminPacket : Command, IGarminCommand
  {
    public ServerToGarminPacket()
      : base(7)
    {
    }

    public ServerToGarminPacket(GarminPacket garminPacket)
      : this()
    {
      GarminPacket = garminPacket;
      Data = GarminEncoding.Instance.Encode(garminPacket);
    }

    public GarminPacket GarminPacket { get; set; }
  }
}
