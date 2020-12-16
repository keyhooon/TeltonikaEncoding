// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Commands.GetImageDataRequest
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using Teltonika.IO;

namespace Teltonika.Avl.Commands
{
  public sealed class GetImageDataRequest : ImageCommand
  {
    private readonly int _imageId;
    private readonly int _offset;
    private readonly ushort _size;

    public GetImageDataRequest(int imageId, int offset, int size)
    {
      _imageId = imageId;
      _offset = offset;
      _size = (ushort) size;
    }

    public override int Id => 2;

    public override int ImageId => _imageId;

    public int Offset => _offset;

    public int Size => _size;

    public override void Encode(IBinaryWriter writer)
    {
      base.Encode(writer);
      writer.Write((uint) ImageId);
      writer.Write((uint) Offset);
      writer.Write((ushort) Size);
    }

    public override string ToString() => string.Format("Get image #{0} data [Offset:{1} Size:{2}]", ImageId, Offset, Size);
  }
}
