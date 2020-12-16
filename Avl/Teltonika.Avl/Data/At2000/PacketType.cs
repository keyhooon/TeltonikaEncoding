// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.At2000.PacketType
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;

namespace Teltonika.Avl.Data.At2000
{
  [Flags]
  public enum PacketType : byte
  {
    Ack = 0,
    Imei = 1,
    StatusRequest = 6,
    StatusRequestEncoded = 134, // 0x86
    StatusResponse = StatusRequest | Imei, // 0x07
    StatusResponseEncoded = 135, // 0x87
    TrackRequest = 8,
    TrackRequestEncoded = 136, // 0x88
    TrackResponse = TrackRequest | Imei, // 0x09
    TrackResponseEncoded = 137, // 0x89
    SystemLogRequest = 10, // 0x0A
    SystemLogRequestEncoded = 138, // 0x8A
    SystemLogResponse = SystemLogRequest | Imei, // 0x0B
    SystemLogResponseEncoded = 139, // 0x8B
    FirmwareUpload = 5,
    FirmwareUploadEncoded = 133, // 0x85
    FirmwareUpdateStatusRequest = FirmwareUpload | TrackRequest, // 0x0D
    FirmwareUpdateStatusRequestEncoded = 141, // 0x8D
    FirmwareUpdateStatusResponse = 14, // 0x0E
    FirmwareUpdateStatusResponseEncoded = 142, // 0x8E
    CloseSession = 12, // 0x0C
    Encrypted = 128, // 0x80
    Unknown = 255, // 0xFF
  }
}
