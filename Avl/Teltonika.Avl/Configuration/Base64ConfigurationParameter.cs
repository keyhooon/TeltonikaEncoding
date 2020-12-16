// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Configuration.Base64ConfigurationParameter
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;

namespace Teltonika.Avl.Configuration
{
  public class Base64ConfigurationParameter : ConfigurationParameter
  {
    public Base64ConfigurationParameter(ushort paramId, string paramValue)
      : base(paramId, paramValue)
    {
    }

    public Base64ConfigurationParameter(ushort paramId, string paramValue, int originalId)
      : base(paramId, paramValue, originalId)
    {
    }

    public override byte[] GetTcpBytes() => Convert.FromBase64String(Value);
  }
}
