// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Commands.GprsCommandPacket
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.Text;
using Teltonika.IO;

namespace Teltonika.Avl.Commands
{
  public sealed class GprsCommandPacket : Command
  {
    private const byte GprsCommandId = 5;

    private GprsCommandPacket(byte id)
      : base(id)
    {
    }

    public GprsCommandPacket()
      : this(5)
    {
    }

    public GprsCommandPacket(string command)
      : this()
      => Data = !string.IsNullOrEmpty(command) ? Encoding.UTF8.GetBytes(command) : throw new ArgumentException("The command must be a non empty string.", nameof (command));

    public static GprsCommandPacket FromCommand(Command command)
    {
      GprsCommandPacket gprsCommandPacket = command.Id == 5 || command.Id == 6 ? new GprsCommandPacket(command.Id) : throw new ArgumentException("The command is not supported command (id must be 5 or 6).");
      gprsCommandPacket.Data = command.Data;
      return gprsCommandPacket;
    }

    public override string ToString() => Encoding.UTF8.GetString(Data);

    public override Command Decode(byte id, IBitReader reader)
    {
      if (id != 5)
        throw new ArgumentOutOfRangeException(nameof (id), id, string.Format("The id for GPRS command must be {0} but instead is {1}.", (byte)5, id));
      GprsCommandPacket gprsCommandPacket = new GprsCommandPacket();
      gprsCommandPacket.Data = reader.ReadBytes(reader.ReadInt32());
      return gprsCommandPacket;
    }
  }
}
