// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Communication.FMPro3.Packet
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using Teltonika.Avl.Data.FMPro3.Commands;

namespace Teltonika.Avl.Communication.FMPro3
{
  public abstract class Packet
  {
    private readonly Command _command;

    protected Packet(Command command) => _command = command;

    public Command Command => _command;

    public abstract override string ToString();
  }
}
