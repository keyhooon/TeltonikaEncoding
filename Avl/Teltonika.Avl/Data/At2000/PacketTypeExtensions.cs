// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.At2000.PacketTypeExtensions
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

namespace Teltonika.Avl.Data.At2000
{
  public static class PacketTypeExtensions
  {
    public static bool HasFlag(this PacketType packetType, PacketType flag) => (packetType & flag) == flag;

    public static bool IsEncrypted(this PacketType packetType) => packetType.HasFlag(PacketType.Encrypted);

    public static bool SupportsEncryption(this PacketType packetType)
    {
      if (packetType.HasFlag(PacketType.Encrypted))
        return true;
      switch (packetType)
      {
        case PacketType.FirmwareUpload:
        case PacketType.StatusRequest:
        case PacketType.StatusResponse:
        case PacketType.TrackRequest:
        case PacketType.TrackResponse:
        case PacketType.SystemLogRequest:
        case PacketType.SystemLogResponse:
        case PacketType.FirmwareUpdateStatusRequest:
        case PacketType.FirmwareUpdateStatusResponse:
        case PacketType.FirmwareUploadEncoded:
        case PacketType.StatusRequestEncoded:
        case PacketType.StatusResponseEncoded:
        case PacketType.TrackRequestEncoded:
        case PacketType.TrackResponseEncoded:
        case PacketType.SystemLogRequestEncoded:
        case PacketType.SystemLogResponseEncoded:
        case PacketType.FirmwareUpdateStatusRequestEncoded:
        case PacketType.FirmwareUpdateStatusResponseEncoded:
          return true;
        default:
          return false;
      }
    }
  }
}
