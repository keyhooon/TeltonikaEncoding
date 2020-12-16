// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.IButtonDataCodec
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using Teltonika.Avl.Data.Encoding;

namespace Teltonika.Avl.Data
{
  public sealed class IButtonDataCodec : Codec<AvlData[], AvlDataArrayEncodingAdapter<AvlDataIButtonEncoding>>
  {
    private static IButtonDataCodec _instance;

    public static IButtonDataCodec Instance => _instance ?? (_instance = new IButtonDataCodec());

    public override byte Id => 6;
  }
}
