// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.AvlDataTm2EncodecCodec11
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

namespace Teltonika.Avl.Data
{
  public sealed class AvlDataTm2EncodecCodec11 : AvlDataTm2EncodedCodec
  {
    private static readonly AvlDataTm2EncodecCodec11 instance = new AvlDataTm2EncodecCodec11();

    public static AvlDataTm2EncodecCodec11 Instance => instance;

    public override byte Id => 11;
  }
}
