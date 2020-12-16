// Decompiled with JetBrains decompiler
// Type: Teltonika.IO.Utilities.DirectoryUtilities
// Assembly: Teltonika.IO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ACB30B4E-8DEF-4DCF-94D3-7CD8E800CE25
// Assembly location: F:\FLEET\Tavl\Teltonika.IO.dll

using System.IO;

namespace Teltonika.IO.Utilities
{
  public static class DirectoryUtilities
  {
    public static void ClearDirectoryContent(string direcotryPath)
    {
      if (!Directory.Exists(direcotryPath))
        return;
      DirectoryInfo directoryInfo = new DirectoryInfo(direcotryPath);
      foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
        directory.Delete(true);
      foreach (FileSystemInfo file in directoryInfo.GetFiles())
        file.Delete();
    }
  }
}
