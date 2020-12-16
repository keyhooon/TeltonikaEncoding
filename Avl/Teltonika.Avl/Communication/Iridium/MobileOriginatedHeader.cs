// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Communication.Iridium.MobileOriginatedHeader
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;

namespace Teltonika.Avl.Communication.Iridium
{
  public class MobileOriginatedHeader : InformationElement
  {
    public MobileOriginatedHeader(
      uint cdrReference,
      long imei,
      byte sessionStatus,
      ushort momsn,
      ushort mtmsn,
      DateTime timeOfSession)
    {
      CdrReference = cdrReference;
      Imei = imei;
      SessionStatus = sessionStatus;
      MOMSN = momsn;
      MTMSN = mtmsn;
      TimeOfSession = timeOfSession;
    }

    public override InformationElementId Id => InformationElementId.Header;

    public uint CdrReference { get; private set; }

    public long Imei { get; private set; }

    public byte SessionStatus { get; private set; }

    public ushort MOMSN { get; private set; }

    public ushort MTMSN { get; private set; }

    public DateTime TimeOfSession { get; private set; }
  }
}
