// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.FMPro3.Commands.CommandEncoding`1
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using Teltonika.IO;

namespace Teltonika.Avl.Data.FMPro3.Commands
{
  public abstract class CommandEncoding<T> : ICommandEncoding, IEncoding<Command>
    where T : Command
  {
    public abstract int CommandId { get; }

    public void Encode(Command command, IBitWriter writer)
    {
      if (command == null)
        throw new ArgumentNullException(nameof (command));
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      if (!(command is T command1))
        throw new ArgumentException("The command type is not supported.");
      EncodeCore(command1, writer);
    }

    public Command Decode(IBitReader reader)
    {
      T obj = reader != null ? DecodeCore(reader) : throw new ArgumentNullException(nameof (reader));
      return obj != null ? obj : throw new NotSupportedException("The encoding was unable to decode the command.");
    }

    protected abstract void EncodeCore(T command, IBitWriter writer);

    protected abstract T DecodeCore(IBitReader reader);
  }
}
