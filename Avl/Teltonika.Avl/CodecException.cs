// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.CodecException
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;

namespace Teltonika.Avl
{
  public sealed class CodecException : ApplicationException
    {
    public CodecException()
    {
    }

    public CodecException(string message)
      : base(message)
    {
    }

    public CodecException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}
