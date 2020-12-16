// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.Encoding.GhGlobalMaskExtensions
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Teltonika.Avl.Data.Encoding
{
  public static class GhGlobalMaskExtensions
  {
    private static readonly int[] Io = new int[4]
    {
      204,
      200,
      202,
      201
    };

    public static bool HasFlag(this GhGlobalMask mask, GhGlobalMask flag) => (mask & flag) == flag;

    public static GhGlobalMask GetMask(this AvlData avlData)
    {
      bool flag1 = !avlData.GPS.IsDefault;
      bool flag2 = avlData.Data.Where(x => !((IEnumerable<int>)Io).Contains(x.Id)).Any(x => x.Size == 1);
      bool flag3 = avlData.Data.Where(x => !((IEnumerable<int>)Io).Contains(x.Id)).Any(x => x.Size == 2);
      bool flag4 = avlData.Data.Where(x => !((IEnumerable<int>)Io).Contains(x.Id)).Any(x => x.Size == 4);
      return (GhGlobalMask) ((byte)((byte)((flag1 ? 1 : 0) | (flag2 ? 2 : 0)) | (flag3 ? 4 : 0)) | (flag4 ? 8 : 0));
    }
  }
}
