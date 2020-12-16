// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.AvlDataTm2Codec
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using Teltonika.Avl.Data.Encoding;

namespace Teltonika.Avl.Data
{
  public class AvlDataTm2Codec : Codec<AvlData[], AvlDataArrayEncodingAdapter<DefaultAvlDataEncoding>>
  {
    private static AvlDataTm2Codec _instance;

    public static AvlDataTm2Codec Instance => _instance ?? (_instance = new AvlDataTm2Codec());

    public override byte Id => 8;
  }
}
