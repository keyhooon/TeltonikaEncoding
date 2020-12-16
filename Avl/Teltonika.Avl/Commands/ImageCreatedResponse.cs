// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Commands.ImageCreatedResponse
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using Teltonika.IO;

namespace Teltonika.Avl.Commands
{
  public sealed class ImageCreatedResponse : ImageCommand
  {
    private readonly byte _id;
    private readonly int _imageId;
    private readonly int _size;

    public ImageCreatedResponse(byte id, int imageId, int size)
    {
      _id = id;
      _imageId = imageId;
      _size = size;
    }

    public override int Id => _id;

    public override int ImageId => _imageId;

    public int Size => _size;

    public static ImageCreatedResponse Decode(byte id, IBinaryReader reader)
    {
      uint num1 = reader.ReadUInt32();
      uint num2 = reader.ReadUInt32();
      return new ImageCreatedResponse(id, (int) num1, (int) num2);
    }

    public override string ToString() => string.Format("Image #{0} created [Size: {1}]", ImageId, Size);
  }
}
