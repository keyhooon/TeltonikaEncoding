// Decompiled with JetBrains decompiler
// Type: Teltonika.IO.IBitWriter
// Assembly: Teltonika.IO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ACB30B4E-8DEF-4DCF-94D3-7CD8E800CE25
// Assembly location: F:\FLEET\Tavl\Teltonika.IO.dll

using System;

namespace Teltonika.IO
{
  public interface IBitWriter : IBinaryWriter, IDisposable
  {
    void WriteBit(bool bit);

    void WriteBits(byte value, int count);
  }
}
