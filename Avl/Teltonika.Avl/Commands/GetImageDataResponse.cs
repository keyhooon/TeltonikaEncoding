// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Commands.GetImageDataResponse
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using Teltonika.IO;

namespace Teltonika.Avl.Commands
{
  public sealed class GetImageDataResponse : ImageCommand
  {
    private readonly int _imageId;
    private readonly int _offset;
    private readonly byte[] _data;

    public GetImageDataResponse(int imageId, int offset, byte[] data)
    {
      _imageId = imageId;
      _offset = offset;
      _data = data;
    }

    public override int Id => 2;

    public override int ImageId => _imageId;

    public int Offset => _offset;

    public byte[] Data => _data;

    public static GetImageDataResponse Decode(byte id, IBinaryReader reader)
    {
      if (id != 2)
        throw new ArgumentOutOfRangeException(nameof (id));
      uint num1 = reader.ReadUInt32();
      uint num2 = reader.ReadUInt32();
      ushort num3 = reader.ReadUInt16();
      byte[] data = reader.ReadBytes(num3);
      return new GetImageDataResponse((int) num1, (int) num2, data);
    }

    public override string ToString() => string.Format("Received image #{0} data [Offset: {1}, Size: {2}]", ImageId, Offset, Data.Length);
  }
}
