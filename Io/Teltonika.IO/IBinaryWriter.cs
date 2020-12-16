// Decompiled with JetBrains decompiler
// Type: Teltonika.IO.IBinaryWriter
// Assembly: Teltonika.IO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ACB30B4E-8DEF-4DCF-94D3-7CD8E800CE25
// Assembly location: F:\FLEET\Tavl\Teltonika.IO.dll

using System;
using System.IO;
using System.Text;

namespace Teltonika.IO
{
  public interface IBinaryWriter : IDisposable
  {
    Stream BaseStream { get; }

    void Close();

    Encoding Encoding { get; }

    void Flush();

    void Seek(int offset, SeekOrigin origin);

    void Write(byte value);

    void Write(byte[] value);

    void Write(byte[] value, int offset, int count);

    void Write(bool value);

    void Write(char value);

    void Write(char[] value);

    void Write(Decimal value);

    void Write(double value);

    void Write(short value);

    void Write(int value);

    void Write(long value);

    void Write(sbyte value);

    void Write(float value);

    void Write(string value);

    void Write(ushort value);

    void Write(uint value);

    void Write(ulong value);

    void Write7BitEncodedInt(int value);
  }
}
