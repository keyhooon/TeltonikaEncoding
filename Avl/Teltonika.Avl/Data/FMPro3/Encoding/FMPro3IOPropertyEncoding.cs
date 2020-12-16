// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.FMPro3.Encoding.FMPro3IOPropertyEncoding
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using Teltonika.Avl.Data.Encoding;

namespace Teltonika.Avl.Data.FMPro3.Encoding
{
  public sealed class FMPro3IOPropertyEncoding : DefaultIoPropertyEncoding
  {
    public static readonly DefaultIoPropertyEncoding Instance8 = new DefaultIoPropertyEncoding(FieldEncoding.Int8);
    public static readonly DefaultIoPropertyEncoding Instance16 = new DefaultIoPropertyEncoding(FieldEncoding.Int16);
    public static readonly DefaultIoPropertyEncoding Instance32 = new DefaultIoPropertyEncoding(FieldEncoding.Int32);
    public static readonly DefaultIoPropertyEncoding Instance64 = new DefaultIoPropertyEncoding(FieldEncoding.Int64);

    internal FMPro3IOPropertyEncoding(FieldEncoding encoding)
      : base(encoding)
    {
      switch (encoding)
      {
        case FieldEncoding.Int8:
          break;
        case FieldEncoding.Int16:
          break;
        case FieldEncoding.Int32:
          break;
        case FieldEncoding.Int64:
          break;
        default:
          throw new NotSupportedException(string.Format("The field encoding ({0}) is not supported.", encoding));
      }
    }
  }
}
