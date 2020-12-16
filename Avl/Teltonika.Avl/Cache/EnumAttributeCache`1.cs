// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Cache.EnumAttributeCache`1
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Teltonika.Avl.Cache
{
  public static class EnumAttributeCache<T>
  {
    private static readonly EnumMemberInfoCache<T> Cache = new EnumMemberInfoCache<T>();

    public static TA GetCustomAttribute<TA>(T value) where TA : Attribute
    {
      MemberInfo memberInfo = Cache.GetMemberInfo(value);
      if (memberInfo == null)
        return default (TA);
      object[] customAttributes = memberInfo.GetCustomAttributes(typeof (TA), true);
      return customAttributes.Length == 0 ? default (TA) : (TA) customAttributes[0];
    }

    private class EnumMemberInfoCache<TEnum>
    {
      private readonly IDictionary<TEnum, MemberInfo> _cache;

      public EnumMemberInfoCache()
      {
        Type type = typeof (TEnum);
        if (!type.IsEnum)
          throw new ArgumentException("ONLY Enum types are expected");
        _cache = ((IEnumerable<string>)Enum.GetNames(type)).Select(x =>
      {
          var data = new
          {
              Name = x,
              Members = type.GetMember(x)
          };
          return data;
      }).Where(x => x.Members.Length > 0).ToDictionary(x => (TEnum)Enum.Parse(type, x.Name), x => ((IEnumerable<MemberInfo>)x.Members).First());
      }

      public MemberInfo GetMemberInfo(TEnum value)
      {
        MemberInfo memberInfo;
        return _cache.TryGetValue(value, out memberInfo) ? memberInfo : null;
      }
    }
  }
}
