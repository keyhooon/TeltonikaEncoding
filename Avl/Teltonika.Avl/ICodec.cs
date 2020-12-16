// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.ICodec
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.IO;

namespace Teltonika.Avl
{
  public interface ICodec
  {
    byte Id { get; }

    Type DataType { get; }

    void Encode(object data, Stream stream);

    byte[] Encode(object data);

    object Decode(Stream stream);

    object Decode(byte[] bytes);
  }
}
