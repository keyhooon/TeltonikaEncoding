// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Commands.CommandArrayEncoding
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using Teltonika.IO;

namespace Teltonika.Avl.Commands
{
  public sealed class CommandArrayEncoding : IEncoding<Command[]>
  {
    public static readonly CommandArrayEncoding Instance = new CommandArrayEncoding();

    public void Encode(Command[] commands, IBitWriter writer)
    {
      if (commands == null)
        throw new ArgumentNullException(nameof (commands));
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      writer.Write((byte) commands.Length);
      foreach (Command command in commands)
        CommandEncoding.Instance.Encode(command, writer);
      writer.Write((byte) commands.Length);
      writer.Flush();
    }

    public Command[] Decode(IBitReader reader)
    {
      byte num1 = reader != null ? reader.ReadByte() : throw new ArgumentNullException(nameof (reader));
      Command[] commandArray = new Command[1]
      {
        CommandEncoding.Instance.Decode(reader)
      };
      byte num2 = reader.ReadByte();
      if (num1 != num2)
        throw new ApplicationException("Incorrect number of commands");
      return commandArray;
    }
  }
}
