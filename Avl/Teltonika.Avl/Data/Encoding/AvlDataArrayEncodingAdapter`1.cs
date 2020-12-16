// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.Encoding.AvlDataArrayEncodingAdapter`1
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.Collections.Generic;
using Teltonika.IO;

namespace Teltonika.Avl.Data.Encoding
{
  public class AvlDataArrayEncodingAdapter<TAvlDataEncoding> : IEncoding<AvlData[]> where TAvlDataEncoding : IEncoding<AvlData>, new()
  {
    public void Encode(AvlData[] obj, IBitWriter writer)
    {
      if (obj == null)
        throw new ArgumentNullException(nameof (obj));
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      writer.Write((byte) obj.Length);
      TAvlDataEncoding avlDataEncoding = new TAvlDataEncoding();
      foreach (AvlData avlData in obj)
        avlDataEncoding.Encode(avlData, writer);
      writer.Write((byte) obj.Length);
      writer.Flush();
    }

    public AvlData[] Decode(IBitReader reader)
    {
      byte num1 = reader != null ? reader.ReadByte() : throw new ArgumentNullException(nameof (reader));
      TAvlDataEncoding avlDataEncoding = new TAvlDataEncoding();
      List<AvlData> avlDataList = new List<AvlData>();
      for (int index = 0; index < num1; ++index)
        avlDataList.Add(avlDataEncoding.Decode(reader));
      byte num2 = reader.ReadByte();
      if (num1 != avlDataList.Count)
        throw new ApplicationException(string.Format("Incorrect number of data: num1={0}, num2={1}, avlDataArray.Count={2}", num1, num2, avlDataList.Count));
      return avlDataList.ToArray();
    }
  }
}
