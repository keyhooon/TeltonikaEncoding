// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.FMPro3.Encoding.ServerPacketEncoding
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using Teltonika.Avl.Communication.FMPro3;
using Teltonika.Avl.Data.FMPro3.Commands;
using Teltonika.IO;

namespace Teltonika.Avl.Data.FMPro3.Encoding
{
  public sealed class ServerPacketEncoding : IEncoding<ServerPacket>
  {
    public static readonly IEncoding<ServerPacket> Instance = new ServerPacketEncoding();

    public void Encode(ServerPacket packet, IBitWriter writer)
    {
      if (packet == null)
        throw new ArgumentNullException(nameof (packet));
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      Command command = packet.Command;
      int id = command != null ? command.Id : throw new ArgumentException("The packet does not contain a valid command.", nameof (packet));
      if (id <= 0 || byte.MaxValue < id)
        throw new ArgumentException("The packet command id is not valid.", nameof (packet));
      writer.Write((byte) id);
      CommandEncodingFactory.Instance.Create(id).Encode(command, writer);
    }

    public ServerPacket Decode(IBitReader reader)
    {
      byte num = reader != null ? reader.ReadByte() : throw new ArgumentNullException(nameof (reader));
      return new ServerPacket(CommandEncodingFactory.Instance.Create(num).Decode(reader));
    }
  }
}
