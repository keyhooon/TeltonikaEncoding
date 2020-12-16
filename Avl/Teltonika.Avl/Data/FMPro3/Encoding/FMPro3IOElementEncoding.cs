// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.FMPro3.Encoding.FMPro3IOElementEncoding
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.Collections.Generic;
using System.Linq;
using Teltonika.IO;

namespace Teltonika.Avl.Data.FMPro3.Encoding
{
  public sealed class FMPro3IOElementEncoding : IEncoding<IoElement>
  {
    private static FMPro3IOElementEncoding _instance;
    private static readonly IEncoding<IoProperty[]> Encoding8 = FMPro3IOPropertyEncoding.Instance8;
    private static readonly IEncoding<IoProperty[]> Encoding16 = FMPro3IOPropertyEncoding.Instance16;
    private static readonly IEncoding<IoProperty[]> Encoding32 = FMPro3IOPropertyEncoding.Instance32;
    private static readonly IEncoding<IoProperty[]> Encoding64 = FMPro3IOPropertyEncoding.Instance64;

    public static FMPro3IOElementEncoding Instance => _instance ?? (_instance = new FMPro3IOElementEncoding());

    public void Encode(IoElement element, IBitWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      if (element.EventId > byte.MaxValue)
        throw new ArgumentException("The event I/O id is not valid.", nameof (element));
      IoProperty[] array1 = element.Where(x => x.Size == 1).Where(new Func<IoProperty, bool>(EncodePropertyFilter)).ToArray();
      IoProperty[] array2 = element.Where(x => x.Size == 2).Where(new Func<IoProperty, bool>(EncodePropertyFilter)).ToArray();
      IoProperty[] array3 = element.Where(x => x.Size == 4).Where(new Func<IoProperty, bool>(EncodePropertyFilter)).ToArray();
      IoProperty[] array4 = element.Where(x => x.Size == 8).Where(new Func<IoProperty, bool>(EncodePropertyFilter)).ToArray();
      writer.Write((byte) element.EventId);
            Encoding8.Encode(array1, writer);
            Encoding16.Encode(array2, writer);
            Encoding32.Encode(array3, writer);
            Encoding64.Encode(array4, writer);
    }

    public IoElement Decode(IBitReader reader)
    {
      byte num = reader != null ? reader.ReadByte() : throw new ArgumentNullException(nameof (reader));
      IoProperty[] ioPropertyArray1 = Encoding8.Decode(reader);
      IoProperty[] ioPropertyArray2 = Encoding16.Decode(reader);
      IoProperty[] ioPropertyArray3 = Encoding32.Decode(reader);
      IoProperty[] ioPropertyArray4 = Encoding64.Decode(reader);
      return new IoElement(num, ((IEnumerable<IoProperty>) ioPropertyArray1).Concat(ioPropertyArray2).Concat(ioPropertyArray3).Concat(ioPropertyArray4));
    }

    private static bool EncodePropertyFilter(IoProperty property) => property.Id != 500001;
  }
}
