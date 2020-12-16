// Decompiled with JetBrains decompiler
// Type: Teltonika.Logging.LoggerWrapper
// Assembly: Teltonika.Logging, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: FE7D5E8C-1D45-4EB6-884D-A6B74E72A062
// Assembly location: F:\FLEET\Tavl\Teltonika.Logging.dll

using log4net;
using System;

namespace Teltonika.Logging
{
  internal class LoggerWrapper : ILog
  {
    private readonly log4net.ILog _logger;

    public LoggerWrapper(log4net.ILog logger) => this._logger = logger != null ? logger : throw new ArgumentNullException(nameof (logger), "LoggerWrapper()");

    void ILog.Debug(object message) => this._logger.Debug(message);

    void ILog.Debug(object message, Exception exception) => this._logger.Debug(message, exception);

    void ILog.DebugFormat(string format, params object[] args) => this._logger.DebugFormat(format, args);

    void ILog.DebugFormat(IFormatProvider provider, string format, params object[] args) => this._logger.DebugFormat(provider, format, args);

    void ILog.Info(object message) => this._logger.Info(message);

    void ILog.Info(object message, Exception exception) => this._logger.Info(message, exception);

    void ILog.InfoFormat(string format, params object[] args) => this._logger.InfoFormat(format, args);

    void ILog.InfoFormat(IFormatProvider provider, string format, params object[] args) => this._logger.InfoFormat(provider, format, args);

    void ILog.Warn(object message) => this._logger.Warn(message);

    void ILog.Warn(object message, Exception exception) => this._logger.Warn(message, exception);

    void ILog.WarnFormat(string format, params object[] args) => this._logger.WarnFormat(format, args);

    void ILog.WarnFormat(IFormatProvider provider, string format, params object[] args) => this._logger.WarnFormat(provider, format, args);

    void ILog.Error(object message) => this._logger.Error(message);

    void ILog.Error(object message, Exception exception) => this._logger.Error(message, exception);

    void ILog.ErrorFormat(string format, params object[] args) => this._logger.ErrorFormat(format, args);

    void ILog.ErrorFormat(IFormatProvider provider, string format, params object[] args) => this._logger.ErrorFormat(provider, format, args);

    void ILog.Fatal(object message) => this._logger.Fatal(message);

    void ILog.Fatal(object message, Exception exception) => this._logger.Fatal(message, exception);

    void ILog.FatalFormat(string format, params object[] args) => this._logger.FatalFormat(format, args);

    void ILog.FatalFormat(IFormatProvider provider, string format, params object[] args) => this._logger.FatalFormat(provider, format, args);

    bool ILog.IsDebugEnabled => this._logger.IsDebugEnabled;

    bool ILog.IsInfoEnabled => this._logger.IsInfoEnabled;

    bool ILog.IsWarnEnabled => this._logger.IsWarnEnabled;

    bool ILog.IsErrorEnabled => this._logger.IsErrorEnabled;

    bool ILog.IsFatalEnabled => this._logger.IsFatalEnabled;

    void ILog.SetThreadContextProperty(string propertyName, object value)
    {
      if (string.IsNullOrEmpty(propertyName))
        return;
      ThreadContext.Properties[propertyName] = value;
    }
  }
}
