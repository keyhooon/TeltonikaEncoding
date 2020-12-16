// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.IoElement
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Teltonika.Avl
{
  public sealed class IoElement : IEnumerable<IoProperty>, IEnumerable, IEquatable<IoElement>
  {
    private readonly int _eventId;
    private readonly int[] _eventIds;
    private readonly List<IoProperty> _properties;

    public IoElement()
      : this(0)
    {
    }

    public IoElement(int eventId)
      : this(eventId, new List<IoProperty>())
    {
    }

    public IoElement(int eventId, IEnumerable<IoProperty> properties)
      : this(eventId, properties.ToList())
    {
    }

    public IoElement(int eventId, List<IoProperty> properties)
    {
      _properties = properties;
      _eventId = eventId;
      int[] numArray;
      if (eventId != 0)
        numArray = new int[1]{ eventId };
      else
        numArray = new int[0];
      _eventIds = numArray;
    }

    public IoElement(IEnumerable<int> eventIds, List<IoProperty> properties)
    {
      _properties = properties;
      int[] array = eventIds.ToArray();
      _eventId = array.Length == 0 ? 0 : array[0];
      _eventIds = array;
    }

    public int EventId => _eventId;

    public int[] EventIds => _eventIds;

    public int Count => _properties == null ? 0 : _properties.Count;

    public IList<IoProperty> Properties => _properties;

    public IoProperty this[int id]
    {
      get
      {
        List<IoProperty> properties = _properties;
        int count = properties.Count;
        for (int index = 0; index < count; ++index)
        {
          if (properties[index].Id == id)
            return properties[index];
        }
        return IoProperty.Default;
      }
    }

    public IoProperty ExtractFirst(Func<IoProperty, bool> condition)
    {
      if (condition == null)
        throw new ArgumentNullException(nameof (condition));
      List<IoProperty> properties = _properties;
      if (properties == null)
        return IoProperty.Default;
      int count = properties.Count;
      for (int index = 0; index < count; ++index)
      {
        IoProperty ioProperty = properties[index];
        if (condition(ioProperty))
        {
          properties.RemoveAt(index);
          return ioProperty;
        }
      }
      return IoProperty.Default;
    }

    public IEnumerable<IoProperty> Extract(Func<IoProperty, bool> condition)
    {
      if (condition == null)
        throw new ArgumentNullException(nameof (condition));
      List<IoProperty> properties = _properties;
      if (properties != null)
      {
        for (int i = 0; i < properties.Count; ++i)
        {
          IoProperty property = properties[i];
          if (condition(property))
          {
            properties.RemoveAt(i--);
            yield return property;
          }
        }
      }
    }

    public IEnumerator<IoProperty> GetEnumerator() => _properties == null ? EmptyEnumerator<IoProperty>.Instance : (IEnumerator<IoProperty>) _properties.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Add(IoProperty property) => _properties.Add(property);

    public bool Remove(int id)
    {
      int index = _properties.Select((x, ix) => new KeyValuePair<int, int>(x.Id, ix)).Where(x => x.Key == id).Select(x => x.Value).DefaultIfEmpty(-1).First();
      if (index < 0)
        return false;
      _properties.RemoveAt(index);
      return true;
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj))
        return false;
      return ReferenceEquals(this, obj) || Equals(obj as IoElement);
    }

    public bool Equals(IoElement other)
    {
      if (ReferenceEquals(null, other))
        return false;
      return ReferenceEquals(this, other) || _eventId == other._eventId && Equals(_properties, other._properties);
    }

    private static bool Equals(IList<IoProperty> lhs, ICollection<IoProperty> rhs)
    {
      if (ReferenceEquals(lhs, rhs))
        return true;
      if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
        return false;
      int count = lhs.Count;
      if (count != rhs.Count)
        return false;
      for (int index = 0; index < count; ++index)
      {
        IoProperty p1 = lhs[index];
        IoProperty ioProperty = rhs.FirstOrDefault(x => x.Id == p1.Id);
        if (!Equals(p1, ioProperty))
          return false;
      }
      return true;
    }

    public override int GetHashCode() => _eventId * 397 ^ _properties.OrderBy(x => x.Id).Aggregate(0, (x, p) => x * 397 ^ p.GetHashCode());

    public static bool operator ==(IoElement left, IoElement right) => Equals(left, right);

    public static bool operator !=(IoElement left, IoElement right) => !Equals(left, right);

    public override string ToString() => _properties == null ? "I/O:EventId=" + EventId : string.Format("I/O:EventId={0}, {1}", EventId, string.Join(",", _properties.Select(x => x.ToString()).ToArray()));
  }
}
