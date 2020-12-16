// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.Bofan.BofanAvlData
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

namespace Teltonika.Avl.Data.Bofan
{
  public sealed class BofanAvlData
  {
    private readonly string _imei;
    private readonly AvlData _avlData;

    public BofanAvlData(string imei, AvlData avlData)
    {
      _imei = imei;
      _avlData = avlData;
    }

    public string Imei => _imei;

    public AvlData AvlData => _avlData;
  }
}
