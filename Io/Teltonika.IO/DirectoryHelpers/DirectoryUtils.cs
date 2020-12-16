// Decompiled with JetBrains decompiler
// Type: Teltonika.IO.DirectoryHelpers.DirectoryUtils
// Assembly: Teltonika.IO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ACB30B4E-8DEF-4DCF-94D3-7CD8E800CE25
// Assembly location: F:\FLEET\Tavl\Teltonika.IO.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Teltonika.IO.DirectoryHelpers
{
  public static class DirectoryUtils
  {
    private static readonly string DirectorySeparatorChar = Path.DirectorySeparatorChar.ToString();

    public static string EscapePathSegment(string filename) => ((IEnumerable<char>) Path.GetInvalidFileNameChars()).Aggregate(filename, (x, s) => x.Replace(s, '_'));

    public static string EscapePathSegments(params string[] paths) => string.Join(DirectorySeparatorChar, ((IEnumerable<string>) paths).Select(new Func<string, string>(EscapePathSegment)).ToArray());

    public static void CreateDirectory(DirectoryInfo directoryInfo)
    {
      if (directoryInfo == null || directoryInfo.Exists)
        return;
            CreateDirectory(directoryInfo.Parent);
      directoryInfo.Create();
      if (!directoryInfo.Name.StartsWith("."))
        return;
      directoryInfo.Attributes = FileAttributes.Hidden;
    }
  }
}
