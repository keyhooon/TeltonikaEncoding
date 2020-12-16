// Decompiled with JetBrains decompiler
// Type: Teltonika.Logging.LogManager
// Assembly: Teltonika.Logging, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: FE7D5E8C-1D45-4EB6-884D-A6B74E72A062
// Assembly location: F:\FLEET\Tavl\Teltonika.Logging.dll

using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository;
using System;
using System.IO;
using System.Reflection;

namespace Teltonika.Logging
{
  public static class LogManager
  {
    private const string LogConfigFile = "log4net.config.xml";
    private static readonly object Lock = new object();
    private static bool _configured;

    public static void Fake()
    {
      if (LogManager._configured)
        return;
      lock (LogManager.Lock)
      {
        if (LogManager._configured)
          return;
        log4net.Repository.Hierarchy.Hierarchy repository = (log4net.Repository.Hierarchy.Hierarchy) log4net.LogManager.GetRepository();
        repository.Root.RemoveAllAppenders();
        repository.Root.Level = Level.All;
        MemoryAppender memoryAppender1 = new MemoryAppender();
        memoryAppender1.Layout = (ILayout) new SimpleLayout();
        MemoryAppender memoryAppender2 = memoryAppender1;
        memoryAppender2.ActivateOptions();
        BasicConfigurator.Configure((IAppender) memoryAppender2);
        LogManager._configured = true;
      }
    }

    public static ILog GetLogger() => (ILog) new LoggerWrapper(log4net.LogManager.GetLogger(""));

    public static ILog GetLogger(string name) => (ILog) new LoggerWrapper(log4net.LogManager.GetLogger(name));

    public static ILog GetLogger(Type type) => (ILog) new LoggerWrapper(log4net.LogManager.GetLogger(type));

    public static ILog GetLogger<T>() => LogManager.GetLogger(typeof (T));

    public static void SetGlobalContextProperty(string propertyName, object value)
    {
      if (string.IsNullOrEmpty(propertyName))
        return;
      GlobalContext.Properties[propertyName] = value;
    }

    public static void SetThreadContextProperty(string propertyName, object value)
    {
      if (string.IsNullOrEmpty(propertyName))
        return;
      ThreadContext.Properties[propertyName] = value;
    }

    public static void Shutdown() => log4net.LogManager.Shutdown();

    public static void Configure() => LogManager.ConfigureCore(AppDomain.CurrentDomain.BaseDirectory + "log4net.config.xml");

    public static void Configure(Assembly assembly) => LogManager.ConfigureCore(assembly.Location + ".config");

    public static void Configure(string filename) => LogManager.ConfigureCore(filename);

    public static ILoggerRepository GetRepository() => log4net.LogManager.GetRepository();

    private static void ConfigureCore(string filename)
    {
      if (File.Exists(filename))
        XmlConfigurator.ConfigureAndWatch(new FileInfo(filename));
      else
        XmlConfigurator.Configure();
    }
  }
}
