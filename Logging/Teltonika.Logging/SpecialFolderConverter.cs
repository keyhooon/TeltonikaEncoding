// Decompiled with JetBrains decompiler
// Type: Teltonika.Logging.SpecialFolderConverter
// Assembly: Teltonika.Logging, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: FE7D5E8C-1D45-4EB6-884D-A6B74E72A062
// Assembly location: F:\FLEET\Tavl\Teltonika.Logging.dll

using log4net.Util;
using System;
using System.IO;

namespace Teltonika.Logging
{
  public sealed class SpecialFolderConverter : PatternConverter
  {
    protected override void Convert(TextWriter writer, object state)
    {
      string folderPath = Environment.GetFolderPath((Environment.SpecialFolder) Enum.Parse(typeof (Environment.SpecialFolder), this.Option, true));
      writer.Write(folderPath);
    }
  }
}
