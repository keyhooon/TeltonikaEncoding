// Decompiled with JetBrains decompiler
// Type: Teltonika.IO.IBinaryReader
// Assembly: Teltonika.IO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ACB30B4E-8DEF-4DCF-94D3-7CD8E800CE25
// Assembly location: F:\FLEET\Tavl\Teltonika.IO.dll

using System;
using System.IO;
using System.Text;

namespace Teltonika.IO
{
  public interface IBinaryReader : IDisposable
  {
    Stream BaseStream { get; }

    void Close();

    Encoding Encoding { get; }

    int Read();

    int Read(byte[] buffer, int index, int count);

    int Read(char[] data, int index, int count);

    int Read7BitEncodedInt();

    int ReadBigEndian7BitEncodedInt();

    byte ReadByte();

    byte[] ReadBytes(int count);

    byte[] ReadBytesOrThrow(int count);

    bool ReadBoolean();

    Decimal ReadDecimal();

    double ReadDouble();

    short ReadInt16();

    int ReadInt32();

    long ReadInt64();

    sbyte ReadSByte();

    float ReadSingle();

    string ReadString();

    ushort ReadUInt16();

    uint ReadUInt32();

    ulong ReadUInt64();

    void Seek(int offset, SeekOrigin origin);
  }
}
