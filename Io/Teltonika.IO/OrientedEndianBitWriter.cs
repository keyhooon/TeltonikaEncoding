// Decompiled with JetBrains decompiler
// Type: Teltonika.IO.OrientedEndianBitWriter
// Assembly: Teltonika.IO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ACB30B4E-8DEF-4DCF-94D3-7CD8E800CE25
// Assembly location: F:\FLEET\Tavl\Teltonika.IO.dll

using System;
using System.IO;
using System.Text;

namespace Teltonika.IO
{
  public class OrientedEndianBitWriter : EndianBinaryWriter, IBitWriter, IBinaryWriter, IDisposable
  {
    public OrientedEndianBitWriter(EndianBitConverter bitConverter, OrientedBitStream stream)
      : base(bitConverter, stream)
    {
    }

    public OrientedEndianBitWriter(
      EndianBitConverter bitConverter,
      OrientedBitStream stream,
      Encoding encoding)
      : base(bitConverter, stream, encoding)
    {
    }

    public void WriteBit(bool bit) => ((OrientedBitStream) BaseStream).WriteBit(bit);

    public void WriteBits(byte value, int count) => ((OrientedBitStream) BaseStream).WriteBits(value, count);
  }
}
