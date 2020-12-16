// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Commands.GetImageSizeRequest
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using Teltonika.IO;

namespace Teltonika.Avl.Commands
{
  public sealed class GetImageSizeRequest : ImageCommand
  {
    private readonly int _imageId;

    public GetImageSizeRequest(int imageId) => _imageId = imageId;

    public override int Id => 3;

    public override int ImageId => _imageId;

    public override void Encode(IBinaryWriter writer)
    {
      base.Encode(writer);
      writer.Write((uint) ImageId);
    }

    public override string ToString() => string.Format("Get image #{0} size", ImageId);
  }
}
