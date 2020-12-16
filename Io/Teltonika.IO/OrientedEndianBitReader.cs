// Decompiled with JetBrains decompiler
// Type: Teltonika.IO.OrientedEndianBitReader
// Assembly: Teltonika.IO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ACB30B4E-8DEF-4DCF-94D3-7CD8E800CE25
// Assembly location: F:\FLEET\Tavl\Teltonika.IO.dll

using System;
using System.IO;
using System.Text;

namespace Teltonika.IO
{
  public class OrientedEndianBitReader : EndianBinaryReader, IBitReader, IBinaryReader, IDisposable
  {
    public OrientedEndianBitReader(EndianBitConverter bitConverter, OrientedBitStream stream)
      : base(bitConverter, stream)
    {
    }

    public OrientedEndianBitReader(
      EndianBitConverter bitConverter,
      OrientedBitStream stream,
      Encoding encoding)
      : base(bitConverter, stream, encoding)
    {
    }

    public int ReadBit(out bool bit) => ((OrientedBitStream) BaseStream).ReadBit(out bit);

    public int ReadBits(out long bits, int count) => ((OrientedBitStream) BaseStream).ReadBits(out bits, count);
  }
}
