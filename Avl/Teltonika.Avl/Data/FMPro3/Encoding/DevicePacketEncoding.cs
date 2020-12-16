// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.FMPro3.Encoding.DevicePacketEncoding
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using Teltonika.Avl.Communication.FMPro3;
using Teltonika.Avl.Data.FMPro3.Commands;
using Teltonika.IO;

namespace Teltonika.Avl.Data.FMPro3.Encoding
{
  public sealed class DevicePacketEncoding : IEncoding<DevicePacket>
  {
    public static IEncoding<DevicePacket> Instance = new DevicePacketEncoding();

    public void Encode(DevicePacket packet, IBitWriter writer)
    {
      if (packet == null)
        throw new ArgumentNullException(nameof (packet));
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      writer.Write(packet.IMEI);
      Command command = packet.Command;
      int id = command != null ? command.Id : throw new ArgumentException("The packet does not contain a valid command.", nameof (packet));
      if (id <= 0 || byte.MaxValue < id)
        throw new ArgumentException("The packet command id is not valid.", nameof (packet));
      writer.Write((byte) id);
      CommandEncodingFactory.Instance.Create(id).Encode(command, writer);
    }

    public DevicePacket Decode(IBitReader reader)
    {
      ulong imei = reader != null ? reader.ReadUInt64() : throw new ArgumentNullException(nameof (reader));
      byte num = reader.ReadByte();
      Command command = CommandEncodingFactory.Instance.Create(num).Decode(reader);
      return new DevicePacket(imei, command);
    }
  }
}
