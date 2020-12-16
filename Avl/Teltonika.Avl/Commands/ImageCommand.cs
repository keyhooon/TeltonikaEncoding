// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Commands.ImageCommand
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using Teltonika.IO;

namespace Teltonika.Avl.Commands
{
  public abstract class ImageCommand
  {
    public abstract int Id { get; }

    public abstract int ImageId { get; }

    public virtual void Encode(IBinaryWriter writer) => writer.Write((byte) Id);

    public abstract override string ToString();
  }
}
