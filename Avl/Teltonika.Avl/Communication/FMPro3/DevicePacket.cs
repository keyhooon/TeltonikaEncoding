// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Communication.FMPro3.DevicePacket
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.Globalization;
using System.IO;
using Teltonika.Avl.Data.FMPro3.Commands;

namespace Teltonika.Avl.Communication.FMPro3
{
  public sealed class DevicePacket : Packet
  {
    private readonly ulong _imei;

    public DevicePacket(ulong imei, Command command)
      : base(command)
      => _imei = imei;

    public ulong IMEI => _imei;

    public override string ToString()
    {
      using (StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture))
      {
        stringWriter.Write("IMEI: ");
        stringWriter.Write(_imei);
        stringWriter.Write(", COMMAND: [");
        stringWriter.Write(Command);
        stringWriter.Write("]");
        return stringWriter.ToString();
      }
    }
  }
}
