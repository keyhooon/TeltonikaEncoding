// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Communication.Iridium.MobileOriginatedPayload
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

namespace Teltonika.Avl.Communication.Iridium
{
  public class MobileOriginatedPayload : InformationElement
  {
    public MobileOriginatedPayload(byte[] payload) => Payload = payload;

    public override InformationElementId Id => InformationElementId.Payload;

    public byte[] Payload { get; private set; }
  }
}
