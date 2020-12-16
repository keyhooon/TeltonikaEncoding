// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.IEncoding`1
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using Teltonika.IO;

namespace Teltonika.Avl
{
  public interface IEncoding<TCodableData>
  {
    void Encode(TCodableData obj, IBitWriter writer);

    TCodableData Decode(IBitReader reader);
  }
}
