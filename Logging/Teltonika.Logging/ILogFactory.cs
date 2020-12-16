// Decompiled with JetBrains decompiler
// Type: Teltonika.Logging.ILogFactory
// Assembly: Teltonika.Logging, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: FE7D5E8C-1D45-4EB6-884D-A6B74E72A062
// Assembly location: F:\FLEET\Tavl\Teltonika.Logging.dll

using System;

namespace Teltonika.Logging
{
  public interface ILogFactory
  {
    ILog GetLogger();

    ILog GetLogger(string name);

    ILog GetLogger(Type type);
  }
}
