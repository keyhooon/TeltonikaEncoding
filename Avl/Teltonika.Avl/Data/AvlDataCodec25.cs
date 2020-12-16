// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.AvlDataCodec25
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using Teltonika.Avl.Data.Encoding;

namespace Teltonika.Avl.Data
{
  public class AvlDataCodec25 : Codec<AvlData[], AvlDataArrayEncodingAdapter<Codec25AvlDataEncoding>>
  {
    private static AvlDataCodec25 _instance;

    public static AvlDataCodec25 Instance => _instance ?? (_instance = new AvlDataCodec25());

    public override byte Id => 25;
  }
}
