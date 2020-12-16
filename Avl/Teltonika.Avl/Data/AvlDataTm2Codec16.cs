// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.AvlDataTm2Codec16
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using Teltonika.Avl.Data.Encoding;

namespace Teltonika.Avl.Data
{
  public class AvlDataTm2Codec16 : Codec<AvlData[], AvlDataArrayEncodingAdapter<DefaultAvlDataEncoding16>>
  {
    private static readonly AvlDataTm2Codec16 instance = new AvlDataTm2Codec16();

    public static AvlDataTm2Codec16 Instance => instance;

    public override byte Id => 90;
  }
}
