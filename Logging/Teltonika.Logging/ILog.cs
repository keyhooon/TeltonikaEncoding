// Decompiled with JetBrains decompiler
// Type: Teltonika.Logging.ILog
// Assembly: Teltonika.Logging, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: FE7D5E8C-1D45-4EB6-884D-A6B74E72A062
// Assembly location: F:\FLEET\Tavl\Teltonika.Logging.dll

using System;

namespace Teltonika.Logging
{
  public interface ILog
  {
    void Debug(object message);

    void Debug(object message, Exception exception);

    void DebugFormat(string format, params object[] args);

    void DebugFormat(IFormatProvider provider, string format, params object[] args);

    void Info(object message);

    void Info(object message, Exception exception);

    void InfoFormat(string format, params object[] args);

    void InfoFormat(IFormatProvider provider, string format, params object[] args);

    void Warn(object message);

    void Warn(object message, Exception exception);

    void WarnFormat(string format, params object[] args);

    void WarnFormat(IFormatProvider provider, string format, params object[] args);

    void Error(object message);

    void Error(object message, Exception exception);

    void ErrorFormat(string format, params object[] args);

    void ErrorFormat(IFormatProvider provider, string format, params object[] args);

    void Fatal(object message);

    void Fatal(object message, Exception exception);

    void FatalFormat(string format, params object[] args);

    void FatalFormat(IFormatProvider provider, string format, params object[] args);

    bool IsDebugEnabled { get; }

    bool IsInfoEnabled { get; }

    bool IsWarnEnabled { get; }

    bool IsErrorEnabled { get; }

    bool IsFatalEnabled { get; }

    void SetThreadContextProperty(string propertyName, object value);
  }
}
