// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Garmin.ClientToServerOpenTextMessage
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.IO;
using System.Text;
using Teltonika.IO;

namespace Teltonika.Avl.Garmin
{
  public class ClientToServerOpenTextMessage : GarminFleetManagementPacket
  {
    private const short FLEET_MANAGEMENT_PACKET_ID = 38;

    public ClientToServerOpenTextMessage()
      : base(38)
    {
    }

    public ClientToServerOpenTextMessage(
      DateTime originationTime,
      int uniqueId,
      string text,
      double x,
      double y,
      byte size = 0,
      byte[] serverMessageId = null)
      : base(38)
    {
      OriginationTime = new DateTime?(originationTime);
      UniqueId = uniqueId;
      Text = text;
      X = x;
      Y = y;
      Size = size;
      ServerMessageId = serverMessageId == null || serverMessageId.Length != 16 ? new byte[16] : serverMessageId;
      formatPayload();
    }

    public override byte[] Payload
    {
      get => base.Payload;
      set => base.Payload = value;
    }

    public DateTime? OriginationTime { get; set; }

    public int UniqueId { get; set; }

    public string Text { get; set; }

    public double X { get; set; }

    public double Y { get; set; }

    public byte Size { get; set; }

    public byte[] ServerMessageId { get; set; }

    protected override void formatPayload()
    {
      byte[] bytes;
      try
      {
        bytes = new UTF8Encoding().GetBytes(Text);
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
        endianBinaryWriter.Write((short) 38);
        endianBinaryWriter.Write(formatOriginationTime(OriginationTime));
        endianBinaryWriter.Write(convertDegreesToSemicircles(Y));
        endianBinaryWriter.Write(convertDegreesToSemicircles(X));
        endianBinaryWriter.Write(UniqueId);
        endianBinaryWriter.Write(Size);
        endianBinaryWriter.Write(new byte[3]);
        endianBinaryWriter.Write(ServerMessageId);
        endianBinaryWriter.Write(bytes);
        endianBinaryWriter.Write(0);
        Payload = bitStream.ToByteArray();
      }
    }

    protected override void parsePayload()
    {
      using (MemoryStream memoryStream = new MemoryStream(FleetManagementPayload))
      {
        EndianBinaryReader endianBinaryReader = new EndianBinaryReader(EndianBitConverter.Little, memoryStream);
        OriginationTime = parseOriginationTime(endianBinaryReader.ReadUInt32());
        Y = convertSemicirclesToDegrees(endianBinaryReader.ReadInt32());
        X = convertSemicirclesToDegrees(endianBinaryReader.ReadInt32());
        UniqueId = (int) endianBinaryReader.ReadUInt32();
        Size = endianBinaryReader.ReadByte();
        endianBinaryReader.ReadBytes(3);
        ServerMessageId = endianBinaryReader.ReadBytes(16);
        try
        {
          byte[] numArray = new byte[endianBinaryReader.BaseStream.Length - endianBinaryReader.BaseStream.Position - 1L];
          endianBinaryReader.Read(numArray, 0, numArray.Length);
          Text = Encoding.UTF8.GetString(numArray);
        }
        catch (Exception ex)
        {
          throw new Exception("0x26 Parse message Text", ex);
        }
      }
    }

    public override string ToString() => base.ToString() + ", Text=" + Text + ", UID=" + UniqueId;
  }
}
