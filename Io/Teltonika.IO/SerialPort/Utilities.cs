// Decompiled with JetBrains decompiler
// Type: Teltonika.IO.SerialPort.Utilities
// Assembly: Teltonika.IO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ACB30B4E-8DEF-4DCF-94D3-7CD8E800CE25
// Assembly location: F:\FLEET\Tavl\Teltonika.IO.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Teltonika.IO.SerialPort
{
  public static class Utilities
  {
    public static string[] GetPortNamesSortedByNumber()
    {
      try
      {
        return ((IEnumerable<string>) System.IO.Ports.SerialPort.GetPortNames()).Where(x => x.Contains("COM")).ToDictionary(x => x, s => int.Parse(s.Substring(s.IndexOf("COM", StringComparison.InvariantCultureIgnoreCase) + 3))).OrderBy(x => x.Value).Select(x => x.Key).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
      }
      catch (Exception ex)
      {
        return ((IEnumerable<string>) System.IO.Ports.SerialPort.GetPortNames()).OrderBy(x => x).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
      }
    }
  }
}
