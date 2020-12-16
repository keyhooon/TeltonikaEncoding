// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.FMPro3.Commands.CommandEncodingFactory
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.Collections.Generic;
using Teltonika.Avl.Data.FMPro3.Commands.Device;
using Teltonika.Avl.Data.FMPro3.Commands.Server;

namespace Teltonika.Avl.Data.FMPro3.Commands
{
  public sealed class CommandEncodingFactory
  {
    public static readonly CommandEncodingFactory Instance = new CommandEncodingFactory();
    private readonly IDictionary<int, ICommandEncoding> _encodings = new Dictionary<int, ICommandEncoding>();

    private CommandEncodingFactory()
    {
      Add(new RecordCommand.Encoding());
      Add(new RecordAckCommand.Encoding());
    }

    public ICommandEncoding Create(int id)
    {
      ICommandEncoding commandEncoding;
      if (!_encodings.TryGetValue(id, out commandEncoding) || commandEncoding == null)
        throw new NotSupportedException(string.Format("The command encoding for command 0x{0:X} is not supported.", id));
      return commandEncoding;
    }

    private void Add(ICommandEncoding encoding)
    {
      if (encoding == null)
        throw new ArgumentNullException(nameof (encoding));
      _encodings.Add(encoding.CommandId, encoding);
    }
  }
}
