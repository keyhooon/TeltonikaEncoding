// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Commands.Codec15Packet
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.IO;
using Teltonika.Avl.Tools;
using Teltonika.IO;

namespace Teltonika.Avl.Commands
{
  public class Codec15Packet
  {
    private const byte CodecId = 15;
    private byte[] payload = null;
    private byte messageType;
    private uint salt;

    public byte[] PacketBytes { get; private set; }

    public byte[] Payload => payload;

    public byte MessageType => messageType;

    public uint Salt => salt;

    public Codec15Packet(byte messageType, byte[] payload)
    {
      PacketBytes = new byte[14 + payload.Length];
      this.messageType = messageType;
      this.payload = payload;
      using (IBitWriter suitableBitWriter = new MemoryStream(PacketBytes).CreateSuitableBitWriter())
      {
        suitableBitWriter.Write(0);
        suitableBitWriter.Write(payload.Length);
        suitableBitWriter.Write((byte) 15);
        suitableBitWriter.Write(messageType);
        salt = (uint) new Random().Next(1073741823, int.MaxValue);
        suitableBitWriter.Write(salt);
        new Encrypter().Encrypt(salt, this.payload);
        suitableBitWriter.Write(this.payload);
        suitableBitWriter.Flush();
      }
    }
  }
}
