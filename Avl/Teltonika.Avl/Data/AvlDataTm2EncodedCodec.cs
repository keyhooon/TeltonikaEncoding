// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.AvlDataTm2EncodedCodec
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.IO;
using Teltonika.Avl.Data.Encoding;
using Teltonika.Avl.Tools;

namespace Teltonika.Avl.Data
{
  public class AvlDataTm2EncodedCodec : Codec<AvlData[], AvlDataArrayEncodingAdapter<DefaultAvlDataEncoding>>
  {
    private static readonly AvlDataTm2EncodedCodec instance = new AvlDataTm2EncodedCodec();

    public static AvlDataTm2EncodedCodec Instance => instance;

    public override byte Id => 9;

    public override object Decode(Stream stream)
    {
      byte num = stream.CreateSuitableBinaryReader().ReadByte();
      byte[] numArray;
      try
      {
        numArray = HuffmanDecoder.Decode(stream);
      }
      catch (Exception ex)
      {
        throw new CodecException("Unable to decode", ex);
      }
      byte[] buffer = numArray != null ? new byte[numArray.Length + 1] : throw new CodecException("Unable to decode");
      buffer[0] = num;
      Buffer.BlockCopy(numArray, 0, buffer, 1, numArray.Length);
      using (MemoryStream memoryStream = new MemoryStream(buffer))
        return base.Decode(memoryStream);
    }

    public override byte[] Encode(object data) => throw new NotImplementedException();

    public override void Encode(object data, Stream stream) => throw new NotImplementedException();
  }
}
