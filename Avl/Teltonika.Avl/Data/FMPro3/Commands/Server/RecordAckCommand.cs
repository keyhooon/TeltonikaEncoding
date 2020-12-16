// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.FMPro3.Commands.Server.RecordAckCommand
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System.IO;
using Teltonika.IO;

namespace Teltonika.Avl.Data.FMPro3.Commands.Server
{
  public sealed class RecordAckCommand : Command
  {
    private const int COMMAND_ID = 100;
    private readonly bool _ok;

    public RecordAckCommand(bool ok) => _ok = ok;

    public override int Id => 100;

    public bool Ok => _ok;

    public override string ToString()
    {
      using (StringWriter stringWriter = new StringWriter())
      {
        stringWriter.Write("RECORD ACK ");
        stringWriter.Write(Ok ? "OK" : "ERROR");
        return stringWriter.ToString();
      }
    }

    public sealed class Encoding : CommandEncoding<RecordAckCommand>
    {
      public override int CommandId => 100;

      protected override void EncodeCore(RecordAckCommand command, IBitWriter writer) => writer.Write(command.Ok ? (byte) 1 : (byte) 0);

      protected override RecordAckCommand DecodeCore(IBitReader reader) => new RecordAckCommand(reader.ReadByte() == 1);
    }
  }
}
