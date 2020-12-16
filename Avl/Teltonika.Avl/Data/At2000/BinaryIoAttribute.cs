// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.At2000.BinaryIoAttribute
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;

namespace Teltonika.Avl.Data.At2000
{
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
  public sealed class BinaryIoAttribute : Attribute
  {
    private readonly byte _bitNumber;
    private readonly IoType _ioIoType;

    public BinaryIoAttribute(IoType ioIoType, byte bitNumber)
    {
      _ioIoType = ioIoType;
      _bitNumber = bitNumber;
    }

    public IoType IoIoType => _ioIoType;

    public byte BitNumber => _bitNumber;
  }
}
