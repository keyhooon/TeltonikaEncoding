// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Commands.Command
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System.IO;
using Teltonika.IO;

namespace Teltonika.Avl.Commands
{
  public class Command
  {
    public Command(byte id) => Id = id;

    public byte Id { get; private set; }

    public virtual byte[] Data { get; set; }

    public virtual void Encode(IBitWriter writer)
    {
      writer.Write(Id);
      writer.Write(Data.Length);
      writer.Write(Data);
      writer.Flush();
    }

    public byte[] Encode()
    {
      using (MemoryStream stream = new MemoryStream())
      {
        using (IBitWriter suitableBitWriter = stream.CreateSuitableBitWriter())
        {
          Encode(suitableBitWriter);
          stream.Position = 0L;
          using (BinaryReader binaryReader = new BinaryReader(stream))
            return binaryReader.ReadBytes((int) stream.Length);
        }
      }
    }

    public virtual Command Decode(byte id, IBitReader reader)
    {
      Command command = new Command(id);
      int count = reader.ReadInt32();
      command.Data = reader.ReadBytes(count);
      return command;
    }
  }
}
