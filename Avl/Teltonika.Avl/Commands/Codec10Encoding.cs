// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Commands.Codec10Encoding
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using Teltonika.IO;

namespace Teltonika.Avl.Commands
{
  public class Codec10Encoding : IEncoding<byte[]>
  {
    public void Encode(byte[] obj, IBitWriter writer)
    {
      writer.Write((byte) 1);
      writer.Write(obj);
      writer.Write((byte) 1);
    }

    public byte[] Decode(IBitReader reader)
    {
      byte num = (byte) ((ulong) reader.BaseStream.Length / 8UL);
      reader.ReadByte();
      return reader.ReadBytes(num - 2);
    }
  }
}
