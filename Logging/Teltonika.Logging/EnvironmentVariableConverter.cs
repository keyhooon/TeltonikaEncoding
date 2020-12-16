// Decompiled with JetBrains decompiler
// Type: Teltonika.Logging.EnvironmentVariableConverter
// Assembly: Teltonika.Logging, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: FE7D5E8C-1D45-4EB6-884D-A6B74E72A062
// Assembly location: F:\FLEET\Tavl\Teltonika.Logging.dll

using log4net.Util;
using System;
using System.IO;

namespace Teltonika.Logging
{
  public sealed class EnvironmentVariableConverter : PatternConverter
  {
    protected override void Convert(TextWriter writer, object state)
    {
      EnvironmentVariableTarget[] environmentVariableTargetArray = new EnvironmentVariableTarget[3]
      {
        EnvironmentVariableTarget.Process,
        EnvironmentVariableTarget.User,
        EnvironmentVariableTarget.Machine
      };
      string option = this.Option;
      foreach (EnvironmentVariableTarget target in environmentVariableTargetArray)
      {
        string environmentVariable = Environment.GetEnvironmentVariable(option, target);
        if (!string.IsNullOrEmpty(environmentVariable))
        {
          writer.Write(environmentVariable);
          break;
        }
      }
    }
  }
}
