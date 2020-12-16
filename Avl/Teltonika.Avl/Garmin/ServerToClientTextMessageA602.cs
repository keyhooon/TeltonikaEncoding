// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Garmin.ServerToClientTextMessageA602
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.IO;
using System.Text;
using Teltonika.IO;

namespace Teltonika.Avl.Garmin
{
  public class ServerToClientTextMessageA602 : GarminFleetManagementPacket
  {
    private const short FLEET_MANAGEMENT_PACKET_ID = 33;

    public ServerToClientTextMessageA602(DateTime date, string text)
      : base(33)
    {
      OriginationTime = new DateTime?(date);
      MessageText = text;
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
        endianBinaryWriter.Write((short) 33);
        endianBinaryWriter.Write(formatOriginationTime(new DateTime?(date)));
        endianBinaryWriter.Write(bytes);
        endianBinaryWriter.Write(0);
        Payload = bitStream.ToByteArray();
      }
    }

    public string MessageText { private set; get; }

    public DateTime? OriginationTime { private set; get; }

    public override byte[] Payload
    {
      get => base.Payload;
      set => base.Payload = value;
    }

    public override string ToString() => base.ToString() + ", Text=" + MessageText;
  }
}
