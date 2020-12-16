// Decompiled with JetBrains decompiler
// Type: Teltonika.IO.IBitReader
// Assembly: Teltonika.IO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ACB30B4E-8DEF-4DCF-94D3-7CD8E800CE25
// Assembly location: F:\FLEET\Tavl\Teltonika.IO.dll

using System;

namespace Teltonika.IO
{
  public interface IBitReader : IBinaryReader, IDisposable
  {
    int ReadBit(out bool bit);

    int ReadBits(out long bits, int count);
  }
}
