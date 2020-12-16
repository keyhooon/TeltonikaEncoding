// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Commands.CameraImageError
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

namespace Teltonika.Avl.Commands
{
  public enum CameraImageError : byte
  {
    OpenFailed = 0,
    GetImageSizeFailed = 1,
    CreateImageFailed = 2,
    WebUiFailed = 3,
    OpenImageFailed = 4,
    POpenMove = 5,
    WriteImageToCard = 6,
    IndexOutOfRange = 7,
    ByteCountOutOfRange = 8,
    NoCard = 9,
    GetCardFreeSpaceFailed = 10, // 0x0A
    ImageNotFound = 11, // 0x0B
    MemoryAllocationFailed = 12, // 0x0C
    ListCardFailed = 13, // 0x0D
    FormatCardFailed = 14, // 0x0E
    CodeCheckFailed = 15, // 0x0F
    CardFull = 16, // 0x10
    ErrorFileRead = 17, // 0x11
    ErrorFileUpdate = 18, // 0x12
    UnknownCommand = 19, // 0x13
    NoCamera = 100, // 0x64
    CameraBusy = 101, // 0x65
    Timeout = 255, // 0xFF
  }
}
