// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Garmin.GarminFleetManagementPacket
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;

namespace Teltonika.Avl.Garmin
{
  public class GarminFleetManagementPacket : GarminPacket
  {
    private const uint INVALID_TIME = 4294967295;
    public const byte PacketId = 161;
    private static readonly DateTime StartingPoint = new DateTime(1989, 12, 31, 0, 0, 0, DateTimeKind.Utc);

    public static int convertDegreesToSemicircles(double degrees) => (int) (degrees * (Math.Pow(2.0, 31.0) / 180.0));

    public static double convertSemicirclesToDegrees(int semicirlcles) => semicirlcles * (180.0 / Math.Pow(2.0, 31.0));

    public static DateTime? parseOriginationTime(uint seconds) => seconds != uint.MaxValue ? new DateTime?(StartingPoint.AddSeconds(seconds)) : new DateTime?();

    public static uint formatOriginationTime(DateTime? date)
    {
      if (!date.HasValue)
        return uint.MaxValue;
      date = new DateTime?(date.Value.ToUniversalTime());
      double totalSeconds = (date.Value - StartingPoint).TotalSeconds;
      return totalSeconds < 0.0 ? uint.MaxValue : (uint) totalSeconds;
    }

    public GarminFleetManagementPacket(short fleetManagementPacketId)
      : base(161)
      => FleetManagementPacketId = fleetManagementPacketId;

    public short FleetManagementPacketId { get; private set; }

    public byte[] FleetManagementPayload
    {
      get
      {
        if (Payload == null || Payload.Length <= 2)
          return new byte[0];
        byte[] numArray = new byte[Payload.Length - 2];
        Buffer.BlockCopy(payload, 2, numArray, 0, numArray.Length);
        return numArray;
      }
    }

    public override string ToString() => base.ToString() + ", FMPacketId=" + FleetManagementPacketId;
  }
}
