// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Configuration.ConfigurationParameter
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System.Text;

namespace Teltonika.Avl.Configuration
{
  public class ConfigurationParameter
  {
    public ConfigurationParameter(ushort paramId, string paramValue)
    {
      Id = paramId;
      Value = paramValue;
    }

    public ConfigurationParameter(ushort paramId, string paramValue, int originalId)
    {
      Id = paramId;
      Value = paramValue;
      OriginalId = originalId;
    }

    public override string ToString() => string.Format("{0}={1}", Id, Value);

    public override bool Equals(object obj) => obj is ConfigurationParameter configurationParameter && (Id == configurationParameter.Id && OriginalId == configurationParameter.OriginalId && Value == configurationParameter.Value);

    public override int GetHashCode() => Id.GetHashCode() ^ Value.GetHashCode() ^ OriginalId.GetHashCode();

    public ushort Id { get; set; }

    public string Value { get; set; }

    public int OriginalId { get; set; }

    public virtual byte[] GetTcpBytes() => Encoding.UTF8.GetBytes(Value);
  }
}
