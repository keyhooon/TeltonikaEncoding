// Decompiled with JetBrains decompiler
// Type: Teltonika.Logging.Log4NetLogFactory
// Assembly: Teltonika.Logging, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: FE7D5E8C-1D45-4EB6-884D-A6B74E72A062
// Assembly location: F:\FLEET\Tavl\Teltonika.Logging.dll

using System;

namespace Teltonika.Logging
{
  public sealed class Log4NetLogFactory : ILogFactory
  {
    public Log4NetLogFactory() => LogManager.Configure();

    public ILog GetLogger() => LogManager.GetLogger();

    public ILog GetLogger(string name) => LogManager.GetLogger(name);

    public ILog GetLogger(Type type) => LogManager.GetLogger(type);
  }
}
