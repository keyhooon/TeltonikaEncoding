// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Commands.ImageCommandPacket
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.IO;
using Teltonika.IO;

namespace Teltonika.Avl.Commands
{
  public sealed class ImageCommandPacket : Command
  {
    public ImageCommandPacket()
      : base(13)
    {
    }

    public ImageCommand ImageCommand
    {
      get => DecodeImageCommand(Data);
      set => Data = EncodeImageCommand(value);
    }

    public override Command Decode(byte id, IBitReader reader)
    {
      int count = reader.ReadInt32();
      Data = reader.ReadBytes(count);
      return this;
    }

    private byte[] EncodeImageCommand(ImageCommand value)
    {
      using (MemoryStream stream = new MemoryStream())
      {
        using (EndianBinaryWriter suitableBinaryWriter = stream.CreateSuitableBinaryWriter())
          value.Encode(suitableBinaryWriter);
        return stream.ToArray();
      }
    }

    private ImageCommand DecodeImageCommand(byte[] data)
    {
      using (MemoryStream stream = new MemoryStream(data, false))
      {
        using (EndianBinaryReader suitableBinaryReader = stream.CreateSuitableBinaryReader())
        {
          byte id = suitableBinaryReader.ReadByte();
          switch (id)
          {
            case 1:
              return ImageCreatedResponse.Decode(id, suitableBinaryReader);
            case 2:
              return GetImageDataResponse.Decode(id, suitableBinaryReader);
            case 3:
              return ImageCreatedResponse.Decode(id, suitableBinaryReader);
            case 238:
            case 239:
              return ErrorResponse.Decode(id, suitableBinaryReader);
            default:
              throw new NotSupportedException();
          }
        }
      }
    }
  }
}
