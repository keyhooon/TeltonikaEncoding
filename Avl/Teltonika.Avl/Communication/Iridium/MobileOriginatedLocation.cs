// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Communication.Iridium.MobileOriginatedLocation
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

namespace Teltonika.Avl.Communication.Iridium
{
  public class MobileOriginatedLocation : InformationElement
  {
    public MobileOriginatedLocation(double latitude, double longitude, uint cepRadius)
    {
      Latitude = latitude;
      Longitude = longitude;
      CepRadius = cepRadius;
    }

    public override InformationElementId Id => InformationElementId.LocationInformation;

    public double Latitude { get; private set; }

    public double Longitude { get; private set; }

    public uint CepRadius { get; private set; }
  }
}
