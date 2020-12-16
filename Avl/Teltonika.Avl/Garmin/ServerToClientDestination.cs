// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Garmin.ServerToClientDestination
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.IO;
using System.Text;
using Teltonika.IO;

namespace Teltonika.Avl.Garmin
{
  public class ServerToClientDestination : GarminFleetManagementPacket
  {
    private const short FLEET_MANAGEMENT_PACKET_ID = 257;

    public ServerToClientDestination()
      : base(257)
    {
    }

    public ServerToClientDestination(DateTime date, string text, uint id, double x, double y)
      : base(257)
    {
      OriginationTime = new DateTime?(date);
      MessageText = text;
      X = x;
      Y = y;
      UniqueId = id;
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
        endianBinaryWriter.Write((short) 257);
        endianBinaryWriter.Write(formatOriginationTime(new DateTime?(date)));
        endianBinaryWriter.Write(convertDegreesToSemicircles(y));
        endianBinaryWriter.Write(convertDegreesToSemicircles(x));
        endianBinaryWriter.Write(id);
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

    public double X { private set; get; }

    public double Y { private set; get; }

    public uint UniqueId { private set; get; }

    public override string ToString() => "ServerToClientDestination: OriginationTime=" + OriginationTime.ToString() + ", MessageText=" + MessageText + ", MessageId=" + UniqueId + ", X=" + X + ", Y=" + Y + ", " + base.ToString();
  }
}
