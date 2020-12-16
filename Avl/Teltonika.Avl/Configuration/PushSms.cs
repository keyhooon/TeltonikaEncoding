// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Configuration.PushSms
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

namespace Teltonika.Avl.Configuration
{
  public class PushSms
  {
    private string _login;
    private string _password;
    private string _configHost;
    private ushort _configPort;
    private string _apn;
    private string _gprsLogin;
    private string _gprsPassword;

    public PushSms(
      string login,
      string password,
      string configHost,
      ushort configPort,
      string apn,
      string gprsLogin,
      string gprsPassword)
    {
      _login = login;
      _password = password;
      _configHost = configHost;
      _configPort = configPort;
      _apn = apn;
      _gprsLogin = gprsLogin;
      _gprsPassword = gprsPassword;
    }

    public PushSms(string login, string password, string configHost, ushort configPort)
    {
      _login = login;
      _password = password;
      _configHost = configHost;
      _configPort = configPort;
    }

    public override string ToString() => string.Format("login:[{0}] password:[{1}] configHost:[{2}] configPort:[{3}] apn:[{4}] gprsLogin:[{5}] gprsPassword:[{6}]", _login, _password, _configHost, _configPort, _apn, _gprsLogin, _gprsPassword);

    public string Login
    {
      get => _login;
      set => _login = value;
    }

    public string Password
    {
      get => _password;
      set => _password = value;
    }

    public string ConfigHost
    {
      get => _configHost;
      set => _configHost = value;
    }

    public ushort ConfigPort
    {
      get => _configPort;
      set => _configPort = value;
    }

    public string Apn
    {
      get => _apn;
      set => _apn = value;
    }

    public string GprsLogin
    {
      get => _gprsLogin;
      set => _gprsLogin = value;
    }

    public string GprsPassword
    {
      get => _gprsPassword;
      set => _gprsPassword = value;
    }
  }
}
