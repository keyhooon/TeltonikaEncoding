// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.FMPro3.Commands.Device.RecordCommand
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Teltonika.Avl.Data.FMPro3.Encoding;
using Teltonika.IO;

namespace Teltonika.Avl.Data.FMPro3.Commands.Device
{
  public sealed class RecordCommand : Command
  {
    private const int COMMAND_ID = 1;
    private readonly bool _areAnyRecordsLeft;
    private readonly IList<AvlData> _records;

    public RecordCommand(IEnumerable<AvlData> records)
      : this(false, records)
    {
    }

    public RecordCommand(bool areAnyRecordsLeft, IEnumerable<AvlData> records)
    {
      _areAnyRecordsLeft = areAnyRecordsLeft;
      if (records is IList<AvlData>)
        _records = records as List<AvlData>;
      else
        _records = records != null ? records.ToArray() : (new AvlData[0]);
    }

    public override int Id => 1;

    public bool AreAnyRecordsLeft => _areAnyRecordsLeft;

    public IList<AvlData> Records => _records;

    public override string ToString()
    {
      using (StringWriter stringWriter = new StringWriter())
      {
        stringWriter.Write("RECORD ");
        stringWriter.Write("Records Left: ");
        stringWriter.Write(AreAnyRecordsLeft);
        stringWriter.Write(", Records: ");
        stringWriter.Write(Records.Count);
        stringWriter.WriteLine("[");
        foreach (AvlData record in Records)
          stringWriter.WriteLine(record);
        stringWriter.Write("]");
        return stringWriter.ToString();
      }
    }

    public sealed class Encoding : CommandEncoding<RecordCommand>
    {
      public static readonly CommandEncoding<RecordCommand> Instance = new Encoding();

      public override int CommandId => 1;

      protected override void EncodeCore(RecordCommand command, IBitWriter writer)
      {
        IList<AvlData> records = command.Records;
        writer.Write(command.AreAnyRecordsLeft ? (byte) 1 : (byte) 0);
        writer.Write((byte) records.Count);
        foreach (AvlData avlData in records)
          FMPro3AvlDataEncoding.Instance.Encode(avlData, writer);
      }

      protected override RecordCommand DecodeCore(IBitReader reader) => new RecordCommand(reader.ReadByte() == 1, Enumerable.Range(0, reader.ReadByte()).Select(_ => FMPro3AvlDataEncoding.Instance.Decode(reader)));
    }
  }
}
