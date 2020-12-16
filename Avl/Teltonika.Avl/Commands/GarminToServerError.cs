// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Commands.GarminToServerError
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using Teltonika.Avl.Garmin;
using Teltonika.IO;

namespace Teltonika.Avl.Commands
{
  public sealed class GarminToServerError : Command, IGarminCommand
  {
    public GarminToServerError()
      : base(9)
    {
    }

    public GarminToServerError(int errorId)
      : this()
      => ErrorId = errorId;

    public int ErrorId { get; set; }

    public override Command Decode(byte id, IBitReader reader) => new GarminToServerError(reader.ReadInt32());

    public GarminPacket GarminPacket
    {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }
  }
}
