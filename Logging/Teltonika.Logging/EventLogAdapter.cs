// Decompiled with JetBrains decompiler
// Type: Teltonika.Logging.EventLogAdapter
// Assembly: Teltonika.Logging, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: FE7D5E8C-1D45-4EB6-884D-A6B74E72A062
// Assembly location: F:\FLEET\Tavl\Teltonika.Logging.dll

using System;
using System.Diagnostics;

namespace Teltonika.Logging
{
  public class EventLogAdapter : ILog
  {
    private readonly EventLog _eventLog;

    public EventLogAdapter(EventLog eventLog) => this._eventLog = eventLog;

    void ILog.Debug(object message)
    {
    }

    void ILog.Debug(object message, Exception exception)
    {
    }

    void ILog.DebugFormat(string format, params object[] args)
    {
    }

    void ILog.DebugFormat(IFormatProvider provider, string format, params object[] args)
    {
    }

    void ILog.Info(object message) => this._eventLog.WriteEntry(message.ToString());

    void ILog.Info(object message, Exception exception) => this._eventLog.WriteEntry(message.ToString() + " " + exception.ToString());

    void ILog.InfoFormat(string format, params object[] args) => this._eventLog.WriteEntry(string.Format(format, args));

    void ILog.InfoFormat(IFormatProvider provider, string format, params object[] args) => this._eventLog.WriteEntry(string.Format(provider, format, args));

    void ILog.Warn(object message) => this._eventLog.WriteEntry(message.ToString(), EventLogEntryType.Warning);

    void ILog.Warn(object message, Exception exception) => this._eventLog.WriteEntry(message.ToString() + " " + exception.ToString(), EventLogEntryType.Warning);

    void ILog.WarnFormat(string format, params object[] args) => this._eventLog.WriteEntry(string.Format(format, args), EventLogEntryType.Warning);

    void ILog.WarnFormat(IFormatProvider provider, string format, params object[] args) => this._eventLog.WriteEntry(string.Format(provider, format, args), EventLogEntryType.Warning);

    void ILog.Error(object message) => this._eventLog.WriteEntry(message.ToString(), EventLogEntryType.Error);

    void ILog.Error(object message, Exception exception) => this._eventLog.WriteEntry(message.ToString() + " " + exception.ToString(), EventLogEntryType.Error);

    void ILog.ErrorFormat(string format, params object[] args) => this._eventLog.WriteEntry(string.Format(format, args), EventLogEntryType.Error);

    void ILog.ErrorFormat(IFormatProvider provider, string format, params object[] args) => this._eventLog.WriteEntry(string.Format(provider, format, args), EventLogEntryType.Error);

    void ILog.Fatal(object message) => this._eventLog.WriteEntry(message.ToString(), EventLogEntryType.Error);

    void ILog.Fatal(object message, Exception exception) => this._eventLog.WriteEntry(message.ToString() + " " + exception.ToString(), EventLogEntryType.Error);

    void ILog.FatalFormat(string format, params object[] args) => this._eventLog.WriteEntry(string.Format(format, args), EventLogEntryType.Error);

    void ILog.FatalFormat(IFormatProvider provider, string format, params object[] args) => this._eventLog.WriteEntry(string.Format(provider, format, args), EventLogEntryType.Error);

    bool ILog.IsDebugEnabled => false;

    bool ILog.IsInfoEnabled => true;

    bool ILog.IsWarnEnabled => true;

    bool ILog.IsErrorEnabled => true;

    bool ILog.IsFatalEnabled => true;

    void ILog.SetThreadContextProperty(string propertyName, object value)
    {
    }
  }
}
