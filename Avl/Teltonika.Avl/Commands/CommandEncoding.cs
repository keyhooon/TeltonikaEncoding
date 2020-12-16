// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Commands.CommandEncoding
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Teltonika.IO;
using Teltonika.Logging;

namespace Teltonika.Avl.Commands
{
  public class CommandEncoding : IEncoding<Command>
  {
    private static readonly ILog Log = LogManager.GetLogger(typeof (CommandEncoding));
    private static CommandEncoding _instance;
    private static readonly Dictionary<byte, Type> CommandIdToType = new Dictionary<byte, Type>();
    private static readonly Type[] EmptyTypes = new Type[0];
    private static readonly object[] EmptyArray = new object[0];

    public static CommandEncoding Instance => _instance ?? (_instance = new CommandEncoding());

    static CommandEncoding()
    {
      foreach (var data in ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).SelectMany(x => x.GetTypes()).Where(x => x.IsSubclassOf(typeof(Command))).Where(x => !x.IsAbstract).Where(x => x.IsClass || x.IsValueType).Select(x =>
      {
        var data = new
        {
          Type = x,
          Constructor = x.GetConstructor(EmptyTypes)
        };
        return data;
      }).Where(x => x.Constructor != null))
      {
        try
        {
          Command command = (Command) data.Constructor.Invoke(EmptyArray);
                    CommandIdToType[command.Id] = data.Type;
        }
        catch (Exception ex)
        {
                    Log.WarnFormat("Unable to construct \"{0}\": {1}", data.Type, ex);
        }
      }
    }

    public void Encode(Command command, IBitWriter writer)
    {
      if (command == null)
        throw new ArgumentNullException(nameof (command));
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      command.Encode(writer);
    }

    public Command Decode(IBitReader reader)
    {
      byte num = reader != null ? reader.ReadByte() : throw new ArgumentNullException(nameof (reader));
      Command command;
      if (CommandIdToType.ContainsKey(num))
      {
        command = ((Command) Activator.CreateInstance(CommandIdToType[num])).Decode(num, reader);
      }
      else
      {
        command = new Command(num);
        int count = reader.ReadInt32();
        command.Data = reader.ReadBytes(count);
      }
      return command;
    }
  }
}
