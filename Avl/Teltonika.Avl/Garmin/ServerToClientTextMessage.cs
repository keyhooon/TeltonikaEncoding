// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Garmin.ServerToClientTextMessage
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.IO;
using System.Text;
using Teltonika.IO;

namespace Teltonika.Avl.Garmin
{
  public class ServerToClientTextMessage : GarminFleetManagementPacket
  {
    private const short FLEET_MANAGEMENT_PACKET_ID = 42;

    public ServerToClientTextMessage(DateTime date, string text, byte[] messageId, byte type)
      : base(42)
    {
      OriginationTime = new DateTime?(date);
      MessageText = text;
      MessageId = messageId;
      Type = type;
      if (messageId != null && messageId.Length > 16)
        throw new ArgumentException("Max Id size is 16: " + messageId.Length);
      byte[] bytes;
      try
      {
        bytes = new UTF8Encoding().GetBytes(text);
        if (bytes.Length > 199)
          throw new ArgumentException("Max text size is 199 bytes: " + bytes.Length);
      }
      catch (EncoderFallbackException ex)
      {
        throw new SystemException("Server doesn't support UTF-8", ex);
      }
      using (BitStream bitStream = new BitStream())
      {
        EndianBinaryWriter endianBinaryWriter = new EndianBinaryWriter(EndianBitConverter.Little, bitStream);
        endianBinaryWriter.Write((short) 42);
        endianBinaryWriter.Write(formatOriginationTime(new DateTime?(date)));
        endianBinaryWriter.Write(messageId == null ? (byte) 0 : (byte) messageId.Length);
        endianBinaryWriter.Write(type);
        endianBinaryWriter.Write((ushort) 0);
        if (messageId != null)
          endianBinaryWriter.Write(messageId);
        for (int length = messageId.Length; length < 16; ++length)
          endianBinaryWriter.Write((byte) 0);
        endianBinaryWriter.Write(bytes);
        endianBinaryWriter.Write(0);
        Payload = bitStream.ToByteArray();
      }
    }

    public override byte[] Payload
    {
      get => base.Payload;
      set => base.Payload = value;
    }

    public string MessageText { private set; get; }

    public DateTime? OriginationTime { private set; get; }

    public byte Type { private set; get; }

    public byte[] MessageId { private set; get; }

    public override string ToString() => base.ToString() + ", Text=" + MessageText;
  }
}
