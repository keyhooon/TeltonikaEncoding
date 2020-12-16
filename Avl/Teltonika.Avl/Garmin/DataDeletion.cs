// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Garmin.DataDeletion
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System.IO;
using Teltonika.IO;

namespace Teltonika.Avl.Garmin
{
  public class DataDeletion : GarminFleetManagementPacket
  {
    private const short FLEET_MANAGEMENT_PACKET_ID = 560;

    public DataDeletion()
      : base(560)
    {
    }

    public DataDeletion(uint deletionType)
      : base(560)
    {
      Type = deletionType;
      using (BitStream bitStream = new BitStream())
      {
        EndianBinaryWriter endianBinaryWriter = new EndianBinaryWriter(EndianBitConverter.Little, bitStream);
        endianBinaryWriter.Write((short) 560);
        endianBinaryWriter.Write(Type);
        endianBinaryWriter.Write(0);
        Payload = bitStream.ToByteArray();
      }
    }

    public override byte[] Payload
    {
      get => base.Payload;
      set => base.Payload = value;
    }

    public uint Type { private set; get; }

    public override string ToString() => string.Format("DataDeletion: DeletionType={0}({1}), {2}", Type, ((DeletionType)Type).ToString(), base.ToString());

    public enum DeletionType
    {
      DELETE_STOPS,
      DELETE_MESSAGES,
      DELETE_ACTIVE_ROUTE,
      DELETE_CANNED_MESSAGES,
      DELETE_CANNED_RESPONSES,
      DELETE_GPI,
      DELETE_DRIVER_ID_STATUS,
      DELETE_ALL_DISABLE,
      DELETE_WAYPOINTS,
    }
  }
}
