// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Commands.GarminToServerPacket
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using Teltonika.Avl.Garmin;
using Teltonika.IO;

namespace Teltonika.Avl.Commands
{
  public sealed class GarminToServerPacket : Command, IGarminCommand
  {
    public GarminToServerPacket()
      : base(8)
    {
    }

    public GarminToServerPacket(byte[] data)
      : this()
      => Data = data;

    public GarminPacket GarminPacket { get; set; }

    public override byte[] Data
    {
      get => base.Data;
      set
      {
        GarminPacket garminPacket = GarminEncoding.Instance.Decode(value);
        if (garminPacket == null)
          throw new ArgumentException("Failed to decode garmin packet.");
        base.Data = value;
        GarminPacket = garminPacket;
      }
    }

    public override Command Decode(byte id, IBitReader reader)
    {
      Command command = base.Decode(id, reader);
      GarminToServerPacket garminToServerPacket = new GarminToServerPacket();
      garminToServerPacket.Data = command.Data;
      return garminToServerPacket;
    }
  }
}
